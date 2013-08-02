using System;
using System.Collections.Generic;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Localization.Interface;

namespace Atlantis.Framework.CH.Localization
{
  public class CountrySiteAnyConditionHandler : IConditionHandler
  {
    public string ConditionName { get { return "countrySiteAny"; } }

    public bool EvaluateCondition(string conditionName, IList<string> parameters, IProviderContainer providerContainer)
    {
      bool result = false;

      ILocalizationProvider localizationProvider = providerContainer.Resolve<ILocalizationProvider>();

      string currentCountrySiteValue = localizationProvider.CountrySite;

      foreach (string countrySiteValue in parameters)
      {
        if (currentCountrySiteValue.Equals(countrySiteValue, StringComparison.OrdinalIgnoreCase))
        {
          result = true;
          break;
        }
      }

      return result;
    }
  }
}
