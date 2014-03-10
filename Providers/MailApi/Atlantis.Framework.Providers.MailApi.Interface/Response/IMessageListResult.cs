using System.Collections.Generic;

namespace Atlantis.Framework.Providers.MailApi.Interface.Response
{
  public interface IMessageListResult : MailApiResult
  {
    List<IMessageHeader> MessageHeaderList { get; }

    bool IsMailApiFault { get; set; }
  }
}
