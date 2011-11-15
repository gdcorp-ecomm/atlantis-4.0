using System;
using System.Reflection;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MobileAndroidPush.Interface
{
  public class MobileAndroidPushResponseData : IResponseData
  {
    private AtlantisException AtlantisException { get; set; }

    public string PushId { get; private set; }

    public string ErrorType { get; private set; }

    public bool IsSuccess { get; private set; }

    public MobileAndroidPushResponseData(string response)
    {
      if(!string.IsNullOrEmpty(response))
      {
        string[] responsePair = response.Split('=');
        if (responsePair.Length == 2)
        {
          if (response.Contains("Error="))
          {
            ErrorType = responsePair[1];
          }
          else if (response.Contains("id="))
          {
            PushId = responsePair[1].Replace("\n", string.Empty).Replace("\r", string.Empty);
          }
        }
      }

      IsSuccess = !string.IsNullOrEmpty(PushId);
    }

    public MobileAndroidPushResponseData(MobileAndroidPushRequestData requestData, Exception ex)
    {
      IsSuccess = false;
      AtlantisException = new AtlantisException(requestData,
                                                MethodBase.GetCurrentMethod().DeclaringType.FullName,
                                                ex.Message,
                                                ex.StackTrace);
    }

    public string ToXML()
    {
      return string.Empty;
    }

    public AtlantisException GetException()
    {
      return AtlantisException;
    }
  }
}
