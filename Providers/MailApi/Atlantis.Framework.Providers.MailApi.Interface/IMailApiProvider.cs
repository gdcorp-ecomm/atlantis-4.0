using Atlantis.Framework.Providers.MailApi.Interface.Response;


namespace Atlantis.Framework.Providers.MailApi.Interface
{
  public interface IMailApiProvider
  {
    /// <summary>
    /// consolidates calls to login a user, fetch their folders, and grab an initial message-list from their INBOX
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <param name="appKey"></param>
    /// <returns></returns>
    LoginFullResult LoginFull(string username, string password, string appKey);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <param name="appKey"></param>
    /// <returns></returns>
    LoginResult Login(string username, string password, string appKey);  //object should be replaced with the DTO defined for composite call

    /// <summary>
    /// Get a single Folder
    /// </summary>
    /// <param name="sessionHash"></param>
    /// <param name="appKey"></param>
    /// <param name="baseUrl"></param>
    /// <param name="folderNumber"></param>
    /// <returns></returns>
    FolderResult GetFolder(string sessionHash, string appKey, string baseUrl, int folderNumber);
    MessageListResult GetMessageList(string sessionHash, string appKey, string baseUrl, int folderNumber, int offset, int count, string filter);
    FolderListResult GetFolderList(string sessionHash, string appKey, string baseUrl);
  }
}