using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Atlantis.Framework.Web.IeEdge
{
  public class IeEdge : HtmlControl
  {
    private const string EDGE_META_TAG = "<meta http-equiv=\"X-UA-Compatible\" content=\"IE=Edge\" />";
    private const string ENABLE_APPLICATION_SETTING = "ATLANTIS_WEB_IEEDGE_ENABLED";

    private static readonly Regex _ieUserAgentRegex = new Regex(@".*MSIE\s[0-9].*", RegexOptions.Compiled);

    private static bool IsEdgeMetaTagEnabled
    {
      get
      {
        bool enabled = false;

        string appSettingValue = HttpContext.Current != null ? HttpContext.Current.Request[("QA--" + ENABLE_APPLICATION_SETTING)] : string.Empty;

        if (string.IsNullOrEmpty(appSettingValue))
        {
          appSettingValue = DataCache.DataCache.GetAppSetting(ENABLE_APPLICATION_SETTING);
        }
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
        return HttpContext.Current != null &&
               !string.IsNullOrEmpty(HttpContext.Current.Request.UserAgent) &&
               _ieUserAgentRegex.IsMatch(HttpContext.Current.Request.UserAgent);
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

    protected override void Render(HtmlTextWriter writer)
    {
      if (IsEdgeMetaTagEnabled && IsInternetExplorerUserAgent)
      {
        writer.Write(EDGE_META_TAG);
      }
    }
  }
}
