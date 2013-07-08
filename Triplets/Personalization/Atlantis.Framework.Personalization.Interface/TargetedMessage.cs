using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Atlantis.Framework.Personalization.Interface
{
  [Serializable, XmlRoot("TargetedMessages")]
  public class TargetedMessages
  {
    public string SessionID { get; set; }
    public int ResultCode { get; set; }
    public List<Message> Messages { get; set; }

    public TargetedMessages()
    {
      Messages = new List<Message>();
    }
  }
}
