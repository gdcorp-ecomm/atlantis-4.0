﻿using Atlantis.Framework.MailApi.Interface;

namespace Atlantis.Framework.Providers.MailApi
{
  public interface IMailApiProvider
  {
    object Login(string username, string password);  //object should be replaced with the DTO defined for composite call
    MailFolder GetFolder(string folderNumber);
    object GetMessageList(string folderNumber, int offset, int count, string filter);
    object GetFolderList();
  }
}