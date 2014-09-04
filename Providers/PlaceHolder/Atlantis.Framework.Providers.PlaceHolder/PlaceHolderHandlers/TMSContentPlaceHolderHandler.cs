using System;
using System.ComponentModel;
using System.Linq;
using Atlantis.Framework.Providers.CDSContent.Interface;
using Atlantis.Framework.Providers.PlaceHolder.Interface;
using Atlantis.Framework.Providers.PlaceHolder.PlaceHolders;
using Atlantis.Framework.Providers.RenderPipeline.Interface;
using Atlantis.Framework.Providers.TMSContent.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder.PlaceHolderHandlers
{
  internal class TMSContentPlaceHolderHandler : CDSDocumentPlaceHolderHandler
  {
    private const string APP_NAME = "tms";
    private const string LOCATION_FORMAT = "{0}/{1}/{2}/default_template";
    private const string TMS_CONTENT_FORMAT = "<div data-tms-msgid=\"{0}\" data-tms-tagname=\"{1}\" data-tms-messagename=\"{2}\" data-tms-trackingid=\"{3}\">\n{4}\n</div>";

    internal TMSContentPlaceHolderHandler(IPlaceHolderHandlerContext context)
      : base(context) {}

    protected override string GetContent()
    {
      string renderContent = string.Empty;

      try
      {
        TMSContentPlaceHolderData placeHolderData = new TMSContentPlaceHolderData(Context.Data);

        ICDSContentProvider cdsContentProvider;
        if (Context.ProviderContainer.TryResolve(out cdsContentProvider))
        {
          string rawContent;
          if (!TryGetMessageVariantContent(placeHolderData, cdsContentProvider, out rawContent))
          {
            rawContent = cdsContentProvider.GetContent(placeHolderData.Default.Application, placeHolderData.Default.Location).Content;
          }

          IRenderPipelineProvider renderPipelineProvider = Context.ProviderContainer.Resolve<IRenderPipelineProvider>();
          renderContent = renderPipelineProvider.RenderContent(rawContent, Context.RenderHandlers);
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

      return renderContent;
    }

    private string TryWrapContent(string rawContent, MessageVariant messageVariant)
    {
      if (messageVariant == null)
        return rawContent;

      return string.Format(TMS_CONTENT_FORMAT, messageVariant.Id, messageVariant.Tags,
        messageVariant.Name, messageVariant.TrackingId, rawContent);
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
          Context.ProviderContainer.SetData("TMSMessageTag", messageVariant.Tags);
          Context.ProviderContainer.SetData("TMSMessageName", messageVariant.Name);
          Context.ProviderContainer.SetData("TMSMessageTrackingId", messageVariant.TrackingId);
        }
      }
      else
      {
        throw new ApplicationException("Could not resolve the required providers. TMSContentProvider is required.");
      }

      return (messageVariant != null);
    }

    private bool TryGetMessageVariantContent(TMSContentPlaceHolderData placeHolderData, ICDSContentProvider cdsContentProvider, out string rawContent)
    {
      rawContent = string.Empty;

      try
      {
        // Get TMS+ Content
        string appProduct;
        string interactionName;
        if (placeHolderData.TryGetAttribute(PlaceHolderAttributes.AppProduct, out appProduct) && (!string.IsNullOrEmpty(appProduct)) &&
            placeHolderData.TryGetAttribute(PlaceHolderAttributes.InteractionPoint, out interactionName) && (!string.IsNullOrEmpty(interactionName)))
        {
          MessageVariant messageVariant;
          if (TryGetMessageVariant(appProduct, interactionName, out messageVariant) && (messageVariant.HasContent))
          {
            rawContent = cdsContentProvider.GetContent(APP_NAME,
              string.Format(LOCATION_FORMAT, appProduct, interactionName, messageVariant.Name)).Content;
          }
          else
          {
            rawContent = cdsContentProvider.GetContent(placeHolderData.Default.Application, placeHolderData.Default.Location).Content;
          }

          rawContent = TryWrapContent(rawContent, messageVariant);
          return true;
        }

        string errorMessage = string.Format("Attributes '{0}', and '{1}' are required.", PlaceHolderAttributes.AppProduct, PlaceHolderAttributes.InteractionPoint);
        LogError(errorMessage, "TMSContentPlaceHolderHandler.Render()");
      }
      catch (Exception ex)
      {
        string errorMessage = string.Format("PlaceHolder render error attempting to load TMS Content. {0}", ex.Message);
        LogError(errorMessage, "TMSContentPlaceHolderHandler.Render()");        
      }

      return false;
    }
  }
}
