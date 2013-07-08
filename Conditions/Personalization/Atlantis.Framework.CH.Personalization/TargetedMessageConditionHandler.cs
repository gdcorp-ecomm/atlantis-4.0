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
    public string ConditionName { get { return "targetMessageTag"; } }

    public bool EvaluateCondition(string conditionName, IList<string> parameters, IProviderContainer providerContainer)
    {
      bool messageTagFound = false;

      if (parameters != null && parameters.Count == 3)
      {
        try
        {
          var personalizationProvider = providerContainer.Resolve<IPersonalizationProvider>();
          TargetedMessages targetedMessages = personalizationProvider.GetTargetedMessages(parameters[1], parameters[2]);

          if (targetedMessages == null) return false;

          foreach (var message in targetedMessages.Messages)
          {
            if (message.MessageTags.Any(messageTag => string.Compare(parameters[0], messageTag.Name, StringComparison.OrdinalIgnoreCase) == 0))
            {
              messageTagFound = true;
              break;
            }
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
