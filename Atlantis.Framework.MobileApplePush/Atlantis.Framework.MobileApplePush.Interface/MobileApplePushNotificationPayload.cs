using System.Collections.Generic;
using System.Text;

namespace Atlantis.Framework.MobileApplePush.Interface
{
  public class MobileApplePushNotificationPayload
  {
    public string Message { get; private set; }

    public int? Badge { get; private set; }

    internal IList<KeyValuePair<string, string>> CustomDataString { get; private set; }

    internal IList<KeyValuePair<string, long>> CustomDataInt { get; private set; }

    internal MobileApplePushNotificationPayload(string message, int? badge)
    {
      Message = message;
      Badge = badge;
      CustomDataString = new List<KeyValuePair<string, string>>(8);
      CustomDataInt = new List<KeyValuePair<string, long>>(8);
    }

    public void AddCustomString(string key, string value)
    {
      CustomDataString.Add(new KeyValuePair<string, string>(key, value));
    }

    public void AddCustomInt(string key, long value)
    {
      CustomDataInt.Add(new KeyValuePair<string, long>(key, value));
    }

    public string ToJson()
    {
      StringBuilder jsonBuilder = new StringBuilder();
      jsonBuilder.Append(@"{""aps"":{");

      if(!string.IsNullOrEmpty(Message))
      {
        jsonBuilder.AppendFormat(@"""alert"":""{0}""", Message);
      }
      
      if(Badge != null && Badge > 0)
      {
        if (!string.IsNullOrEmpty(Message))
        {
          jsonBuilder.AppendFormat(@",""badge"":{0}", Badge);
        }
        else
        {
          jsonBuilder.AppendFormat(@"""badge"":{0}", Badge);
        }
      }

      jsonBuilder.Append("}");

      foreach (KeyValuePair<string, string> pair in CustomDataString)
      {
        jsonBuilder.AppendFormat(@",""{0}"":""{1}""", pair.Key, pair.Value);
      }

      foreach (KeyValuePair<string, long> pair in CustomDataInt)
      {
        jsonBuilder.AppendFormat(@",""{0}"":{1}", pair.Key, pair.Value);
      }

      jsonBuilder.Append("}");
      return jsonBuilder.ToString();
    }
  }
}
