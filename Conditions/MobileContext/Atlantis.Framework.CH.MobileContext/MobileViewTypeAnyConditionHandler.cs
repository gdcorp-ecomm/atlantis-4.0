using System;
using System.Collections.Generic;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.MobileContext.Interface;

namespace Atlantis.Framework.CH.MobileContext
{
  public class MobileViewTypeAnyConditionHandler : IConditionHandler
  {
    public string ConditionName
    {
      get
      {
        return "mobileViewTypeAny";
      }
    }

    public bool EvaluateCondition(string conditionName, IList<string> parameters, IProviderContainer providerContainer)
    {
      bool result = false;

      if (parameters.Count == 0)
      {
        throw new ArgumentException("mobileViewTypeAny condition was called without arguments");
      }
      var mobileContext = providerContainer.Resolve<IMobileContextProvider>();
      MobileViewType contextViewType = mobileContext.MobileViewType;

      foreach (var parameter in parameters)
      {
        switch (parameter.ToLowerInvariant())
        {
          case "webkit":
            result = contextViewType == MobileViewType.Webkit;
            break;
          case "firefox":
            result = contextViewType == MobileViewType.FireFox;
            break;
          case "opera":
            result = contextViewType == MobileViewType.Opera;
            break;
          case "default":
            result = contextViewType == MobileViewType.Default;
            break;
          case "apple":
            result = contextViewType == MobileViewType.Apple;
            break;
          case "android":
            result = contextViewType == MobileViewType.Android;
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