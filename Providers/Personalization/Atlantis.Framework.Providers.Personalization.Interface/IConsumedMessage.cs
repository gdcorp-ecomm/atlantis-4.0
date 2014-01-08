using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.Providers.Personalization.Interface
{
  public interface IConsumedMessage
  {
    string MessageId { get; }
    string MessageName { get; }
    string TagName { get; }
    string TrackingId { get; }

    string TrackingData { get; }
  }
}
