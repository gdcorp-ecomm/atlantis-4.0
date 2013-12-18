using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Atlantis.Framework.Personalization.Interface
{
  [Serializable]
  public class Message
  {
    [XmlElement("MessageTrackingID")]
    public string MessageTrackingId { get; set; }
    [XmlElement("MessageID")]
    public string MessageId { get; set; }
    public string MessageName { get; set; }
    public string MessageSequence { get; set; }
    public List<MessageTag> MessageTags { get; set; }

    public Message()
    {
      MessageTags = new List<MessageTag>();
    }
  }
}
