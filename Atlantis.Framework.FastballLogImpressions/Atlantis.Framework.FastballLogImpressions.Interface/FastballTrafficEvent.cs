using System;

namespace Atlantis.Framework.FastballLogImpressions.Interface
{
  public enum FastballTrafficEventType
  {
    Click,
    Impression
  }

  public class FastballTrafficEvent
  {
    public string EventOfferId { get; set; }
    public string PageSequence { get; set; }
    public DateTime EventDate { get; set; }
    public FastballTrafficEventType EventType { get; set; }
  }
}
