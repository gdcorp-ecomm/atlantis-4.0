using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommInvoices.Interface
{
  public class EcommInvoicesRequestData: RequestData
  {
    public RetrievalAttributes RetrievalAttributes { get; set; }
    public EcommInvoicesRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, RetrievalAttributes retAttributes)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      RetrievalAttributes = (RetrievalAttributes)retAttributes;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }
  }
}
