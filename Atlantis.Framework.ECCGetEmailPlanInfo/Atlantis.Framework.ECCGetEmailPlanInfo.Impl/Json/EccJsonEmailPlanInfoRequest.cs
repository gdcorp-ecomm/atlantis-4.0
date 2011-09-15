using System.Runtime.Serialization;

namespace Atlantis.Framework.ECCGetEmailPlanInfoForShopper.Impl.Json
{
  [DataContract]
  public class EccJsonEmailPlanInfoRequest
  {
    [DataMember(Name = "shopper")]
    public string ShopperId { get; set; }

    [DataMember(Name = "reseller")]
    public string ResellerId { get; set; }

    [DataMember(Name = "uid")]
    public string AccountUid { get; set; }

    [DataMember(Name = "type")]
    public string EmailType { get; set; }

    [DataMember(Name = "dynamic_data")]
    private string DynamicData { get; set; }
    public bool DeepLoad
    {
      get
      {
        bool parse;
        bool.TryParse(DynamicData, out parse);
        return parse;
      }
      set { DynamicData = value ? "1" : "0"; }
    }

    [DataMember(Name = "subaccount")]
    public string SubAccount { get; set; }
  }
}
