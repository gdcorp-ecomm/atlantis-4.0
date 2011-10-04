using System;
using System.Runtime.Serialization;

namespace Atlantis.Framework.ECCSetAutoResponder.Impl.Json
{
  [DataContract]
  public class EccJsonSetAutoResponderRequest
  {
    private const string NoDateTimeValue = "0000-00-00 00:00:00";

    [DataMember(Name = "shopper")]
    public string ShopperId { get; set; }

    [DataMember(Name = "reseller")]
    public string ResellerId { get; set; }

    [DataMember(Name = "emailaddress")]
    public string EmailAddress { get; set; }

    [DataMember(Name = "ar_message")]
    public string AutoResponderMessage { get; set; }

    [DataMember(Name = "ar_subject")]
    public string AutoResponderSubject { get; set; }

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
        }
        else
        {
          ar_status = value.ToString();
        }
      }
    }

    [DataMember]
    private string ar_start { get; set; }
    public DateTime? AutoResponderStart
    {
      get
      {
        DateTime startDate;
        DateTime.TryParse(ar_start, out startDate);
        return startDate == DateTime.MinValue ? (DateTime?)null : startDate;
      }
      set
      {
        if (!value.HasValue || value == DateTime.MinValue)
        {
          ar_start = NoDateTimeValue;
        }
        else
        {
          ar_start = value.Value.ToString("yyyy-MM-dd HH:mm:ss");
        }
      }
    }

    [DataMember]
    private string ar_end { get; set; }
    public DateTime? AutoResponderEnd
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
          ar_end = value.Value.ToString("yyyy-MM-dd HH:mm:ss");
        }
      }
    }

    [DataMember(Name = "ar_from")]
    public string AutoResponderFrom { get; set; }

    [DataMember]
    private string ar_single { get; set; }
    public bool SendSingleResponse
    {
      get
      {
        bool singleResp;
        bool.TryParse(ar_single, out singleResp);
        return singleResp;
      }
      set { ar_single = value ? "1" : "0"; }
    }

    [DataMember(Name = "subaccount")]
    public string SubAccount { get; set; }

  }
}
