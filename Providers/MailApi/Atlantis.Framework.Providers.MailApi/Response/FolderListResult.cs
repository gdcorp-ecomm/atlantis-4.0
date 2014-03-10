using Atlantis.Framework.Providers.MailApi.Interface.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.Providers.MailApi.Response
{
  class FolderListResult : IFolderListResult
  {
    private  List<IFolder> folderList = new List<IFolder>();

    public List<IFolder> FolderList { get { return folderList; } }

    public int ResultCode { get; set; }
    public bool IsMailApiFault { get; set; }
    public string Session { get; set; }
  }
}
