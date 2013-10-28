using Atlantis.Framework.Interface;
using System.Collections.Generic;

namespace Atlantis.Framework.Testing.MockProviders
{
  public class MockDebugContext : ProviderBase, IDebugContext
  {
    private readonly List<KeyValuePair<string, string>> _debugData;

    public MockDebugContext(IProviderContainer container)
      : base(container)
    {
      _debugData = new List<KeyValuePair<string, string>>(10);
    }

    public List<KeyValuePair<string, string>> GetDebugTrackingData()
    {
      return _debugData;
    }

    public string GetQaSpoofQueryValue(string spoofParamName)
    {
      return string.Empty;
    }

    public void LogDebugTrackingData(string key, string data)
    {
      _debugData.Add(new KeyValuePair<string, string>(key, data));
    }
  }
}
