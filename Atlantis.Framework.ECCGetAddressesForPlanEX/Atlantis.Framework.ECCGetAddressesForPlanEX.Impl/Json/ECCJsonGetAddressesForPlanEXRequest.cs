using System.Runtime.Serialization;

namespace Atlantis.Framework.ECCGetAddressesForPlanEX.Impl.Json
{
  [DataContract]
  public class ECCJsonGetAddressesForPlanEXRequest
  {
    [DataMember(Name = "shopper")]
    public string ShopperId { get; set; }

    [DataMember(Name = "reseller")]
    public string ResellerId { get; set; }

    [DataMember(Name = "uid")]
    public string AccountUid { get; set; }
    
    [DataMember(Name = "active")]
    private int active { get; set; }
    public bool IncludeActiveOnly
    {
      get
      {
        bool parse;
        bool.TryParse(active.ToString(), out parse);
        return parse;
      }
      set { active = (value ? 1 : 0); }
    }

    [DataMember(Name = "subaccount")]
    public string SubAccount { get; set; }

    [DataMember(Name = "fields")]
    public string Fields { get; set; }
  }
}
