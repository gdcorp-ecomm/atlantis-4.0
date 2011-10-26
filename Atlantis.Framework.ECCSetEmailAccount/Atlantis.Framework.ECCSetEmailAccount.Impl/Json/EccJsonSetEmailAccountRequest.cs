using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Atlantis.Framework.ECCSetEmailAccount.Impl.Json
{

  [DataContract]
  public class EccJsonSetEmailAccountRequest
  {
    private const string NoDateTimeValue = "0000-00-00 00:00:00";

    [DataMember(Name = "shopper")]
    public string ShopperId { get; set; }

    [DataMember(Name = "reseller")]
    public string ResellerId { get; set; }

    [DataMember(Name="emailaddress")]
    public string EmailAddress { get; set; }
    
    [DataMember]
    private string uid { get; set; }
    public Guid AccountUid { 
      get
      {
        return new Guid(uid);
      }
      set { uid = value.ToString(); }
    }

    [DataMember(Name="password")]
    public string Password { get; set; }

    [DataMember(Name="cc")]
    private string cc { get; set; }
    public List<string> CCList
    {
      get
      {
        List<string> ccList = new List<string>();
        
        if (cc.Contains(","))
        {
          ccList.AddRange(cc.Split(",".ToCharArray()));
        
        } else
        {
          ccList.Add(cc);
        }
        return ccList;
      }
      set
      {
        cc = string.Join(",", value.ToArray());
      }
    }

    [DataMember(Name="subaccount")]
    public string Subaccount { get; set; }

    [DataMember]
    private string disk_space { get; set; }
    public float DiskSpace {
      get
      {
        float diskSpace;
        float.TryParse(disk_space, out diskSpace);
        return diskSpace;
      }
      set { disk_space = value.ToString(); }
    }
   
    [DataMember]
    private string catchall { get; set; }
    public bool IsCatchAll {
      get
      {
        return catchall == "1" ? true : false;
      }
      set
      {
        catchall = value ? "1" : "0";
      }
    }

    [DataMember]
    private string smtp_relays { get; set; }
    public int SmtpRelays { 
      get { 
        int smtpRelays;
        int.TryParse(smtp_relays, out smtpRelays);
        return smtpRelays;
      }
      set
      {
        if ((value < 250 || value % 50 > 0) && value != 0)
        {
          //not allowed
          throw new ArgumentOutOfRangeException("value", "Value must either be greater than 250 and a multiple of 50 or can be 0.");
        } else
        {
          smtp_relays = value.ToString();
        } 
      }
    }
    
    [DataMember]
    private string spam_filter { get; set; }
    public bool IsSpamFilterActive { 
      get { return spam_filter == "1" ? true : false; }
      set {
        spam_filter = value ? "1" : "0";
      }
    }

    [DataMember]
    private string ar_single { get; set; }
    public bool SendSingleResponse
    {
      get { return ar_single == "1" ? true : false; }
      set { ar_single = value ? "1" : "0"; }
    }

    [DataMember(Name = "ar_subject")]
    public string AutoResponderSubject { get; set; }

    [DataMember(Name="ar_message")]
    public string AutoResponderMessage { get; set; }
 
    [DataMember(Name = "ar_from")]
    public string AutoResponderFromAddress{ get; set; }
    
    [DataMember]
    private string ar_start { get; set; }
    public DateTime? AutoResponderStartDate
    {
      get
      {
        DateTime startDate;
        DateTime.TryParse(ar_start, out startDate);
        return startDate == DateTime.MinValue ? (DateTime?) null : startDate;
      }
      set
      {
        if (!value.HasValue || value == DateTime.MinValue)
        {
          ar_start = NoDateTimeValue;
        } else
        {
          ar_start = value.Value.ToShortDateString();
        }
      }
    }

    [DataMember]
    private string  ar_end { get; set; }
    public DateTime? AutoResponderEndDate 
    {
      get
      {
        DateTime endDate;
        DateTime.TryParse(ar_start, out endDate);
        return endDate == DateTime.MinValue ? (DateTime?)null : endDate;
      }
      set
      {
        if (!value.HasValue || value == DateTime.MinValue)
        {
          ar_end = NoDateTimeValue;
        }
        else
        {
          ar_end = value.Value.ToShortDateString();
        }
      }
    }

    [DataMember]
    private string ar_status { get; set; }
    public int AutoResponderStatus
    {
      get
      {
        int status;
        int.TryParse(ar_status, out status);
        return status;
      }
      set
      {
        if (value < -1 && value > 1)
        {
          throw new ArgumentOutOfRangeException("value", "Value must fall within the following range: -1 to 1  -1 = No Change, 0 = Deactivate, 1 = Activate");
        } else
        {
          ar_status = value.ToString();
        }
      }
    }
    
   
  }
}
