using Atlantis.Framework.Interface;
using System.Collections.Generic;

namespace Atlantis.Framework.Providers.ProxyContext
{
  public static class ActiveProxyTypes
  {
    private static HashSet<ProxyTypes> _activeProxyTypes;

    static ActiveProxyTypes()
    {
      _activeProxyTypes = new HashSet<ProxyTypes>();
    }

    public static void Add(params ProxyTypes[] activeProxyTypes)
    {
      if (activeProxyTypes != null)
      {
        foreach (ProxyTypes proxyType in activeProxyTypes)
        {
          _activeProxyTypes.Add(proxyType);
        }
      }
    }

    public static bool Contains(ProxyTypes proxyType)
    {
      return _activeProxyTypes.Contains(proxyType);
    }

    public static void Clear()
    {
      _activeProxyTypes.Clear();
    }

  }
}
