using System;
using System.Runtime.Serialization;
using Atlantis.Framework.Ecc.Interface.Enums;

namespace Atlantis.Framework.Ecc.Interface
{
  [DataContract]
  public class EccCalendarPlan
  {
    [DataMember(Name = "AccountUid")]
    private string AccountUid { get; set; }
    public Guid AccountGuid
    {
      get {return new Guid(AccountUid);}
      set { AccountUid = value.ToString(); }
    }

    [DataMember(Name = "ProductName")]
    private string ProductName { get; set; }

    public EccAccountType ProductType
    {
      get { return (EccAccountType)Enum.Parse(typeof(EccAccountType), ProductName); } 
      set { ProductName = value.ToString(); }
    }

    [DataMember(Name = "Status")]
    //TODO: Convert to ENUM?
    public string Status { get; set; }

    [DataMember(Name = "ExpireDate")]
    private string ExpireDate { get; set; }
    public DateTime ExpirationDate
    {
      get
      {
        DateTime expDate;
        DateTime.TryParse(ExpireDate, out expDate);

        return expDate;
      }
      set { ExpireDate = value.ToShortDateString(); }
    }

    [DataMember(Name = "addl_users_num")]
    public int UsersAdditional { get; set; }

    [DataMember(Name = "group_calendar_allowed_num")]
    public int UsersAllowed { get; set; }

    [DataMember(Name = "pf_id")]
    public int Pfid { get; set; }

    [DataMember(Name = "remaining_users")]
    public int UsersRemaining { get; set; }

  }
}
