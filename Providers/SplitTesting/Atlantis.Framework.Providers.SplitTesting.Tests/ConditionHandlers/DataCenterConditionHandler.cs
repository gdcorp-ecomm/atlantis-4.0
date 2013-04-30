using System.Collections.Generic;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Providers.SplitTesting.Tests.ConditionHandlers
{
  internal class DataCenterConditionHandler : IConditionHandler
  {
    private const string ACTUAL_VALUE = "AP";

    public string ConditionName { get { return "dataCenter"; } }

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
