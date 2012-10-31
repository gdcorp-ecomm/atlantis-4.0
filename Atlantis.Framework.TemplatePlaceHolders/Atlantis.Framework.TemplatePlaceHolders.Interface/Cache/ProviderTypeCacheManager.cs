using Atlantis.Framework.Interface;

namespace Atlantis.Framework.TemplatePlaceHolders.Interface
{
  internal static class ProviderTypeCacheManager
  {
    private static readonly ProviderTypeCache _providerTypeCache = new ProviderTypeCache();

    internal static bool GetProvider<T>(string assemblyName, string providerType, IProviderContainer providerContainer, out T provider) where T : class
    {
      return _providerTypeCache.GetProvider(assemblyName, providerType, providerContainer, out provider);
    }
  }
}
