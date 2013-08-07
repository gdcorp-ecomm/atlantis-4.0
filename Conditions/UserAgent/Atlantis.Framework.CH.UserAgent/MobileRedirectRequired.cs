using System;
using System.Collections.Generic;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.MobileRedirect.Interface;

namespace Atlantis.Framework.CH.UserAgent
{
  public class MobileRedirectRequired : IConditionHandler
  {
    public string ConditionName
    {
      get { return "mobileRedirectRequired"; }
    }

    public bool EvaluateCondition(string conditionName, IList<string> parameters, IProviderContainer providerContainer)
    {
      IMobileRedirectProvider mobileRedirectProvider;
      if (!providerContainer.TryResolve(out mobileRedirectProvider))
      {
        throw new Exception("IMobileRedirectProvider is not registered.");
      }

      return mobileRedirectProvider.IsRedirectRequired();
    }
  }
}
