using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommInvoiceCancel.Interface
{
  public class EcommInvoiceCancelRequestData: RequestData
  {
    public string UID { get; set; }
    public EcommInvoiceCancelRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, string uid)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      UID = uid;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }
  }
}
