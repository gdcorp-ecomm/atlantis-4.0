using System.Collections.Generic;
using System.Threading;

namespace Atlantis.Framework.Interface
{
  public class ConfigElement
  {
    private int _requestType;
    private string _progId;
    private string _assembly;
    private Dictionary<string, string> _configValues = null;

    private volatile string _assemblyFileVersion = "0.0.0.0";
    private volatile string _assemblyDescription = string.Empty;

    ConfigElementStats _stats;

    public ConfigElement(int requestType, string progId, string assembly)
    {
      _requestType = requestType;
      _progId = progId;
      _assembly = assembly;
      _stats = new ConfigElementStats();
    }

    public ConfigElement(int requestType, string progId, string assembly, Dictionary<string, string> configValues)
      : this(requestType, progId, assembly)
    {
      if ((configValues != null) && (configValues.Count > 0))
      {
        _configValues = configValues;
      }
    }

    public string GetConfigValue(string key)
    {
      string result = string.Empty;
      string foundValue;
      if ((_configValues != null) && (_configValues.TryGetValue(key, out foundValue)))
      {
        result = foundValue;
      }
      return result;
    }

    public int RequestType
    {
      get { return _requestType; }
    }

    public string ProgID
    {
      get { return _progId; }
    }

    public string Assembly
    {
      get { return _assembly; }
    }

    public string AssemblyFileVersion
    {
      get { return _assemblyFileVersion; }
      set { _assemblyFileVersion = value; }
    }

    public string AssemblyDescription
    {
      get { return _assemblyDescription; }
      set { _assemblyDescription = value; }
    }

    public ConfigElementStats Stats
    {
      get { return _stats; }
    }

    public ConfigElementStats ResetStats()
    {
      ConfigElementStats newStats = new ConfigElementStats();
      return Interlocked.Exchange<ConfigElementStats>(ref _stats, newStats);
    }
  }
}
