using System.Runtime.Serialization;

namespace Atlantis.Framework.ECCGetAddressesForShopper.Impl.Json
{
  [DataContract]
  public class EccJsonGetAddressesForShopperRequest
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
  }
}
