using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
namespace Atlantis.Framework.EcommInvoiceDetails.Interface
{
  class EcommInvoiceDetailsRequestData : RequestData
  {
    public EcommInvoiceDetailsRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {

    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }
  }
}
