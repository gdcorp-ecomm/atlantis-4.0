using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Providers.Localization.Interface;
using System.Collections.Generic;

namespace Atlantis.Framework.CH.Localization
{
  public class IsGlobalSite : IConditionHandler
  {
    public string ConditionName
    {
      get { return "isGlobalSite"; }
    }

    public bool EvaluateCondition(string conditionName, IList<string> parameters, Interface.IProviderContainer providerContainer)
    {
      ILocalizationProvider localizationProvider = providerContainer.Resolve<ILocalizationProvider>();
      return localizationProvider.IsGlobalSite();
    }
  }
}
