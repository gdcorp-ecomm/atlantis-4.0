using System;
using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Personalization.Interface;
using Atlantis.Framework.Providers.Personalization.Interface;

namespace Atlantis.Framework.CH.Personalization
{
  /// <summary>
  /// Example Condition: targetMessageTag(TagName, AppId, InteractionPoint)
  /// </summary>
  public class TargetedMessageConditionHandler : IConditionHandler
  {
    public string ConditionName { get { return "targetMessageTagAny"; } }

    public bool EvaluateCondition(string conditionName, IList<string> parameters, IProviderContainer providerContainer)
    {
      bool messageTagFound = false;

      if (parameters != null && parameters.Count > 1)
      {
        try
        {
          var personalizationProvider = providerContainer.Resolve<IPersonalizationProvider>();
          string interactionPoint = parameters[0];
          IEnumerable<string> messageTagNames = parameters.Skip(1);

          TargetedMessages targetedMessages = personalizationProvider.GetTargetedMessages(interactionPoint);

          if (targetedMessages == null) return false;

          foreach (var message in targetedMessages.Messages)
          {
            foreach (string messageTagName in messageTagNames)
            {
              if (message.MessageTags.Any(messageTag => string.Compare(messageTagName, messageTag.Name, StringComparison.OrdinalIgnoreCase) == 0))
              {
                messageTagFound = true;
                break;
              }
            }
            if (messageTagFound) break;
          }
        }
        catch (Exception ex)
        {
          throw new AtlantisException(null, "TargetedMessageConditionHandler.EvaulateCondition", ex.Message, String.Join(",", parameters));
        }

      }

      return messageTagFound;
    }
  }
}
