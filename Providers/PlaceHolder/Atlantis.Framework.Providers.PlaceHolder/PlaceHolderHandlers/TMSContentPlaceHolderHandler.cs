using System;
using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.Providers.CDSContent.Interface;
using Atlantis.Framework.Providers.PlaceHolder.Interface;
using Atlantis.Framework.Providers.PlaceHolder.PlaceHolders;
using Atlantis.Framework.Providers.RenderPipeline.Interface;
using Atlantis.Framework.Providers.TMSContent.Interface;
using Atlantis.Framework.Render.Containers;

namespace Atlantis.Framework.Providers.PlaceHolder.PlaceHolderHandlers
{
  internal class TMSContentPlaceHolderHandler : CDSDocumentPlaceHolderHandler
  {
    private const string APPLICATION_NAME = "tms";
    private const string CONTENT_DOC_NAME = "default_template";
    private const string TRACKING_DIV_FORMAT = "<div data-tms-trackingid=\"{0}\">\n{1}\n</div>";

    internal TMSContentPlaceHolderHandler(IPlaceHolderHandlerContext context)
      : base(context) {}

    protected override string GetContent()
    {
      string renderedContent = string.Empty;

      try
      {
        TMSContentPlaceHolderData placeHolderData = new TMSContentPlaceHolderData(Context.Data);

        ICDSContentProvider cdsContentProvider;
        if (Context.ProviderContainer.TryResolve(out cdsContentProvider))
        {
          renderedContent = GetMessageVariantContent(placeHolderData, cdsContentProvider);
        }
        else
        {
          throw new ApplicationException("Could not resolve the required providers. CDSContentProvider is required.");
        }
      }
      catch (Exception ex)
      {
        string errorMessage = string.Format("PlaceHolder render error. Type: {0}, Message: {1}", Context.Type, ex.Message);
        LogError(errorMessage, "TMSContentPlaceHolderHandler.Render()");
      }

      return renderedContent ?? string.Empty;
    }

    private string AddTrackingDataToContent(string content, MessageVariant messageVariant)
    {
      string finalContent;

      finalContent = (messageVariant != null) ? 
        string.Format(TRACKING_DIV_FORMAT, messageVariant.TrackingId, content) : content;

      return finalContent;
    }

    private string GetMessageVariantContent(TMSContentPlaceHolderData placeHolderData, ICDSContentProvider cdsContentProvider)
    {
      string content = string.Empty;

      try
      {
        if (string.IsNullOrEmpty(placeHolderData.AppProduct) || string.IsNullOrEmpty(placeHolderData.InteractionName))
        {
          string errorMessage = string.Format("Attributes '{0}', and '{1}' are required.", PlaceHolderAttributes.AppProduct, PlaceHolderAttributes.InteractionPoint);
          LogError(errorMessage, "TMSContentPlaceHolderHandler.Render()");
        }

        string applName = placeHolderData.DefaultElement.App;
        string relativePath = placeHolderData.DefaultElement.Location;

        MessageVariant messageVariant;
        if (TryGetMessageVariant(placeHolderData.AppProduct, placeHolderData.InteractionName, out messageVariant) && messageVariant.HasContent)
        {
          if ((placeHolderData.ContentElement != null) && (placeHolderData.ContentElement.IsValid()))
          {
            applName = placeHolderData.ContentElement.App;
            relativePath = string.Format("{0}/{1}{2}", 
              placeHolderData.ContentElement.Location, messageVariant.Name,
              (placeHolderData.ContentElement.OverrideDocumentName) ? string.Empty : string.Format("/{0}", CONTENT_DOC_NAME));
          }
          else {
            applName = APPLICATION_NAME;
            relativePath = string.Format("{0}/{1}/{2}/{3}", 
              placeHolderData.AppProduct, placeHolderData.InteractionName, messageVariant.Name, CONTENT_DOC_NAME);
          }
        }

        content = cdsContentProvider.GetContent(applName, relativePath).Content;

        if (string.IsNullOrEmpty(content))
        {
          content = cdsContentProvider.GetContent(placeHolderData.DefaultElement.App, placeHolderData.DefaultElement.Location).Content;
        }

        content = ReplaceDataTokens(content);
        content = AddTrackingDataToContent(content, messageVariant);
      }
      catch (Exception ex)
      {
        string errorMessage = string.Format("PlaceHolder render error attempting to load TMS Content. {0}", ex.Message);
        LogError(errorMessage, "TMSContentPlaceHolderHandler.Render()");
      }

      return content;
    }

    private string ReplaceDataTokens(string rawContent)
    {
      IRenderPipelineProvider renderPipelineProvider = Context.ProviderContainer.Resolve<IRenderPipelineProvider>();
      return renderPipelineProvider.RenderContent(rawContent, new IRenderHandler[] {new ProviderContainerDataTokenRenderHandler()});
    }

    private bool TryGetMessageVariant(string appProduct, string interactionName, out MessageVariant messageVariant)
    {
      messageVariant = null;

      ITMSContentProvider tmsContentProvider;
      if (Context.ProviderContainer.TryResolve(out tmsContentProvider))
      {
        if (tmsContentProvider.TryGetNextMessageVariant(appProduct, interactionName, out messageVariant))
        {
          tmsContentProvider.ConsumeMessageVariant(appProduct, interactionName, messageVariant);
          Context.ProviderContainer.SetData("TMSMessageId", messageVariant.Id);
          Context.ProviderContainer.SetData("TMSMessageTag", messageVariant.Tag);
          Context.ProviderContainer.SetData("TMSMessageName", messageVariant.Name);
          Context.ProviderContainer.SetData("TMSMessageTrackingId", messageVariant.TrackingId);
          IEnumerable<KeyValuePair<string, string>> messageData;
          if ((messageData = messageVariant.Data) != null)
          {
            foreach (KeyValuePair<string, string> item in messageData)
            {
              if (!string.IsNullOrEmpty(item.Key))
              {
                Context.ProviderContainer.SetData(string.Format("TMSMessageData.{0}", item.Key), item.Value);
              }
            }
          }
        }
      }
      else
      {
        throw new ApplicationException("Could not resolve the required providers. TMSContentProvider is required.");
      }

      return (messageVariant != null);
    }
  }
}
