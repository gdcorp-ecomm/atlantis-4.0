using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.Providers.PlaceHolder.PlaceHolders
{
  internal class TMSMessageData
  {
    internal string SelectedMessageName { get; private set; }
    internal string SelectedTagName { get; private set; }
    internal string SelectedTrackingId { get; private set; }

    internal TMSMessageData(string msgName, string tagName, string trackingId)
    {
      SelectedMessageName = msgName;
      SelectedTagName = tagName;
      SelectedTrackingId = trackingId;
    }
  }
}
