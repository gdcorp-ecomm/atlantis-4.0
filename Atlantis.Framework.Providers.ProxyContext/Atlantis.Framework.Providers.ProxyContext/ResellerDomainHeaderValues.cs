using Atlantis.Framework.Interface;
using System.Web;

namespace Atlantis.Framework.Providers.ProxyContext
{
  internal class ResellerDomainHeaderValues : HeaderValuesBase
  {
    const string _RESELLERORIGINALIP = "X-ARR-PL-OriginalIP";
    const string _RESELLERORIGINALHOST = "X-ARR-PL-OriginalHost";

    public override HeaderValueStatus CheckForProxyHeaders(string sourceIpAddress, out IProxyData proxyData)
    {
      proxyData = null;
      if (HttpContext.Current == null)
      {
        return HeaderValueStatus.Unknown;
      }

      HeaderValueStatus result = HeaderValueStatus.Invalid;

      string originalIP = GetFirstHeaderValue(_RESELLERORIGINALIP);
      string originalHost = GetFirstHeaderValue(_RESELLERORIGINALHOST);

      if ((originalHost == null) && (originalIP == null))
      {
        // Empty is only valid if it comes from a not whitelisted ipAddress that or its the loopback
        result = HeaderValueStatus.Empty;
        if (ResellerProxyWhitelist.IsValidRequest(sourceIpAddress) && !IsAddressThisMachine(sourceIpAddress))
        {
          result = HeaderValueStatus.Invalid;
        }
      }
      else if ((originalHost != null) && (originalIP != null))
      {
        bool isWhiteListed = ResellerProxyWhitelist.IsValidRequest(sourceIpAddress) || IsAddressThisMachine(sourceIpAddress);
        result = isWhiteListed ? HeaderValueStatus.Valid : HeaderValueStatus.Invalid;
      }

      if (result == HeaderValueStatus.Valid)
      {
        proxyData = ProxyData.FromValidData(ProxyType, originalIP, originalHost, false);
      }

      return result;
    }

    public override ProxyTypes ProxyType
    {
      get { return ProxyTypes.CustomResellerARR; }
    }
  }
}
