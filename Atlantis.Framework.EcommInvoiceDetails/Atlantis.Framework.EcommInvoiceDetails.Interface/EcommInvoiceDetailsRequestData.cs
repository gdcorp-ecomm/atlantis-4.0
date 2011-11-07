using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommInvoiceDetails.Interface
{
  public class EcommInvoiceDetailsRequestData : RequestData
  {
    public string InvoiceId { get; set; }
    public EcommInvoiceDetailsRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, string invoiceId)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      InvoiceId = invoiceId;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }
  }
}
