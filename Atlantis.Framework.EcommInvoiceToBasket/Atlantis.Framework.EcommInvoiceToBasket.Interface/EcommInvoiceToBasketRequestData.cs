using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommInvoiceToBasket.Interface
{
  public class EcommInvoiceToBasketRequestData: RequestData
  {
    public string UID { get; private set; }
    public string AlternateShopperId { get; private set; }
    public EcommInvoiceToBasketRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, string uid, string alternateShopperId = "")
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      UID = uid;
      AlternateShopperId = alternateShopperId;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }
  }
}
