using System.Collections.Generic;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.MobileContext.Interface;

namespace Atlantis.Framework.CH.MobileContext
{
  public class MobileApplicationTypeIsAppConditionHandler : IConditionHandler
  {
    public string ConditionName
    {
      get
      {
        return "mobileApplicationTypeIsApp";
      }
    }

    public bool EvaluateCondition(string conditionName, IList<string> parameters, IProviderContainer providerContainer)
    {
      var mobileContext = providerContainer.Resolve<IMobileContextProvider>();

      return mobileContext.MobileApplicationType != MobileApplicationType.None;
    }
  }
}
