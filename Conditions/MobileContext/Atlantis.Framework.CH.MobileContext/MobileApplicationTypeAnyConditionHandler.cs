using System;
using System.Collections.Generic;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.MobileContext.Interface;

namespace Atlantis.Framework.CH.MobileContext
{
  public class MobileApplicationTypeAnyConditionHandler : IConditionHandler
  {
    public string ConditionName
    {
      get
      {
        return "mobileApplicationTypeAny";
      }
    }

    public bool EvaluateCondition(string conditionName, IList<string> parameters, IProviderContainer providerContainer)
    {
      bool result = false;

      if (parameters.Count == 0)
      {
        throw new ArgumentException("mobileApplicationTypeAny condition was called without arguments");
      }
      var mobileContext = providerContainer.Resolve<IMobileContextProvider>();
      MobileApplicationType contextApplicationType = mobileContext.MobileApplicationType;

      foreach (var parameter in parameters)
      {
        switch (parameter.ToLowerInvariant())
        {
          case "none":
            result = contextApplicationType == MobileApplicationType.None;
            break;
          case "iphone":
            result = contextApplicationType == MobileApplicationType.iPhone;
            break;
          case "ipad":
            result = contextApplicationType == MobileApplicationType.iPad;
            break;
          case "android":
            result = contextApplicationType == MobileApplicationType.Android;
            break;
        }

        if (result)
        {
          break;
        }
      }
      return result;
    }
  }
}