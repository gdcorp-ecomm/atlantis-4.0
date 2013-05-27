using System.Collections.Generic;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Providers.CDSContent.Tests.ConditionHandlers
{
  internal class CountrySiteContextConditionHandler : IConditionHandler
  {
    private const string ACTUAL_VALUE = "IN";

    public string ConditionName { get { return "countrySiteContext"; } }

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
