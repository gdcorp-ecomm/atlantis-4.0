using Atlantis.Framework.Interface;
using System.Web;

namespace Atlantis.Framework.Providers.ProxyContext
{
  internal class ARRHeaderValues : HeaderValuesBase
  {
    const string _ARRORIGINALIP = "X-ARR-OriginalIP";
    const string _ARRORIGINALHOST = "X-ARR-OriginalHost";

    public override HeaderValueStatus CheckForProxyHeaders(string sourceIpAddress, out IProxyData proxyData)
    {
      proxyData = null;
      if (HttpContext.Current == null)
      {
        return HeaderValueStatus.Unknown;
      }

      HeaderValueStatus result = HeaderValueStatus.Invalid;

      string originalIP = GetFirstHeaderValue(_ARRORIGINALIP);
      string originalHost = GetFirstHeaderValue(_ARRORIGINALHOST);

      if ((originalHost == null) && (originalIP == null))
      {
        result = HeaderValueStatus.Empty;
      }
      else if ((originalHost != null) && (originalIP != null))
      {
        result = IsAddressThisMachine(sourceIpAddress) ? HeaderValueStatus.Valid : HeaderValueStatus.Invalid;
      }

      if (result == HeaderValueStatus.Valid)
      {
        proxyData = ProxyData.FromValidData(ProxyType, originalIP, originalHost, true);
      }

      return result;
    }

    public override ProxyTypes ProxyType
    {
      get { return ProxyTypes.LocalARR; }
    }
  }

}
