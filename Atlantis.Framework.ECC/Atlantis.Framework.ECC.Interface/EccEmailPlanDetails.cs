using System;
using System.Runtime.Serialization;
using Atlantis.Framework.Ecc.Interface.Enums;

namespace Atlantis.Framework.Ecc.Interface
{
  [DataContract(Namespace = "")]
  public class EccEmailPlanDetails
  {
    [DataMember(Name = "data_center")]
    public string DataCenter { get; set; }

    [DataMember(Name = "address_count")]
    public int AddressCount { get; set; }

    [DataMember(Name="address_used")]
    public int AddressesUsed { get; set; }

    [DataMember(Name = "address_remain")]
    public int AddressesRemaining{ get; set; }

    [DataMember(Name = "max_size")]
    public long MaxSize { get; set; }

    [DataMember(Name = "addl_disk_space_mb")]
    public long AdditionalDiskSpaceMb { get; set; }

    [DataMember(Name = "total_size")]
    public long TotalSize { get; set; }

    [DataMember(Name = "quota_assigned")]
    public long QuotaAssigned { get; set; }

    [DataMember(Name="quota_remaining")]
    public long QuotaRemaning { get; set; }

    [DataMember(Name = "imap")]
    private string isImap { get; set; }
    public bool IsImap { 
      get{
        bool parse = isImap == "1" ? true : false;
        return parse;
      }
      set { isImap = value ? "1" : "0"; }
    }

    [DataMember(Name = "poolable")]
    private string isPoolable { get; set; }
    public bool IsPoolable
    {
      get { 
        bool parse = isPoolable == "1" ? true : false;
        return parse;
      }
      set { isPoolable = value ? "1" : "0"; }
    }

    [DataMember(Name = "pack_uid")]
    public string PackUid { get; set; }
    public Guid PackGuid { get { return new Guid(PackUid); } }
    
    [DataMember(Name = "pack_id")]
    public int Id { get; set; }

    [DataMember(Name = "pack_plid")]
    public int PrivateLabelId { get; set; }

    [DataMember(Name = "pack_pfid")]
    public int Pfid{ get; set; }
    
    [DataMember(Name = "delivery_mode")]
    public int DeliveryMode { get; set; }
    public EccDeliveryMode AccountDeliveryMode
    {
      get { return (EccDeliveryMode)Enum.Parse(typeof (EccDeliveryMode), DeliveryMode.ToString()); }
    }

    [DataMember(Name = "delivery_type")]
    public string DeliveryType { get; set; }
    public EccDeliveryType AccountDeliveryType
    {
      get { return (EccDeliveryType)Enum.Parse(typeof(EccDeliveryType), DeliveryType); }
    }

    [DataMember(Name = "plan_type")]
    public string PlanType{ get; set; }

    [DataMember(Name = "AccountUid")]
    public string AccountUid { get; set; }
    public Guid AccountGuid { get { return new Guid(AccountUid);} }

    [DataMember]
    public string ProductName { get; set; }
    public EccAccountType ProductType
    {
      get { return (EccAccountType)Enum.Parse(typeof(EccAccountType), ProductName); }
    }

    [DataMember(Name = "status")]
    public string Status { get; set; }
    public EccAccountStatus AccountStatus {
      get { return (EccAccountStatus)Enum.Parse(typeof(EccAccountStatus), Status); }
    }

    [DataMember(Name = "expire_date")]
    public string ExpireDate { get; set; }
    public DateTime ExpirationDate
    {
       get
       {
         DateTime expDate;
         DateTime.TryParse(ExpireDate, out expDate);

         return expDate;
       }
    }

    [DataMember(Name = "country_code")]
    public string Region { get; set; }

    [DataMember(Name = "pack_name")]
    public string Description { get; set; }
      
    public EccEmailPlanDetails() {}
    public EccEmailPlanDetails(string accountId, string productType, string status, string expiration, string region, string description) 
    {
      AccountUid = accountId;
      ProductName = productType;
      Status = status;
      ExpireDate = expiration;
      Region = region;
      Description = description;
    }

    public EccEmailPlanDetails(Guid accountId, EccAccountType productType, EccAccountStatus status, DateTime expiration, string region, string description)
    {
      AccountUid = accountId.ToString();
      ProductName = Enum.GetName(typeof(EccAccountType), productType);
      Status = Enum.GetName(typeof(EccAccountStatus), status);
      ExpireDate = expiration.ToLongDateString();
      Region = region;
      Description = description;
    }


 

  }
}
