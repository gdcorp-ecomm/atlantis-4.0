using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CarmaAddTrustedShopper.Interface
{
  public class CarmaAddTrustedShopperRequestData : RequestData
  {
    #region Properties
    public string PrimaryShopperId { get; private set; }
    #endregion

    public CarmaAddTrustedShopperRequestData(string shopperId
      , string sourceUrl
      , string orderId
      , string pathway
      , int pageCount
      , string primaryShopperId)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(5);
      PrimaryShopperId = primaryShopperId;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in CarmaAddTrustedShopperRequestData");     
    }
  }
}
