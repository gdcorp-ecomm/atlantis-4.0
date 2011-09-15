using System.Web;

namespace Atlantis.Framework.Providers.ProxyContext
{
  internal class ARRHeaderValues : HeaderValuesBase
  {
    const string _ARRORIGINALIP = "X-ARR-OriginalIP";
    const string _ARRORIGINALHOST = "X-ARR-OriginalHost";
    const string _ARRORIGINALURL = "X-ARR-OriginalUrl";

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

        if ((OriginalHost == null) && (OriginalIP == null) && (OriginalUrl == null))
        {
          _status = HeaderValueStatus.Empty;
        }
        else if ((OriginalHost != null) && (OriginalIP != null))
        {
          // For the Local ARR request to be Valid, the header values must exist
          // and the IPAddress must be a valid loopback.
          _status = IsLoopBack(ipAddress) ? HeaderValueStatus.Valid : HeaderValueStatus.Invalid;
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
        _originalIP = GetFirstHeaderValue(_ARRORIGINALIP);
        _originalHost = GetFirstHeaderValue(_ARRORIGINALHOST);
        _originalUrl = GetFirstHeaderValue(_ARRORIGINALURL);
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
