using Atlantis.Framework.Interface;

namespace Atlantis.Framework.TemplatePlaceHolders.Interface
{
  internal static class ProviderTypeCacheManager
  {
    private static readonly ProviderTypeCache _providerTypeCache = new ProviderTypeCache();

    internal static bool GetTemplateDataSourceProvider(string assemblyName, string providerType, IProviderContainer providerContainer, out ITemplateDataSourceProvider templateDataSourceProvider)
    {
      return _providerTypeCache.GetTemplateDataSourceProvider(assemblyName, providerType, providerContainer, out templateDataSourceProvider);
    }

    internal static bool GetTemplateContentProvider(string assemblyName, string providerType, IProviderContainer providerContainer, out ITemplateContentProvider templateContentProvider)
    {
      return _providerTypeCache.GetTemplateContentProviderProvider(assemblyName, providerType, providerContainer, out templateContentProvider);
    }
  }
}
