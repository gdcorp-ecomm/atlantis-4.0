using Atlantis.Framework.Interface;
using Atlantis.Framework.MailApi.Interface;
using Atlantis.Framework.Providers.MailApi.Response;
using Atlantis.Framework.Providers.MailApi.Interface;
using Atlantis.Framework.Providers.MailApi.Interface.Response;
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

    public ILoginFullResult
      LoginFetchFoldersAndInbox(string username, string password, string appKey)
    {
      ILoginFullResult result = null;

      LoginResponseData loginResponseData = null;
      GetFolderListResponseData getFolderListResponseData;
      GetMessageListResponseData getMessageListResponseData;

      loginResponseData = MailApiTriplets.LoginViaTriplet(username, password, appKey);
      // check for login failure here
      if (valid(loginResponseData))
      {
        getFolderListResponseData = MailApiTriplets.GetFolderListFromTriplet(loginResponseData.LoginData.Hash, appKey, loginResponseData.LoginData.BaseUrl);
        // check for folder list failure here

        getMessageListResponseData = MailApiTriplets.GetMessageListFromTriplet(getFolderListResponseData.State.Session, appKey, loginResponseData.LoginData.BaseUrl, DetermineInboxFolderNum(getFolderListResponseData), DEFAULT_OFFSET, DEFAULT_MESSAGE_COUNT, DEFAULT_FILTER);
        result = new LoginFullResult();
        result.Session = getFolderListResponseData.State.Session;
        result.BaseUrl = loginResponseData.LoginData.BaseUrl;
        result.MessageHeaderList = MailApiTriplets.Convert(getMessageListResponseData).MessageHeaderList;
        result.FolderList = MailApiTriplets.Convert(getFolderListResponseData).FolderList;
      }
      return result;
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

    public ILoginResult Login(string username, string password, string appKey)
    {
       LoginResponseData data = MailApiTriplets.LoginViaTriplet(username, password, appKey);
       ILoginResult result = MailApiTriplets.Convert(data);
       return result;
    }

    public IFolderResult GetFolder(string sessionHash, string appKey, string baseUrl, int folderNumber)
    {
      GetFolderResponseData data =  MailApiTriplets.GetFolderFromTriplet(sessionHash, appKey, baseUrl, folderNumber);
      IFolderResult result = MailApiTriplets.Convert(data);
      return result;
    }

    public IMessageListResult GetMessageList(string sessionHash, string appKey, string baseUrl, int folderNumber, int offset, int count, string filter)
    {
      GetMessageListResponseData data =  MailApiTriplets.GetMessageListFromTriplet(sessionHash, appKey, baseUrl, folderNumber, offset, count, filter);
      IMessageListResult result = MailApiTriplets.Convert(data);
      return result;
    }

    public IFolderListResult GetFolderList(string sessionHash, string appKey, string baseUrl)
    {
       GetFolderListResponseData data = MailApiTriplets.GetFolderListFromTriplet(sessionHash, appKey, baseUrl);
       IFolderListResult result = MailApiTriplets.Convert(data);
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
