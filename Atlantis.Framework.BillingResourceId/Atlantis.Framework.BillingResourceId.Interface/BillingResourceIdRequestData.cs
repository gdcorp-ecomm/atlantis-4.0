using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BillingResourceId.Interface
{
  public class BillingResourceIdRequestData : RequestData
  {
    public TimeSpan RequestTimeout { get; set; }
    public int ProductId { get; set; }
    
    public BillingResourceIdRequestData(
      string shopperId,
      string sourceURL,
      string orderId,
      string pathway,
      int pageCount,
      int productId)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      this.RequestTimeout = new TimeSpan(0, 0, 2);
      this.ProductId = productId;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }
  }
}
