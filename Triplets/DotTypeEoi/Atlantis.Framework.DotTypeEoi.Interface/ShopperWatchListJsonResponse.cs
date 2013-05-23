using System.Runtime.Serialization;

namespace Atlantis.Framework.DotTypeEoi.Interface
{
  [DataContract]
  public class ShopperWatchListJsonResponse
  {
    [DataMember(Name = "shopperWatchList")]
    public ShopperWatchListResponse ShopperWatchListResponse { get; set; }
  }
}
