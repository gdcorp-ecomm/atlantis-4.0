namespace Atlantis.Framework.Providers.ProxyContext
{
  internal class TranslationProxyWhitelist
  {
    private const string _PROXYTYPEID = "5";

    public static bool IsValidRequest(string ipAddress)
    {
      ProxyWhiteList proxyWhiteList = DataCache.DataCache.GetCustomCacheData(_PROXYTYPEID, ProxyWhiteList.LoadProxyWhiteList);
      return proxyWhiteList.IsValidProxyIP(ipAddress);
    }
  }
}
