using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Atlantis.Framework.MobileAndroidPush.Interface
{
  public class MobileAndroidPushNotification
  {
    private const string REGISTRATION_ID_KEY = "registration_id";
    private const string MESSAGE_KEY = "msg";
    private const string NAMESPACE_KEY = "collapse_key";
    private const string DATA_KEY_FORMAT = "data.{0}";
    private const string POST_DATA_FIRST_ITEM_FORMAT = "{0}={1}";
    private const string POST_DATA_ITEM_FORMAT = "&{0}={1}";

    private IDictionary<string, string> Data { get; set; }

    internal MobileAndroidPushNotification(string registrationId, string message, string messageNamespace)
    {
      Data = new Dictionary<string, string>(32);
      Data[REGISTRATION_ID_KEY] = registrationId;
      Data[NAMESPACE_KEY] = messageNamespace;
      AddItem(MESSAGE_KEY, message);
    }

    public void AddItem(string key, string value)
    {
      Data[string.Format(DATA_KEY_FORMAT, key)] = value;
    }

    public byte[] GetPostData()
    {
      string postDataString = string.Empty;

      if (Data != null)
      {
        StringBuilder postDataBuilder = new StringBuilder();
        foreach (string dataKey in Data.Keys)
        {
          if (postDataBuilder.Length == 0)
          {
            postDataBuilder.AppendFormat(POST_DATA_FIRST_ITEM_FORMAT, HttpUtility.UrlEncode(dataKey), HttpUtility.UrlEncode(Data[dataKey]));
          }
          else
          {
            postDataBuilder.AppendFormat(POST_DATA_ITEM_FORMAT, HttpUtility.UrlEncode(dataKey), HttpUtility.UrlEncode(Data[dataKey]));
          }
        }

        postDataString = postDataBuilder.ToString();
      }

      return Encoding.ASCII.GetBytes(postDataString);
    }
  }
}
