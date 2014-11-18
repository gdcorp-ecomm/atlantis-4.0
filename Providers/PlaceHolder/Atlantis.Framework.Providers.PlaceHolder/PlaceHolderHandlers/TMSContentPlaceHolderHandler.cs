using System;
using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.CDSContent.Interface;
using Atlantis.Framework.Providers.PlaceHolder.Interface;
using Atlantis.Framework.Providers.PlaceHolder.PlaceHolders;
using Atlantis.Framework.Providers.RenderPipeline.Interface;
using Atlantis.Framework.Providers.TMSContent.Interface;
using Atlantis.Framework.Providers.Web.Interface;
using Atlantis.Framework.Render.Containers;

namespace Atlantis.Framework.Providers.PlaceHolder.PlaceHolderHandlers
{
  internal class TMSContentPlaceHolderHandler : CDSDocumentPlaceHolderHandler
  {
    private const string APPLICATION_NAME = "tms";
    private const string CONTENT_DOC_NAME = "default_template";

    private const string TMS_SPOOF_DELIM = "|";
    private const string TMS_SPOOF_KEY = "tmsContentSpoof";
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
        if (!Context.ProviderContainer.TryResolve(out cdsContentProvider))
        {
          throw new ApplicationException("Could not resolve the required providers. CDSContentProvider is required.");
        }

        renderedContent = GetMessageVariantContent(placeHolderData, cdsContentProvider);
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
          else
          {
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
      if (!TryGetMessageVariantSpoof(appProduct, interactionName, out messageVariant))
      {
        ITMSContentProvider tmsContentProvider;
        if (!Context.ProviderContainer.TryResolve(out tmsContentProvider))
        {
          throw new ApplicationException("Could not resolve the required providers. TMSContentProvider is required.");
        }

        if (tmsContentProvider.TryGetNextMessageVariant(appProduct, interactionName, out messageVariant))
        {
          tmsContentProvider.ConsumeMessageVariant(appProduct, interactionName, messageVariant);
        }
      }

      if (messageVariant != null)
      {
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

      return (messageVariant != null);
    }

    private bool TryGetMessageVariantSpoof(string appProduct, string interactionName, out MessageVariant messageVariant)
    {
      messageVariant = null;

      ISiteContext siteContextProvider;
      IWebContext webContextProvider;
      if (!Context.ProviderContainer.TryResolve(out siteContextProvider) || !Context.ProviderContainer.TryResolve(out webContextProvider))
      {
        throw new ApplicationException("Could not resolve the required providers. ISiteContext, IWebContext are required.");
      }

      if (webContextProvider.HasContext && siteContextProvider.IsRequestInternal)
      {
        string[] rawSpoofValues = webContextProvider.ContextBase.Request.QueryString.GetValues(TMS_SPOOF_KEY);
        if (rawSpoofValues != null)
        {
          foreach (string spoofValue in rawSpoofValues)
          {
            // Format: {appProduct}/{interactioName}|name=abc,tag=abc_tag,etc...|dataItem1=value1,dataItem2=value2,etc...
            // DataItem(s) are optional. 
            string[] values = spoofValue.Split(new[] {TMS_SPOOF_DELIM}, StringSplitOptions.RemoveEmptyEntries);
            if (values.Length >= 2)
            {
              // Format: {appProduct}/{interactionName}
              string interactionPath = values[0].ToLowerInvariant();
              if (!interactionPath.Equals(string.Format("{0}/{1}", appProduct, interactionName)))
              {
                continue;
              }

              // Format: name=abc,tag=abc_tag,etc...
              Dictionary<string, string> messageVariantInfo = new Dictionary<string, string>();
              string infoValues = values[1].ToLowerInvariant();
              foreach (string infoValue in infoValues.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries))
              {
                string[] infoSplit = infoValue.Split(new[] {'='}, StringSplitOptions.RemoveEmptyEntries);
                if (infoSplit.Length == 2)
                {
                  messageVariantInfo.Add(infoSplit[0], infoSplit[1]);
                }
              }

              // OPTIONAL
              // Format: dataItem1=value1,dataItem2=value2,etc...
              List<KeyValuePair<string, string>> messageVariantData = new List<KeyValuePair<string, string>>();
              if (values.Length > 2)
              {
                string dataValues = values[2].ToLowerInvariant();
                foreach (string dataValue in dataValues.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                  string[] dataSplit = dataValue.Split(new[] {'='}, StringSplitOptions.RemoveEmptyEntries);
                  if (dataSplit.Length == 2)
                  {
                    messageVariantData.Add(new KeyValuePair<string, string>(dataSplit[0], dataSplit[1]));
                  }
                }
              }

              messageVariant = new MessageVariant(messageVariantData);
              messageVariant.Name = messageVariantInfo["name"];
              messageVariant.Tag = messageVariantInfo.ContainsKey("tag") ? messageVariantInfo["tag"] : messageVariantInfo["name"];
              messageVariant.TrackingId = String.Empty;
              messageVariant.Id = String.Empty;
              messageVariant.HasContent = true;
            }
          }
        }
      }

      return (messageVariant != null);
    }
  }
}
