using Atlantis.Framework.Providers.MailApi.Interface.Response;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Atlantis.Framework.Providers.MailApi.Response
{
  [DataContract]
  public class MessageListResult : IMessageListResult
  {
    private List<IMessageHeader> msgList = new List<IMessageHeader>();

    [DataMember]
    public List<IMessageHeader> MessageHeaderList { get { return msgList;  } }

    [DataMember]
    public int ResultCode { get; set; }

    [DataMember]
    public bool IsMailApiFault { get; set; }
    
    [DataMember]
    public string Session { get; set; }

  }
}
