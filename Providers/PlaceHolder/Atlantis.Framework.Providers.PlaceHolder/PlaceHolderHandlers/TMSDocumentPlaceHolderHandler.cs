using Atlantis.Framework.Providers.CDSContent.Interface;
using Atlantis.Framework.Providers.Personalization;
using Atlantis.Framework.Providers.PlaceHolder.Interface;
using Atlantis.Framework.Providers.RenderPipeline.Interface;
using System;
using System.Linq;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using System.Web;
using Atlantis.Framework.Providers.Personalization.Interface;
using Atlantis.Framework.Personalization.Interface;
using Atlantis.Framework.Providers.PlaceHolder.PlaceHolders;

namespace Atlantis.Framework.Providers.PlaceHolder.PlaceHolderHandlers
{
  internal class TMSDocumentPlaceHolderHandler : CDSDocumentPlaceHolderHandler
  {
    private const string APP_NAME = "tms";
    private const string LOCATION_FORMAT = "{0}/default_template";

    internal TMSDocumentPlaceHolderHandler(IPlaceHolderHandlerContext context)
      : base(context)
    {
    }

    protected override string GetContent()
    {
      string renderContent = string.Empty;
      ICDSContentProvider cdsContentProvider;
      IPersonalizationProvider personalizationProvider;

      try
      {
        if (Context.ProviderContainer.TryResolve(out cdsContentProvider) && Context.ProviderContainer.TryResolve(out personalizationProvider))
        {

          TMSPlaceHolderData placeHolderData = new TMSPlaceHolderData(Context.Data);

          string interactionPoint;
          if (placeHolderData.TryGetAttribute(PlaceHolderAttributes.InteractionPoint, out interactionPoint) &&
              !string.IsNullOrEmpty(interactionPoint) &&
              placeHolderData.MessageTags.Count > 0)
          {
            TMSMessageData msgData = GetMessage(personalizationProvider, interactionPoint, placeHolderData.MessageTags);
            if (msgData != null)
            {
              UpdateContainerData(msgData);
              string rawContent = cdsContentProvider.GetContent(APP_NAME, string.Format(LOCATION_FORMAT, msgData.TagName)).Content;

              IRenderPipelineProvider renderPipelineProvider = Context.ProviderContainer.Resolve<IRenderPipelineProvider>();
              renderContent = renderPipelineProvider.RenderContent(rawContent, Context.RenderHandlers);
            }
            else
            {
              throw new Exception(string.Format("None of the requested tags are found.  Tags: \"{0}\"", string.Join(", ", placeHolderData.MessageTags)));
            }
          }
          else
          {
            throw new Exception(string.Format("Attributes {0} is required and at least one message tag is required", PlaceHolderAttributes.InteractionPoint));
          }
        }
        else
        {
          throw new Exception("Could not resolve the required providers.  CDSContent and Personalization are required.");
        }
      }
      catch (Exception ex)
      {
        string errorMessage = string.Format("PlaceHolder render error. Type: {0}, Message: {1}", Context.Type, ex.Message);
        LogError(errorMessage, "TMSDocumentPlaceHolderHandler.Render()");
      }
            
      return renderContent;
    }

    private TMSMessageData GetMessage(IPersonalizationProvider personalizationProvider, string interactionPoint, IList<string> messageTags)
    {
      TargetedMessages targetedMessages = null;

      try
      {
        targetedMessages = personalizationProvider.GetTargetedMessages(interactionPoint);
      }
      catch (Exception ex)
      {
        string errorMessage = string.Format("Exception occurred when calling the TMS webservice. InteractionPoint: {0}.", interactionPoint);
        throw new Exception(errorMessage, ex);
      }

      return FindTheFirstMessageThatMatches(targetedMessages, messageTags);
    }

    private TMSMessageData FindTheFirstMessageThatMatches(TargetedMessages targetedMessages, IList<string> messageTags)
    {
      TMSMessageData msgData = null;

      if (targetedMessages != null && targetedMessages.Messages != null)
      {
        msgData = (from sourceMessage in targetedMessages.Messages
                   where sourceMessage.MessageTags != null
                   from sourceTag in sourceMessage.MessageTags
                   from requestedTag in messageTags
                   where string.Compare(sourceTag.Name, requestedTag, StringComparison.OrdinalIgnoreCase) == 0
                   select new TMSMessageData(sourceMessage.MessageId, sourceMessage.MessageName, requestedTag, sourceMessage.MessageTrackingId)).FirstOrDefault();
      }
      
      return msgData;
    }

    private void UpdateContainerData(TMSMessageData msgData)
    {
      Context.ProviderContainer.SetData<string>(TMSMessageData.DATA_TOKEN_MESSAGE_Id, msgData.MessageId);
      Context.ProviderContainer.SetData<string>(TMSMessageData.DATA_TOKEN_MESSAGE_TAG, msgData.TagName);
      Context.ProviderContainer.SetData<string>(TMSMessageData.DATA_TOKEN_MESSAGE_NAME, msgData.MessageName);
      Context.ProviderContainer.SetData<string>(TMSMessageData.DATA_TOKEN_MESSAGE_TRACKING_ID, msgData.TrackingId);

      var list = Context.ProviderContainer.GetData<List<TMSMessageData>>(TMSMessageData.DATA_TOKEN_MESSAGES, new List<TMSMessageData>());
      list.Add(msgData);
      Context.ProviderContainer.SetData<List<TMSMessageData>>(TMSMessageData.DATA_TOKEN_MESSAGES, list);
    }

  }
}
