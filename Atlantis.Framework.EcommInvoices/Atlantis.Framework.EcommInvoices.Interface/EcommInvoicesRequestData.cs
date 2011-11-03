using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommInvoices.Interface
{
  class EcommInvoicesRequestData: RequestData
  {
    public EcommInvoicesRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    { 
    
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }
  }
}
