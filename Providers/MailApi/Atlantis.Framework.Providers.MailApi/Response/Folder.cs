using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Providers.MailApi.Interface.Response;

namespace Atlantis.Framework.Providers.MailApi.Response
{
  class Folder : IFolder
  {
    public string DisplayName { get; set; }

    public string FolderName { get; set; }

    public int NumMessages { get; set; }

    public int FolderNum { get; set; }

    public int NumRead { get; set; }

    public int UserNum { get; set; }
  }
}
