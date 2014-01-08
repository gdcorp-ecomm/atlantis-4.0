using Atlantis.Framework.Providers.Personalization.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.Providers.Personalization.Tests
{
  public class MockConsumedMessage : IConsumedMessage
  {
    public MockConsumedMessage(string id, string msg, string tag, string trackignId)
    {
      MessageId = id;
      MessageName = msg;
      TagName = tag;
      TrackingId = trackignId;
    }

    public string MessageId { get; set; }

    public string MessageName { get; set; }

    public string TagName { get; set; }

    public string TrackingId { get; set; }

    public string TrackingData { get { return string.Concat(MessageId, MessageName, TagName, TrackingId); } }

  }
}
