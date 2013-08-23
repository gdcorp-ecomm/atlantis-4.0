using System.Collections.Generic;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CH.ManagerContext
{
  public class ManagerContextConditionHandler : IConditionHandler
  {
    public string ConditionName
    {
      get
      {
        return "isManager";
      }
    }

    public bool EvaluateCondition(string conditionName, IList<string> parameters, Interface.IProviderContainer providerContainer)
    {
      var managerContext = providerContainer.Resolve<IManagerContext>();
      return managerContext.IsManager;
    }
  }
}
