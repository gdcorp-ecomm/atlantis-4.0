using System;
using System.Collections.Generic;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.MobileContext.Interface;

namespace Atlantis.Framework.CH.MobileContext
{
  public class MobileDeviceTypeAnyConditionHandler : IConditionHandler
  {
    public string ConditionName
    {
      get
      {
        return "mobileDeviceTypeAny";
      }
    }

    public bool EvaluateCondition(string conditionName, IList<string> parameters, IProviderContainer providerContainer)
    {
      bool result = false;

      if (parameters.Count == 0)
      {
        throw new ArgumentException("mobileDeviceTypeAny condition was called without arguments");
      }
      var mobileContext = providerContainer.Resolve<IMobileContextProvider>();
      MobileDeviceType contextDeviceType = mobileContext.MobileDeviceType;

      foreach (var parameter in parameters)
      {
        switch (parameter.ToLowerInvariant())
        {
          case "iphone":
            result = contextDeviceType == MobileDeviceType.iPhone;
            break;
          case "ipad":
            result = contextDeviceType == MobileDeviceType.iPad;
            break;
          case "android":
            result = contextDeviceType == MobileDeviceType.Android;
            break;
          case "unknown":
            result = contextDeviceType == MobileDeviceType.Unknown;
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