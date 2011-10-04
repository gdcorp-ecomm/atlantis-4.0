using System;
using System.Runtime.Serialization;

namespace Atlantis.Framework.EccRenameEmailPlan.Impl.Json
{
  [DataContract(Namespace = "")]
  public class EccJsonRenameEmailPlanRequest
  {
    [DataMember(Name="shopper")]
    public string ShopperId { get; set; }

    [DataMember(Name= "subaccount")]
    public string Subaccount { get; set; }

    [DataMember]
    private string reseller { get; set; }
    public int ResellerId
    {
      get
      {
        int parse;
        int.TryParse(reseller, out parse);
        return parse;
      }
      set { reseller = value.ToString(); }
    }

    [DataMember(Name = "uid")]
    private string accountUid { get; set; }
    public Guid AccountUid { 
      get { return new Guid(accountUid);}
      set { accountUid = value.ToString(); }
    }

    [DataMember(Name = "name")]
    public string NewPlanName { get; set; }
  }
}
