using System.Collections.Generic;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CH.SiteContext
{
  public class RequestIsInternalConditionHandler : IConditionHandler
  {
    #region Implementation of IConditionHandler
    
    public string ConditionName { get { return "requestIsInternal"; } }

    public bool EvaluateCondition(string conditionName, IList<string> parameters, IProviderContainer providerContainer)
    {
      var siteContext = providerContainer.Resolve<ISiteContext>();

      return siteContext.IsRequestInternal;

    }

    #endregion
  }
}
