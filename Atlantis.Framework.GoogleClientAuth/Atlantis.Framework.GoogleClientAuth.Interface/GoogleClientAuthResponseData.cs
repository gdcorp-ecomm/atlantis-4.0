using System;
using System.Reflection;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GoogleClientAuth.Interface
{
  public class GoogleClientAuthResponseData : IResponseData
  {
    private AtlantisException AtlantisException { get; set; }

    public bool IsSuccess { get; private set; }

    public string ClientAuthToken { get; private set; }

    public bool ServiceUnavailable { get; private set; }

    public GoogleClientAuthResponseData(string response, bool serviceUnavailable)
    {
      ServiceUnavailable = serviceUnavailable;

      if(!string.IsNullOrEmpty(response))
      {
        string[] responsePair = response.Split('\n');
        for (int i = 0; i < responsePair.Length; i++)
        {
          string[] tokenPair = responsePair[i].Split('=');
          if (tokenPair.Length == 2 &&
              tokenPair[0] == "Auth")
          {
            ClientAuthToken = tokenPair[1];
            break;
          }
        }
      }

      IsSuccess = !string.IsNullOrEmpty(ClientAuthToken);
    }

    public GoogleClientAuthResponseData(GoogleClientAuthRequestData requestData, Exception ex)
    {
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
