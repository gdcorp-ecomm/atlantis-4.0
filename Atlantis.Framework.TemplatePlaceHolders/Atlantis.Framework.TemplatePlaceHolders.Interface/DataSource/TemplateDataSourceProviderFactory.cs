using System;
using System.Reflection;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.TemplatePlaceHolders.Interface
{
  internal static class TemplateDataSourceProviderFactory
  {
    internal static ITemplateDataSourceProvider GetInstance(IDataSource dataSource, IProviderContainer providerContainer)
    {
      // TODO: Cache all of this reflection

      ITemplateDataSourceProvider templateDataSourceProvider;

      Assembly assembly = !string.IsNullOrEmpty(dataSource.ProviderAssembly) ? Assembly.Load(dataSource.ProviderAssembly) : AssemblyHelper.GetApplicationAssembly();
      Type typeOfDataSourceProvider = assembly.GetType(dataSource.ProviderType);

      if (typeOfDataSourceProvider != null)
      {
        if (typeOfDataSourceProvider.GetInterface("ITemplateDataSourceProvider") != typeof(ITemplateDataSourceProvider))
        {
          throw new Exception(string.Format("ProviderType \"{0}\" does not implement \"ITemplateDataSourceProvider\".", dataSource.ProviderType));
        }

        if(!providerContainer.TryResolve(typeOfDataSourceProvider, out templateDataSourceProvider))
        {
          throw new Exception(string.Format("Unable to resolve ProviderType \"{0}\". Make sure it is registered by your provider container.", dataSource.ProviderType));
        }
      }
      else
      {
        throw new Exception(string.Format("Unable to reflect type of ProviderType \"{0}\".", dataSource.ProviderType));
      }

      return templateDataSourceProvider;
    }
  }
}
