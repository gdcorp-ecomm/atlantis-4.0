using Atlantis.Framework.Providers.MailApi.Interface.Response;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Atlantis.Framework.Providers.MailApi.Interface.Response
{
  [DataContract]
  public class MessageListResult : MailApiResult
  {
    private List<MessageHeader> msgList = new List<MessageHeader>();

    [DataMember]
    public List<MessageHeader> MessageHeaderList { get { return msgList;  } }

    [DataMember]
    public int ResultCode { get; set; }



  }
}
