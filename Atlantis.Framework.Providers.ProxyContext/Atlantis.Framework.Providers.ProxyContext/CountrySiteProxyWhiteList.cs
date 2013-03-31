namespace Atlantis.Framework.Providers.ProxyContext
{
  internal class CountrySiteProxyWhitelist
  {
    private const string _PROXYTYPEID = "6";

    public static bool IsValidRequest(string ipAddress)
    {
      ProxyWhiteList proxyWhiteList = DataCache.DataCache.GetCustomCacheData(_PROXYTYPEID, ProxyWhiteList.LoadProxyWhiteList);
      return proxyWhiteList.IsValidProxyIP(ipAddress);
    }
  }
}
