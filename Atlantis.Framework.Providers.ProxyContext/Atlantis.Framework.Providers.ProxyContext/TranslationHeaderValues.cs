using System.Web;

namespace Atlantis.Framework.Providers.ProxyContext
{
  internal class TranslationHeaderValues : HeaderValuesBase
  {
    const string _TRANSLATIONORIGINALHOST = "X-OriginalHost";
    const string _TRANSLATIONORIGINALPORT = "X-OriginalPort";
    const string _TRANSLATIONORIGINALIP = "X-OriginalIP";
    const string _TRANSLATIONORIGINALLANG = "X-OriginalLang";

    private HeaderValueStatus _status = HeaderValueStatus.Unknown;
    public override HeaderValueStatus GetStatus(string ipAddress)
    {
      if (HttpContext.Current == null)
      {
        return _status;
      }

      if (_status == HeaderValueStatus.Unknown)
      {
        _status = HeaderValueStatus.Invalid;

        bool isWhiteListed = TranslationProxyWhitelist.IsValidRequest(ipAddress);

        if ((OriginalHost == null) && (OriginalIP == null) && (OriginalPort == null) && (Language == null))
        {
          // Empty is only valid if it comes from a not whitelisted ipAddress that or its the loopback
          _status = HeaderValueStatus.Empty;
          if (isWhiteListed && !IsLoopBack(ipAddress))
          {
            _status = HeaderValueStatus.Invalid;
          }
        }
        else if ((OriginalHost != null) && (OriginalIP != null))
        {
          _status = isWhiteListed ? HeaderValueStatus.Valid : HeaderValueStatus.Invalid;
        }
      }
      return _status;
    }

    private bool _isInitialized = false;

    private void GetHeaderValues()
    {
      if (!_isInitialized)
      {
        _isInitialized = true;
        _originalIP = GetFirstHeaderValue(_TRANSLATIONORIGINALIP);
        _originalHost = GetFirstHeaderValue(_TRANSLATIONORIGINALHOST);
        _originalPort = GetFirstHeaderValue(_TRANSLATIONORIGINALPORT);
        _language = GetFirstHeaderValue(_TRANSLATIONORIGINALLANG);
      }
    }

    private string _originalIP = null;
    public string OriginalIP
    {
      get
      {
        GetHeaderValues();
        return _originalIP;
      }
    }

    private string _originalHost = null;
    public string OriginalHost
    {
      get
      {
        GetHeaderValues();
        return _originalHost;
      }
    }

    private string _originalPort = null;
    public string OriginalPort
    {
      get
      {
        GetHeaderValues();
        return _originalPort;
      }
    }

    private string _language = null;
    public string Language
    {
      get
      {
        GetHeaderValues();
        return _language;
      }
    }
  }
}
