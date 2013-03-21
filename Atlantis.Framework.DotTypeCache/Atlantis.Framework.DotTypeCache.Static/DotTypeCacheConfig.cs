using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml;

namespace Atlantis.Framework.DotTypeCache.Static
{
  class DotTypeCacheConfig
  {
    private const string _CONF_FILE_NAME = "dottypecache.config";

    private Dictionary<string, ConfigElement> _configItems = new Dictionary<string, ConfigElement>();
    public Dictionary<string, ConfigElement> ConfigItems
    {
      get { return _configItems; }
    }

    ReaderWriterLockSlim _configLock = new ReaderWriterLockSlim();
    const int LOCK_TIME_OUT = 5000;

    private string _assemblyPath;
    internal string AssemblyPath
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
            string message = ex.Message + Environment.NewLine + ex.StackTrace;
            Logging.LogException("DotTypeCacheConfig.AssemblyPath", message, string.Empty);
          }
        }

        return _assemblyPath;
      }
    }

    public DotTypeCacheConfig()
    {
      Load();
    }

    public void Load()
    {
      XmlDocument xmlDoc = new XmlDocument();
      string configFilePath = string.Empty;

      if (_configLock.TryEnterWriteLock(LOCK_TIME_OUT))
      {
        try
        {
          configFilePath = GetConfigFilePath();
          _configItems.Clear();
          xmlDoc.Load(configFilePath);
          XmlNodeList configElements = xmlDoc.SelectNodes("/ConfigElements/ConfigElement");

          if (configElements != null)
          {
            foreach (XmlElement configElement in configElements)
            {
              ConfigElement item = HydrateFromXml(configElement);
              if (item != null)
              {
                _configItems[item.ProgId] = item;
              }
            }
          }
        }
        finally
        {
          _configLock.ExitWriteLock();
        }
      }
      else
      {
        string message = "Acquiring DotTypeCacheConfig write lock timed out.";
        throw new TimeoutException(message);
      }
    }

    private string GetConfigFilePath()
    {
      string configFilePath = string.Empty;

      try
      {
        configFilePath = Path.Combine(AssemblyPath, _CONF_FILE_NAME);

        if (String.IsNullOrEmpty(configFilePath) || !File.Exists(configFilePath))
        {
          throw new Exception("Unable to load configuration file");
        }
      }
      catch (Exception ex)
      {
        string message = ex.Message + Environment.NewLine + ex.StackTrace;
        Logging.LogException("DotTypeCacheConfig.GetConfigFilePath", message, string.Empty);
      }

      return configFilePath;
    }

    private ConfigElement HydrateFromXml(XmlElement configElement)
    {
      ConfigElement result = null;

      string dotType = configElement.GetAttribute("dottype");
      string progId = configElement.GetAttribute("progid");
      string assembly = configElement.GetAttribute("assembly");
      string isMultiRegistrar = configElement.GetAttribute("multi_registrar") ?? string.Empty;
      string assemblyPath
        = Path.Combine(AssemblyPath, assembly);

      if (!string.IsNullOrEmpty(dotType)
        && !string.IsNullOrEmpty(progId)
        && !string.IsNullOrEmpty(assemblyPath))
      {
        Dictionary<string, string> configValues = null;
        XmlNodeList customConfigNodes = configElement.SelectNodes("./ConfigValue");

        if (customConfigNodes.Count > 0)
        {
          configValues = new Dictionary<string, string>(customConfigNodes.Count);

          foreach (XmlElement configValueElement in customConfigNodes)
          {
            if (configValueElement != null)
            {
              string key = configValueElement.GetAttribute("key");
              if (!string.IsNullOrEmpty(key))
              {
                configValues[key] = configValueElement.GetAttribute("value");
              }
            }
          }
        }

        result = new ConfigElement(dotType, progId, assemblyPath, isMultiRegistrar, configValues);
      }
      else
      {
        string input = String.Format("DotType: {0}; ProgId: {1}; Assembly: {2}", dotType, progId, assemblyPath);
        Logging.LogException("DotTypeCacheConfig.Load", "Unable to Create ConfigElement: Invalid Attribute", input);
      }

      return result;
    }
  }
}
