using Atlantis.Framework.Interface;
using System.Collections.Generic;

namespace Atlantis.Framework.Providers.Links.Tests.Mocks
{
  internal class TransperfectTestWebProxy : ProviderBase, IProxyContext
  {
    IProxyData _data = new TransperfectData();

    public TransperfectTestWebProxy(IProviderContainer container)
      : base(container)
    { }

    public ProxyStatusType Status
    {
      get { return ProxyStatusType.Valid; }
    }

    public bool IsProxyActive(ProxyTypes proxyType)
    {
      return proxyType == ProxyTypes.TransPerfectTranslation;
    }

    public bool TryGetActiveProxy(ProxyTypes proxyType, out IProxyData proxyData)
    {
      bool result = false;
      proxyData = null;

      if (proxyType == ProxyTypes.TransPerfectTranslation)
      {
        result = true;
        proxyData = _data;
      }

      return result;
    }

    public IEnumerable<IProxyData> ActiveProxyChain
    {
      get { return new List<IProxyData>(new IProxyData[1] { _data }); }
    }

    public string OriginIP
    {
      get { return _data.OriginalIP; }
    }

    public string OriginHost
    {
      get { return _data.OriginalHost; }
    }

    public string ContextHost
    {
      get { return _data.OriginalHost; }
    }

    internal class TransperfectData : IProxyData
    {
      public string OriginalIP
      {
        get { return "2.2.2.2"; }
      }

      public string OriginalHost
      {
        get { return "es.mysite.com"; }
      }

      public bool IsContextualHost
      {
        get { return true; }
      }

      public ProxyTypes ProxyType
      {
        get { return ProxyTypes.TransPerfectTranslation; }
      }

      public bool TryGetExtendedData(string key, out string value)
      {
        value = null;
        bool result = false;
        if (key == "language")
        {
          value = "es";
          result = true;
        }

        return result;
      }
    }
  }
}
