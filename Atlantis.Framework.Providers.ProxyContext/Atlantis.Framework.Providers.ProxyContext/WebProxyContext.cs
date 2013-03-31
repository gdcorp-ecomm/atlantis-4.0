using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Web;

namespace Atlantis.Framework.Providers.ProxyContext
{
  public class WebProxyContext : ProviderBase, IProxyContext
  {
    private Lazy<WebProxyData> _proxyData;

    public WebProxyContext(IProviderContainer container)
      :base(container)
    {
      _proxyData = new Lazy<WebProxyData>(() => { return new WebProxyData(); });
    }

    public ProxyStatusType Status
    {
      get
      {
        if (HttpContext.Current == null)
        {
          return ProxyStatusType.Undetermined;
        }
        else
        {
          return _proxyData.Value.Status;
        }
      }
    }

    public IEnumerable<IProxyData> ActiveProxyChain
    {
      get 
      {
        if (HttpContext.Current == null)
        {
          return new List<IProxyData>(0);
        }
        else
        {
          return _proxyData.Value.ActiveProxyChain;
        }
      }
    }

    public bool IsProxyActive(ProxyTypes proxyType)
    {
      if (HttpContext.Current == null)
      {
        return false;
      }
      else
      {
        return _proxyData.Value.IsProxyActive(proxyType);
      }
    }

    public bool TryGetActiveProxy(ProxyTypes proxyType, out IProxyData proxyData)
    {
      if (HttpContext.Current == null)
      {
        proxyData = null;
        return false;
      }
      else
      {
        return _proxyData.Value.TryGetActiveProxy(proxyType, out proxyData);
      }
    }

    public string OriginIP
    {
      get
      {
        if (HttpContext.Current == null)
        {
          return string.Empty;
        }
        else
        {
          return _proxyData.Value.OriginIP;
        }
      }
    }

    public string OriginHost
    {
      get
      {
        if (HttpContext.Current == null)
        {
          return string.Empty;
        }
        else
        {
          return _proxyData.Value.OriginHost;
        }
      }
    }

    public string ContextHost
    {
      get
      {
        if (HttpContext.Current == null)
        {
          return string.Empty;
        }
        else
        {
          return _proxyData.Value.ContextHost;
        }
      }
    }

    #region Obsolete

    public bool IsLocalARR
    {
      get
      {
        if (HttpContext.Current == null)
        {
          return false;
        }
        else
        {
          return _proxyData.Value.IsProxyActive(ProxyTypes.LocalARR);
        }
      }
    }

    public bool IsResellerDomain
    {
      get
      {
        if (HttpContext.Current == null)
        {
          return false;
        }
        else
        {
          return _proxyData.Value.IsProxyActive(ProxyTypes.CustomResellerARR);
        }
      }
    }

    public bool IsTransalationDomain
    {
      get
      {
        if (HttpContext.Current == null)
        {
          return false; 
        }
        else
        {
          return _proxyData.Value.IsProxyActive(ProxyTypes.TransPerfectTranslation);
        }
      }
    }

    public string ARRHost
    {
      get
      {
        if (HttpContext.Current == null)
        {
          return string.Empty;
        }
        else
        {
          string result = string.Empty;
          IProxyData data;
          if (_proxyData.Value.TryGetActiveProxy(ProxyTypes.LocalARR, out data))
          {
            result = data.OriginalHost;
          }

          return result;
        }
      }
    }

    public string ARRIP
    {
      get
      {
        if (HttpContext.Current == null)
        {
          return string.Empty;
        }
        else
        {
          string result = string.Empty;
          IProxyData data;
          if (_proxyData.Value.TryGetActiveProxy(ProxyTypes.LocalARR, out data))
          {
            result = data.OriginalIP;
          }

          return result;
        }
      }
    }

    public string ResellerDomainHost
    {
      get
      {
        if (HttpContext.Current == null)
        {
          return string.Empty;
        }
        else
        {
          string result = string.Empty;
          IProxyData data;
          if (_proxyData.Value.TryGetActiveProxy(ProxyTypes.CustomResellerARR, out data))
          {
            result = data.OriginalHost;
          }

          return result;
        }
      }
    }

    public string ResellerDomainIP
    {
      get
      {
        if (HttpContext.Current == null)
        {
          return string.Empty;
        }
        else
        {
          string result = string.Empty;
          IProxyData data;
          if (_proxyData.Value.TryGetActiveProxy(ProxyTypes.CustomResellerARR, out data))
          {
            result = data.OriginalIP;
          }

          return result;
        }
      }
    }

    public string TranslationHost
    {
      get
      {
        if (HttpContext.Current == null)
        {
          return string.Empty;
        }
        else
        {
          string result = string.Empty;
          IProxyData data;
          if (_proxyData.Value.TryGetActiveProxy(ProxyTypes.TransPerfectTranslation, out data))
          {
            result = data.OriginalHost;
          }

          return result;
        }
      }
    }

    public string TranslationIP
    {
      get
      {
        if (HttpContext.Current == null)
        {
          return string.Empty;
        }
        else
        {
          string result = string.Empty;
          IProxyData data;
          if (_proxyData.Value.TryGetActiveProxy(ProxyTypes.TransPerfectTranslation, out data))
          {
            result = data.OriginalIP;
          }

          return result;
        }
      }
    }

    public string TranslationLanguage
    {
      get
      {
        if (HttpContext.Current == null)
        {
          return string.Empty;
        }
        else
        {
          string result = string.Empty;
          IProxyData data;
          if (_proxyData.Value.TryGetActiveProxy(ProxyTypes.TransPerfectTranslation, out data))
          {
            string language;
            if (data.TryGetExtendedData("language", out language))
            {
              result = language;
            }
          }

          return result;
        }
      }
    }

    #endregion
  }
}
