using Atlantis.Framework.MailApi.Interface;
using Atlantis.Framework.Providers.MailApi.DTOs;

namespace Atlantis.Framework.Providers.MailApi
{
  public interface IMailApiProvider
  {



    object Login(string username, string password);  //object should be replaced with the DTO defined for composite call
    CompositeLoginResponse LoginAndFetchFoldersAndInbox(string username, string password, string appKey);
    MailFolder GetFolder(string folderNumber);
    object GetMessageList(string folderNumber, int offset, int count, string filter);
    object GetFolderList();
  }
}