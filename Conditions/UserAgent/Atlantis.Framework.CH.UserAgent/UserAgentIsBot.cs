using System;
using System.Collections.Generic;
using System.Web;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.UserAgentDetection.Interface;

namespace Atlantis.Framework.CH.UserAgent
{
  public class UserAgentIsBot : IConditionHandler
  {
    public string ConditionName
    {
      get { return "userAgentIsBot";  }
    }

    public bool EvaluateCondition(string conditionName, IList<string> parameters, IProviderContainer providerContainer)
    {
      IUserAgentDetectionProvider userAgentDetectionProvider;
      if (!providerContainer.TryResolve(out userAgentDetectionProvider))
      {
        throw new Exception("IUserAgentDetectionProvider is not registered.");
      }

      bool isUserAgentBot = false;
      
      if (HttpContext.Current != null)
      {
        isUserAgentBot = userAgentDetectionProvider.IsSearchEngineBot(HttpContext.Current.Request.UserAgent);
      }

      return isUserAgentBot;
    }
  }
}
