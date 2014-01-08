using Atlantis.Framework.Personalization.Interface;
using System.Collections.Generic;

namespace Atlantis.Framework.Providers.Personalization.Interface
{
  public interface IPersonalizationProvider
  {
    TargetedMessages GetTargetedMessages(string interactionPoint);
    void AddToConsumedMessages(IConsumedMessage message);
    string TrackingData { get; }
    IEnumerable<IConsumedMessage> ConsumedMessages { get; }
  }
}
