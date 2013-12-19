using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.Providers.PlaceHolder.PlaceHolders
{
  public class TMSMessageData : IEquatable<TMSMessageData>
  {
    public const string DATA_TOKEN_MESSAGES = "A.F.Providers.PlaceHolder.TMSMessages";
    public const string DATA_TOKEN_MESSAGE_Id = "TMSMessageId";
    public const string DATA_TOKEN_MESSAGE_NAME = "TMSMessageName";
    public const string DATA_TOKEN_MESSAGE_TAG = "TMSMessageTag";
    public const string DATA_TOKEN_MESSAGE_TRACKING_ID = "TMSMessageTrackingId";

    public string MessageId { get; private set; }
    public string MessageName { get; private set; }
    public string TagName { get; private set; }
    public string TrackingId { get; private set; }

    public TMSMessageData(string msgid, string msgName, string tagName, string trackingId)
    {
      MessageId = msgid;
      MessageName = msgName;
      TagName = tagName;
      TrackingId = trackingId;
    }

    public bool Equals(TMSMessageData other)
    {
      if (other == null)
        return false;

      if (this.MessageId == other.MessageId && this.MessageName == other.MessageName && this.TagName == other.TagName && this.TrackingId == other.TrackingId)
        return true;
      else
        return false;
    }
  }
}
