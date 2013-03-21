using System.Collections.Generic;

namespace Atlantis.Framework.DotTypeCache.Static
{
  public sealed class ConfigElement
  {
    private string _dotType;
    public string DotType
    {
      get { return _dotType; }
    }

    private string _progId;
    public string ProgId
    {
      get { return _progId; }
    }

    private string _assembly;
    public string Assembly
    {
      get { return _assembly; }
    }

    private bool _isMultiRegistrar;
    public bool IsMultiRegistrar
    {
      get { return _isMultiRegistrar; }
    }

    private Dictionary<string, string> _configValues = new Dictionary<string,string>();
    public Dictionary<string, string> ConfigValues
    {
      get
      {
        return _configValues;
      }
    }

    public ConfigElement(string dotType, string progId, string assembly)
    {
      this._dotType = dotType;
      this._progId = progId;
      this._assembly = assembly;
      this._isMultiRegistrar = false;
    }

    public ConfigElement(string dotType, string progId, string assembly, string isMultiRegistrar)
    {
      this._dotType = dotType;
      this._progId = progId;
      this._assembly = assembly;
      this._isMultiRegistrar = isMultiRegistrar.Equals("1");
    }

    public ConfigElement(string dotType, string progId, string assembly, Dictionary<string, string> configValues)
      : this(dotType, progId, assembly, string.Empty)
    {
      if ((configValues != null) && (configValues.Count > 0))
      {
        _configValues = configValues;
      }
    }

    public ConfigElement(string dotType, string progId, string assembly, 
      string isMultiRegistrar , Dictionary<string, string> configValues)
      : this(dotType, progId, assembly, isMultiRegistrar)
    {
      if ((configValues != null) && (configValues.Count > 0))
      {
        _configValues = configValues;
      }
    }
  }
}
