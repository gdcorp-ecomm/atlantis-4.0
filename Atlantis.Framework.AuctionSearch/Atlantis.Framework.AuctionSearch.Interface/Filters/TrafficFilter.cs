using System.Runtime.Serialization;

namespace Atlantis.Framework.AuctionSearch.Interface
{
  [DataContract]
  public class TrafficFilter
  {
    [DataMember]
    public int MinTraffic { get; set; }

    [DataMember]
    public int MaxTraffic { get; set; }
  }
}
