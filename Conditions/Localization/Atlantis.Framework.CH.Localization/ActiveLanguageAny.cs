using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Providers.Localization.Interface;
using System.Collections.Generic;

namespace Atlantis.Framework.CH.Localization
{
  public class ActiveLanguageAny : IConditionHandler
  {
    public string ConditionName
    {
      get { return "activeLanguageAny"; }

    }

    public bool EvaluateCondition(string conditionName, IList<string> parameters, Interface.IProviderContainer providerContainer)
    {
      bool result = false;
      ILocalizationProvider localizationProvider = providerContainer.Resolve<ILocalizationProvider>();

      foreach (string language in parameters)
      {
        if (localizationProvider.IsActiveLanguage(language))
        {
          result = true;
          break;
        }
      }

      return result;
    }
  }
}
