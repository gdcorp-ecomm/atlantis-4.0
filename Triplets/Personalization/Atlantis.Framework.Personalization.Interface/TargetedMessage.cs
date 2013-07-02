using System.Collections.Generic;

namespace Atlantis.Framework.Personalization.Interface
{
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

  public class MessageTag
  {
    public string Name { get; set; }
  }
}
