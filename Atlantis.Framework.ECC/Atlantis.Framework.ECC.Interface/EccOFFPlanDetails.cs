using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Atlantis.Framework.Ecc.Interface.Enums;

namespace Atlantis.Framework.Ecc.Interface
{
  [DataContract(Namespace = "")]
  public class EccOFFPlanDetails
  {
    /*
     *  AccountUid Orion Account UID of a Group OFF Plan
     * ProductName "OnlineFileFolder"
     * Status setup = available for customer use
     * pendremove = disabled/inactive/deleted
     * ExpireDate CCYY-MM-DD formatted date the OFF Plan expires
     */

    [DataMember(Name = "orion_uid")]
    private string accountUid { get; set; }
    public Guid AccountUid
    {
      get
      {
        return new Guid(accountUid);
      }
      set { accountUid = value.ToString(); }
    }

    [DataMember(Name = "ProductName")]
    public string ProductName { get; set; }


    [DataMember(Name = "status")]
    private string status { get; set; }
    public EccAccountStatus Status
    {
      get { return (EccAccountStatus)Enum.Parse(typeof(EccAccountStatus), status); }
    }

    [DataMember(Name = "ExpireDate")]
    private string expireDate { get; set; }
    public DateTime ExpirationDate
    {
      get
      {
        DateTime expDate;
        DateTime.TryParse(expireDate, out expDate);

        return expDate;
      }
    }
  }
}
