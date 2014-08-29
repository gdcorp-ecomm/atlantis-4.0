using System;
using System.Linq;
using Atlantis.Framework.Providers.CDSContent.Interface;
using Atlantis.Framework.Providers.PlaceHolder.Interface;
using Atlantis.Framework.Providers.PlaceHolder.PlaceHolders;
using Atlantis.Framework.Providers.RenderPipeline.Interface;
using Atlantis.Framework.Providers.TMSContent.Interface;
using Atlantis.Framework.TMSContent.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder.PlaceHolderHandlers
{
  internal class TMSContentPlaceHolderHandler : CDSDocumentPlaceHolderHandler
  {
    private const string APP_NAME = "tms";
    private const string LOCATION_FORMAT = "{0}/{1}/{2}/default_template";
    private const string TMS_CONTENT_FORMAT = "<div data-tms-msgid=\"{0}\" data-tms-tagname=\"{1}\" data-tms-messagename=\"{2}\" data-tms-trackingid=\"{3}\">\n{4}\n</div>";

    internal TMSContentPlaceHolderHandler(IPlaceHolderHandlerContext context) : base(context) {}

    protected override string GetContent()
    {
      string renderContent = String.Empty;

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
        LogError(errorMessage, "TMSDocumentPlaceHolderHandler.Render()");
      }

      return renderContent;
    }

    private string GetWrappedContent(string rawContent, MessageVariant messageVariant)
    {
      return String.Format(TMS_CONTENT_FORMAT, messageVariant.ID, messageVariant.Tags,
        messageVariant.Name, messageVariant.TrackingID, rawContent);
    }

    private bool TryGetMessageVariantContent(TMSContentPlaceHolderData placeHolderData, ICDSContentProvider cdsContentProvider, out string rawContent)
    {
      rawContent = String.Empty;

      // Get TMS+ Content
      string appProduct;
      string interactionPoint;
      if (placeHolderData.TryGetAttribute(PlaceHolderAttributes.AppProduct, out appProduct) && (!String.IsNullOrEmpty(appProduct)) &&
          placeHolderData.TryGetAttribute(PlaceHolderAttributes.InteractionPoint, out interactionPoint) && (!String.IsNullOrEmpty(interactionPoint)))
      {
        MessageVariant messageVariant;
        if (TryGetMessageVariant(appProduct, interactionPoint, out messageVariant) && messageVariant.HasContent)
        {
          rawContent = cdsContentProvider.GetContent(APP_NAME,
            String.Format(LOCATION_FORMAT, appProduct, interactionPoint, messageVariant.Name)).Content;
          if (String.IsNullOrEmpty(rawContent))
          {
            rawContent = cdsContentProvider.GetContent(placeHolderData.Default.Application, placeHolderData.Default.Location).Content;
          }

          rawContent = GetWrappedContent(rawContent, messageVariant);
        }
        else
        {
          rawContent = cdsContentProvider.GetContent(placeHolderData.Default.Application, placeHolderData.Default.Location).Content;
        }

        return true;
      }

      string errorMessage = String.Format("Attributes '{0}', and '{1}' are required.", PlaceHolderAttributes.AppProduct, PlaceHolderAttributes.InteractionPoint);
      LogError(errorMessage, "TMSContentPlaceHolderHandler.Render()");
      return false;
    }

    private bool TryGetMessageVariant(string appProduct, string interactionPoint, out MessageVariant messageVariant)
    {
      messageVariant = null;

      ITMSContentProvider tmsContentProvider;
      if (Context.ProviderContainer.TryResolve(out tmsContentProvider))
      {
        messageVariant = tmsContentProvider.GetMessageVariant(appProduct, interactionPoint);
        if (messageVariant != null)
        {
          tmsContentProvider.ConsumeMessageVariant(appProduct, interactionPoint, messageVariant);
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
