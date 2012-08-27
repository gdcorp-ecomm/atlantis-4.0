using System;
using System.Security.Cryptography;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.TrafficSmsTracking.Interface
{
  public class TrafficSmsTrackingRequestData : RequestData
  {
    public Guid SmsId { get; set; }

    public Guid MessageId { get; set; }

    private string _phoneNumber;
    public string PhoneNumber
    {
      get
      {
        return _phoneNumber;
      }
      set 
      { 
        _phoneNumber = value;
        _phoneNumberHash = GetHash(value);
      }
    }

    private string _phoneNumberHash;
    public string PhoneNumberHash
    {
      get { return _phoneNumberHash; }
    }

    public string MobileCarrier { get; set; }

    public string ActionPerformed { get; set; }

    public string MessageReceieved { get; set; }

    public string MessageSent { get; set; }

    public DateTime MessageReceivedDate { get; set; }

    public DateTime MessageSentDate { get; set; }

    public string SearchedSld { get; set; }

    public string SearchedTld { get; set; }

    public bool IsAvailable { get; set; }

    public string ProposedSld { get; set; }

    public string ProposedTld { get; set; }

    /// <summary>
    /// Default Timeout is 8 seconds
    /// </summary>
    private TimeSpan _requestTimout = TimeSpan.FromSeconds(8);
    public TimeSpan RequestTimeout
    {
      get { return _requestTimout; }
      set { _requestTimout = value; }
    }

    public TrafficSmsTrackingRequestData(Guid smsId,
                                         Guid messageId,
                                         string phoneNumber,
                                         string mobileCarrier,
                                         string actionPerformed,
                                         string messageReceived,
                                         string messageSent,
                                         DateTime messagedReceivedDate,
                                         DateTime messageSentDate,
                                         string searchedSld,
                                         string searchedTld,
                                         bool isAvailable,
                                         string proposedSld,                                   
                                         string proposedTld,
                                         string shopperId, 
                                         string sourceUrl, 
                                         string orderId, 
                                         string pathway, 
                                         int pageCount) : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      SmsId = smsId;
      MessageId = messageId;
      PhoneNumber = phoneNumber;
      MobileCarrier = mobileCarrier;
      ActionPerformed = actionPerformed;
      MessageReceieved = messageReceived;
      MessageSent = messageSent;
      MessageReceivedDate = messagedReceivedDate;
      MessageSentDate = messageSentDate;
      SearchedSld = searchedSld;
      SearchedTld = searchedTld;
      IsAvailable = isAvailable;
      ProposedSld = proposedSld;
      ProposedTld = proposedTld;
    }

    public override string GetCacheMD5()
    {
      throw new Exception("TrafficSmsTrackingRequest is not cacheable.");
    }

    private string GetHash(string value)
    {
      SHA1 sha1Provider = new SHA1CryptoServiceProvider();
      byte[] data = Encoding.ASCII.GetBytes(value);
      byte[] hashedData = sha1Provider.ComputeHash(data);
      
      StringBuilder hashBuilder = new StringBuilder();

      for (int i = 0; i < hashedData.Length; i++)
      {
        hashBuilder.Append(hashedData[i].ToString("X2"));
      }
      return hashBuilder.ToString();
    }
  }
}
