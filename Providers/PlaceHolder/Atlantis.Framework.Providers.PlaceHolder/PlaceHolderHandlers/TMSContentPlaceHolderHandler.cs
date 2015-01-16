using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.CDSContent.Interface;
using Atlantis.Framework.Providers.PlaceHolder.Interface;
using Atlantis.Framework.Providers.PlaceHolder.PlaceHolders;
using Atlantis.Framework.Providers.RenderPipeline.Interface;
using Atlantis.Framework.Providers.TMSContent.Interface;
using Atlantis.Framework.Providers.TMSContentData.Interface;
using Atlantis.Framework.Providers.Web.Interface;
using Atlantis.Framework.Render.Containers;
using Newtonsoft.Json.Linq;

namespace Atlantis.Framework.Providers.PlaceHolder.PlaceHolderHandlers
{
  internal class TMSContentPlaceHolderHandler : CDSDocumentPlaceHolderHandler
  {
    private const string CONTEXT_DATA_PREFIX = "tms.message";

    private const string TRACKING_DIV_FORMAT = "<div data-tms-name='[@D[tms.message.name]@D]' " +
                                               "data-tms-strategy='[@D[tms.message.strategy]@D]' " +
                                               "data-tms-trackingid='[@D[tms.message.tracking_id]@D]'>\n{0}\n</div>";
    private const string TMS_SPOOF_DELIM = ",";
    private const string TMS_SPOOF_KEY = "tmsPlaceholderSpoof";

    private readonly Lazy<ICDSContentProvider> _cdsContentProvider;
    private readonly Lazy<IDebugContext> _debugContextProvider;
    private readonly Lazy<IRenderPipelineProvider> _renderPipelineProvider;
    private readonly Lazy<ITMSContentDataProvider> _tmsContentDataProvider;
    private readonly Lazy<ITMSContentProvider> _tmsContentProvider;
    private readonly Lazy<ISiteContext> _siteContextProvider;
    private readonly Lazy<IWebContext> _webContextProvider;

    internal TMSContentPlaceHolderHandler(IPlaceHolderHandlerContext context)
      : base(context)
    {
      _cdsContentProvider = new Lazy<ICDSContentProvider>(() => context.ProviderContainer.Resolve<ICDSContentProvider>());
      _renderPipelineProvider = new Lazy<IRenderPipelineProvider>(() => context.ProviderContainer.Resolve<IRenderPipelineProvider>());
      _tmsContentProvider = new Lazy<ITMSContentProvider>(() => context.ProviderContainer.Resolve<ITMSContentProvider>());
      _tmsContentDataProvider = new Lazy<ITMSContentDataProvider>(() => context.ProviderContainer.Resolve<ITMSContentDataProvider>());

      // Optional Provider(s)
      _debugContextProvider = new Lazy<IDebugContext>(() =>
      {
        IDebugContext value;
        return context.ProviderContainer.TryResolve(out value) ? value : null;
      });

      _siteContextProvider = new Lazy<ISiteContext>(() =>
      {
        ISiteContext value;
        return context.ProviderContainer.TryResolve(out value) ? value : null;
      });

      _webContextProvider = new Lazy<IWebContext>(() =>
      {
        IWebContext value;
        return context.ProviderContainer.TryResolve(out value) ? value : null;
      });
    }

    protected override string GetContent()
    {
      try
      {
        TMSContentPlaceHolderData placeHolderData = new TMSContentPlaceHolderData(Context.Data);
        string shopper_id = _tmsContentDataProvider.Value.GetShopperID();
        JObject postData = _tmsContentDataProvider.Value.GetPostData();
        string content;

        // Spoof Content
        if (TryGetSpoofMessageContent(placeHolderData, out content))
        {
          return content;
        }

        // TMS Content
        IList<IMessageVariant> messages;
        if (placeHolderData.Rank == null)
        {
          // Consumption-Based Selection
          if (_tmsContentProvider.Value.TryGetMessages(shopper_id, placeHolderData.Product, placeHolderData.Interaction,
            placeHolderData.Channel, postData, out messages, message => !message.IsConsumed))
          {
            IMessageVariant message = messages.FirstOrDefault();
            if ((message != null) && (message.IsControl ?
              TryGetMessageContent(placeHolderData, out content) :
              TryGetMessageContent(placeHolderData, out content, message)))
            {
              _tmsContentProvider.Value.ConsumeMessage(placeHolderData.Product, placeHolderData.Interaction,
                placeHolderData.Channel, message);
              SetContextData(message, CONTEXT_DATA_PREFIX);
              content = string.Format(TRACKING_DIV_FORMAT, content);
              content = _renderPipelineProvider.Value.RenderContent(content,
                new IRenderHandler[] {new ProviderContainerDataTokenRenderHandler()});
              return content;
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
            if ((message != null) && (message.IsControl ?
              TryGetMessageContent(placeHolderData, out content) :
              TryGetMessageContent(placeHolderData, out content, message)))
            {
              _tmsContentProvider.Value.ConsumeMessage(placeHolderData.Product, placeHolderData.Interaction,
                placeHolderData.Channel, message);
              SetContextData(message, CONTEXT_DATA_PREFIX);
              content = string.Format(TRACKING_DIV_FORMAT, content);
              content = _renderPipelineProvider.Value.RenderContent(content,
                new IRenderHandler[] {new ProviderContainerDataTokenRenderHandler()});
              return content;
            }
          }
        }

        // Default Content
        if (TryGetMessageContent(placeHolderData, out content))
        {
          return content;
        }

        throw new SystemException(
          string.Format("Failed to load any content for placeholder. '{0}'", placeHolderData.Serialize()));
      }
      catch (Exception ex)
      {
        string errorMessage = string.Format("PlaceHolder render error. Type: {0}, Message: {1}", Context.Type, ex.Message);
        LogError(errorMessage, "TMSContentPlaceHolderHandler.GetContent()");
        return string.Empty;
      }
    }

    private void SetContextData(IMessageVariant message, string prefix)
    {
      SetContextData(message.ToJson(), prefix);
    }

    private void SetContextData(JObject jObject, string prefix)
    {
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

    private bool TryGetMessageContent(TMSContentPlaceHolderData placeHolderData, out string content, IMessageVariant message = null)
    {
      // Default Path: {Location}/{Channel}/{Template||Interaction}
      // Message Path: {Location}/{Channel}/{Message}/{Template||Interaction}
      StringBuilder path = new StringBuilder(256);

      // Set {Location}
      if (!string.IsNullOrEmpty(placeHolderData.Location))
      {
        path.Append(placeHolderData.Location + "/");
      }

      // Set {Channel}
      if (!string.IsNullOrEmpty(placeHolderData.Channel))
      {
        path.Append(placeHolderData.Channel + "/");
      }

      // Set {Message}
      if (message != null)
      {
        path.Append(message.Name + "/");
      }

      // Set {Template||Interaction}
      path.Append(!string.IsNullOrEmpty(placeHolderData.Template) ? placeHolderData.Template : placeHolderData.Interaction);

      // Get Content
      content = _cdsContentProvider.Value.GetContent(placeHolderData.App, path.ToString()).Content;
      return (!string.IsNullOrEmpty(content));
    }

    [ExcludeFromCodeCoverage]
    private bool TryGetSpoofMessageContent(TMSContentPlaceHolderData placeHolderData, out string content)
    {
      content = null;

      if ((_webContextProvider.Value != null) && _webContextProvider.Value.HasContext &&
          (_webContextProvider.Value.ContextBase != null) &&
          (_webContextProvider.Value.ContextBase.Request != null) &&
          (_webContextProvider.Value.ContextBase.Request.QueryString != null) &&
          (_siteContextProvider.Value != null) && _siteContextProvider.Value.IsRequestInternal)
      {
        string[] spoofValuesList = _webContextProvider.Value.ContextBase.Request.QueryString.GetValues(TMS_SPOOF_KEY);
        if (spoofValuesList != null)
        {
          foreach (string spoofValue in spoofValuesList)
          {
            List<KeyValuePair<string, string>> attributes = new List<KeyValuePair<string, string>>(12);
            // Format: spoofPlaceholderContent=app:tms,location:,product:hp,interaction:marquee,channel:homepage,template:marquee,rank:0
            // Required Parameter(s): product, interaction
            // Optional Parameter(s): channel, template, rank
            string[] values = spoofValue.Split(new[] {TMS_SPOOF_DELIM}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var value in values)
            {
              Match match = Regex.Match(value, @"(?<key>[^:]+):(?<value>[^:]+)");
              if (match.Success)
              {
                attributes.Add(new KeyValuePair<string, string>(match.Groups["key"].Value, match.Groups["value"].Value));
              }
            }

            TMSContentPlaceHolderData spoofData = new TMSContentPlaceHolderData(attributes);
            if ((placeHolderData.Product.Equals(spoofData.Product, StringComparison.OrdinalIgnoreCase)) &&
                (placeHolderData.Interaction.Equals(spoofData.Interaction, StringComparison.OrdinalIgnoreCase)) &&
                (string.Equals(placeHolderData.Channel, spoofData.Channel, StringComparison.OrdinalIgnoreCase)) &&
                (string.Equals(placeHolderData.Template, spoofData.Template, StringComparison.OrdinalIgnoreCase)) &&
                (placeHolderData.Rank == spoofData.Rank))
            {
              // Spoof Path: {Location}/{Template||Interaction}
              StringBuilder path = new StringBuilder(256);

              // Set {Location}
              if (!string.IsNullOrEmpty(spoofData.Location))
              {
                path.Append(spoofData.Location + "/");
              }

              // Set {Template||Interaction}
              path.Append(!string.IsNullOrEmpty(spoofData.Template) ? spoofData.Template : spoofData.Interaction);

              // Get Content
              TryLogSpoofMessageContent(spoofData, path.ToString());
              content = _cdsContentProvider.Value.GetContent(spoofData.App, path.ToString()).Content;
              return (!string.IsNullOrEmpty(content));
            }
          }
        }
      }
      return false;
    }

    [ExcludeFromCodeCoverage]
    private void TryLogSpoofMessageContent(TMSContentPlaceHolderData spoofData, string path)
    {
      if ((_siteContextProvider.Value != null) && _siteContextProvider.Value.IsRequestInternal)
      {
        TryLogDebugMessage(string.Format("TMS Placeholder Spoof ({0}/{1}/{2}){{{3}}}",
          spoofData.App, spoofData.Product, spoofData.Interaction, spoofData.Channel), path);
      }
    }

    [ExcludeFromCodeCoverage]
    private void TryLogDebugMessage(string key, string value)
    {
      if ((_siteContextProvider.Value != null) && _siteContextProvider.Value.IsRequestInternal &&
          (_debugContextProvider.Value != null))
      {
        _debugContextProvider.Value.LogDebugTrackingData(key, value);
      }
    }
  }
}
