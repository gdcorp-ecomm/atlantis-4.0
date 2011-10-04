using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ECCSetAutoResponder.Interface
{
  public class ECCSetAutoResponderRequestData : RequestData
  {
    public int PrivateLabelId { get; private set; }
    public string EmailAddress { get; private set; }
    public string AutoResponderMessage { get; private set; }
    public string AutoResponderSubject { get; private set; }
    public int AutoResponderStatus { get; private set; }
    public DateTime? AutoResponderStart { get; private set; }
    public DateTime? AutoResponderEnd { get; private set; }
    public string AutoResponderFrom { get; private set; }
    public bool SendSingleResponse { get; private set; }
    public string SubAccount { get; private set; }

    public ECCSetAutoResponderRequestData(string shopperId, 
      int privateLabelId, 
      string emailAddress, 
      string autoResponderMessage, 
      string autoResponderSubject, 
      int autoResponderStatus, 
      DateTime? autoResponderStart, 
      DateTime? autoResponderEnd, 
      string autoResponderFrom, 
      bool sendSingleResponse, 
      string subAccount, 
      string sourceURL, 
      string orderId, 
      string pathway, 
      int pageCount)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      PrivateLabelId = privateLabelId;
      EmailAddress = emailAddress;
      AutoResponderMessage = autoResponderMessage;
      AutoResponderSubject = autoResponderSubject;
      AutoResponderStatus = autoResponderStatus;
      AutoResponderStart = autoResponderStart;
      AutoResponderEnd = autoResponderEnd;
      AutoResponderFrom = autoResponderFrom;
      SendSingleResponse = sendSingleResponse;
      SubAccount = subAccount;
    }

    public override string GetCacheMD5()
    {
      throw new Exception("ECCSetAutoResponder is not a cacheable request.");
    }
  }
}
