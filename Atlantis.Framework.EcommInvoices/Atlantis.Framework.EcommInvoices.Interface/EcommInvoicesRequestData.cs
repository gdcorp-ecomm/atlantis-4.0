using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommInvoices.Interface
{
  public class EcommInvoicesRequestData: RequestData
  {
    public RetrievalAttributes RetrievalAttributes { get; set; }
    public EcommInvoicesRequestData(string shopperId, RetrievalAttributes retAttributes)
    {
      RetrievalAttributes = retAttributes;
      ShopperID = shopperId;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }
  }
}
