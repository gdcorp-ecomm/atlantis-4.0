using System.Web;

namespace Atlantis.Framework.Providers.ProxyContext
{
  internal class ResellerDomainHeaderValues : HeaderValuesBase
  {
    const string _RESELLERORIGINALIP = "X-ARR-PL-OriginalIP";
    const string _RESELLERORIGINALHOST = "X-ARR-PL-OriginalHost";
    const string _RESELLERORIGINALURL = "X-ARR-PL-OriginalUrl";

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

        bool isWhiteListed = ResellerProxyWhitelist.IsValidRequest(ipAddress);

        if ((OriginalHost == null) && (OriginalIP == null) && (OriginalUrl == null))
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
        _originalIP = GetFirstHeaderValue(_RESELLERORIGINALIP);
        _originalHost = GetFirstHeaderValue(_RESELLERORIGINALHOST);
        _originalUrl = GetFirstHeaderValue(_RESELLERORIGINALURL);
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

    private string _originalUrl = null;
    public string OriginalUrl
    {
      get
      {
        GetHeaderValues();
        return _originalUrl;
      }
    }

  }
}
