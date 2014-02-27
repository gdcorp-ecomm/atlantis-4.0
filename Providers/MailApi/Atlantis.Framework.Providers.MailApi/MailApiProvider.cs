using Atlantis.Framework.Interface;
using Atlantis.Framework.MailApi.Interface;
using Atlantis.Framework.Providers.MailApi.DTOs;

namespace Atlantis.Framework.Providers.MailApi
{
  public class MailApiProvider : ProviderBase, IMailApiProvider
  {

    const string offset = "0";
    const string count = "20";
    const string filter = ""; // not sure what this is yet

    public string RestrictedKey { get; set; }
    public string AppKey { get; set; }
    public string MailHash { get; set; }

    public MailApiProvider(IProviderContainer container)
      : base(container)
    {
    }

    public MailApiProvider(IProviderContainer container, string mailHash, string appKey, string restrictedKey) : base(container)
    {
      MailHash = mailHash;
      AppKey = appKey;
      RestrictedKey = restrictedKey;
    }

    public CompositeLoginResponse LoginAndFetchFoldersAndInbox(string username, string password, string appKey)
    {
      LoginRequestData loginReqData = new LoginRequestData(username, password, appKey);
      LoginResponseData loginResponseData =(LoginResponseData)Engine.Engine.ProcessRequest(loginReqData, EngineRequests.MAILAPI_LOGIN);

      // check for login failure here

      GetFolderListRequestData getFolderListReqData = new GetFolderListRequestData(loginResponseData.LoginData.Hash, appKey, string.Empty, loginResponseData.LoginData.BaseUrl);
      GetFolderListResponseData getFolderListResponseData = (GetFolderListResponseData)Engine.Engine.ProcessRequest(getFolderListReqData, EngineRequests.MAILAPI_FOLDER_LIST);

      // check for folder list failure here

      GetMessageListRequestData getMessageListReqData = new GetMessageListRequestData(DetermineInboxFolderNum(getFolderListResponseData).ToString(), offset, count, filter, getFolderListResponseData.State.Session, loginResponseData.LoginData.BaseUrl, appKey);
      GetMessageListResponseData getMessageListResponseData = (GetMessageListResponseData)Engine.Engine.ProcessRequest(getMessageListReqData, EngineRequests.MAILAPI_MSG_LIST);

      CompositeLoginResponse response = new CompositeLoginResponse(getMessageListResponseData.State, loginResponseData.LoginData, getFolderListResponseData.MailFolders, getMessageListResponseData.MessageListData);
      
      return response;
    }

    private int DetermineInboxFolderNum(GetFolderListResponseData getFolderListResponseData)
    {
      int inboxFolderNum = -1;
      foreach (MailFolder mailFolder in getFolderListResponseData.MailFolders)
      {
        if (mailFolder.DisplayName.Equals("INBOX", System.StringComparison.InvariantCultureIgnoreCase))
        {
          inboxFolderNum = mailFolder.FolderNum;
          break;
        }
      }
      if (inboxFolderNum == -1)
      {
        throw new System.Exception("Could not determine INBOX FolderNum after login");
      }
      return inboxFolderNum;
    }

    public object Login(string username, string password)
    {
      throw new System.NotImplementedException();
    }


    public MailFolder GetFolder(string folderNumber)
    {
      throw new System.NotImplementedException();
    }

    public object GetMessageList(string folderNumber, int offset, int count, string filter)
    {
      throw new System.NotImplementedException();
    }

    public object GetFolderList()
    {
      throw new System.NotImplementedException();
    }
  }
}
