using Atlantis.Framework.Interface;
using System.Collections.Generic;
using System.Web;

namespace Atlantis.Framework.Providers.ProxyContext
{
  internal class TranslationHeaderValues : HeaderValuesBase
  {
    const string _TRANSLATIONORIGINALHOST = "X-OriginalHost";
    const string _TRANSLATIONORIGINALIP = "X-OriginalIP";
    const string _TRANSLATIONORIGINALLANG = "X-OriginalLang";

    public override HeaderValueStatus CheckForProxyHeaders(string sourceIpAddress, out IProxyData proxyData)
    {
      proxyData = null;
      if (HttpContext.Current == null)
      {
        return HeaderValueStatus.Unknown;
      }

      HeaderValueStatus result = HeaderValueStatus.Invalid;

      string originalIP = GetFirstHeaderValue(_TRANSLATIONORIGINALIP);
      string originalHost = GetFirstHeaderValue(_TRANSLATIONORIGINALHOST);
      string language = GetFirstHeaderValue(_TRANSLATIONORIGINALLANG);

      if ((originalHost == null) && (originalIP == null) && (language == null))
      {
        // Empty is only valid if it comes from a not whitelisted ipAddress that or its the loopback
        result = HeaderValueStatus.Empty;
        if (TranslationProxyWhitelist.IsValidRequest(sourceIpAddress) && !IsAddressThisMachine(sourceIpAddress))
        {
          result = HeaderValueStatus.Invalid;
        }
      }
      else if ((originalHost != null) && (originalIP != null) && (language != null))
      {
        bool isWhiteListed = TranslationProxyWhitelist.IsValidRequest(sourceIpAddress) || IsAddressThisMachine(sourceIpAddress);
        result = isWhiteListed ? HeaderValueStatus.Valid : HeaderValueStatus.Invalid;
      }

      if (result == HeaderValueStatus.Valid)
      {
        Dictionary<string, string> extendedData = new Dictionary<string, string>(1);
        extendedData["language"] = language;
        proxyData = ProxyData.FromValidData(ProxyType, originalIP, originalHost, false, extendedData);
      }

      return result;
    }

    public override ProxyTypes ProxyType
    {
      get { return ProxyTypes.TransPerfectTranslation; }
    }
  }
}
