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
      bool returnValue = false;

      if (parameters != null && parameters.Count > 0)
      {
        var personalizationProvider = providerContainer.Resolve<IPersonalizationProvider>();
        TargetedMessages targetedMessages = personalizationProvider.GetTargetedMessages();

        foreach (var parameter in parameters)
        {
          if (returnValue) { break; }

          foreach (var message in targetedMessages.Messages)
          {
            if (returnValue) { break; }

            if (message.MessageTags.Any(messageTag => parameter == messageTag.Name))
            {
              returnValue = true;
            }
          }
        }
      }

      return returnValue;
    }
  }
}
