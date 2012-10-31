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
    readonly IDictionary<string, ITemplateDataSourceProvider> _templateDataSourceProviderDictionary = new Dictionary<string, ITemplateDataSourceProvider>();
    readonly IDictionary<string, ITemplateContentProvider> _templateContentProviderDictionary = new Dictionary<string, ITemplateContentProvider>();

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

    internal bool GetTemplateDataSourceProvider(string assemblyName, string providerType, IProviderContainer providerContainer, out ITemplateDataSourceProvider templateDataSourceProvider)
    {
      bool found;
      templateDataSourceProvider = null;

      string key = GetKey(assemblyName, providerType);

      try
      {
        using (_cacheLock.GetReadLock())
        {
          found = _templateDataSourceProviderDictionary.TryGetValue(key, out templateDataSourceProvider);
        }

        if(!found)
        {
          using(_cacheLock.GetWriteLock())
          {
            if(!_templateDataSourceProviderDictionary.ContainsKey(key))
            {
              Assembly assembly = !string.IsNullOrEmpty(assemblyName) ? Assembly.Load(assemblyName) : AssemblyHelper.GetApplicationAssembly();
              Type typeOfTemplateDataSourceProvider = assembly.GetType(providerType);

              if(providerContainer.TryResolve(typeOfTemplateDataSourceProvider, out templateDataSourceProvider))
              {
                found = true;
                _templateDataSourceProviderDictionary[key] = templateDataSourceProvider;
              }
              else
              {
                throw new Exception(string.Format("Unable to resolve ProviderType \"{0}\". Make sure it is registered by your provider container.", providerType));
              }
            }
          }
        }
      }
      catch(Exception ex)
      {
        found = false;
        ErrorLogHelper.LogError(new Exception(string.Format("Unable to get provider type. Assembly: {0}, ProviderType: {1}, Error: {2}", assemblyName, providerType, ex.Message)), MethodBase.GetCurrentMethod().DeclaringType.FullName);
      }

      return found;
    }

    internal bool GetTemplateContentProviderProvider(string assemblyName, string providerType, IProviderContainer providerContainer, out ITemplateContentProvider templateContentProvider)
    {
      bool found;
      templateContentProvider = null;

      string key = GetKey(assemblyName, providerType);

      try
      {
        using (_cacheLock.GetReadLock())
        {
          found = _templateContentProviderDictionary.TryGetValue(key, out templateContentProvider);
        }

        if (!found)
        {
          using (_cacheLock.GetWriteLock())
          {
            if (!_templateContentProviderDictionary.ContainsKey(key))
            {
              Assembly assembly = !string.IsNullOrEmpty(assemblyName) ? Assembly.Load(assemblyName) : AssemblyHelper.GetApplicationAssembly();
              Type typeOfTemplateContentProvider = assembly.GetType(providerType);

              if (providerContainer.TryResolve(typeOfTemplateContentProvider, out templateContentProvider))
              {
                found = true;
                _templateContentProviderDictionary[key] = templateContentProvider;
              }
              else
              {
                throw new Exception(string.Format("Unable to resolve ProviderType \"{0}\". Make sure it is registered by your provider container.", providerType));
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        found = false;
        ErrorLogHelper.LogError(new Exception(string.Format("Unable to get provider type. Assembly: {0}, ProviderType: {1}, Error: {2}", assemblyName, providerType, ex.Message)), MethodBase.GetCurrentMethod().DeclaringType.FullName);
      }

      return found;
    }
  }
}
