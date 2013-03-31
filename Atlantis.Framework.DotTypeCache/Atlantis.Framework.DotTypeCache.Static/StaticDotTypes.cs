using Atlantis.Framework.DotTypeCache.Interface;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Atlantis.Framework.DotTypeCache.Static
{
  public static class StaticDotTypes
  {
    static DotTypeCacheConfig _config;
    static Dictionary<string, IDotTypeInfo> _staticDotTypes;
    static HashSet<string> _multiRegistryDotTypes;
    static Dictionary<string, Dictionary<string, string>> _additionalData;

    static StaticDotTypes()
    {
      _config = new DotTypeCacheConfig();
      _staticDotTypes = new Dictionary<string, IDotTypeInfo>(_config.ConfigItems.Count, StringComparer.OrdinalIgnoreCase);
      _multiRegistryDotTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
      _additionalData = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);

      foreach (KeyValuePair<string, ConfigElement> configItem in _config.ConfigItems)
      {
        IDotTypeInfo dotTypeInfo = GetDotTypeInfoFromAssembly(configItem.Value);

        if (dotTypeInfo != null)
        {
          _staticDotTypes[dotTypeInfo.DotType] = dotTypeInfo;
        }
      }
    }

    public static IDotTypeInfo GetDotType(string dotType)
    {
      if (_staticDotTypes.ContainsKey(dotType))
      {
        return _staticDotTypes[dotType];
      }
      else
      {
        return InvalidDotType.Instance;
      }
    }

    public static bool HasDotType(string dotType)
    {
      return _staticDotTypes.ContainsKey(dotType);
    }

    public static bool IsDotTypeMultiRegistry(string dotType)
    {
      return _multiRegistryDotTypes.Contains(dotType);
    }

    public static string GetAdditionalInfoValue(string dotType, string key)
    {
      string result = string.Empty;

      Dictionary<string, string> additionalDataForDotType;
      if (_additionalData.TryGetValue(dotType, out additionalDataForDotType))
      {
        if (additionalDataForDotType.ContainsKey(key))
        {
          result = additionalDataForDotType[key];
        }
      }

      return result;
    }

    private static IDotTypeInfo GetDotTypeInfoFromAssembly(ConfigElement configItem)
    {
      IDotTypeInfo dotTypeInfo = null;

      try
      {
        Assembly loadedAssembly = Assembly.LoadFrom(configItem.Assembly);
        object obj = loadedAssembly.CreateInstance(configItem.ProgId, true);
        dotTypeInfo = (IDotTypeInfo)obj;

        if (configItem.IsMultiRegistrar)
        {
          _multiRegistryDotTypes.Add(dotTypeInfo.DotType);
        }

        Dictionary<string, string> additionalDataForThisDotType = new Dictionary<string, string>(configItem.ConfigValues);
        _additionalData[dotTypeInfo.DotType] = additionalDataForThisDotType;
      }
      catch (Exception ex)
      {
        Logging.LogException("StaticDotTypes.GetDotTypeInfoFromAssembly", ex.Message, ex.Source);
      }

      return dotTypeInfo;
    }
  }
}
