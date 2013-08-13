using System;
using System.Collections.Generic;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DataCenter.Interface;

namespace Atlantis.Framework.CH.DataCenter
{
  public class DataCenterAny : IConditionHandler
  {
    public string ConditionName { get { return "dataCenterAny"; } }

    public bool EvaluateCondition(string conditionName, IList<string> parameters, IProviderContainer providerContainer)
    {
      bool result = false;

      IDataCenterProvider dataCenterProvider = providerContainer.Resolve<IDataCenterProvider>();

      string dataCenterPreferenceValue = dataCenterProvider.DataCenterCode;

      foreach (string dataCenter in parameters)
      {
        if (dataCenterPreferenceValue.Equals(dataCenter, StringComparison.OrdinalIgnoreCase))
        {
          result = true;
          break;
        }
      }

      return result;
    }
  }
}
