using Atlantis.Framework.Interface;
using Atlantis.Framework.MailApi.Interface;
using Atlantis.Framework.Providers.MailApi.Interface.Response;
using Atlantis.Framework.Providers.MailApi.Response;
using System;
using System.Text;

// triplet helper - makes calls through Triplets and converts their responses to provider objects

namespace Atlantis.Framework.Providers.MailApi
{
  internal static class MailApiTriplets
  {

    internal static LoginResponseData LoginViaTriplet(string username, string password, string appKey)
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

    internal static GetFolderListResponseData GetFolderListFromTriplet(string sessionHash, string appKey, string baseUrl)
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
        var exception = new AtlantisException("MailApiProvider.GetFolderList", "0", ex.Message, GetFolderListExceptionData(getFolderListReqData), null, null);
        Engine.Engine.LogAtlantisException(exception);
      }

      return getFolderListResponseData;
    }


    internal static GetFolderResponseData GetFolderFromTriplet(string sessionHash, string appKey, string baseUrl, int folderNumber)
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
        var exception = new AtlantisException("MailApiProvider.GetFolder", "0", ex.Message, GetFolderExceptionData(getFolderReqData), null, null);
        Engine.Engine.LogAtlantisException(exception);
      }
      return getFolderResponseData;
    }

    internal static  GetMessageListResponseData GetMessageListFromTriplet(string sessionHash, string appKey, string baseUrl, int folderNumber, int offset, int count, string filter)
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
        var exception = new AtlantisException("MailApiProvider.GetMessageList", "0", ex.Message, GetMessageListExceptionData(getMessageListReqData), null, null);
        Engine.Engine.LogAtlantisException(exception);
      }

      return getMessageListResponseData;
    }

    private static string GetLoginExceptionData(LoginRequestData loginReqData)
    {
      // should we include hash of the password here too, for potential matching against a customer provided password?

      StringBuilder sb = new StringBuilder();
      sb.Append("(login request: email: ");
      sb.Append(loginReqData.Username);
      sb.Append(" appKey: ");
      sb.Append(loginReqData.Appkey);
      sb.Append(")");
      return sb.ToString();
    }

    private static string GetFolderExceptionData(GetFolderRequestData getFolderReqData)
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

    private static string GetMessageListExceptionData(GetMessageListRequestData getMessageListReqData)
    {
      StringBuilder sb = new StringBuilder();
      sb.Append("(msgList request: mailApi url: ");
      sb.Append(getMessageListReqData.MailBaseUrl);
      sb.Append(" session: ");
      sb.Append(getMessageListReqData.MailHash);
      sb.Append(")");
      return sb.ToString();
    }

    internal static IFolderResult Convert(GetFolderResponseData data)
    {
      FolderResult result = new FolderResult();
      result.Folder = Convert(data.MailFolder);
      result.ResultCode = data.ResultCode;
      return result;
    }

    internal static IFolder Convert(MailFolder mailFolder)
    {
      Folder folder = new Folder();
      folder.DisplayName = mailFolder.DisplayName;
      folder.NumMessages = mailFolder.ExistsCount;
      folder.FolderName = mailFolder.Folder;
      folder.FolderNum = mailFolder.FolderNum;
      folder.NumRead = mailFolder.SeenCount;
      folder.UserNum = mailFolder.UserNum;

      // do we use FolderPrefs?

      return folder;
    }

    internal static IMessageListResult Convert(GetMessageListResponseData data)
    {
      MessageListResult messageListResult = new MessageListResult();
      messageListResult.ResultCode = data.ResultCode;
      messageListResult.IsMailApiFault = data.IsJsoapFault;
      messageListResult.Session = data.State.Session;

      foreach (MailHeader header in data.MessageListData.MailHeaderList)
      {
        IMessageHeader mailHeader = Convert(header);
        messageListResult.MessageHeaderList.Add(mailHeader);
      }
      return messageListResult;
    }

    private static IMessageHeader Convert(MailHeader header)
    {
      MessageHeader msgHeader = new MessageHeader();
      msgHeader.Cc = header.Cc;
      msgHeader.FileName = header.Filename;
      msgHeader.Flagged = header.Flagged;
      msgHeader.FolderNum = header.FolderNum;
      msgHeader.From = header.FromFld;
      msgHeader.FromSort = header.FromSort;  // what are these sort fields? (as subjectSort, ToSort)
      msgHeader.HasAttachment = header.HasAttachment;
      msgHeader.HeaderNum = header.HeaderNum;
      msgHeader.InternalDate = header.InternalDate;
      msgHeader.IsAnswered = header.IsAnswered;
      msgHeader.IsDraft = header.IsDraft;
      msgHeader.IsForwarded = header.IsForwarded;
      msgHeader.IsSecure = header.IsSecure;
      msgHeader.MessageId = header.MessageId;
      msgHeader.ModifiedDate = header.ModifiedDate;
      msgHeader.MsgDate = header.MsgDate;
      msgHeader.MsgSize = header.MsgSize;
      msgHeader.MsgUid = header.MsgUid;
      msgHeader.IsPreferred = header.Preferred;
      msgHeader.Priority = header.Priority;
      msgHeader.IsRead = header.Read;
      msgHeader.IsRecallable = header.Recallable;
      msgHeader.ReplyTo = header.ReplyTo;
      msgHeader.Subject = header.Subject;
      msgHeader.SubjectSort = header.SubjectSort;
      msgHeader.To = header.ToFld;
      msgHeader.ToSort = header.ToSort;

      return msgHeader;
    }

    internal static IFolderListResult Convert(GetFolderListResponseData data)
    {
      FolderListResult result = new FolderListResult();
      result.ResultCode = data.ResultCode;
      result.Session = data.State.Session;

      foreach (MailFolder mailFolder in data.MailFolders )
      {
        IFolder folder = Convert(mailFolder);
        result.FolderList.Add(folder);
      }
      return result;
    }

    internal static ILoginResult Convert(LoginResponseData data)
    {
      LoginResult loginResult = new LoginResult();
      loginResult.IsMailApiFault = data.IsJsoapFault;
      loginResult.Session = data.State.Session;
      loginResult.BaseUrl = data.LoginData.BaseUrl;
      return loginResult;
    }

  }
}
