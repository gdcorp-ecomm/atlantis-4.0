using System.Collections.Generic;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Conditions.Tests.ConditionHandlers
{
  internal class TargetedMessageIdConditionHandler : IConditionHandler
  {
    private const string ACTUAL_VALUE = "1234";

    public string ConditionName { get { return "targetedMessageId"; } }

    public bool EvaluateCondition(string conditionName, IList<string> parameters, IProviderContainer providerContainer)
    {
      bool evaluationResult = false;

      foreach (string parameter in parameters)
      {
        if (parameter == ACTUAL_VALUE)
        {
          evaluationResult = true;
          break;
        }
      }

      return evaluationResult;
    }
  }
}
