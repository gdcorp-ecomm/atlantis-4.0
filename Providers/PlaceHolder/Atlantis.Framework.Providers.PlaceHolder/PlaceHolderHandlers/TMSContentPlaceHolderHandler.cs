using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Providers.CDSContent.Interface;
using Atlantis.Framework.Providers.PlaceHolder.Interface;
using Atlantis.Framework.Providers.PlaceHolder.PlaceHolders;
using Atlantis.Framework.Providers.RenderPipeline.Interface;
using Atlantis.Framework.Providers.TMSContent.Interface;
using Atlantis.Framework.Providers.TMSContentData.Interface;
using Atlantis.Framework.Render.Containers;
using Newtonsoft.Json.Linq;

namespace Atlantis.Framework.Providers.PlaceHolder.PlaceHolderHandlers
{
  internal class TMSContentPlaceHolderHandler : CDSDocumentPlaceHolderHandler
  {
    private const string CONTEXT_DATA_PREFIX = "tms.message";
    private const string TRACKING_DIV_FORMAT = "<div data-tms-trackingid=\"[@D[tms.message.tracking_id]@D]\">\n{0}\n</div>";

    private readonly Lazy<ICDSContentProvider> _cdsContentProvider;
    private readonly Lazy<IRenderPipelineProvider> _renderPipelineProvider;
    private readonly Lazy<ITMSContentDataProvider> _tmsContentDataProvider;
    private readonly Lazy<ITMSContentProvider> _tmsContentProvider;

    internal TMSContentPlaceHolderHandler(IPlaceHolderHandlerContext context)
      : base(context)
    {
      _cdsContentProvider = new Lazy<ICDSContentProvider>(() => context.ProviderContainer.Resolve<ICDSContentProvider>());
      _renderPipelineProvider = new Lazy<IRenderPipelineProvider>(() => context.ProviderContainer.Resolve<IRenderPipelineProvider>());
      _tmsContentProvider = new Lazy<ITMSContentProvider>(() => context.ProviderContainer.Resolve<ITMSContentProvider>());
      _tmsContentDataProvider = new Lazy<ITMSContentDataProvider>(() => context.ProviderContainer.Resolve<ITMSContentDataProvider>());
    }

    protected override string GetContent()
    {
      try
      {
        TMSContentPlaceHolderData placeHolderData = new TMSContentPlaceHolderData(Context.Data);
        string shopper_id = _tmsContentDataProvider.Value.GetShopperID();
        JObject postData = _tmsContentDataProvider.Value.GetPostData();
        string content;

        IList<IMessageVariant> messages;
        if (placeHolderData.Rank == null)
        {
          // Consumption-Based Selection
          if (_tmsContentProvider.Value.TryGetMessages(shopper_id, placeHolderData.Product, placeHolderData.Interaction,
            placeHolderData.Channel, postData, out messages, message => !message.IsConsumed))
          {
            foreach (IMessageVariant message in messages)
            {
              if (TryGetMessageContent(placeHolderData, message, out content))
              {
                _tmsContentProvider.Value.ConsumeMessage(placeHolderData.Product, placeHolderData.Interaction,
                  placeHolderData.Channel, message);
                SetContextData(message.ToJson(), CONTEXT_DATA_PREFIX);
                content = string.Format(TRACKING_DIV_FORMAT, content);
                content = _renderPipelineProvider.Value.RenderContent(content,
                  new IRenderHandler[] {new ProviderContainerDataTokenRenderHandler()});
                return content;
              }
            }
          }
        }
        else
        {
          // Rank-Based Selection
          if (_tmsContentProvider.Value.TryGetMessages(shopper_id, placeHolderData.Product, placeHolderData.Interaction,
            placeHolderData.Channel, postData, out messages))
          {
            IMessageVariant message = messages.ElementAtOrDefault(placeHolderData.Rank.Value);
            if ((message != null) && (TryGetMessageContent(placeHolderData, message, out content)))
            {
              _tmsContentProvider.Value.ConsumeMessage(placeHolderData.Product, placeHolderData.Interaction,
                placeHolderData.Channel, message);
              SetContextData(message.ToJson(), CONTEXT_DATA_PREFIX);
              content = string.Format(TRACKING_DIV_FORMAT, content);
              content = _renderPipelineProvider.Value.RenderContent(content,
                new IRenderHandler[] {new ProviderContainerDataTokenRenderHandler()});
              return content;
            }
          }
        }

        // Default Content
        if (TryGetMessageContent(placeHolderData, null, out content))
        {
          return content;
        }

        throw new SystemException(
          string.Format("Failed to load any content for using placeholder data '{0}'", placeHolderData.Serialize()));
      }
      catch (Exception ex)
      {
        string errorMessage = string.Format("PlaceHolder render error. Type: {0}, Message: {1}", Context.Type, ex.Message);
        LogError(errorMessage, "TMSContentPlaceHolderHandler.GetContent()");
        return string.Empty;
      }
    }

    private void SetContextData(JObject jObject, string prefix)
    {
      // TODO
      // Clear all previous context data

      foreach (JProperty property in jObject.Properties())
      {
        string contextKey = prefix + "." + property.Name;
        switch (property.Value.Type)
        {
          case JTokenType.Boolean:
          case JTokenType.Date:
          case JTokenType.Float:
          case JTokenType.Integer:
          case JTokenType.String:
            Context.ProviderContainer.SetData(contextKey.ToLower(), property.Value.ToObject<object>());
            break;
          case JTokenType.Object:
            SetContextData(property.Value.ToObject<JObject>(), contextKey);
            break;
        }
      }
    }

    private bool TryGetMessageContent(TMSContentPlaceHolderData placeHolderData, IMessageVariant message, out string content)
    {
      StringBuilder path = new StringBuilder(256);

      // Default Path: {Location}/{Channel}/{Template||Interaction}
      // Message Path: {Location}/{Channel}/{Message}/{Template||Interaction}

      // {Location}
      if (!string.IsNullOrEmpty(placeHolderData.Location))
      {
        path.Append(placeHolderData.Location + "/");
      }

      // Path: {Channel}
      if (!string.IsNullOrEmpty(placeHolderData.Channel))
      {
        path.Append(placeHolderData.Channel + "/");
      }

      // Path: {Message}
      if (message != null)
      {
        path.Append(message.Name + "/");
      }

      // Path: {Template||Interaction}
      path.Append(!string.IsNullOrEmpty(placeHolderData.Template) ? placeHolderData.Template : placeHolderData.Interaction);

      content = _cdsContentProvider.Value.GetContent(placeHolderData.App, path.ToString()).Content;
      return (!string.IsNullOrEmpty(content));
    }
  }
}
