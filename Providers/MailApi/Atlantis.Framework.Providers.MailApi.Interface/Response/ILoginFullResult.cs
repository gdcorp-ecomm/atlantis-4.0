using System.Collections.Generic;

namespace Atlantis.Framework.Providers.MailApi.Interface.Response
{
  public interface ILoginFullResult : MailApiResult
  {
    List<IMessageHeader> MessageHeaderList { get; set;  }

    List<IFolder> FolderList { get; set;  }

    string BaseUrl { get; set; }
  }
}
