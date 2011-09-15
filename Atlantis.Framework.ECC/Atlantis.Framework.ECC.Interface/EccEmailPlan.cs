using System;
using System.Runtime.Serialization;
using Atlantis.Framework.Ecc.Interface.Enums;

namespace Atlantis.Framework.Ecc.Interface
{
  [DataContract]
  public class EccEmailPlan
  {
    [DataMember(Name = "AccountUid")]
    private string AccountUid { get; set; }
    public Guid AccountGuid
    {
      get { return new Guid(AccountUid); }
      set { AccountUid = value.ToString(); }
    }

    [DataMember]
    private string ProductName { get; set; }
    public EccAccountType ProductType
    {
      get { return (EccAccountType)Enum.Parse(typeof(EccAccountType), ProductName); }
      set { ProductName = value.ToString(); }
    }

    [DataMember]
    public string Status { get; set; }
    public EccAccountStatus AccountStatus
    {
      get
      {
        EccAccountStatus myStatus;
        try
        {
          myStatus = (EccAccountStatus)Enum.Parse(typeof(EccAccountStatus), Status);
        }
        catch
        {
          myStatus = EccAccountStatus.Unknown;
        }
        return myStatus;
      }
      set { Status = value.ToString(); }
    }

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

    [DataMember]
    public string Region { get; set; }

    [DataMember(Name = "pack_name")]
    public string Description { get; set; }

    public EccEmailPlan() { }
    public EccEmailPlan(string accountId, string productType, string status, string expiration, string region, string description)
    {
      AccountUid = accountId;
      ProductName = productType;
      Status = status;
      ExpireDate = expiration;
      Region = region;
      Description = description;
    }

    public EccEmailPlan(Guid accountId, EccAccountType productType, EccAccountStatus status, DateTime expiration, string region, string description)
    {
      AccountUid = accountId.ToString();
      ProductName = Enum.GetName(typeof(EccAccountType), productType);
      Status = Enum.GetName(typeof(EccAccountStatus), status);
      ExpireDate = expiration.ToShortDateString();
      Region = region;
      Description = description;
    }

  }
}
