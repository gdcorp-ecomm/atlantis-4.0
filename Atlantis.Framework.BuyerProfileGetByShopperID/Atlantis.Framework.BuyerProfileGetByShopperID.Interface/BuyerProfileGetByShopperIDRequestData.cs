using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BuyerProfileGetByShopperID.Interface
{
  public class BuyerProfileGetByShopperIDRequestData : RequestData
  {

    public BuyerProfileGetByShopperIDRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {

    }
    
    public override string GetCacheMD5()
    {
      throw new Exception("GetBuyerProfileByID is not a cacheable request.");
    }

  }

}
