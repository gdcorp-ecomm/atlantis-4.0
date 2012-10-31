using System;
using System.Collections.Generic;
using System.Reflection;
using Atlantis.Framework.DataCache;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.TemplatePlaceHolders.Interface
{
  internal class ProviderTypeCache
  {
    private readonly SlimLock _cacheLock;
    readonly IDictionary<string, Type> _providerTypeDictionary = new Dictionary<string, Type>();

    internal ProviderTypeCache()
    {
      _cacheLock = new SlimLock();
    }

    ~ProviderTypeCache()
    {
      _cacheLock.Dispose();
    }

    private string GetKey(string assemblyName, string providerType)
    {
      return string.Format("{0}|{1}", assemblyName, providerType);
    }

    internal bool GetProvider<T>(string assemblyName, string providerType, IProviderContainer providerContainer, out T provider) where T : class
    {
      bool found;
      provider = null;

      string key = GetKey(assemblyName, providerType);

      try
      {
        Type typeOfProvider;

        using (_cacheLock.GetReadLock())
        {
          found = _providerTypeDictionary.TryGetValue(key, out typeOfProvider);
        }

        if(!found)
        {
          using(_cacheLock.GetWriteLock())
          {
            if(_providerTypeDictionary.ContainsKey(key))
            {
              typeOfProvider = _providerTypeDictionary[key];
              found = true;
            }
            else
            {
              Assembly assembly = !string.IsNullOrEmpty(assemblyName) ? Assembly.Load(assemblyName) : AssemblyHelper.GetApplicationAssembly();
              Type typeOfTemplateDataSourceProvider = assembly.GetType(providerType);

              _providerTypeDictionary[key] = typeOfTemplateDataSourceProvider;
              typeOfProvider = typeOfTemplateDataSourceProvider;
              found = true;
            }
          }
        }

        if (!providerContainer.TryResolve(typeOfProvider, out provider))
        {
          throw new Exception(string.Format("Unable to resolve ProviderType \"{0}\". Make sure it is registered by your provider container.", providerType));
        }
      }
      catch(Exception ex)
      {
        found = false;
        ErrorLogHelper.LogError(new Exception(string.Format("Unable to get provider type. Assembly: {0}, ProviderType: {1}, Error: {2}", assemblyName, providerType, ex.Message)), MethodBase.GetCurrentMethod().DeclaringType.FullName);
      }

      return found;
    }
  }
}
