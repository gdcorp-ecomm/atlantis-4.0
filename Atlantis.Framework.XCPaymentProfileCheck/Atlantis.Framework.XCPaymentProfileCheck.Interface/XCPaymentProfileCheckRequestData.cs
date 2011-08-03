using System;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.XCPaymentProfileCheck.Interface
{
  public class XCPaymentProfileCheckRequestData : RequestData
  {
    public XCPaymentProfileCheckRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderId,
                                  string pathway,
                                  int pageCount)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(2d);
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in XCPaymentProfileCheckRequestData");     
    }
  }
}
