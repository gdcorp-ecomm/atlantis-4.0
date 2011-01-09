using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Reflection;
using Microsoft.Win32;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Engine
{

  class EngineConfig
  {

    const string _REGISTRYPATH = @"SOFTWARE\Parsons\Atlantis";
    const string _DEFAULTLOGWEBSERVICEURL =
      "http://commgtwyws.dev.glbt1.gdg/WSCgdSiteLog/WSCgdSiteLog.dll?Handler=Default";

    string _configFileName = "atlantis.config";
    string _logWebServiceUrl;
    string _assemblyPath;
    Dictionary<int, ConfigElement> _configItems = new Dictionary<int, ConfigElement>();
    EngineLock _engineLock = new EngineLock();

    public EngineConfig()
    {
      _logWebServiceUrl = GetLogWebServiceUrl();
      Load();
    }

    [Obsolete("Use LogWebServiceUrl instead.")]
    public string LogWSURL
    {
      get { return _logWebServiceUrl; }
    }

    public string LogWebServiceUrl
    {
      get { return _logWebServiceUrl; }
    }

    //
    // <ConfigElements>
    //  <ConfigElement progid="" assembly="" request_type="" lpc="0|1" [ws_url=""] />
    //  <ConfigElement progid="" assembly="" request_type="" lpc="0|1" [ws_url=""] >
    //    <ConfigValue key="" value="" />
    //  </ConfigElement>
    //   ...
    // </ConfigElements>
    // 

    public void Load()
    {
      XmlDocument xdConfig = new XmlDocument();
      string configFilePath = string.Empty;
      bool isRegistry = false;

      try
      {
        _engineLock.GetWriterLock();

        configFilePath = GetConfigFilePath(out isRegistry);
        _configItems.Clear();

        xdConfig.Load(configFilePath);

        XmlNodeList xnlConfigElements = xdConfig.SelectNodes("/ConfigElements/ConfigElement");

        foreach (XmlElement xlConfigElement in xnlConfigElements)
        {
          int requestType = 0;
          int lpc = 0;
          string assemblyPath = isRegistry ? xlConfigElement.GetAttribute("assembly")
                                         : Path.Combine(AssemblyPath, xlConfigElement.GetAttribute("assembly"));
          string progId = xlConfigElement.GetAttribute("progid");
          string requestTypeString = xlConfigElement.GetAttribute("request_type");
          string lpcString = xlConfigElement.GetAttribute("lpc");

          if (progId.Length > 0
              && assemblyPath.Length > 0
              && int.TryParse(requestTypeString, out requestType)
              && int.TryParse(lpcString, out lpc))
          {
            bool isLPC = lpc != 0;

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

            ConfigElement newConfigElement = null;
            if (xlConfigElement.HasAttribute("ws_url"))
            {
              string configItemWebServiceUrl = xlConfigElement.GetAttribute("ws_url");
              newConfigElement = new WsConfigElement(progId, assemblyPath, isLPC, configItemWebServiceUrl, configValues);
            }
            else
            {
              newConfigElement = new ConfigElement(progId, assemblyPath, isLPC, configValues);
            }

            _configItems[requestType] = newConfigElement;

          }
          else
          {
            LogError(GetType().Name + "." + MethodBase.GetCurrentMethod().Name,
                     String.Format("ProgID: {0}, Assembly: {1} RequestType: {2} LPC: {3}",
                                   progId, assemblyPath, requestTypeString, lpcString),
                     "Unable to Create ConfigElement: Invalid Attribute");
          }

        }
      }
      catch (Exception ex)
      {
        LogError(GetType().Name + "." + MethodBase.GetCurrentMethod().Name,
                 "Filename: " + configFilePath,
                 ex.Message);
      }
      finally
      {
        _engineLock.ReleaseWriterLock();
      }
    }

    public ConfigElement GetConfig(int key)
    {
      ConfigElement configItem = null;
      _engineLock.GetReaderLock();
      if (!_configItems.TryGetValue(key, out configItem))
        throw new Exception(String.Format("Unable to Find ConfigElement for Request Type: {0}", key));
      _engineLock.ReleaseReaderLock();
      return configItem;
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

    private string GetConfigFilePath(out bool isRegistry)
    {
      string configFilePath = string.Empty;
      isRegistry = false;

      try
      {
        configFilePath = Path.Combine(AssemblyPath, _configFileName);

        if (!File.Exists(configFilePath))
        {
          RegistryKey rkAtlantis = Registry.LocalMachine.OpenSubKey(_REGISTRYPATH);

          if (rkAtlantis != null)
            configFilePath = (string)rkAtlantis.GetValue("ConfigFile");

          isRegistry = true;

          if (String.IsNullOrEmpty(configFilePath) || !File.Exists(configFilePath))
            throw new Exception("Unable to load Atlantis configuration file");
        }
      }
      catch (Exception ex)
      {
        LogError(GetType().Name + "." + MethodBase.GetCurrentMethod().Name, "", ex.Message);
      }

      return configFilePath;
    }

    private string GetLogWebServiceUrl()
    {
      string result = _DEFAULTLOGWEBSERVICEURL;

      RegistryKey rkAtlantis = Registry.LocalMachine.OpenSubKey(_REGISTRYPATH);

      if (rkAtlantis != null)
      {
        result = (string)rkAtlantis.GetValue("LogWSURL");
      }

      return result;
    }

    private void LogError(string sourceFunction, string input, string errorMessage)
    {
      try
      {
        string sourceServer = Environment.MachineName;
        DateTime logDate = DateTime.Now;
        gdSiteLog.WSCgdSiteLogService oLog = new Atlantis.Framework.Engine.gdSiteLog.WSCgdSiteLogService();
        oLog.Url = LogWebServiceUrl;
        oLog.LogErrorEx(sourceServer, sourceFunction, string.Empty, 0, errorMessage, input,
          string.Empty, string.Empty, string.Empty, string.Empty, 0);
      }
      catch { }
    }

  }

}
