using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CreateIncidentInIRIS.Interface
{
  public class CreateIncidentInIRISRequestData : RequestData
  {    
    //<subscriberID>int</subscriberID>
    //  <subject>string</subject>
    //  <Note>string</Note>
    //  <customerEmailAddress>string</customerEmailAddress>
    //  <originalIPAddress>string</originalIPAddress>
    //  <groupID>int</groupID>
    //  <serviceID>int</serviceID>
    //  <privateLabelID>int</privateLabelID>
    //  <shopperID>string</shopperID>
    //  <createdBy>string</createdBy>

    private int _subscriberId;
    private string _subject;
    private string _note;
    private string _customerEmailAddress;
    private string _originalIPAddress;
    private int _groupId;
    private int _serviceId;
    private int _privateLabelId;
    private string _shopperId;
    private string _createdBy;

    public CreateIncidentInIRISRequestData(
      string shopperId, string sourceUrl, string orderId, string pathway, int pageCount,
      int subscriberId, string subject, string note, string customerEmailAddress, string originalIPAddress,
      int groupId, int serviceId, int privateLabelId, string createdBy)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      _subscriberId = subscriberId;
      _subject = subject;
      _note = note;
      _customerEmailAddress = customerEmailAddress;
      _originalIPAddress = originalIPAddress;
      _groupId = groupId;
      _serviceId = serviceId;
      _privateLabelId = privateLabelId;
      _shopperId = shopperId;
      _createdBy = createdBy;
    }

    public int SubscriberId
    {
      get { return _subscriberId; }
    }

    public string Subject
    {
      get { return _subject; }
    }

    public string Note
    {
      get { return _note; }
    }

    public string CustomerEmailAddress
    {
      get { return _customerEmailAddress; }
    }

    public string OriginalIPAddress
    {
      get { return _originalIPAddress; }
    }

    public int GroupId
    {
      get { return _groupId; }
    }

    public int ServiceId
    {
      get { return _serviceId; }
    }

    public int PrivateLabelId
    {
      get { return _privateLabelId; }
    }

    public string ShopperId
    {
      get { return _shopperId; }
    }

    public string CreatedBy
    {
      get { return _createdBy; }
    }


    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }
  }
}
