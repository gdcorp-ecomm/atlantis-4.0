using System;
using System.Runtime.Serialization;

namespace Atlantis.Framework.EccGetEmailAddressesForPlan.Impl.Json
{
  [DataContract(Namespace = "")]
  public class EccJsonGetEmailAddressesForPlanRequest
  {
    [DataMember(Name = "shopper")]
    public string ShopperId { get; set; }

    [DataMember(Name = "reseller")]
    public string ResellerId { get; set; }

    [DataMember(Name = "uid")]
    private string accountuid { get; set; }
    public Guid AccountUid
    {
      get { return new Guid(accountuid); }
      set { accountuid = value.ToString(); }
    }

    [DataMember]
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
  }
}
