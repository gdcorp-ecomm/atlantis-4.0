using Atlantis.Framework.Interface;
using System.Net;
using System.Web;

namespace Atlantis.Framework.Providers.ProxyContext
{
  internal abstract class HeaderValuesBase
  {
    private static IPAddress[] _localAddresses = null;

    static HeaderValuesBase()
    {
      _localAddresses = Dns.GetHostAddresses(string.Empty);
    }

    public abstract HeaderValueStatus CheckForProxyHeaders(string sourceIpAddress, out IProxyData proxyData);
    public abstract ProxyTypes ProxyType { get; }

    protected static string GetFirstHeaderValue(string name)
    {
      string result = null;

      if (HttpContext.Current != null)
      {
        string[] headerValues = HttpContext.Current.Request.Headers.GetValues(name);
        if ((headerValues != null) && (headerValues.Length > 0))
        {
          result = headerValues[0];
          string key = "WebProxyContext.RawHeaderValue." + name;
        }
      }

      return result;
    }

    protected static bool IsAddressThisMachine(string textIpAddress)
    {
      bool result = false;

      IPAddress ip;
      if (IPAddress.TryParse(textIpAddress, out ip))
      {
        if (IPAddress.IsLoopback(ip))
        {
          result = true;
        }
        else if (_localAddresses != null)
        {
          foreach (IPAddress machineAddress in _localAddresses)
          {
            if (ip.Equals(machineAddress))
            {
              result = true;
              break;
            }
          }

        }
      }

      return result;
    }
  }
}
