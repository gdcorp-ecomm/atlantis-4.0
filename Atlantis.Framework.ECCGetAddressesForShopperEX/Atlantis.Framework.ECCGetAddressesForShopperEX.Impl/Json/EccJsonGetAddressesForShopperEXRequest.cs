using System.Runtime.Serialization;

namespace Atlantis.Framework.ECCGetAddressesForShopperEX.Impl.Json
{
  [DataContract]
  public class EccJsonGetAddressesForShopperEXRequest
  {
    [DataMember(Name = "shopper")]
    public string ShopperId { get; set; }

    [DataMember(Name = "reseller")]
    public string ResellerId { get; set; }

    [DataMember(Name = "type")]
    public string EmailType { get; set; }

    [DataMember(Name = "active")]
    public string Active { get; set; }

    [DataMember(Name = "subaccount")]
    public string SubAccount { get; set; }

    [DataMember(Name = "fields")]
    public string Fields { get; set; }
  }
}
