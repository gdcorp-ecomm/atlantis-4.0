using Atlantis.Framework.Interface;
using System.Web;

namespace Atlantis.Framework.Providers.ProxyContext
{
  internal class CountrySiteHeaderValues : HeaderValuesBase
  {
    const string _COUNTRYORIGINALIP = "X-CTRY-OriginalIP";
    const string _COUNTRYORIGINALHOST = "X-CTRY-OriginalHost";

    public override HeaderValueStatus CheckForProxyHeaders(string sourceIpAddress, out IProxyData proxyData)
    {
      proxyData = null;
      if (HttpContext.Current == null)
      {
        return HeaderValueStatus.Unknown;
      }

      HeaderValueStatus result = HeaderValueStatus.Invalid;

      string originalIP = GetFirstHeaderValue(_COUNTRYORIGINALIP);
      string originalHost = GetFirstHeaderValue(_COUNTRYORIGINALHOST);

      if ((originalHost == null) && (originalIP == null))
      {
        result = HeaderValueStatus.Empty;
        if (CountrySiteProxyWhitelist.IsValidRequest(sourceIpAddress) && !IsAddressThisMachine(sourceIpAddress))
        {
          result = HeaderValueStatus.Invalid;
        }
      }
      else if ((originalHost != null) && (originalIP != null))
      {
        bool isWhiteListed = CountrySiteProxyWhitelist.IsValidRequest(sourceIpAddress) || IsAddressThisMachine(sourceIpAddress);
        result = isWhiteListed ? HeaderValueStatus.Valid : HeaderValueStatus.Invalid;
      }

      if (result == HeaderValueStatus.Valid)
      {
        proxyData = ProxyData.FromValidData(ProxyType, originalIP, originalHost, true);
      }

      return result;
    }

    public override ProxyTypes ProxyType
    {
      get { return ProxyTypes.CountrySiteARR; }
    }
  }

}
