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

namespace Atlantis.Framework.Providers.PlaceHolder.PlaceHolderHandlers
{
  internal class TMSDocumentPlaceHolderHandler : CDSDocumentPlaceHolderHandler
  {
    private const string TMS_DATA_TOKEN_MESSAGE_NAME = "TMSMessageName";
    private const string TMS_DATA_TOKEN_MESSAGE_TAG = "TMSMessageTag";
    private const string LOCATION_FORMAT = "tms/{0}";
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
          PlaceHolderData placeHolderData = new PlaceHolderData(Context.Data);

          string app;
          string interactionPoint;
          string messageTag;
          string tmsAppId;
          if (placeHolderData.TryGetAttribute(PlaceHolderAttributes.Application, out app) &&
              placeHolderData.TryGetAttribute(PlaceHolderAttributes.TMSAppId, out tmsAppId) &&
              placeHolderData.TryGetAttribute(PlaceHolderAttributes.InteractionPoint, out interactionPoint) &&
              placeHolderData.TryGetAttribute(PlaceHolderAttributes.MessageTag, out messageTag) &&
              !string.IsNullOrEmpty(app) &&
              !string.IsNullOrEmpty(tmsAppId) &&
              !string.IsNullOrEmpty(interactionPoint) &&
              !string.IsNullOrEmpty(messageTag)
            )
          {
            string rawContent = cdsContentProvider.GetContent(app, GetLocation(personalizationProvider, tmsAppId, interactionPoint, messageTag)).Content;

            IRenderPipelineProvider renderPipelineProvider = Context.ProviderContainer.Resolve<IRenderPipelineProvider>();
            renderContent = renderPipelineProvider.RenderContent(rawContent, Context.RenderHandlers);
          }
          else
          {
            throw new Exception(string.Format("Attributes {0}, {1}, {2} and {3} are required", PlaceHolderAttributes.Application, PlaceHolderAttributes.TMSAppId, PlaceHolderAttributes.InteractionPoint, PlaceHolderAttributes.MessageTag));
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

    private string GetLocation(IPersonalizationProvider personalizationProvider, string tmsAppId, string interactionPoint, string messageTag)
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
          targetedMessages = personalizationProvider.GetTargetedMessages(tmsAppId, interactionPoint);
        }
        catch (Exception)
        {
          string errorMessage = string.Format("Exception occurred when calling the TMS webservice. TMSAppId: {0}, InteractionPoint: {1}", tmsAppId, interactionPoint);
          LogError(errorMessage, "TMSDocumentPlaceHolderHandler.GetLocation");
        }
      }
      else
      {
        targetedMessages = GetFakeData(showFakeDataForShopperId);
      }
      string messageName = GetMessageName(targetedMessages, messageTag);
      UpdateDataContainer(messageTag, messageName);
      return string.Format(LOCATION_FORMAT, messageName);
    }

    private string GetMessageName(TargetedMessages targetedMessages, string messageTag)
    {
      string messageName = null;

      if (targetedMessages != null && targetedMessages.Messages != null)
      {
        var firstMessage = (from msg in targetedMessages.Messages
                            where msg.MessageTags != null
                            from msgTag
                              in msg.MessageTags
                            where msgTag.Name == messageTag
                            select msg).FirstOrDefault();

        if (firstMessage != null)
        {
          messageName = firstMessage.MessageName;
        }
      }

      return messageName ?? DEFAULT_MESSAGE_NAME;
    }

    private void UpdateDataContainer(string messageTag, string messageName)
    {
      Context.ProviderContainer.SetData<string>(TMS_DATA_TOKEN_MESSAGE_NAME, messageName);
      Context.ProviderContainer.SetData<string>(TMS_DATA_TOKEN_MESSAGE_TAG, messageTag);
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
            MessageTags = new List<MessageTag>() { new MessageTag() { Name = "tag9" }, new MessageTag() { Name = "tag1" }, new MessageTag() { Name = "tag10" } } 
          },
          new Message() { 
            MessageName = "name2", 
            MessageTags = new List<MessageTag>() { new MessageTag() { Name = "tag11" }, new MessageTag() { Name = "tag12" }, new MessageTag() { Name = "tag2" } } 
          },
          new Message() { 
            MessageName = "name3", 
            MessageTags = new List<MessageTag>() { new MessageTag() { Name = "tag20" }, new MessageTag() { Name = "tag21" }, new MessageTag() { Name = "tag23" } } 
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
            MessageTags = new List<MessageTag>() { new MessageTag() { Name = "tag9" }, new MessageTag() { Name = "tag8" }, new MessageTag() { Name = "tag10" } } 
          },
          new Message() { 
            MessageName = "name20", 
            MessageTags = new List<MessageTag>() { new MessageTag() { Name = "tag11" }, new MessageTag() { Name = "tag2" }, new MessageTag() { Name = "tag13" } } 
          },
          new Message() { 
            MessageName = "name3", 
            MessageTags = new List<MessageTag>() { new MessageTag() { Name = "tag20" }, new MessageTag() { Name = "tag21" }, new MessageTag() { Name = "tag1" } } 
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
