using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Reflection;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Web.DynamicRouteHandler
{
  internal class DynamicRouteHandlerLoader : IDisposable
  {
    private CompositionContainer _container;

    [ImportMany]
    IEnumerable<Lazy<DynamicRouteHandlerBase>> _loadedDynamicRouteHandlers;
    public IEnumerable<Lazy<DynamicRouteHandlerBase>> DynamicRouteHandlersFound
    {
      get { return _loadedDynamicRouteHandlers; }
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

    private static void LogError(string sourceFunction, string input, string errorMessage)
    {
      try
      {
        AtlantisException aex = new AtlantisException(sourceFunction, "0", errorMessage, input, null, null);
        Engine.Engine.LogAtlantisException(aex);
      }
      catch { }
    }

    public DynamicRouteHandlerLoader(IEnumerable<string> assemblySearchPatterns, params Assembly[] additionalAssemblies)
    {
      var catalog = new AggregateCatalog();

      // Add from all assemblies matching naming convention
      foreach (string searchPattern in assemblySearchPatterns)
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

    public void Dispose()
    {
      if (_container != null)
      {
        _container.Dispose();
      }
    }
  }
}
