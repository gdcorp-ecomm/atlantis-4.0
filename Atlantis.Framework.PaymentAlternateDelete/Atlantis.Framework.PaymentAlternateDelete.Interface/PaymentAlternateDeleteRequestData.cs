using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PaymentAlternateDelete.Interface
{
  public class PaymentAlternateDeleteRequestData: RequestData
  {

    public PaymentAlternateDeleteRequestData(string shopperId,
                                            string sourceURL,
                                            string orderId,
                                            string pathway,
                                            int pageCount)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in PaymentAlternateDeleteRequestData");
    }
  }
}
