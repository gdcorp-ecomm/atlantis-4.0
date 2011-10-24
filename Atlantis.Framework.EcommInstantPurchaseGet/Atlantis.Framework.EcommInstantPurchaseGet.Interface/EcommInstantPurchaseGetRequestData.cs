using System;
using Atlantis.Framework.Interface;


namespace Atlantis.Framework.EcommInstantPurchaseGet.Interface
{
  public class EcommInstantPurchaseGetRequestData : RequestData
  {
    public EcommInstantPurchaseGetRequestData(
      string shopperId,
      string sourceURL,
      string orderId,
      string pathway,
      int pageCount)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(5);
    }
    
    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }
  }
}
