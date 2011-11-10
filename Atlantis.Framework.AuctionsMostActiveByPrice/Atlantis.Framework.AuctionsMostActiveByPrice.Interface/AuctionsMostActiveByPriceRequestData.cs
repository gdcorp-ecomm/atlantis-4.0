using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionsMostActiveByPrice.Interface
{
  public class AuctionsMostActiveByPriceRequestData : RequestData
  {
    public int AuctionCount { get; set; }

    public AuctionsMostActiveByPriceRequestData( 
      string shopperID, string sourceURL, string orderID, string pathway,
      int pageCount, int auctionCount)
      : base (shopperID, sourceURL, orderID, pathway, pageCount)
    {
      AuctionCount = auctionCount;
    }

    public override string GetCacheMD5()
    {
      throw new Exception("AuctionsByArea is not a cacheable request.");
    }
  }
}
