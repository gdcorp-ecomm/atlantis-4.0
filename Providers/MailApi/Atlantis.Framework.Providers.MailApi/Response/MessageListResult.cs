using Atlantis.Framework.Providers.MailApi.Interface.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.Providers.MailApi.Response
{
  public class MessageListResult : IMessageListResult
  {
    private List<IMessageHeader> msgList = new List<IMessageHeader>();

    public List<IMessageHeader> MessageHeaderList { get { return msgList;  } }

    public int ResultCode { get; set; }

    public bool IsMailApiFault { get; set; }
    public string Session { get; set; }

  }
}
