using Atlantis.Framework.Interface;
using System.Collections.Generic;
using System.Web;

namespace Atlantis.Framework.Providers.ProxyContext
{
  internal class WebProxyData
  {
    private static List<HeaderValuesBase> _proxyHeaderHandlers;

    static WebProxyData()
    {
      _proxyHeaderHandlers = new List<HeaderValuesBase>();
      _proxyHeaderHandlers.Add(new ARRHeaderValues());
      _proxyHeaderHandlers.Add(new CountrySiteHeaderValues());
      _proxyHeaderHandlers.Add(new ResellerDomainHeaderValues());
      _proxyHeaderHandlers.Add(new TranslationHeaderValues());
      _proxyHeaderHandlers.Add(new SmartlingHeaderValues());
      _proxyHeaderHandlers.Add(new AkamaiHeaderValues());
    }

    private List<IProxyData> _activeProxyChain;
    private Dictionary<ProxyTypes, IProxyData> _activeProxies;

    public ProxyStatusType Status { get; private set; }
    public string OriginIP { get; private set; }
    public string OriginHost { get; private set; }
    public string ContextHost { get; private set; }

    public WebProxyData()
    {
      DetermineProxyStatusAndProperties();
    }

    private string GetRequestHost()
    {
      string result = null;
      try
      {
        result = HttpContext.Current.Request.Url.Host;
      }
      catch
      {
        result = null;
      }

      return result ?? GetHostHeaderValue();;
    }

    private string GetHostHeaderValue()
    {
      string result;

      try
      {
        result = HttpContext.Current.Request.Headers["Host"];
      }
      catch
      {
        result = string.Empty;
      }

      return result;
    }

    private void DetermineProxyStatusAndProperties()
    {
      Status = ProxyStatusType.Invalid;

      _activeProxyChain = new List<IProxyData>();
      _activeProxies = new Dictionary<ProxyTypes, IProxyData>();

      int invalidCount = 0;
      int validCount = 0;

      OriginIP = HttpContext.Current.Request.UserHostAddress;
      OriginHost = GetRequestHost();
      ContextHost = OriginHost;

      string inProgressOriginIP = OriginIP;
      string inProgressOriginHost = OriginHost;
      string inProgressContextHost = ContextHost;

      foreach (HeaderValuesBase headerValues in _proxyHeaderHandlers)
      {
        if (ActiveProxyTypes.Contains(headerValues.ProxyType))
        {
          IProxyData proxyData;
          var headerStatus = headerValues.CheckForProxyHeaders(inProgressOriginIP, out proxyData);
          if (headerStatus == HeaderValueStatus.Invalid)
          {
            ++invalidCount;
          }
          else if (headerStatus == HeaderValueStatus.Valid)
          {
            if (proxyData == null)
            {
              ++invalidCount;
            }
            else
            {
              _activeProxyChain.Add(proxyData);
              _activeProxies[proxyData.ProxyType] = proxyData;

              inProgressOriginIP = proxyData.OriginalIP;
              inProgressOriginHost = proxyData.OriginalHost;
              if (proxyData.IsContextualHost)
              {
                inProgressContextHost = proxyData.OriginalHost;
              }

              ++validCount;
            }
          }
        }
      }

      if (validCount > 0)
      {
        OriginIP = inProgressOriginIP;
        OriginHost = inProgressOriginHost;
        ContextHost = inProgressContextHost;
      }

      if (invalidCount == 0)
      {
        if (validCount > 0)
        {
          Status = ProxyStatusType.Valid;
        }
        else
        {
          Status = ProxyStatusType.None;
        }
      }
    }

    public IEnumerable<IProxyData> ActiveProxyChain
    {
      get { return _activeProxyChain; }
    }

    public bool IsProxyActive(ProxyTypes proxyType)
    {
      return _activeProxies.ContainsKey(proxyType);
    }

    public bool TryGetActiveProxy(ProxyTypes proxyType, out IProxyData proxyData)
    {
      return _activeProxies.TryGetValue(proxyType, out proxyData);
    }

  }
}
