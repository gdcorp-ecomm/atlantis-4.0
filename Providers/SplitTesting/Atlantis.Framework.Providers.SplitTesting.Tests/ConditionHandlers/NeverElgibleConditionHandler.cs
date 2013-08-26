using System.Collections.Generic;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Providers.SplitTesting.Tests.ConditionHandlers
{
  internal class NeverElgibleConditionHandler : IConditionHandler
  {

    public string ConditionName { get { return "neverElgible"; } }

    public bool EvaluateCondition(string conditionName, IList<string> parameters, IProviderContainer providerContainer)
    {
      return false;
    }
  }
}
