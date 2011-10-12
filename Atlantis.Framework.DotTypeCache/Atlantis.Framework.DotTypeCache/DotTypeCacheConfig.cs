using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DotTypeCache
{
  class DotTypeCacheConfig
  {
    private const string _CONF_FILE_NAME = "dottypecache.config";

    private Dictionary<string, ConfigElement> _configItems = new Dictionary<string, ConfigElement>();
    public Dictionary<string, ConfigElement> ConfigItems
    {
      get { return _configItems; }
    }

    private ConfigLock _configLock = new ConfigLock();

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
            LogError("Atlantis.Framework.DotTypeCache.DotTypeCacheConfig.AssemblyPath", "", ex.Message);
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

      try
      {
        _configLock.GetWriterLock();
        configFilePath = GetConfigFilePath();
        _configItems.Clear();
        xmlDoc.Load(configFilePath);
        XmlNodeList configElements = xmlDoc.SelectNodes("/ConfigElements/ConfigElement");

        foreach (XmlElement configElement in configElements)
        {
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

            ConfigElement newConfigElement = new ConfigElement(dotType, progId, assemblyPath, isMultiRegistrar, configValues);

            _configItems[progId] = newConfigElement;
          }
          else
          {
            LogError("Atlantis.Framework.DotTypeCache.DotTypeCacheConfig.Load()",
                     String.Format("DotType: {0}; ProgId: {1}; Assembly: {2}", dotType, progId, assemblyPath),
                     "Unable to Create ConfigElement: Invalid Attribute");
          }
        }

      }
      catch (Exception ex)
      {
        LogError("Atlantis.Framework.DotTypeCache.DotTypeCacheConfig.Load()",
                 "Filename: " + _CONF_FILE_NAME, ex.Message);
      }
      finally
      {
        _configLock.ReleaseWriterLock();
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
        LogError("Atlantis.Framework.DotTypeCache.DotTypeCacheConfig.GetConfigFilePath()", "", ex.Message);
      }

      return configFilePath;
    }

    private void LogError(string sourceFunction, string input, string errorMessage)
    {
      AtlantisException atlEx = new AtlantisException(sourceFunction,
        string.Empty, string.Empty, errorMessage, string.Empty, string.Empty, string.Empty,
        string.Empty, string.Empty, 0);
      Atlantis.Framework.Engine.Engine.LogAtlantisException(atlEx);
    }
  }
}
