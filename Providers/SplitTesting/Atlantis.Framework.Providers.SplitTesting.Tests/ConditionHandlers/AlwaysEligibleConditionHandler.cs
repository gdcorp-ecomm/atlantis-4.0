using System.Collections.Generic;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Providers.SplitTesting.Tests.ConditionHandlers
{
  internal class AlwaysEligibleConditionHandler : IConditionHandler
  {

    public string ConditionName { get { return "alwaysEligible"; } }

    public bool EvaluateCondition(string conditionName, IList<string> parameters, IProviderContainer providerContainer)
    {
      return true;
    }
  }
}
