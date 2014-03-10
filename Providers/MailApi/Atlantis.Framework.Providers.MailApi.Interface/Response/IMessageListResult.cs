using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.Providers.MailApi.Interface.Response
{
  public interface IMessageListResult : MailApiResult
  {
    List<IMessageHeader> MessageHeaderList { get; }

    bool IsMailApiFault { get; set; }
  }
}
