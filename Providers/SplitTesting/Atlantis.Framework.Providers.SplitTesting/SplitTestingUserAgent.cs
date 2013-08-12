using System.Web;

namespace Atlantis.Framework.Providers.SplitTesting
{
  internal class SplitTestingUserAgent
  {
    internal static string UserAgent 
    {
      get
      {
        var userAgent = string.Empty;
        if (HttpContext.Current != null)
        {
          userAgent = HttpContext.Current.Request.UserAgent ?? string.Empty;
        }
        return userAgent;
      }
    }
  }
}
