using System.Collections.Generic;

namespace Atlantis.Framework.Providers.Containers
{
  internal class ObjectProviderContainerData : ProviderContainerDataBase
  {
    private readonly IDictionary<string, object> _containerData = new Dictionary<string, object>(128);

    protected override object GetContextData(string key)
    {
      object value;

      _containerData.TryGetValue(key, out value);

      return value;
    }

    protected override void SetContextData(string key, object value)
    {
      _containerData[key] = value;
    }
  }
}