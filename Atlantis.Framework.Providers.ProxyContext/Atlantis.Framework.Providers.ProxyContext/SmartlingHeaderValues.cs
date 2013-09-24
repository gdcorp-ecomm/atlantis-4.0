using Atlantis.Framework.Interface;
using System.Collections.Generic;
using System.Web;

namespace Atlantis.Framework.Providers.ProxyContext
{
  internal class SmartlingHeaderValues : HeaderValuesBase
  {
    const string _SMARTLINGORIGINALHOST = "X-Smartling-OriginalHost";
    const string _SMARTLINGORIGINALIP = "X-Smartling-OriginalIP";
    const string _SMARTLINGLANGUAGE = "X-Language-Locale";
    const string _SMARTLINGSECRET = "X-Smartling-Secret";

    public override HeaderValueStatus CheckForProxyHeaders(string sourceIpAddress, out IProxyData proxyData)
    {
      proxyData = null;
      if (HttpContext.Current == null)
      {
        return HeaderValueStatus.Unknown;
      }

      HeaderValueStatus result = HeaderValueStatus.Invalid;

      string originalIP = GetFirstHeaderValue(_SMARTLINGORIGINALIP);
      string originalHost = GetFirstHeaderValue(_SMARTLINGORIGINALHOST);
      string language = GetFirstHeaderValue(_SMARTLINGLANGUAGE);
      string secret = GetFirstHeaderValue(_SMARTLINGSECRET);

      if ((originalHost == null) && (originalIP == null) && (language == null) && (secret == null))
      {
        result = HeaderValueStatus.Empty;
      }
      else if ((originalHost != null) && (originalIP != null) && (language != null) && (secret != null))
      {
        string validSecrets = DataCache.DataCache.GetAppSetting("ATLANTIS_PROXY_SMARTLING_SECRET");
        bool isSecretValid = validSecrets.Contains(secret);
        result = isSecretValid ? HeaderValueStatus.Valid : HeaderValueStatus.Invalid;
      }

      if (result == HeaderValueStatus.Valid)
      {
        Dictionary<string, string> extendedData = new Dictionary<string, string>(1);
        extendedData["language"] = language;
        proxyData = ProxyData.FromValidData(ProxyType, originalIP, originalHost, true, extendedData);
      }

      return result;
    }

    public override ProxyTypes ProxyType
    {
      get { return ProxyTypes.SmartlingTranslation; }
    }
  }
}
