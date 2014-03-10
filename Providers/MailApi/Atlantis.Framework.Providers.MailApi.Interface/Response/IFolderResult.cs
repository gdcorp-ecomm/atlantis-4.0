using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.Providers.MailApi.Interface.Response
{
  public interface IFolderResult
  {
    IFolder Folder { get; set; }

    int ResultCode { get; set; }

    bool IsJsoapFault { get; set; }
  }
}
