using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Atlantis.Framework.MSA.Interface
{
  [DataContract]
  public class MessageList
  {
   
    [DataMember(Name = "mail_headers")]
    public List<MessageHeader> MailHeaders { get; set; }

    [DataMember(Name = "timestamp")]
    public string TimeStamp { get; set; }

    [DataMember(Name = "total_messages")]
    public int TotalMessages { get; set; }


  }
}
