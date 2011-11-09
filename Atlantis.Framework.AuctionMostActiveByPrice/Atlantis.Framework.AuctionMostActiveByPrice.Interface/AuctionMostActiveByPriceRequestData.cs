using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionMostActiveByPrice.Interface
{
  public class AuctionMostActiveByPriceRequestData :RequestData
  {

    public int Rows { get; set; }

    public AuctionMostActiveByPriceRequestData(string shopperId, 
                                            string sourceURL, 
                                            string orderId, 
                                            string pathway, 
                                            int pageCount,
                                            int rows) 
                                            : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      Rows = rows;
    }


    
    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      throw new Exception("AuctionMostActiveByPrice is not a cacheable request.");
    }

    #endregion  

  }
}
