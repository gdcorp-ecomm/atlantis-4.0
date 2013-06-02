using System.Collections.Generic;

namespace Atlantis.Framework.Testing.MockProviders
{
  internal class MockProviderContainerData
  {
    private readonly IDictionary<string, object> _containerData = new Dictionary<string, object>(128);
 
    internal T GetData<T>(string key, T defaultValue)
    {
      T value = defaultValue;

      object contextValue;
      _containerData.TryGetValue(key, out contextValue);

      if (contextValue != null)
      {
        try
        {
          value = (T)contextValue;
        }
        catch
        {
          value = defaultValue;
        }
      }

      return value;
    }

    internal void SetData<T>(string key, T value)
    {
      _containerData[key] = value;
    }
  }
}
