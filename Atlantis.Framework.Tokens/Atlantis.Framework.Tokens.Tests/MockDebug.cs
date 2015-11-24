using Atlantis.Framework.Interface;
using System.Collections.Generic;

namespace Atlantis.Framework.Tokens.Tests
{
  public class MockDebug : ProviderBase, IDebugContext
  {
    List<KeyValuePair<string, string>> _data;

    public MockDebug(IProviderContainer container) : base(container) 
    {
      _data = new List<KeyValuePair<string, string>>();
    }

    public List<KeyValuePair<string, string>> GetDebugTrackingData()
    {
      return _data;
    }

    public void LogDebugTrackingData(string key, string data)
    {
      _data.Add(new KeyValuePair<string, string>(key, data));
    }

    public string GetQaSpoofQueryValue(string spoofParamName)
    {
      return string.Empty;
    }
  }
}
