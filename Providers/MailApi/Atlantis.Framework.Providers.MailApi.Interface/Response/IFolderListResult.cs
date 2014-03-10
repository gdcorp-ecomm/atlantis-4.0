using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.Providers.MailApi.Interface.Response
{
  public interface IFolderListResult : MailApiResult
  {
    List<IFolder> FolderList { get; }
  }
}
