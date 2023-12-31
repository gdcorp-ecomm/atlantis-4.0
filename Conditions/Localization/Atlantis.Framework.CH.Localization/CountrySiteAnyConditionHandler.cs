﻿using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Localization.Interface;
using System.Collections.Generic;

namespace Atlantis.Framework.CH.Localization
{
  public class CountrySiteAnyConditionHandler : IConditionHandler
  {
    public string ConditionName { get { return "countrySiteAny"; } }

    public bool EvaluateCondition(string conditionName, IList<string> parameters, IProviderContainer providerContainer)
    {
      bool result = false;
      ILocalizationProvider localizationProvider = providerContainer.Resolve<ILocalizationProvider>();

      foreach (string countrySiteValue in parameters)
      {
        if (localizationProvider.IsCountrySite(countrySiteValue))
        {
          result = true;
          break;
        }
      }

      return result;
    }
  }
}
