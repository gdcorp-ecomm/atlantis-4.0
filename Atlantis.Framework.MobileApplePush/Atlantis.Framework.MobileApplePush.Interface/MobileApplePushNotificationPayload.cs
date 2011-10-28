using System.Collections.Generic;
using System.Text;

namespace Atlantis.Framework.MobileApplePush.Interface
{
  public class MobileApplePushNotificationPayload
  {
    public string Message { get; private set; }

    internal IList<KeyValuePair<string, string>> CustomDataString { get; private set; }

    internal IList<KeyValuePair<string, int>> CustomDataInt { get; private set; }

    public MobileApplePushNotificationPayload(string message)
    {
      Message = message;
      CustomDataString = new List<KeyValuePair<string, string>>(8);
      CustomDataInt = new List<KeyValuePair<string, int>>(8);
    }

    public void AddCustomString(string key, string value)
    {
      CustomDataString.Add(new KeyValuePair<string, string>(key, value));
    }

    public void AddCustomInt(string key, int value)
    {
      CustomDataInt.Add(new KeyValuePair<string, int>(key, value));
    }

    public string ToJson()
    {
      StringBuilder jsonBuilder = new StringBuilder();
      jsonBuilder.AppendFormat(@"{{""aps"":{{""alert"":""{0}""}}", Message);

      foreach (KeyValuePair<string, string> pair in CustomDataString)
      {
        jsonBuilder.AppendFormat(@",""{0}"":""{1}""", pair.Key, pair.Value);
      }

      foreach (KeyValuePair<string, int> pair in CustomDataInt)
      {
        jsonBuilder.AppendFormat(@",""{0}"":{1}", pair.Key, pair.Value);
      }

      jsonBuilder.Append("}");
      return jsonBuilder.ToString();
    }
  }
}
