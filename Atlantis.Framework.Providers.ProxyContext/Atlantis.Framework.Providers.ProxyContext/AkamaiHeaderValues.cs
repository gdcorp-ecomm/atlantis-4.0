using Atlantis.Framework.Interface;
using System.Web;

namespace Atlantis.Framework.Providers.ProxyContext
{
  internal class AkamaiHeaderValues : HeaderValuesBase
  {
    const string _AKAMAIORIGINALIP = "X-Akamai-OriginalIP";
    const string _AKAMAISECRET = "X-Akamai-Secret";

    public override HeaderValueStatus CheckForProxyHeaders(string sourceIpAddress, out Interface.IProxyData proxyData)
    {
      proxyData = null;
      if (HttpContext.Current == null)
      {
        return HeaderValueStatus.Unknown;
      }

      HeaderValueStatus result = HeaderValueStatus.Invalid;

      string originalIP = GetFirstHeaderValue(_AKAMAIORIGINALIP);
      string originalHost = HttpContext.Current.Request.Url.Host;
      string secret = GetFirstHeaderValue(_AKAMAISECRET);

      if ((originalIP == null) && (secret == null))
      {
        result = HeaderValueStatus.Empty;
      }
      else if ((originalIP != null) && (secret != null))
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
