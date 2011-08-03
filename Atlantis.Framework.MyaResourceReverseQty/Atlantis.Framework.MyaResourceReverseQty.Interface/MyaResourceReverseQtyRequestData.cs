using System;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MyaResourceReverseQty.Interface
{
  public class MyaResourceReverseQtyRequestData : RequestData
  {
    public int PrivateLabelId { get; set; }
    public int BillingResourceId { get; set; }

    public MyaResourceReverseQtyRequestData(string shopperId
      , string sourceUrl
      , string orderId
      , string pathway
      , int pageCount
      , int privateLabelId
      , int billingResourceId)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(5d);
      PrivateLabelId = privateLabelId;
      BillingResourceId = billingResourceId;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in MyaResourceReverseQtyRequestData");
    }

  }
}
