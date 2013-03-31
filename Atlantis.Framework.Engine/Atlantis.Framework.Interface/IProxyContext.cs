using System.Collections.Generic;

namespace Atlantis.Framework.Interface
{
  public interface IProxyContext
  {
    /// <summary>
    /// Gets the status of all proxies. If Valid or Empty, the values can be trusted from the ActiveProxyChain
    /// </summary>
    ProxyStatusType Status { get; }

    /// <summary>
    /// Checks to see if a proxy of the given type is active for the request.
    /// </summary>
    /// <param name="proxyType">Type of proxy<see cref="ProxyTypes"/></param>
    /// <returns>true if the type is active for the current request</returns>
    bool IsProxyActive(ProxyTypes proxyType);

    /// <summary>
    /// Attempts to get the proxy data from the active proxy chain.
    /// </summary>
    /// <param name="proxyType">Type of proxy<see cref="ProxyTypes"/></param>
    /// <param name="proxyData">Proxy data output<see cref="IProxyData"/></param>
    /// <returns>Proxy data<see cref="IProxyData"/></returns>
    bool TryGetActiveProxy(ProxyTypes proxyType, out IProxyData proxyData);

    /// <summary>
    /// Returns an enumerable IProxyData objects that are active. The first one is the nearest proxy to the request, the last one is the farthest away proxy in the chain.
    /// OriginIP comes from the farthest proxy.
    /// </summary>
    IEnumerable<IProxyData> ActiveProxyChain { get; }

    /// <summary>
    /// The IP address of the furthest active proxy. If no proxies exist, this is the UserHostAddress of the current request.
    /// </summary>
    string OriginIP { get; }

    /// <summary>
    /// The Host of the furthest active proxy.  If no proxies exist, this is the Request.Url.Host of the current request.
    /// </summary>
    string OriginHost { get; }

    /// <summary>
    /// The Host that should be used to get context.  Not always the furthest away proxy host.
    /// </summary>
    string ContextHost { get; }

  }
}
