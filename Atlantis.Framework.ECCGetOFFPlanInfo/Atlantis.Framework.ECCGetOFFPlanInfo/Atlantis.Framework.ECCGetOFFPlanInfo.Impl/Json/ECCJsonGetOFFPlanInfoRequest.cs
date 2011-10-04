using System;
using System.Runtime.Serialization;
using Atlantis.Framework.Ecc.Interface.Enums;

namespace Atlantis.Framework.ECCGetOFFPlanInfo.Impl.Json
{
 
  [DataContract]
  public class ECCJsonGetOFFPlanInfoRequest
  {
    [DataMember(Name = "shopper")]
    public string ShopperId { get; set; }

    [DataMember(Name = "reseller")]
    public string ResellerId { get; set; }

    [DataMember(Name = "uid")]
    public string AccountUid { get; set; }

    [DataMember(Name = "status")]
    private int status { get; set; }
    public EccOFFPlanInfoRequestStatus Status {
      get { return (EccOFFPlanInfoRequestStatus)Enum.Parse(typeof(EccOFFPlanInfoRequestStatus), status.ToString()); }
      set { status = (int) value; }
    }

    [DataMember(Name = "subaccount")]
    public string SubAccount { get; set; }
  }
}
