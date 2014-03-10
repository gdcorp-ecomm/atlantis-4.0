using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlantis.Framework.Providers.MailApi.Interface.Response
{
  public interface ILoginFullResult : MailApiResult
  {
    List<IMessageHeader> MessageHeaderList { get; }

    List<IFolder> FolderList { get; }

    string BaseUrl { get; set; }
  }
}
