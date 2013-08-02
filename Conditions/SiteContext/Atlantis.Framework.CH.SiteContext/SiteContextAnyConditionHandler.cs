using System;
using System.Collections.Generic;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CH.SiteContext
{
  public class SiteContextAnyConditionHandler : IConditionHandler
  {
    public string ConditionName
    {
      get
      {
        return "siteContextAny";
      }
    }

    public bool EvaluateCondition(string conditionName, IList<string> parameters, IProviderContainer providerContainer)
    {
      bool result = false;

      if (parameters.Count == 0)
      {
        throw new ArgumentException("siteContextAny condition was called without arguments");
      }
      else
      {
        var siteContext = providerContainer.Resolve<ISiteContext>();
      
        foreach (var parameter in parameters)
        {
          switch (parameter.ToLowerInvariant())
          {
            case "gd":
              result = this.IsMatchedSiteContext(ContextIds.GoDaddy, siteContext);
              break;
            case "br":
              result = this.IsMatchedSiteContext(ContextIds.BlueRazor, siteContext);
              break;
            case "wwd":
              result = this.IsMatchedSiteContext(ContextIds.WildWestDomains, siteContext);
              break;
            default:
              result = parameter.ToLowerInvariant() == "pl" && this.IsMatchedSiteContext(ContextIds.Reseller, siteContext);
              break;
          }

          if (result)
          {
            break;
          }
        }
      }
      return result;
    }

    private bool IsMatchedSiteContext(int contextId, ISiteContext siteContext)
    {
      return siteContext.ContextId == contextId;
    }
  }
}