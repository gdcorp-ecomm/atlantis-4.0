using System.Collections.Generic;

namespace Atlantis.Framework.Personalization.Interface
{
  public class Message
  {
    public string MessageTrackingId { get; set; }
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
