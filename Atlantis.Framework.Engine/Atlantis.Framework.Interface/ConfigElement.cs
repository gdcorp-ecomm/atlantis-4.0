using System.Collections.Generic;

namespace Atlantis.Framework.Interface
{
  public class ConfigElement
  {
    private string _progId;
    private string _assembly;
    private bool _lpc;
    private Dictionary<string, string> _configValues = null;

    public ConfigElement(string progId, string assembly, bool lpc)
    {
      _progId = progId;
      _assembly = assembly;
      _lpc = lpc;
    }

    public ConfigElement(string progId, string assembly, bool lpc, Dictionary<string, string> configValues)
      : this(progId, assembly, lpc)
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

    public string ProgID
    {
      get { return _progId; }
    }

    public string Assembly
    {
      get { return _assembly; }
    }

    public bool LPC
    {
      get { return _lpc; }
    }
  }
}
