using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ExpressCheckoutProfileGet.Interface
{
  public class ExpressCheckoutProfileGetRequestData : RequestData
  {
    public ExpressCheckoutProfileGetRequestData(
      string shopperId,
      string sourceURL,
      string orderId,
      string pathway,
      int pageCount)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }
  }
}
