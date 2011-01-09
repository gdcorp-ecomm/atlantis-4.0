using System.Collections.Generic;

namespace Atlantis.Framework.Interface
{
  public interface IDebugContext
  {
    List<KeyValuePair<string, string>> GetDebugTrackingData();
    void LogDebugTrackingData(string key, string data);
    string GetQaSpoofQueryValue(string spoofParamName);
  }
}
