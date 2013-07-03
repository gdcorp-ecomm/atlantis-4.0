using System;
using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Personalization.Interface;
using Atlantis.Framework.Providers.Personalization.Interface;

namespace Atlantis.Framework.CH.Personalization
{
  public class TargetedMessageAnyConditionHandler : IConditionHandler
  {
    public string ConditionName { get { return "targetMessageTagAny"; } }

    public bool EvaluateCondition(string conditionName, IList<string> parameters, IProviderContainer providerContainer)
    {
      bool messageTagFound = false;

      if (parameters != null && parameters.Count > 0)
      {
        var personalizationProvider = providerContainer.Resolve<IPersonalizationProvider>();
        TargetedMessages targetedMessages = personalizationProvider.GetTargetedMessages();

        foreach (var parameter in parameters)
        {
          foreach (var message in targetedMessages.Messages)
          {
            if (message.MessageTags.Any(messageTag => string.Compare(parameter, messageTag.Name, StringComparison.OrdinalIgnoreCase) == 0))
            {
              messageTagFound = true;
              break;
            }
          }
          if (messageTagFound) { break; }
        }
      }

      return messageTagFound;
    }
  }
}
