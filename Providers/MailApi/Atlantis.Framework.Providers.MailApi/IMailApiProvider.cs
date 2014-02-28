using Atlantis.Framework.MailApi.Interface;
using Atlantis.Framework.Providers.MailApi.DTOs;

namespace Atlantis.Framework.Providers.MailApi
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
    LoginFoldersInboxResponse LoginFetchFoldersAndInbox(string username, string password, string appKey);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <param name="appKey"></param>
    /// <returns></returns>
    LoginResponseData Login(string username, string password, string appKey);  //object should be replaced with the DTO defined for composite call

    /// <summary>
    /// Get a single Folder
    /// </summary>
    /// <param name="sessionHash"></param>
    /// <param name="appKey"></param>
    /// <param name="baseUrl"></param>
    /// <param name="folderNumber"></param>
    /// <returns></returns>
    GetFolderResponseData GetFolder(string sessionHash, string appKey, string baseUrl, int folderNumber);
    GetMessageListResponseData GetMessageList(string sessionHash, string appKey, string baseUrl, int folderNumber, int offset, int count, string filter);
    GetFolderListResponseData GetFolderList(string sessionHash, string appKey, string baseUrl);
  }
}