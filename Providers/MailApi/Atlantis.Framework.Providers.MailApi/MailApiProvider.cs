using Atlantis.Framework.Interface;
using Atlantis.Framework.MailApi.Interface;
using Atlantis.Framework.Providers.MailApi.Interface;
using Atlantis.Framework.Providers.MailApi.Interface.Response;

using System;
using System.Text;
using Atlantis.Framework.Providers.MailApi.Interface.Exceptions;

namespace Atlantis.Framework.Providers.MailApi
{
  public class MailApiProvider : ProviderBase, IMailApiProvider
  {

    const int DEFAULT_OFFSET = 0;
    const int DEFAULT_MESSAGE_COUNT = 20;
    const string DEFAULT_FILTER = ""; // not sure what this is yet

    public MailApiProvider(IProviderContainer container)
      : base(container)
    {
    }

    public LoginFullResult LoginFull(string username, string password, string appKey)
    {
      LoginFullResult result = null;

      LoginResponseData loginResponseData = null;
      GetFolderListResponseData getFolderListResponseData;
      GetMessageListResponseData getMessageListResponseData;

      try
      {
        loginResponseData = MailApiTriplets.LoginViaTriplet(username, password, appKey);

        if (loginResponseData != null)
        {
          getFolderListResponseData = MailApiTriplets.GetFolderListFromTriplet(loginResponseData.LoginData.Hash, appKey, loginResponseData.LoginData.BaseUrl);

          if (getFolderListResponseData != null)
          {
            getMessageListResponseData = MailApiTriplets.GetMessageListFromTriplet(getFolderListResponseData.State.Session, appKey, loginResponseData.LoginData.BaseUrl, DetermineInboxFolderNum(getFolderListResponseData), DEFAULT_OFFSET, DEFAULT_MESSAGE_COUNT, DEFAULT_FILTER);
            if (getMessageListResponseData != null)
            {
              result = new LoginFullResult();
              result.Session = getFolderListResponseData.State.Session;
              result.BaseUrl = loginResponseData.LoginData.BaseUrl;
              result.MessageHeaderList = MailApiTriplets.Convert(getMessageListResponseData).MessageHeaderList;
              result.FolderList = MailApiTriplets.Convert(getFolderListResponseData).FolderList;
            }
          }
        }
      }
      catch (Exception ex)
      {
          var exception = new AtlantisException("MailApiProvider.LoginFull", "0", ex.Message, string.Empty, null, null);
          Engine.Engine.LogAtlantisException(exception);
      }
      return result;
    }



    public LoginResult Login(string username, string password, string appKey)
    {
      LoginResult result = null;
      try
      {
        LoginResponseData data = MailApiTriplets.LoginViaTriplet(username, password, appKey);

        if (data != null)
        {
          if (data.IsJsoapFault)
          {
            MailApiException ex = new MailApiException(data.JsoapMessage, null, username, string.Empty, data.MailApiRequestString, data.MailApiResponseString);
            throw (ex);
          }
          result = MailApiTriplets.Convert(data);
        }
      }
      catch (Exception ex)
      {
        var exception = new AtlantisException("MailApiProvider.Login", "0", ex.Message, string.Empty, null, null);
        Engine.Engine.LogAtlantisException(exception);
      }
      return result;
    }

    public FolderResult GetFolder(string sessionHash, string appKey, string baseUrl, int folderNumber)
    {
      FolderResult result = null;
      try
      {
        GetFolderResponseData data = MailApiTriplets.GetFolderFromTriplet(sessionHash, appKey, baseUrl, folderNumber);

        if (data != null)
        {
          if (data.IsJsoapFault)
          {
            MailApiException ex = new MailApiException(data.JsoapMessage, sessionHash, null, baseUrl, data.MailApiRequestString, data.MailApiResponseString);
            throw (ex);
          }

          result = MailApiTriplets.Convert(data);
        }
      }
      catch (Exception ex)
      {
        var exception = new AtlantisException("MailApiProvider.GetFolder", "0", ex.Message, string.Empty, null, null);
        Engine.Engine.LogAtlantisException(exception);
      }
      return result;
    }

    public MessageListResult GetMessageList(string sessionHash, string appKey, string baseUrl, int folderNumber, int offset, int count, string filter)
    {
      MessageListResult result = null;
      try
      {
        GetMessageListResponseData data = MailApiTriplets.GetMessageListFromTriplet(sessionHash, appKey, baseUrl, folderNumber, offset, count, filter);

        if (data != null)
        {
          if (data.IsJsoapFault)
          {
            MailApiException ex = new MailApiException(data.JsoapMessage, sessionHash, null, baseUrl, data.MailApiRequestString, data.MailApiResponseString);
            throw (ex);
          }

          result = MailApiTriplets.Convert(data);
        }
      }
      catch (Exception ex)
      {
        var exception = new AtlantisException("MailApiProvider.GetMessageList", "0", ex.Message, string.Empty, null, null);
        Engine.Engine.LogAtlantisException(exception);
      }
      return result;
    }

    public FolderListResult GetFolderList(string sessionHash, string appKey, string baseUrl)
    {
      FolderListResult result = null;
      try
      {
        GetFolderListResponseData data = MailApiTriplets.GetFolderListFromTriplet(sessionHash, appKey, baseUrl);

        if (data != null)
        {
          if (data.IsJsoapFault)
          {
            MailApiException ex = new MailApiException(data.JsoapMessage, sessionHash, string.Empty, baseUrl, data.MailApiRequestString, data.MailApiResponseString);
            throw (ex);
          }

          result = MailApiTriplets.Convert(data);
        }
      }
      catch (Exception ex)
      {
        var exception = new AtlantisException("MailApiProvider.GetFolderList", "0", ex.Message, string.Empty, null, null);
        Engine.Engine.LogAtlantisException(exception);
      }
      return result;
    }

    private int DetermineInboxFolderNum(GetFolderListResponseData getFolderListResponseData)
    {
      int inboxFolderNum = -1;
      foreach (MailFolder mailFolder in getFolderListResponseData.MailFolders)
      {
        if (mailFolder.Folder.Equals("INBOX", System.StringComparison.InvariantCultureIgnoreCase))
        {
          inboxFolderNum = mailFolder.FolderNum;
          break;
        }
      }
      if (inboxFolderNum == -1)
      {
        throw new Exception("Could not determine INBOX FolderNum after login");
      }
      return inboxFolderNum;
    }

  }
}
