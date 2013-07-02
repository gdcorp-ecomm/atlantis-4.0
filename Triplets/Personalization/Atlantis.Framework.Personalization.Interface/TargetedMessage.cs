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
}
