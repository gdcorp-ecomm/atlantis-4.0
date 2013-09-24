using Atlantis.Framework.Interface;
using System.Web;

namespace Atlantis.Framework.Providers.ProxyContext
{
  internal class AkamaiHeaderValues : HeaderValuesBase
  {
    const string _AKAMAIORIGINALIP = "X-DSA-OriginalIP";
    const string _AKAMAISECRET = "X-DSA-Secret";
    const string _AKAMAIHOST = "X-DSA-Host";

    public override HeaderValueStatus CheckForProxyHeaders(string sourceIpAddress, out IProxyData proxyData)
    {
      proxyData = null;
      if (HttpContext.Current == null)
      {
        return HeaderValueStatus.Unknown;
      }

      HeaderValueStatus result = HeaderValueStatus.Invalid;

      string originalIP = GetFirstHeaderValue(_AKAMAIORIGINALIP);
      string originalHost = GetFirstHeaderValue(_AKAMAIHOST);
      string secret = GetFirstHeaderValue(_AKAMAISECRET);

      if ((originalIP == null) && (secret == null) && (originalHost == null))
      {
        result = HeaderValueStatus.Empty;
      }
      else if ((originalIP != null) && (secret != null) && (originalHost != null))
      {
        string validSecrets = DataCache.DataCache.GetAppSetting("ATLANTIS_PROXY_AKAMAI_SECRET");
        bool isSecretValid = validSecrets.Contains(secret);
        result = isSecretValid ? HeaderValueStatus.Valid : HeaderValueStatus.Invalid;
      }

      if (result == HeaderValueStatus.Valid)
      {
        proxyData = ProxyData.FromValidData(ProxyType, originalIP, originalHost, true);
      }

      return result;
    }

    public override ProxyTypes ProxyType
    {
      get { return ProxyTypes.AkamaiDSA; }
    }
  }
}
