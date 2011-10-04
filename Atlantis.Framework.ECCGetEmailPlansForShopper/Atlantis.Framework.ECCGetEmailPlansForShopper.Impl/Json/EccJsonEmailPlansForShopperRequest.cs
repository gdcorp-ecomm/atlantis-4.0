using System;
using System.Runtime.Serialization;

namespace Atlantis.Framework.ECCGetEmailPlansForShopper.Impl.Json
{
  [DataContract]
  public class EccJsonEmailPlansForShopperRequest
  {
    [DataMember(Name = "shopper")]
    public String ShopperId { get; set; }

    [DataMember(Name = "reseller")]
    public string ResellerId { get; set;}

    [DataMember(Name = "subaccount")]
    public string Subaccount { get; set; }

    [DataMember(Name = "type")]
    public string EmailType { get; set; }
  }
}
