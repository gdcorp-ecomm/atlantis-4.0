using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlantis.Framework.Providers.MailApi.Interface.Response
{
  public interface ILoginFullResult : MailApiResult
  {
    List<IMessageHeader> MessageHeaderList { get; set;  }

    List<IFolder> FolderList { get; set;  }

    string BaseUrl { get; set; }
  }
}
