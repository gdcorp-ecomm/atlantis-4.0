using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.OrionEvent.Interface
{
  public class OrionInsertEventRequestData : RequestData
  {
    #region Properites
    // No Default Properties
    public string EventType { get; private set; }
    public string PrivateLabelId { get; private set; }
    public string ProductItemTypeValue { get; private set; }
    
    // With Parked Page Defaults - i.e. very likely nobody else will ever use this triplet
    public string AuditMessage { get; private set; }
    public string ProductItemType { get; private set; }
    public string Requestor { get; private set; }
    #endregion

    public OrionInsertEventRequestData(string shopperId
                                       , string sourceUrl
                                       , string orderId
                                       , string pathway
                                       , int pageCount
                                       , string eventType
                                       , string privateLabelId
                                       , string productItemTypeValue
                                       , string auditMessage = "One Page Website Setup call from ParkedPageSubmitHandler.ashx"
                                       , string productItemType = "DOMAIN"
                                       , string requestor = "MYA")
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      EventType = eventType;
      PrivateLabelId = privateLabelId;
      ProductItemTypeValue = productItemTypeValue;
      AuditMessage = auditMessage;
      ProductItemType = productItemType;
      Requestor = requestor;
      RequestTimeout = TimeSpan.FromMilliseconds(70000);
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in OrionInsertEventRequestData");     
    }
  }
}
