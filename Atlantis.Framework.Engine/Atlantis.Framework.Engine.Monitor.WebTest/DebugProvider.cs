using Atlantis.Framework.Interface;
using System.Collections.Generic;

namespace Atlantis.Framework.Engine.Monitor.WebTest
{
  public class DebugProvider : ProviderBase, IDebugContext
  {
    public DebugProvider(IProviderContainer container)
      : base(container)
    {

    }

    private List<KeyValuePair<string, string>> _debugData = new List<KeyValuePair<string, string>>();

    public List<KeyValuePair<string, string>> GetDebugTrackingData()
    {
      return _debugData;
    }

    public void LogDebugTrackingData(string key, string data)
    {
      _debugData.Add(new KeyValuePair<string, string>(key, data));
    }

    public string GetQaSpoofQueryValue(string spoofParamName)
    {
      return string.Empty;
    }
  }
}