using System.Collections.Generic;

namespace Atlantis.Framework.Providers.MailApi.Interface.Response
{
  public interface IFolderListResult : MailApiResult
  {
    List<IFolder> FolderList { get; }
  }
}
