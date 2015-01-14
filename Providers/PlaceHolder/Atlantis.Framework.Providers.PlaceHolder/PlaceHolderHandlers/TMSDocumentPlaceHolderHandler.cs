using Atlantis.Framework.Personalization.Interface;
using Atlantis.Framework.Providers.AppSettings.Interface;
using Atlantis.Framework.Providers.CDSContent.Interface;
using Atlantis.Framework.Providers.Personalization;
using Atlantis.Framework.Providers.Personalization.Interface;
using Atlantis.Framework.Providers.PlaceHolder.Interface;
using Atlantis.Framework.Providers.PlaceHolder.PlaceHolders;
using System;
using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.Providers.RenderPipeline.Interface;
using Atlantis.Framework.Render.Containers;

namespace Atlantis.Framework.Providers.PlaceHolder.PlaceHolderHandlers
{
  internal class TMSDocumentPlaceHolderHandler : CDSDocumentPlaceHolderHandler
  {
    private const string APP_NAME = "tms";
    private const string LOCATION_FORMAT = "{0}/default_template";
    private const string EnablementAppSetting = "ATLANTIS_PERSONALIZATION_TRIPLET_TMS_ON";

    internal TMSDocumentPlaceHolderHandler(IPlaceHolderHandlerContext context) : base(context) { }

    protected override string GetContent()
    {
      string renderContent = string.Empty;

      try
      {
        IPersonalizationProvider personalizationProvider;
        ICDSContentProvider cdsProvider;

        if (Context.ProviderContainer.TryResolve(out personalizationProvider) && Context.ProviderContainer.TryResolve(out cdsProvider))
        {
          TMSPlaceHolderData placeHolderData = new TMSPlaceHolderData(Context.Data);

          string interactionPoint;
          if (placeHolderData.TryGetAttribute(PlaceHolderAttributes.TMS_Interaction, out interactionPoint) &&
              !string.IsNullOrEmpty(interactionPoint) && (placeHolderData.MessageTags.Count > 0))
          {
            IConsumedMessage message = GetMessage(interactionPoint, placeHolderData.MessageTags, personalizationProvider);
            
            if (message != null)
            {
              personalizationProvider.AddConsumedMessage(message);

              renderContent = cdsProvider.GetContent(APP_NAME, string.Format(LOCATION_FORMAT, message.TagName)).Content;
              
              IRenderPipelineProvider renderPipelineProvider = Context.ProviderContainer.Resolve<IRenderPipelineProvider>();
              renderContent = renderPipelineProvider.RenderContent(renderContent, new[] { new ProviderContainerDataTokenRenderHandler() });
            }
            else
            {
              bool isTmsOn = true;
              IAppSettingsProvider appSettingsProvider;

              if (Context.ProviderContainer.TryResolve(out appSettingsProvider))
              {
                string enablementSetting = appSettingsProvider.GetAppSetting(EnablementAppSetting);
                isTmsOn = (!String.IsNullOrWhiteSpace(enablementSetting) && enablementSetting.Equals("true", StringComparison.OrdinalIgnoreCase));
              }

              if (isTmsOn)
              {
                throw new Exception(string.Format("None of the requested tags are found.  Tags: \"{0}\"", string.Join(", ", placeHolderData.MessageTags)));
              }
            }
          }
          else
          {
            throw new Exception(string.Format("Attributes {0} is required and at least one message tag is required", PlaceHolderAttributes.TMS_Interaction));
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

    private IConsumedMessage GetMessage(string interactionPoint, IList<string> placeHolderMessageTags, IPersonalizationProvider personalizationProvider)
    {
      TargetedMessages targetedMessages;

      try
      {
        targetedMessages = personalizationProvider.GetTargetedMessages(interactionPoint);
      }
      catch (Exception ex)
      {
        string errorMessage = string.Format("Exception occurred when calling the TMS webservice. InteractionPoint: {0}.", interactionPoint);
        throw new Exception(errorMessage, ex);
      }

      return FindMessageMatch(targetedMessages, placeHolderMessageTags, personalizationProvider);
    }

    private IConsumedMessage FindMessageMatch(TargetedMessages targetedMessages, IList<string> placeHolderMessageTags, IPersonalizationProvider personalizationProvider)
    {
      IConsumedMessage message = null;

      if (targetedMessages != null && targetedMessages.Messages != null)
      {
        foreach (Message targetedMessage in targetedMessages.Messages)
        {
          if (IsMessageValid(targetedMessage, personalizationProvider))
          {
            if (TryGetMatch(targetedMessage, placeHolderMessageTags, out message))
            {
              break;
            }
          }
        }
      }
      
      return message;
    }

    private bool IsMessageValid(Message targetedMessage, IPersonalizationProvider personalizationProvider)
    {
      bool isMessageValid = (targetedMessage != null) && targetedMessage.HasMessageTags();

      if (isMessageValid)
      {
        foreach (var consumedMessage in personalizationProvider.GetConsumedMessages())
        {
          if (consumedMessage.MessageId.Equals(targetedMessage.MessageId, StringComparison.InvariantCultureIgnoreCase))
          {
            isMessageValid = false;
            break;
          }
        }
      }

      return isMessageValid;
    }

    private bool TryGetMatch(Message targetedMessage, IList<string> placeHolderMessageTags, out IConsumedMessage consumedMessage)
    {
      bool match = false;
      consumedMessage = null;

      foreach (MessageTag messageTag in targetedMessage.MessageTags)
      {
        if (placeHolderMessageTags.Any(placeHolderMessageTag => messageTag.Name.Equals(placeHolderMessageTag, StringComparison.OrdinalIgnoreCase)))
        {
          match = true;
          consumedMessage = new ConsumedMessage(targetedMessage.MessageId, targetedMessage.MessageName, messageTag.Name, targetedMessage.MessageTrackingId);
          break;
        }
      }

      return match;
    }
  }
}
