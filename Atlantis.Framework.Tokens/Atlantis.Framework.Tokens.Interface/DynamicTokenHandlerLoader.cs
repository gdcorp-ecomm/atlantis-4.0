using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Reflection;

namespace Atlantis.Framework.Tokens.Interface
{
  internal class DynamicTokenHandlerLoader : IDisposable
  {
    // To allow additional patterns in the future, add methods to TokenManager that
    // will allow control of this pattern list.
    private static List<string> _assemblySearchPatterns;

    static DynamicTokenHandlerLoader()
    {
      _assemblySearchPatterns = new List<string>();
      _assemblySearchPatterns.Add("Atlantis.Framework.TH.*");
    }

    [ImportMany]
    IEnumerable<Lazy<ITokenHandler>> _loadedTokenHandlers;

    CompositionContainer _container;

    public DynamicTokenHandlerLoader(params Assembly[] additionalAssemblies)
    {
      var catalog = new AggregateCatalog();

      // Add from all assemblies matching naming convention
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

    private string _assemblyPath;
    private string AssemblyPath
    {
      get
      {
        if (_assemblyPath == null)
        {
          try
          {
            Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
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

    public void Dispose()
    {
      if (_container != null)
      {
        _container.Dispose();
      }
    }

    public IEnumerable<Lazy<ITokenHandler>> TokenHandlersFound
    {
      get { return _loadedTokenHandlers; }
    }
  }
}
