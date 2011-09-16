using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Atlantis.Framework.Ecc.Interface.Enums;

namespace Atlantis.Framework.Ecc.Interface
{
  [DataContract]
	public class EccEmailAddressDetails
	{
    [DataMember]
    private string create_timestamp{ get; set; }
    public DateTime CreateDate
    {
      get { 
        DateTime parse;
        DateTime.TryParse(create_timestamp, out parse);
        return parse;
      }
      set { create_timestamp = value.ToShortDateString(); }
    }

    [DataMember]
    private string last_updated_timestamp{ get; set; }
    public DateTime LastUpdatedDate
    {
      get
      {
        DateTime parse;
        DateTime.TryParse(last_updated_timestamp, out parse);
        return parse;
      }
      set { last_updated_timestamp = value.ToShortDateString(); }
    }

    [DataMember]
    private string use_smtp_relay{ get; set; }
    public bool UseSmtpRelay {
      get { return use_smtp_relay == "1" ? true : false; }
      set { use_smtp_relay = value ? "1" : "0"; }
    }

    [DataMember]
    private string pod_id{ get; set; }
    public long PodId {
      get
      {
        long parse;
        long.TryParse(pod_id, out parse);
        return parse;
      }
      set { pod_id = value.ToString(); }
    }

    [DataMember(Name = "addr")]
    public string EmailAddress { get; set; }

    [DataMember]
    private string pack_uid { get; set; }
    public Guid PackUid
    {
      get { return new Guid(pack_uid);}
      set { pack_uid = value.ToString(); }
    }

    [DataMember]
    private string has_imap { get; set; }
    public bool HasImap
    {
      get { return has_imap == "1" ? true : false; }
      set { has_imap = value ? "1" : "0"; }
    }

    [DataMember]
    private string has_rim { get; set; }
    public bool HasRim
    {
      get { return has_rim == "1" ? true : false; }
      set { has_rim = value ? "1" : "0"; }
    }
    [DataMember]
    private string has_calendar { get; set; }
    public bool HasCalendar
    {
      get { return has_calendar == "1" ? true : false; }
      set { has_calendar = value ? "1" : "0"; }
    }

    [DataMember]
    private string has_off { get; set; }
    public bool HasOff
    {
      get { return has_off == "1" ? true : false; }
      set { has_off = value ? "1" : "0"; }
    }

    [DataMember(Name = "ar_status")]
    private string ar_status { get; set; }
    public int AutoResponderStatus {
      get
      {
        int parse;
        int.TryParse(ar_status, out parse);
        if (!int.TryParse(ar_status, out parse))
        {
          parse = 0;
        }

        return parse;
      }
      set { ar_status = value.ToString(); }
    }

    [DataMember(Name = "ar_message")]
    public string AutoResponderMessage { get; set; }

    [DataMember(Name = "ar_subject")]
    public string AutoResponderSubject { get; set; }

    [DataMember(Name = "ar_from")]
    public string AutoResponderFrom { get; set; }

    [DataMember(Name = "ar_start")]
    public string AutoResponderStartDate { get; set; }

    [DataMember(Name = "ar_end")]
    public string AutoResponderEndDate { get; set; }

    [DataMember(Name = "ar_single_reply")]
    private string ar_single { get; set; }
    public int AutoResponderSingleResponse {
      get
      {
        int parse;
        int.TryParse(ar_single, out parse);
        if (!int.TryParse(ar_single, out parse))
        {
          parse = 0;
        }

        return parse;
      }
      set { ar_single = value.ToString(); }
    }

    [DataMember]
    private string password_salt{ get; set; }

    [DataMember]
    private string relays_per_day{ get; set; }
    public int RelaysPerDay
    {
      get
      {
        int parse;
        int.TryParse(relays_per_day, out parse);
        return parse;
      }
      set { relays_per_day = value.ToString(); }
    }

    [DataMember]
    private string relays_today { get; set; }
    public int RelaysToday
    {
      get
      {
        int parse;
        int.TryParse(relays_today, out parse);
        return parse;
      }
      set { relays_today = value.ToString(); }
    }
    
    [DataMember]
    private string AccountUid { get; set; }
    public Guid AccountGuid {
      get { return new Guid(AccountUid);}
    }

    [DataMember]
    private string ProductName { get; set; }
    public EccAccountType AccountType {
      get { return (EccAccountType)Enum.Parse(typeof(EccAccountType), ProductName); }
      set { ProductName = value.ToString(); }
    }

    [DataMember(Name = "email_address_id")]
    public string EmailAddressId { get; set; }

    [DataMember]
    private string pack_id { get; set; }
    public Guid PackId
    {
      get { return new Guid(pack_id);}
      set { pack_id = value.ToString(); } 
    }
    
    [DataMember]
    private string folder_id{ get; set; }
  
    [DataMember]
    private string email_address_status { get; set; }
    
    [DataMember(Name = "username")]
    public string Username { get; set; }
    
    [DataMember(Name = "domain")]
    public string Domain{ get; set; }
    
    [DataMember(Name="password")]
    public string Password { get; set; }
    
    [DataMember]
    private string email_id { get; set; }
  
    [DataMember]
    private string recurring_id { get; set; }
    
    [DataMember]
    private string catchall { get; set; }
    public bool  IsCatchAll
    {
      get { 
        bool parse = catchall == "1" ? true : false;
        return parse;
      }
      set { catchall = (value ? "1" : "0"); }
    }

    [DataMember]
    private string isSpamFilterOn { get; set; }
    public bool HasSpamFilter
    {
      get {
        bool parse = isSpamFilterOn == "1" ? true : false;
        return parse;
      }
      set { isSpamFilterOn = (value ? "1" : "0"); }
    } 

    [DataMember]
    private string imapStatus { get; set; }
    public bool IsImapEnabled
    {
      get
      {
        bool parse = imapStatus == "1" ? true : false;
        return parse;
      }
      set { imapStatus = (value ? "1" : "0"); }
    }

    [DataMember]
    private string virus_recurring_id { get; set; }
    
    [DataMember]
    private string alias_redirect { get; set; }
    public List<string>CCList
    {
      get
      {
        List<string> ccList = new List<string>();

        if (alias_redirect.Contains(","))
        {
          ccList.AddRange(alias_redirect.Split(",".ToCharArray()));
        }
        else
        {
          ccList.Add(alias_redirect);
        }
        return ccList;
      }
      set
      {
        alias_redirect = string.Join(",", value.ToArray());
      }
    }
    
    [DataMember]
    private string other_language { get; set; }
    
    [DataMember]
    private string max_size { get; set; }
    public int MaxSize
    {
      get
      {
        int parse;
        int.TryParse(max_size, out parse);
        return parse;
      }
      set { max_size = value.ToString(); }
    }
    
    [DataMember]
    private string used_bytes { get; set; }
    public int UsedBytes
    {
      get {
        int parse;
        int.TryParse(used_bytes, out parse);
        return parse;
      }
      set { used_bytes = value.ToString(); }

    }
    
    [DataMember]
    private string quota_bytes { get; set; }
    public int QuotaBytes
    {
      get
      {
        int parse;
        int.TryParse(quota_bytes, out parse);
        return parse;
      }
      set { quota_bytes = value.ToString(); }

    }

    [DataMember]
    private string remaining_space { get; set; }
    public int RemainingSpace
    {
      get
      {
        int parse;
        int.TryParse(remaining_space, out parse);
        return parse;
      }
      set { remaining_space = value.ToString(); }
    }

    [DataMember]
    private int status { get; set; }
    public EccEmailAddressStatus Status
    {
      get { return (EccEmailAddressStatus)Enum.Parse(typeof(EccEmailAddressStatus), status.ToString()); }
      set { status = (int)value; }
    }

	}
}
