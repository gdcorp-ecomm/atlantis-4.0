using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Atlantis.Framework.Web.IeEdge
{
  public class IeEdge : HtmlControl
  {
    private const string EDGE_META_TAG = "<meta http-equiv=\"X-UA-Compatible\" content=\"IE-Edge\" />";
    private const string ENABLE_APPLICATION_SETTING = "ATLANTIS_WEB_IEEDGE_ENABLED";
    private const string IS_IE_USER_AGENT_COOKIE_NAME = "atlantis.web.ieedge.isieuseragent";

    private static readonly Regex _ieUserAgentRegex = new Regex(@".*MSIE\s[0-9].*", RegexOptions.Compiled);

    private static bool IsEdgeMetaTagEnabled
    {
      get
      {
        bool enabled = false;

        string appSettingValue = DataCache.DataCache.GetAppSetting(ENABLE_APPLICATION_SETTING);
        if(!string.IsNullOrEmpty(appSettingValue))
        {
          enabled = appSettingValue == "1" || appSettingValue.ToLowerInvariant() == "true";
        }

        return enabled;
      }
    }

    private static bool IsInternetExplorerUserAgent
    {
      get
      {
        bool isIeUserAgent;
        bool cookieExists = GetIsIeUserAgentCookie(out isIeUserAgent);
        
        if(!cookieExists)
        {
          isIeUserAgent = HttpContext.Current != null &&
                          !string.IsNullOrEmpty(HttpContext.Current.Request.UserAgent) &&
                          _ieUserAgentRegex.IsMatch(HttpContext.Current.Request.UserAgent);

          SetIsIeUserAgentCookie(isIeUserAgent);
        }

        return isIeUserAgent;
      }
    }

    private static string CrossDomainCookieDomain
    {
      get
      {
        string hostName = HttpContext.Current.Request.Url.Host;

        if (hostName.Contains("."))
        {
          string[] parts = hostName.Split('.');
          if (parts.Length > 2)
          {
            hostName = parts[parts.Length - 2] + "." + parts[parts.Length - 1];
          }
        }

        return hostName;
      }
    }

    private static HttpCookie CreateCrossDomainSessionCookie(string cookieName)
    {
      HttpCookie result = new HttpCookie(cookieName);
      result.Path = "/";
      result.Domain = CrossDomainCookieDomain;
      return result;
    }

    private static bool GetIsIeUserAgentCookie(out bool isIeUserAgent)
    {
      bool cookieExists = false;
      isIeUserAgent = false;

      if (HttpContext.Current != null)
      {
        HttpCookie isIeUserAgentCookie = HttpContext.Current.Request.Cookies[IS_IE_USER_AGENT_COOKIE_NAME];
        if (isIeUserAgentCookie != null)
        {
          cookieExists = true;
          isIeUserAgent = isIeUserAgentCookie.Value == "1";
        }
      }

      return cookieExists;
    }

    private static void SetIsIeUserAgentCookie(bool value)
    {
      if (HttpContext.Current != null)
      {
        HttpCookie isIeUserAgentCookie = HttpContext.Current.Request.Cookies[IS_IE_USER_AGENT_COOKIE_NAME];
        if (isIeUserAgentCookie == null)
        {
          isIeUserAgentCookie = CreateCrossDomainSessionCookie(IS_IE_USER_AGENT_COOKIE_NAME);
        }

        isIeUserAgentCookie.Value = value ? "1" : "0";

        bool cookieExistsInResponse = HttpContext.Current.Response.Cookies[IS_IE_USER_AGENT_COOKIE_NAME] != null;

        if (cookieExistsInResponse)
        {
          HttpContext.Current.Response.Cookies.Set(isIeUserAgentCookie);
        }
        else
        {
          HttpContext.Current.Response.Cookies.Add(isIeUserAgentCookie);
        }
      }
    }

    protected override void Render(HtmlTextWriter writer)
    {
      if (IsEdgeMetaTagEnabled && IsInternetExplorerUserAgent)
      {
        writer.Write(EDGE_META_TAG);
      }
    }
  }
}
