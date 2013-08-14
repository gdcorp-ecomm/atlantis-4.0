using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Xml;

namespace Atlantis.Framework.Engine
{
  class EngineConfig
  {
    string _assemblyPath;
    Dictionary<int, ConfigElement> _configItems = new Dictionary<int, ConfigElement>();

    ReaderWriterLockSlim _engineLock = new ReaderWriterLockSlim();
    const int LOCK_TIME_OUT = 5000;

    public EngineConfig()
    {
      ConfigName = "atlantis.config";
      Load();
    }

    public string ConfigName { get; set; }

    private void LogError(string sourceFunction, string input, string errorMessage)
    {
      try
      {
        AtlantisException aex = new AtlantisException(sourceFunction, "0", errorMessage, input, null, null);
        IErrorLogger errorLogger = EngineLogging.EngineLogger;
        if (errorLogger != null)
        {
          errorLogger.LogAtlantisException(aex);
        }
      }
      catch { }
    }

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

    private string GetConfigFilePath()
    {
      string configFilePath = string.Empty;

      try
      {
        configFilePath = Path.Combine(AssemblyPath, ConfigName);

        if (!File.Exists(configFilePath))
        {
          throw new FileNotFoundException("Unable to load Atlantis configuration file.", configFilePath);
        }
      }
      catch (Exception ex)
      {
        LogError(GetType().Name + "." + MethodBase.GetCurrentMethod().Name, string.Empty, ex.Message);
      }

      return configFilePath;
    }

    private ConfigElement HydrateFromXml(XmlElement xlConfigElement)
    {
      ConfigElement result = null;

      int requestType = 0;
      string assemblyPath = Path.Combine(AssemblyPath, xlConfigElement.GetAttribute("assembly"));
      string progId = xlConfigElement.GetAttribute("progid");
      string requestTypeString = xlConfigElement.GetAttribute("request_type");

      if (progId.Length > 0
          && assemblyPath.Length > 0
          && int.TryParse(requestTypeString, out requestType))
      {
        // Get custom config values
        Dictionary<string, string> configValues = null;
        XmlNodeList customConfigNodes = xlConfigElement.SelectNodes("./ConfigValue");
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

        if (xlConfigElement.HasAttribute("ws_url"))
        {
          string configItemWebServiceUrl = xlConfigElement.GetAttribute("ws_url");
          result = new WsConfigElement(requestType, progId, assemblyPath, configItemWebServiceUrl, configValues);
        }
        else
        {
          result = new ConfigElement(requestType, progId, assemblyPath, configValues);
        }
      }
      else
      {
        LogError(GetType().Name + "." + MethodBase.GetCurrentMethod().Name,
                  String.Format("ProgID: {0}, Assembly: {1} RequestType: {2}", progId, assemblyPath, requestTypeString),
                  "Unable to Create ConfigElement: Invalid Attribute");
      }

      return result;
    }

    // See configuration/atlantis.xsd for config schema
    public void Load()
    {
      XmlDocument xdConfig = new XmlDocument();
      string configFilePath = string.Empty;

      try
      {
        if (_engineLock.TryEnterWriteLock(LOCK_TIME_OUT))
        {
          try
          {
            configFilePath = GetConfigFilePath();
            _configItems.Clear();

            xdConfig.Load(configFilePath);

            XmlNodeList xnlConfigElements = xdConfig.SelectNodes("/ConfigElements/ConfigElement");

            foreach (XmlElement xlConfigElement in xnlConfigElements)
            {
              ConfigElement item = HydrateFromXml(xlConfigElement);
              if (item != null)
              {
                _configItems[item.RequestType] = item;
              }
            }
          }
          finally
          {
            _engineLock.ExitWriteLock();
          }
        }
        else
        {
          string message = "Acquiring EngineConfig write lock timed out.";
          throw new TimeoutException(message);
        }
      }
      catch (Exception ex)
      {
        LogError(GetType().Name + "." + MethodBase.GetCurrentMethod().Name,
                 "Filename: " + configFilePath,
                 ex.Message);
      }
    }

    public ConfigElement GetConfig(int requestType)
    {
      ConfigElement configItem = null;

      if (_engineLock.TryEnterReadLock(LOCK_TIME_OUT))
      {
        try
        {
          if (!_configItems.TryGetValue(requestType, out configItem))
          {
            throw new ArgumentException(String.Format("Unable to Find ConfigElement for Request Type: {0}", requestType));
          }
        }
        finally
        {
          _engineLock.ExitReadLock();
        }
      }
      else
      {
        string message = "Acquiring EngineConfig read lock timed out.";
        throw new TimeoutException(message);
      }

      return configItem;
    }

    public bool TryGetConfigElement(int requestType, out ConfigElement configElement)
    {
      bool result = false;
      configElement = null;

      if (_engineLock.TryEnterReadLock(LOCK_TIME_OUT))
      {
        try
        {
          result = _configItems.TryGetValue(requestType, out configElement);
        }
        finally
        {
          _engineLock.ExitReadLock();
        }
      }
      else
      {
        string message = "Acquiring EngineConfig read lock timed out.";
        throw new TimeoutException(message);
      }

      return result;
    }

    public List<ConfigElement> GetAllConfigs()
    {
      List<ConfigElement> result = new List<ConfigElement>(1000);

      if (_engineLock.TryEnterReadLock(LOCK_TIME_OUT))
      {
        try
        {
          foreach (int requestType in _configItems.Keys)
          {
            result.Add(_configItems[requestType]);
          }
        }
        finally
        {
          _engineLock.ExitReadLock();
        }
      }
      else
      {
        string message = "Acquiring EngineConfig read lock timed out.";
        throw new TimeoutException(message);
      }

      return result;
    }



  }

}
