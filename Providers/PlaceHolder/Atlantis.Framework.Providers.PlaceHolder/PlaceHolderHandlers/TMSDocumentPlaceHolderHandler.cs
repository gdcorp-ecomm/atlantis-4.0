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
    private const string TMS_DATA_TOKEN_MESSAGE_NAME = "TMSMessageName";
    private const string TMS_DATA_TOKEN_MESSAGE_TAG = "TMSMessageTag";
    private const string TMS_DATA_TOKEN_MESSAGE_TRACKING_ID = "TMSMessageTrackingId";
    private const string LOCATION_FORMAT = "{0}/default_template";
    private const string DEFAULT_MESSAGE_NAME = "_default";
    private const string FAKE_DATA_QUERY_STRING = "QA--FakeShopperId";

    internal TMSDocumentPlaceHolderHandler(IPlaceHolderHandlerContext context)
      : base(context)
    {
    }

    protected override string GetContent()
    {
      string renderContent = string.Empty;
      ICDSContentProvider cdsContentProvider;
      IPersonalizationProvider personalizationProvider;

      if (Context.ProviderContainer.TryResolve(out cdsContentProvider) && Context.ProviderContainer.TryResolve(out personalizationProvider))
      {
        try
        {
          TMSPlaceHolderData placeHolderData = new TMSPlaceHolderData(Context.Data);

          string interactionPoint;
          if (placeHolderData.TryGetAttribute(PlaceHolderAttributes.InteractionPoint, out interactionPoint) &&
              !string.IsNullOrEmpty(interactionPoint) &&
              placeHolderData.MessageTags.Count > 0 &&
              !string.IsNullOrEmpty(placeHolderData.MessageTags[0])
            )
          {
            TMSMessageData msgData = GetMessage(personalizationProvider, interactionPoint, placeHolderData.MessageTags);
            UpdateContainerData(msgData);
            string rawContent = cdsContentProvider.GetContent(APP_NAME, string.Format(LOCATION_FORMAT, msgData.SelectedTagName)).Content;

            IRenderPipelineProvider renderPipelineProvider = Context.ProviderContainer.Resolve<IRenderPipelineProvider>();
            renderContent = renderPipelineProvider.RenderContent(rawContent, Context.RenderHandlers);
          }
          else
          {
            throw new Exception(string.Format("Attributes {0} and {1} are required", PlaceHolderAttributes.InteractionPoint, PlaceHolderAttributes.MessageTag));
          }
        }
        catch (Exception ex)
        {
          string errorMessage = string.Format("PlaceHolder render error. Type: {0}, Message: {1}", Context.Type, ex.Message);
          LogError(errorMessage, "TMSDocumentPlaceHolderHandler.Render()");
        }
      }

      return renderContent;
    }

    private TMSMessageData GetMessage(IPersonalizationProvider personalizationProvider, string interactionPoint, IList<string> messageTags)
    {
      TargetedMessages targetedMessages = null;
      ISiteContext siteContext;
      string showFakeDataForShopperId = null;
      if (Context.ProviderContainer.TryResolve(out siteContext) && siteContext.IsRequestInternal)
      {
        showFakeDataForShopperId = HttpContext.Current.Request.QueryString[FAKE_DATA_QUERY_STRING];
      }

      if (string.IsNullOrEmpty(showFakeDataForShopperId))
      {
        try
        {
          targetedMessages = personalizationProvider.GetTargetedMessages(interactionPoint);
        }
        catch (Exception ex)
        {
          string errorMessage = string.Format("Exception occurred when calling the TMS webservice. InteractionPoint: {0}. {1}", interactionPoint, ex.ToString());
          LogError(errorMessage, "TMSDocumentPlaceHolderHandler.GetLocation");
        }
      }
      else
      {
        targetedMessages = GetFakeData(showFakeDataForShopperId);
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
                   select new TMSMessageData(sourceMessage.MessageName, requestedTag, sourceMessage.MessageTrackingId)).FirstOrDefault();
      }

      if (msgData == null)
      {
        msgData = new TMSMessageData(DEFAULT_MESSAGE_NAME, messageTags[0], string.Empty);
      }
      
      return msgData;
    }

    private void UpdateContainerData(TMSMessageData msgData)
    {
      Context.ProviderContainer.SetData<string>(TMS_DATA_TOKEN_MESSAGE_TAG, msgData.SelectedTagName);
      Context.ProviderContainer.SetData<string>(TMS_DATA_TOKEN_MESSAGE_NAME, msgData.SelectedMessageName);
      Context.ProviderContainer.SetData<string>(TMS_DATA_TOKEN_MESSAGE_TRACKING_ID, msgData.SelectedTrackingId);
    }

    #region Fake Data

    private TargetedMessages GetFakeData(string shopperId)
    {
      List<Message> messages = null;

      if (shopperId == "912111")
      {
        messages = new List<Message>() { 
          new Message() { 
            MessageName = "name1", 
            MessageTags = new List<MessageTag>() { new MessageTag() { Name = "tag9" }, new MessageTag() { Name = "tag1" }, new MessageTag() { Name = "tag10" } } ,
            MessageTrackingId = "12345"
          },
          new Message() { 
            MessageName = "name2", 
            MessageTags = new List<MessageTag>() { new MessageTag() { Name = "tag11" }, new MessageTag() { Name = "tag12" }, new MessageTag() { Name = "tag2" } },
            MessageTrackingId = "23456"
          },
          new Message() { 
            MessageName = "name3", 
            MessageTags = new List<MessageTag>() { new MessageTag() { Name = "tag20" }, new MessageTag() { Name = "tag21" }, new MessageTag() { Name = "tag23" } },
            MessageTrackingId = "34567"
          }
        };
      }
      else
      {
        if (shopperId == "912117")
        {
          messages = new List<Message>() { 
          new Message() { 
            MessageName = "name1", 
            MessageTags = new List<MessageTag>() { new MessageTag() { Name = "tag9" }, new MessageTag() { Name = "tag8" }, new MessageTag() { Name = "tag10" } } ,
            MessageTrackingId = "12345"
          },
          new Message() { 
            MessageName = "name20", 
            MessageTags = new List<MessageTag>() { new MessageTag() { Name = "tag11" }, new MessageTag() { Name = "tag2" }, new MessageTag() { Name = "tag1" } } ,
            MessageTrackingId = "23456"
          },
          new Message() { 
            MessageName = "name3", 
            MessageTags = new List<MessageTag>() { new MessageTag() { Name = "tag20" }, new MessageTag() { Name = "tag8" }, new MessageTag() { Name = "tag1" } } ,
            MessageTrackingId = "34567"

          }
        };
        }
      }

      TargetedMessages targetedMessages = new TargetedMessages();
      targetedMessages.Messages = messages;

      return targetedMessages;
    }
    #endregion
  }
}
