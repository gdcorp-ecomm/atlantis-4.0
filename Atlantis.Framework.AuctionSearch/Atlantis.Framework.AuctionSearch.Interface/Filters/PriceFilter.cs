using System.Runtime.Serialization;

namespace Atlantis.Framework.AuctionSearch.Interface
{
  [DataContract]
  public class PriceFilter
  {
    [DataMember]
    public int MinPrice { get; set; }

    [DataMember]
    public int MaxPrice { get; set; }
  }
}
