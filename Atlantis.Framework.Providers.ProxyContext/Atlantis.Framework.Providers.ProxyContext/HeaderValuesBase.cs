using System.Net;
using System.Web;

namespace Atlantis.Framework.Providers.ProxyContext
{
  internal abstract class HeaderValuesBase
  {
    public abstract HeaderValueStatus GetStatus(string ipAddress);

    protected static string GetFirstHeaderValue(string name)
    {
      string result = null;

      if (HttpContext.Current != null)
      {
        string[] headerValues = HttpContext.Current.Request.Headers.GetValues(name);
        if ((headerValues != null) && (headerValues.Length > 0))
        {
          result = headerValues[0];
        }
      }

      return result;
    }

    protected static bool IsLoopBack(string ipAddress)
    {
      bool result = false;

      IPAddress ip;
      if (IPAddress.TryParse(ipAddress, out ip))
      {
        result = IPAddress.IsLoopback(ip);
      }

      return result;
    }
  }
}
