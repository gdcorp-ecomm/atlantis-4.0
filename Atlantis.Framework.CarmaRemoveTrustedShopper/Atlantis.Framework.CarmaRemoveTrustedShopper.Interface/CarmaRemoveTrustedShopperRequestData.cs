using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CarmaRemoveTrustedShopper.Interface
{
  public class CarmaRemoveTrustedShopperRequestData : RequestData
  {
    #region Properties
    public string SecondaryShopperId { get; private set; }
    #endregion

    public CarmaRemoveTrustedShopperRequestData(string shopperId
      , string sourceUrl
      , string orderId
      , string pathway
      , int pageCount
      , string secondaryShopperId)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(5);
      SecondaryShopperId = secondaryShopperId;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in CarmaRemoveTrustedShopperRequestData");     
    }
  }
}
