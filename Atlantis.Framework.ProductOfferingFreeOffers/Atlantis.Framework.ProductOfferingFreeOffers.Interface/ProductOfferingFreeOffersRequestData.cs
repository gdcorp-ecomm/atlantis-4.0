using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ProductOfferingFreeOffers.Interface
{
  public class ProductOfferingFreeOffersRequestData : RequestData
  {

    public int ResellerId { get; set; }

    public ProductOfferingFreeOffersRequestData(string shopperId
      , string sourceUrl
      , string orderId
      , string pathway
      , int pageCount, int resellerId)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      ResellerId = resellerId;
    }

    #region Overridden Methods
    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in ProductOfferingFreeOffersRequestData");
    }
    #endregion
  }
}
