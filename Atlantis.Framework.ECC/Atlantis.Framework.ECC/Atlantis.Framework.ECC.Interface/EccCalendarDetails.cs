using System;
using System.Runtime.Serialization;
using Atlantis.Framework.Ecc.Interface.Enums;

namespace Atlantis.Framework.Ecc.Interface
{
  [DataContract]
  public class EccCalendarDetails
  {
    [DataMember(Name = "AccountUid")]
    private string AccountUid { get; set; }
    public Guid AccountGuid
    {
      get { return new Guid(AccountUid);}
      set { AccountUid = value.ToString(); }
    }

    [DataMember(Name="ProductName")]
    private string ProductName { get; set; }
    public EccAccountType AccountType
    {
      get { return (EccAccountType) Enum.Parse(typeof (EccAccountType), ProductName); }
      set { ProductName = value.ToString(); }
    }

    [DataMember(Name = "sid")]
    public string RecordId { get; set; }

    [DataMember(Name = "email_user")]
    public string Username { get; set; }

    [DataMember(Name = "email_domain")]
    public string Domain { get; set; }

    [DataMember(Name="name_first")]
    public string  FirstName { get; set; }
    
    [DataMember(Name="name_last")]
    public string LastName { get; set; }
    
    [DataMember(Name="last_login")]
    public String LastLogin { get; set; }
    
    
    [DataMember(Name = "account_status")]
    private string Status { get; set; }
    //A = Active, D = Deleted, U = Unverified/Pending EULA, E = Expired, B = Blocked
    public EccCalendarAccountStatus AccountStatus
    {
      get
      {
        EccCalendarAccountStatus myStatus;

        switch (Status)
        {
          case "U":
            myStatus = EccCalendarAccountStatus.Pending;
            break;
          case "A":
            myStatus = EccCalendarAccountStatus.Active;
            break;
          case "B":
            myStatus = EccCalendarAccountStatus.Blocked;
            break;
          case "E":
            myStatus = EccCalendarAccountStatus.Expired;
            break;
          case "D":
            myStatus = EccCalendarAccountStatus.Deleted;
            break;
          default:
            myStatus = EccCalendarAccountStatus.Pending;
            break;
        }

        return myStatus;
      }

      set
      {
        string myString = string.Empty;

        switch (value)
        {
          case EccCalendarAccountStatus.Pending:
            myString = "U";
            break;
          case EccCalendarAccountStatus.Active:
            myString = "A";
            break;
          case EccCalendarAccountStatus.Blocked:
            myString = "B";
            break;
          case EccCalendarAccountStatus.Expired:
            myString = "E";
            break;
          case EccCalendarAccountStatus.Deleted:
            myString = "D";
            break;
        }

        Status = myString;
      }
    }

    [DataMember(Name = "is_webmail")]
    public string IsWebmail { get; set; }

    [DataMember(Name = "passwd")]
    public string EncryptedPassword { get; set; }

    [DataMember(Name = "plid")]
    public string PrivateLabelId { get; set; }

    [DataMember(Name = "pack_uid")]
    private string PackUid { get; set; }
    public Guid CalendarPlanGuid
    {
      get {return new Guid(PackUid);}
      set { PackUid = value.ToString(); }
    }

    [DataMember(Name = "attr_uid")]
    private string AttrUid { get; set; }
    public Guid CalendarAccountGuid
    {
      get {return new Guid(AttrUid);}
      set { AttrUid = value.ToString(); }
    }
    
    [DataMember(Name="delete_date")]
    public string DeleteDate { get; set; }
    
    [DataMember(Name="emailaddress")]
    public string EmailAddress { get; set; }
  }
}
