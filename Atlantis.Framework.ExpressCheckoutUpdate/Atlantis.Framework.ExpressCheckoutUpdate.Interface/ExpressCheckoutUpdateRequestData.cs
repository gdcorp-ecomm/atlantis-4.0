using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ExpressCheckoutUpdate.Interface
{
  public class ExpressCheckoutUpdateRequestData : RequestData
  {
    public ExpressCheckoutUpdateRequestData(
      string shopperId,
      int paymentProfileId,
      string sourceURL,
      string orderId,
      string pathway,
      int pageCount)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(5);
      PaymentProfileId = paymentProfileId;
    }

    public int PaymentProfileId { get; set; }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }
  }
}