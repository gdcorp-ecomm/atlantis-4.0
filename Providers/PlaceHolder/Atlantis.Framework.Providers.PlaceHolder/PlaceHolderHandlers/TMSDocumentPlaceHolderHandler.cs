using Atlantis.Framework.Personalization.Interface;
using Atlantis.Framework.Providers.CDSContent.Interface;
using Atlantis.Framework.Providers.Personalization.Interface;
using Atlantis.Framework.Providers.PlaceHolder.Interface;
using Atlantis.Framework.Providers.PlaceHolder.PlaceHolders;
using Atlantis.Framework.Providers.RenderPipeline.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Atlantis.Framework.Providers.PlaceHolder.PlaceHolderHandlers
{
  internal class TMSDocumentPlaceHolderHandler : CDSDocumentPlaceHolderHandler
  {
    private const string APP_NAME = "tms";
    private const string LOCATION_FORMAT = "{0}/default_template";

    private readonly Lazy<bool> _canCallTMS = new Lazy<bool>(() => DataCache.DataCache.GetAppSetting("ATLANTIS_PERSONALIZATION_TRIPLET_TMS_ON").Equals("true", StringComparison.OrdinalIgnoreCase));

    internal TMSDocumentPlaceHolderHandler(IPlaceHolderHandlerContext context)
      : base(context)
    {
    }

    protected override string GetContent()
    {
      string renderContent = string.Empty;

      try
      {
        IPersonalizationProvider personalizationProvider;
        ICDSContentProvider cdsProvider;

        if (Context.ProviderContainer.TryResolve<IPersonalizationProvider>(out personalizationProvider) && Context.ProviderContainer.TryResolve<ICDSContentProvider>(out cdsProvider))
        {
          TMSPlaceHolderData placeHolderData = new TMSPlaceHolderData(Context.Data);

          string interactionPoint;
          if (placeHolderData.TryGetAttribute(PlaceHolderAttributes.InteractionPoint, out interactionPoint) &&
              !string.IsNullOrEmpty(interactionPoint) &&
              placeHolderData.MessageTags.Count > 0)
          {
            IConsumedMessage message = GetMessage(personalizationProvider, interactionPoint, placeHolderData.MessageTags);
            if (message != null)
            {
              personalizationProvider.AddToConsumedMessages(message);

              string rawContent = cdsProvider.GetContent(APP_NAME, string.Format(LOCATION_FORMAT, message.TagName)).Content;
              
              IRenderPipelineProvider renderPipelineProvider = Context.ProviderContainer.Resolve<IRenderPipelineProvider>();
              renderContent = renderPipelineProvider.RenderContent(rawContent, Context.RenderHandlers);
            }
            else
            {
              if (_canCallTMS.Value)
              {
                throw new Exception(string.Format("None of the requested tags are found.  Tags: \"{0}\"", string.Join(", ", placeHolderData.MessageTags)));
              }
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

    private IConsumedMessage GetMessage(IPersonalizationProvider personalizationProvider, string interactionPoint, IList<string> messageTags)
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

      return FindTheFirstMessageThatMatches(personalizationProvider, targetedMessages, messageTags);
    }

    private IConsumedMessage FindTheFirstMessageThatMatches(IPersonalizationProvider personalizationProvider, TargetedMessages targetedMessages, IList<string> messageTags)
    {
      IConsumedMessage message = null;

      if (targetedMessages != null && targetedMessages.Messages != null)
      {
      
        message = (from requestedTag in messageTags
                   from sourceMessage in targetedMessages.Messages
                   where sourceMessage.MessageTags != null
                   from sourceTag in sourceMessage.MessageTags
                   where string.Compare(sourceTag.Name, requestedTag, StringComparison.OrdinalIgnoreCase) == 0
                   select new ConsumedMessage(sourceMessage.MessageId, sourceMessage.MessageName, requestedTag, sourceMessage.MessageTrackingId) as IConsumedMessage
                  ).Except(personalizationProvider.ConsumedMessages).FirstOrDefault();
      }
      
      return message;
    }
  }
}
