using System;
using System.Runtime.Serialization;
using Atlantis.Framework.Ecc.Interface.Enums;

namespace Atlantis.Framework.Ecc.Interface
{
  [DataContract(Namespace = "")]
  public class EccOFFAccountDetails
  {
    const string OFF_PRODUCT_NAME = "OnlineFileFolder";

    [DataMember(Name = "user_id")]
    public string UserId { get; set; }

    [DataMember(Name = "user_num")]
    public string RecordId { get; set; }

    [DataMember(Name = "password")]
    public string Password { get; set; }

    [DataMember(Name = "quota_bytes")]
    public int QuotaBytes { get; set; }

    [DataMember(Name = "orion_uid")]
    private string orionUid { get; set; }

    [DataMember(Name = "AccountUid")]
    public string AccountUid
    {
      get { return orionUid; }
      set { orionUid = value; }
    }

    [DataMember(Name = "email")]
    public string EmailAddress { get; set; }

    [DataMember(Name = "status")]
    private string status { get; set; }
    public EccOFFAccountStatus AccountStatus
    {
      get
      {
        EccOFFAccountStatus myStatus;

        switch (status.ToUpperInvariant())
        {
          case "UNVERIFIED":
          case "U":
            myStatus = EccOFFAccountStatus.Unverified;
            break;
          case "A":
          case "ACTIVE":
            myStatus = EccOFFAccountStatus.Active;
            break;
          case "BLOCKED":
          case "B":
            myStatus = EccOFFAccountStatus.Blocked;
            break;
          case "EXPIRED":
          case "E":
            myStatus = EccOFFAccountStatus.Expired;
            break;
          case "DELETED":
          case "D":
            myStatus = EccOFFAccountStatus.Deleted;
            break;
          case "MIGRATING":
          case "M":
            myStatus = EccOFFAccountStatus.Migrating;
            break;
          case "LOCKED":
          case "L":
            myStatus = EccOFFAccountStatus.Locked;
            break;
          case "SUSPENDED":
          case "S":
            myStatus = EccOFFAccountStatus.Suspended;
            break;
          default:
            myStatus = EccOFFAccountStatus.Unverified;
            break;
        }

        return myStatus;
      }

      set
      {
        string myString = string.Empty;

        switch (value)
        {
          case EccOFFAccountStatus.Unverified:
            myString = status.Trim().Length == 1 ? "U" : "UNVERIFIED";
            break;
          case EccOFFAccountStatus.Active:
            myString = status.Trim().Length == 1 ? "A" : "ACTIVE";
            break;
          case EccOFFAccountStatus.Blocked:
            myString = status.Trim().Length == 1 ? "B" : "BLOCKED";
            break;
          case EccOFFAccountStatus.Expired:
            myString = status.Trim().Length == 1 ? "E" : "EXPIRED";
            break;
          case EccOFFAccountStatus.Deleted:
            myString = status.Trim().Length == 1 ? "D" : "DELETED";
            break;
          case EccOFFAccountStatus.Locked:
            myString = status.Trim().Length == 1 ? "L" : "LOCKED";
            break;
          case EccOFFAccountStatus.Migrating:
            myString = status.Trim().Length == 1 ? "M" : "MIGRATING";
            break;
          case EccOFFAccountStatus.Suspended:
            myString = status.Trim().Length == 1 ? "S" : "SUSPENDED";
            break;
        }

        status = myString;
      }
    }

    [DataMember(Name = "pl_id")]
    public int PrivateLabelId { get; set; }

    [DataMember(Name = "ProductName")]
    private string productName { get; set; }

    [DataMember(Name = "type")]
    private string type { get; set; }

    public EccAccountType ProductName
    {
      get
      {
        EccAccountType myType;
        switch (type.ToUpperInvariant())
        {
          case "P":
            myType = EccAccountType.OFFPaid;
            break;
          case "T":
          default:
            myType = EccAccountType.OFFTrial;
            break;
        }
        return myType;
      }
      set
      {
        if (value != EccAccountType.OFFPaid || value != EccAccountType.OFFTrial)
        {
          throw new ArgumentException("ProductName must be a valid OFF Type.");
        }
        string enumVal = string.Empty;
        switch (value)
        {
          case EccAccountType.OFFPaid:
            enumVal = "P";
            break;
          case EccAccountType.OFFTrial:
            enumVal = "T";
            break;
        }
        type = enumVal;
        productName = OFF_PRODUCT_NAME;
      }
    }

    [DataMember(Name = "expiration_date")]
    private string expirationDate { get; set; }
    public DateTime ExpirationDate
    {
      get
      {
        DateTime parse;
        DateTime.TryParse(expirationDate, out parse);
        return parse;
      }
      set { expirationDate = value.ToShortDateString(); }
    }
  }
}
