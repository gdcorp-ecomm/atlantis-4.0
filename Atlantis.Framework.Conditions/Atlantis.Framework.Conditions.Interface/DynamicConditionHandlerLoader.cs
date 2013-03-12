using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Reflection;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Conditions.Interface
{
  internal class DynamicConditionHandlerLoader : IDisposable
  {
    private static List<string> _assemblySearchPatterns;
    CompositionContainer _container;

    [ImportMany]
    IEnumerable<Lazy<IConditionHandler>> _loadedConditionHandlers;
    public IEnumerable<Lazy<IConditionHandler>> ConditionHandlersFound
    {
      get { return _loadedConditionHandlers; }
    }

    private string _assemblyPath;
    private string AssemblyPath
    {
      get
      {
        if (_assemblyPath == null)
        {
          try
          {
            Uri pathUri = new Uri(Path.GetDirectoryName(GetType().Assembly.CodeBase));
            _assemblyPath = pathUri.LocalPath;
          }
          catch (Exception ex)
          {
            LogError(GetType().Name + "." + MethodBase.GetCurrentMethod().Name, "", ex.Message);
          }
        }

        return _assemblyPath;
      }
    }

    static DynamicConditionHandlerLoader()
    {
      _assemblySearchPatterns = new List<string>();
      _assemblySearchPatterns.Add("Atlantis.Framework.Conditions.Handlers.*");
    }

    public DynamicConditionHandlerLoader(params Assembly[] additionalAssemblies)
    {
      var catalog = new AggregateCatalog();

      foreach (string searchPattern in _assemblySearchPatterns)
      {
        catalog.Catalogs.Add(new DirectoryCatalog(AssemblyPath, searchPattern));
      }

      if (additionalAssemblies != null)
      {
        foreach (Assembly assembly in additionalAssemblies)
        {
          catalog.Catalogs.Add(new AssemblyCatalog(assembly));
        }
      }

      _container = new CompositionContainer(catalog);
      _container.ComposeParts(this);
    }

    private static void LogError(string sourceFunction, string input, string errorMessage)
    {
      try
      {
        AtlantisException aex = new AtlantisException(sourceFunction, "0", errorMessage, input, null, null);
        Engine.Engine.LogAtlantisException(aex);
      }
      catch { }
    }

    public void Dispose()
    {
      if (_container != null)
      {
        _container.Dispose();
      }
    }
  }
}
