using Atlantis.Framework.Interface;
using System.Collections.Generic;

namespace Atlantis.Framework.Providers.Geo.Tests
{
  public class MockProxy : ProviderBase, IProxyContext
  {
    public MockProxy(IProviderContainer container)
      : base(container)
    {

    }

    public ProxyStatusType Status
    {
      get { return ProxyStatusType.Valid; }
    }

    public bool IsProxyActive(ProxyTypes proxyType)
    {
      return (proxyType == ProxyTypes.AkamaiDSA);
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
      get { return "1.4.16.3"; }
    }

    public string OriginHost
    {
      get { return string.Empty; }
    }

    public string ContextHost
    {
      get { return string.Empty; }
    }
  }
}
