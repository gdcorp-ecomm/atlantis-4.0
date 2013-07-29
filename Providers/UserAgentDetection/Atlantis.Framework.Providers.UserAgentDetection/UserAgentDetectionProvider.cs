using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.UserAgentDetection.Interface;
using Atlantis.Framework.UserAgentEx.Interface;

namespace Atlantis.Framework.Providers.UserAgentDetection
{
  public class UserAgentDetectionProvider : ProviderBase, IUserAgentDetectionProvider
  {
    private const int SEARCH_ENGINE_BOT_EXPRESSION_TYPE = 10;
    private const int MOBILE_DEVICE_EXPRESSION_TYPE = 11;
    private const int NO_REDIRECT_BROWSER_EXPRESSION_TYPE = 12;
    private const int OUT_DATED_BROWSER_EXPRESSION_TYPE = 13;
    
    public UserAgentDetectionProvider(IProviderContainer container) : base(container)
    {
    }

    private bool IsUserAgentMatch(string userAgent, int expressionType, bool defaultValue)
    {
      bool isUserAgentMatch = defaultValue;

      if (!string.IsNullOrEmpty(userAgent))
      {
        UserAgentExRequestData requestData = new UserAgentExRequestData(expressionType);

        try
        {
          UserAgentExResponseData resonseData = (UserAgentExResponseData)DataCache.DataCache.GetProcessRequest(requestData, 528);
          isUserAgentMatch = resonseData.IsMatch(userAgent);
        }
        catch (Exception ex)
        {
          LogException(string.Format("Unable to retreive user agent regular expressions. Exception: {0} | Stack: {1}", ex.Message, ex.StackTrace),  
                       string.Format("User Agent: {0} | Expression Type: {1}", userAgent, expressionType), 
                       "UserAgentDetectionProvider.IsUserAgentMatch()");
        }
      }

      return isUserAgentMatch;
    }

    private void LogException(string message, string data, string sourceFunction)
    {
      try
      {
        Engine.Engine.LogAtlantisException(new AtlantisException(sourceFunction, "0", message, data, null, null));
      }
      catch
      {
      }
    }

    public bool IsMobileDevice(string userAgent)
    {
      return IsUserAgentMatch(userAgent, MOBILE_DEVICE_EXPRESSION_TYPE, false);
    }

    public bool IsNoRedirectBrowser(string userAgent)
    {
      return IsUserAgentMatch(userAgent, NO_REDIRECT_BROWSER_EXPRESSION_TYPE, true);
    }

    public bool IsOutDatedBrowser(string userAgent)
    {
      return IsUserAgentMatch(userAgent, OUT_DATED_BROWSER_EXPRESSION_TYPE, false);
    }

    public bool IsSearchEngineBot(string userAgent)
    {
      return IsUserAgentMatch(userAgent, SEARCH_ENGINE_BOT_EXPRESSION_TYPE, true);
    }
  }
}
