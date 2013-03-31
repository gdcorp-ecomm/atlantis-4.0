using Atlantis.Framework.Interface;
using System.Collections.Generic;

namespace Atlantis.Framework.Providers.Localization.Tests
{
  internal class LocalizationTestWebProxy : ProviderBase, IProxyContext
  {
    public LocalizationTestWebProxy(IProviderContainer container)
      : base(container)
    { }

    public ProxyStatusType Status
    {
      get { return ProxyStatusType.Valid; }
    }

    public bool IsProxyActive(ProxyTypes proxyType)
    {
      return false;
    }

    public bool TryGetActiveProxy(ProxyTypes proxyType, out IProxyData proxyData)
    {
      proxyData = null;
      return false;
    }

    public IEnumerable<IProxyData> ActiveProxyChain
    {
      get { return new List<IProxyData>(); }
    }

    public string OriginIP
    {
      get { return "1.1.1.1"; }
    }

    public string OriginHost
    {
      get { return "uk.mysite.com"; }
    }

    public string ContextHost
    {
      get { return "uk.mysite.com"; }
    }
  }


}
