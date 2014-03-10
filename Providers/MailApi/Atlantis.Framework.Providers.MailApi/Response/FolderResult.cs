using Atlantis.Framework.Providers.MailApi.Interface.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.Providers.MailApi.Response
{
  class FolderResult : IFolderResult
  {
    public IFolder Folder { get; set; }

    public int ResultCode { get; set; }
    public bool IsJsoapFault { get; set; }
  }
}
