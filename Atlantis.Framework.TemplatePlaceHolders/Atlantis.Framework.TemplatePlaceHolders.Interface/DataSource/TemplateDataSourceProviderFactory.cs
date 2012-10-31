using Atlantis.Framework.Interface;

namespace Atlantis.Framework.TemplatePlaceHolders.Interface
{
  internal static class TemplateDataSourceProviderFactory
  {
    internal static ITemplateDataSourceProvider GetInstance(IDataSource dataSource, IProviderContainer providerContainer)
    {
      ITemplateDataSourceProvider templateDataSourceProvider;
      
      if(!ProviderTypeCacheManager.GetProvider(dataSource.ProviderAssembly, dataSource.ProviderType, providerContainer, out templateDataSourceProvider))
      {
        templateDataSourceProvider = new NullTemplateDataSourceProvider();
      }

      return templateDataSourceProvider;
    }
  }
}
