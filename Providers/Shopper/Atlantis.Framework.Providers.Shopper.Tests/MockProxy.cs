using Atlantis.Framework.Interface;
using System;

namespace Atlantis.Framework.Providers.Shopper.Tests
{
  public class MockProxy : ProviderBase, IProxyContext
  {
    public MockProxy(IProviderContainer container)
      : base(container)
    {
    }

    public ProxyStatusType Status
    {
      get { throw new NotImplementedException(); }
    }

    public bool IsProxyActive(ProxyTypes proxyType)
    {
      throw new NotImplementedException();
    }

    public bool TryGetActiveProxy(ProxyTypes proxyType, out IProxyData proxyData)
    {
      throw new NotImplementedException();
    }

    public System.Collections.Generic.IEnumerable<IProxyData> ActiveProxyChain
    {
      get { throw new NotImplementedException(); }
    }

    public string OriginIP
    {
      get { return "4.3.2.1"; }
    }

    public string OriginHost
    {
      get { throw new NotImplementedException(); }
    }

    public string ContextHost
    {
      get { throw new NotImplementedException(); }
    }
  }
}
