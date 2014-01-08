using Atlantis.Framework.Providers.Personalization.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.Providers.PlaceHolder.PlaceHolders
{
  internal class ConsumedMessage : IConsumedMessage, IEquatable<ConsumedMessage>, IEqualityComparer<ConsumedMessage>
  {
    private const string DELIM = ".";

    public string MessageId { get; private set; }
    public string MessageName { get; private set; }
    public string TagName { get; private set; }
    public string TrackingId { get; private set; }

    public string TrackingData { get { return string.Concat(MessageId, DELIM, TrackingId); } }

    public ConsumedMessage(string msgid, string msgName, string tagName, string trackingId)
    {
      MessageId = msgid;
      MessageName = msgName;
      TagName = tagName;
      TrackingId = trackingId;
    }

    public bool Equals(ConsumedMessage other)
    {
      return (this.MessageName == other.MessageName) && (this.TagName == other.TagName);
    }

    public bool Equals(ConsumedMessage x, ConsumedMessage y)
    {
      if (Object.ReferenceEquals(x, y)) return true;

      return x.Equals(y);
    }

    public int GetHashCode(ConsumedMessage obj)
    {
      return obj.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      if (obj is ConsumedMessage)
        return this.Equals(obj as ConsumedMessage);
      else
        return false;
    }

    public override int GetHashCode()
    {
      return MessageName.GetHashCode() ^ TagName.GetHashCode();
    }
  }
}
