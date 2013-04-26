using System;
using System.Collections.Generic;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Providers.CDS.Tests.ConditionHandlers
{
  public class CountrySiteAnyUnitTestConditionHandler : IConditionHandler
  {
    public string ConditionName { get { return "countrySiteAny"; } }

    public bool EvaluateCondition(string conditionName, IList<string> parameters, IProviderContainer providerContainer)
    {
      bool result = false;

      foreach (string parameter in parameters)
      {
        if (parameter.Equals("in", StringComparison.OrdinalIgnoreCase))
        {
          result = true;
          break;
        }
      }

      return result;
    }
  }
}
