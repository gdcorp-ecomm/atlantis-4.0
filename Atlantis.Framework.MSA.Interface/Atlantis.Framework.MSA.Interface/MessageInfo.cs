using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Atlantis.Framework.MSA.Interface
{
  [DataContract]
  public class MessageInfo
  {
    [DataMember(Name = "message_header")]
    public MessageHeader MessageHeader { get; set; }

    [DataMember(Name = "attachment_count")]
    public int AttachmentCount { get; set; }

    [DataMember(Name = "attachment_info")]
    public List<MessageAttachment> AttachmentList { get; set; }

    [DataMember(Name = "body_type")]
    public string BodyType { get; set; }
  }
}
