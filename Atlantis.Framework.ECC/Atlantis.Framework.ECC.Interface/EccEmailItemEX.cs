using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Atlantis.Framework.Ecc.Interface.Enums;

namespace Atlantis.Framework.Ecc.Interface
{
  [DataContract]
  public class EccEmailItemEX
  {
    [DataMember(Name ="addr",IsRequired=true)]
    public string EmailAddress { get; set; }

    [DataMember(Name="pack_uid")]
    private string packUid { get; set; }
    public Guid PackUid
    {
      get { return new Guid(packUid);}
      set { packUid = value.ToString(); }
    }
    
    [DataMember(Name="status")]
    private int status { get; set; }
    public EccEmailAddressStatus Status
    {
      get { return (EccEmailAddressStatus)Enum.Parse(typeof(EccEmailAddressStatus), status.ToString()); }
      set { status = (int)value; }
    }
    
    [DataMember(Name="email_id")]
    public string EmailId { get; set; }
    
    [DataMember(Name="catchall")]
    private string isCatchall { get; set; }
    public bool  IsCatchAll
    {
      get { 
        bool parse = isCatchall == "1" ? true : false;
        return parse;
      }
      set { isCatchall = (value ? "1" : "0"); }
    }
    
    [DataMember(Name="isSpamFileterOn")]
    private string hasSpamfilter { get; set; }
    public bool HasSpamFilter
    {
      get {
        bool parse = hasSpamfilter == "1" ? true : false;
        return parse;
      }
      set { hasSpamfilter = (value ? "1" : "0"); }
    } 

    [DataMember(Name="imapStatus")]
    private string isImapEnabled { get; set; }
    public bool IsImapEnabled
    {
      get
      {
        bool parse = isImapEnabled == "1" ? true : false;
        return parse;
      }
      set { isImapEnabled = (value ? "1" : "0"); }
    }

    [DataMember(Name = "virus_recurring_id")]
    public string VirusRecurringId { get; set; }

    [DataMember(Name="alias_redirect")]
    private string ccList { get; set; }
    public List<string>CCList
    {
      get
      {
        List<string> list = new List<string>();

        if (ccList.Contains(","))
        {
          list.AddRange(ccList.Split(",".ToCharArray()));
        }
        else
        {
          list.Add(ccList);
        }
        return list;
      }
      set
      {
        ccList = string.Join(",", value.ToArray());
      }
    }

    [DataMember(Name="other_language")]
    public string OtherLanguage { get; set; }

    [DataMember(Name = "use_smtp_relay")]
    private string useSmtpRelay { get; set; }
    public bool UseSmtpRelay
    {
      get { return useSmtpRelay == "1" ? true : false; }
      set { useSmtpRelay = value ? "1" : "0"; }
    }

    [DataMember(Name = "single_reply")]
    private string autoResponderSingleResponse { get; set; }
    public int AutoResponderSingleResponse
    {
      get
      {
        int parse;
        int.TryParse(autoResponderSingleResponse, out parse);
        if (!int.TryParse(autoResponderSingleResponse, out parse))
        {
          parse = 0;
        }

        return parse;
      }
      set { autoResponderSingleResponse = value.ToString(); }
    }

    [DataMember(Name = "delivery_mode")]
    private string deliveryMode { get; set; }
    public EccAccountType MailboxType
    {
      get
      {
        EccAccountType rType = EccAccountType.email;

        switch (deliveryMode)
        {
          case "local":
            rType = EccAccountType.email;
            break;
          case "forward":
            rType = EccAccountType.emailforwarding;
            break;
        }
        return rType;
      }
      set
      {
        switch (value)
        {
          case EccAccountType.email:
            deliveryMode = "local";
            break;
          case EccAccountType.emailforwarding:
            deliveryMode = "forward";
            break;
        }
      }
    }

    [DataMember(Name = "status_descr")]
    public string StatusDesc { get; set; }
    

  }
}
