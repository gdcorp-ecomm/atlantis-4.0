using System;
using System.Web;

namespace Atlantis.Framework.PixelsGet.Interface.PixelObjects.Helpers
{
  public class CookieHelper
  {
    public HttpCookie NewCrossDomainCookie(string cookieName, DateTime expiration)
    {
      HttpCookie result = new HttpCookie(cookieName);
      result.Expires = expiration;
      result.Path = "/";
      result.Domain = CrossDomainCookieDomain;
      return result;
    }

    private string _crossDomainCookieDomain;
    private string CrossDomainCookieDomain
    {
      get
      {
        if (_crossDomainCookieDomain == null)
        {
          string result = HttpContext.Current.Request.Url.Host;
          if (result == "localhost")
            result = null;
          else if (result.Contains("."))
          {
            string[] parts = result.Split('.');
            if (parts.Length > 2)
              result = parts[parts.Length - 2] + "." + parts[parts.Length - 1];
          }
          _crossDomainCookieDomain = result;
        }
        return _crossDomainCookieDomain;
      }
    }

  }
}
