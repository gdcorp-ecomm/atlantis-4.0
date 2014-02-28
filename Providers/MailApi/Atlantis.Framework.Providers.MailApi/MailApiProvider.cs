using Atlantis.Framework.Interface;
using Atlantis.Framework.MailApi.Interface;
using Atlantis.Framework.Providers.MailApi.DTOs;
using System;
using System.Text;

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

    public LoginFoldersInboxResponse LoginFetchFoldersAndInbox(string username, string password, string appKey)
    {
      LoginResponseData loginResponseData = null;
      GetFolderListResponseData getFolderListResponseData;
      GetMessageListResponseData getMessageListResponseData;
      LoginFoldersInboxResponse response = null; // consolidates login/folders/messages and gets returned

      loginResponseData = Login(username, password, appKey);
      // check for login failure here
      if (valid(loginResponseData))
      {
        getFolderListResponseData = GetFolderList(loginResponseData.LoginData.Hash, appKey, loginResponseData.LoginData.BaseUrl);
        // check for folder list failure here

        getMessageListResponseData = GetMessageList(getFolderListResponseData.State.Session, appKey, loginResponseData.LoginData.BaseUrl, DetermineInboxFolderNum(getFolderListResponseData), DEFAULT_OFFSET, DEFAULT_MESSAGE_COUNT, DEFAULT_FILTER);

        response = new LoginFoldersInboxResponse(getMessageListResponseData.State, loginResponseData.LoginData, getFolderListResponseData.MailFolders, getMessageListResponseData.MessageListData);
      }
      return response;
    }

    private bool valid(LoginResponseData loginResponseData)
    {
      bool valid = true;
      if (loginResponseData == null)
      {
        valid = false;
      }
      else if (loginResponseData.IsJsoapFault)
      {
        valid = false;
      }
      return valid;
    }



    public LoginResponseData Login(string username, string password, string appKey)
    {
      LoginRequestData loginReqData = null;
      LoginResponseData loginResponseData = null;

      try
      {
        loginReqData = new LoginRequestData(username, password, appKey);
        loginResponseData = (LoginResponseData)Engine.Engine.ProcessRequest(loginReqData, EngineRequests.MAILAPI_LOGIN);
      }
      catch (Exception ex)
      {
        var exception = new AtlantisException("MailApiProvider.GetFolderList", "0", ex.Message, GetLoginExceptionData(loginReqData), null, null);
        Engine.Engine.LogAtlantisException(exception);
      }

      return loginResponseData;
    }

    public GetFolderResponseData GetFolder(string sessionHash, string appKey, string baseUrl, int folderNumber)
    {
      GetFolderRequestData getFolderReqData = null;
      GetFolderResponseData getFolderResponseData = null;
      try
      {
        getFolderReqData = new GetFolderRequestData(folderNumber.ToString(), sessionHash, appKey, string.Empty, baseUrl);
        getFolderResponseData = (GetFolderResponseData)Engine.Engine.ProcessRequest(getFolderReqData, EngineRequests.MAILAPI_GET_FOLDER);
      }
      catch (Exception ex)
      {
        var exception = new AtlantisException("MailApiProvider.GetFolder", "0", ex.Message, GetFolderExceptionData(getFolderReqData),  null, null);
        Engine.Engine.LogAtlantisException(exception);
      }
      return getFolderResponseData;
    }



    public GetMessageListResponseData GetMessageList(string sessionHash, string appKey, string baseUrl, int folderNumber, int offset, int count, string filter)
    {
      GetMessageListRequestData getMessageListReqData = null;
      GetMessageListResponseData getMessageListResponseData = null;

      try
      {
        getMessageListReqData = new GetMessageListRequestData(folderNumber.ToString(), offset.ToString(), count.ToString(), filter, sessionHash, baseUrl, appKey);
        getMessageListResponseData = (GetMessageListResponseData)Engine.Engine.ProcessRequest(getMessageListReqData, EngineRequests.MAILAPI_MSG_LIST);
      }
      catch (Exception ex)
      {
        var exception = new AtlantisException("MailApiProvider.GetMessageList", "0", ex.Message, GetMessageListExceptionData(getMessageListReqData),  null, null);
        Engine.Engine.LogAtlantisException(exception);
      }

      return getMessageListResponseData;
    }


    public GetFolderListResponseData GetFolderList(string sessionHash, string appKey, string baseUrl)
    {
      GetFolderListRequestData getFolderListReqData = null;
      GetFolderListResponseData getFolderListResponseData = null;
      try
      {
        getFolderListReqData = new GetFolderListRequestData(sessionHash, appKey, string.Empty, baseUrl);
        getFolderListResponseData = (GetFolderListResponseData)Engine.Engine.ProcessRequest(getFolderListReqData, EngineRequests.MAILAPI_FOLDER_LIST);
      }
      catch (Exception ex)
      {
        var exception = new AtlantisException("MailApiProvider.GetFolderList", "0", ex.Message, GetFolderListExceptionData(getFolderListReqData),  null, null);
        Engine.Engine.LogAtlantisException(exception);
      }

      return getFolderListResponseData;
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

    private string GetLoginExceptionData(LoginRequestData loginReqData)
    {
      StringBuilder sb = new StringBuilder();
      sb.Append("(login request: email: ");
      sb.Append(loginReqData.Username);
      sb.Append(" appKey: ");
      sb.Append(loginReqData.Appkey);
      sb.Append(")");
      return sb.ToString();
    }

    private string GetFolderExceptionData(GetFolderRequestData getFolderReqData)
    {
      StringBuilder sb = new StringBuilder();
      sb.Append("(folder request: mailApi url: ");
      sb.Append(getFolderReqData.MailBaseUrl);
      sb.Append(" folderNum: ");
      sb.Append(getFolderReqData.FolderNum);
      sb.Append(" session: ");
      sb.Append(getFolderReqData.Session);
      sb.Append(")");
      return sb.ToString();
    }

    private static string GetFolderListExceptionData(GetFolderListRequestData getFolderListReqData)
    {
      StringBuilder sb = new StringBuilder();
      sb.Append("(folderList request: mailApi url: ");
      sb.Append(getFolderListReqData.MailBaseUrl);
      sb.Append(" session: ");
      sb.Append(getFolderListReqData.Session);
      sb.Append(")");
      return sb.ToString();
    }

    private string GetMessageListExceptionData(GetMessageListRequestData getMessageListReqData)
    {
      StringBuilder sb = new StringBuilder();
      sb.Append("(msgList request: mailApi url: ");
      sb.Append(getMessageListReqData.MailBaseUrl);
      sb.Append(" session: ");
      sb.Append(getMessageListReqData.MailHash);
      sb.Append(")");
      return sb.ToString();
    }

  }
}
