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
    private const string LOCATION_FORMAT = "{0}/{1}/{2}/default_template";
    private const string TMS_CDS_APP_NAME = "tms";
    private const string TMS_TRACKING_DIV_FORMAT = "<div data-tms-msgid=\"{0}\" data-tms-tagname=\"{1}\" data-tms-messagename=\"{2}\" data-tms-trackingid=\"{3}\">\n{4}\n</div>";

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

      if (messageVariant != null)
      {
        finalContent = string.Format(TMS_TRACKING_DIV_FORMAT, messageVariant.Id, messageVariant.Tag, messageVariant.Name, messageVariant.TrackingId, content);
      }
      else
      {
        finalContent = content;
      }

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

        string applicationName = placeHolderData.DefaultApplication;
        string relativePath = placeHolderData.DefaultLocation;

        MessageVariant messageVariant;

        if (TryGetMessageVariant(placeHolderData.AppProduct, placeHolderData.InteractionName, out messageVariant) && messageVariant.HasContent)
        {
          applicationName = TMS_CDS_APP_NAME;
          relativePath = string.Format(LOCATION_FORMAT, placeHolderData.AppProduct, placeHolderData.InteractionName, messageVariant.Name);
        }

        content = cdsContentProvider.GetContent(applicationName, relativePath).Content;

        if (string.IsNullOrEmpty(content))
        {
          content = cdsContentProvider.GetContent(placeHolderData.DefaultApplication, placeHolderData.DefaultLocation).Content;
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
      return renderPipelineProvider.RenderContent(rawContent, new[] {new ProviderContainerDataTokenRenderHandler()});
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
