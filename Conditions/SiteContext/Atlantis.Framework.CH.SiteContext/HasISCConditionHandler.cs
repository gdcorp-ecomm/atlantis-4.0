using System.Collections.Generic;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CH.SiteContext
{
  public class HasISCConditionHandler : IConditionHandler
  {
    public string ConditionName
    {
      get { return "hasISC"; }
    }

    public bool EvaluateCondition(string conditionName, IList<string> parameters, IProviderContainer providerContainer)
    {
      var siteContext = providerContainer.Resolve<ISiteContext>();

      return !(string.IsNullOrEmpty(siteContext.ISC));
    }
  }
}
