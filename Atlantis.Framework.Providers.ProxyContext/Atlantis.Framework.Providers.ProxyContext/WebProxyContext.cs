using System.Web;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Providers.ProxyContext
{
  public class WebProxyContext : ProviderBase, IProxyContext
  {
    private ARRHeaderValues _arrHeaderValues;
    private ResellerDomainHeaderValues _resellerHeaderValues;
    private TranslationHeaderValues _translationHeaderValues;

    public WebProxyContext(IProviderContainer container)
      :base(container)
    {
      _arrHeaderValues = new ARRHeaderValues();
      _resellerHeaderValues = new ResellerDomainHeaderValues();
      _translationHeaderValues = new TranslationHeaderValues();
    }

    ProxyStatusType _status = ProxyStatusType.Undetermined;
    public ProxyStatusType Status
    {
      get
      {
        // return early to keep in undetermined state
        if (HttpContext.Current == null)
        {
          return _status;
        }

        // Conditions we care about for Invalid status:
        // 1. Whitelisted server that is not loopback but sent bad headers (reseller and translation header status will be Invalid)
        // 2. Not on whitelist but sent headers (reseller and translation header status will be Invalid)
        // 3. ARR Incomplete header values or not loopback
        if (_status == ProxyStatusType.Undetermined)
        {
          _status = ProxyStatusType.Invalid;

          HeaderValueStatus arrStatus = _arrHeaderValues.GetStatus(HttpContext.Current.Request.UserHostAddress);
          string originIP = IsLocalARR ? ARRIP : HttpContext.Current.Request.UserHostAddress;
          HeaderValueStatus resellerStatus = _resellerHeaderValues.GetStatus(originIP);
          HeaderValueStatus translationStatus = _translationHeaderValues.GetStatus(originIP);

          if (arrStatus == HeaderValueStatus.Empty && resellerStatus == HeaderValueStatus.Empty && translationStatus == HeaderValueStatus.Empty)
          {
            _status = ProxyStatusType.None;
          }
          else if ((arrStatus != HeaderValueStatus.Invalid) && (resellerStatus != HeaderValueStatus.Invalid) && (translationStatus != HeaderValueStatus.Invalid))
          {
            _status = ProxyStatusType.Valid;
          }
        }

        return _status;
      }
    }

    bool? _isLocalARR;
    public bool IsLocalARR
    {
      get
      {
        if (HttpContext.Current == null)
        {
          return false; // early return on purpose to not set any bool? values
        }

        if (!_isLocalARR.HasValue)
        {
          _isLocalARR = _arrHeaderValues.GetStatus(HttpContext.Current.Request.UserHostAddress) == HeaderValueStatus.Valid;
        }
        return _isLocalARR.Value;
      }
    }

    bool? _isResellerDomain;
    public bool IsResellerDomain
    {
      get
      {
        if (HttpContext.Current == null)
        {
          return false; // early return on purpose to not set any bool? values
        }

        if (!_isResellerDomain.HasValue)
        {
          string originIP = IsLocalARR ? ARRIP : HttpContext.Current.Request.UserHostAddress;
          _isResellerDomain = _resellerHeaderValues.GetStatus(originIP) == HeaderValueStatus.Valid;
        }
        return _isResellerDomain.Value;
      }
    }

    bool? _isTranslationDomain;
    public bool IsTransalationDomain
    {
      get
      {
        if (HttpContext.Current == null)
        {
          return false; // early return on purpose to not set any bool? values
        }

        if (!_isTranslationDomain.HasValue)
        {
          string originIP = IsLocalARR ? ARRIP : HttpContext.Current.Request.UserHostAddress;
          _isTranslationDomain = _translationHeaderValues.GetStatus(originIP) == HeaderValueStatus.Valid;
        }
        return _isTranslationDomain.Value;
      }
    }

    public string ResellerDomainUrl
    {
      get { return _resellerHeaderValues.OriginalUrl; }
    }

    public string ResellerDomainHost
    {
      get { return _resellerHeaderValues.OriginalHost; }
    }

    public string ResellerDomainIP
    {
      get { return _resellerHeaderValues.OriginalIP; }
    }

    public string ARRUrl
    {
      get { return _arrHeaderValues.OriginalUrl; }
    }

    public string ARRHost
    {
      get { return _arrHeaderValues.OriginalHost; }
    }

    public string ARRIP
    {
      get { return _arrHeaderValues.OriginalIP; }
    }

    public string TranslationHost
    {
      get { return _translationHeaderValues.OriginalHost; }
    }

    public string TranslationPort
    {
      get { return _translationHeaderValues.OriginalPort; }
    }

    public string TranslationIP
    {
      get { return _translationHeaderValues.OriginalIP; }
    }

    public string TranslationLanguage
    {
      get { return _translationHeaderValues.Language; }
    }

    public string OriginIP
    {
      get 
      { 
        string result = string.Empty;

        if (HttpContext.Current != null)
        {
          result = HttpContext.Current.Request.UserHostAddress;
        }

        if (IsLocalARR)
        {
          result = ARRIP;
        }

        if (IsResellerDomain)
        {
          result = ResellerDomainIP;
        }
        else if (IsTransalationDomain)
        {
          result = TranslationIP;
        }

        return result;
      }
    }

    public string ContextHost
    {
      get
      {
        string result = string.Empty;

        if (HttpContext.Current != null)
        {
          result = HttpContext.Current.Request.Url.Host;
        }

        if (IsLocalARR)
        {
          result = ARRHost;
        }

        return result;
      }
    }
  }
}
