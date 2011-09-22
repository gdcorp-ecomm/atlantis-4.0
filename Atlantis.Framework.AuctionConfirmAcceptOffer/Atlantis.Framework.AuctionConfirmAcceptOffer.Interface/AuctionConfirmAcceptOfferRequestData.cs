using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionConfirmAcceptOffer.Interface
{
  public class AuctionConfirmAcceptOfferRequestData : RequestData
  {
    private static readonly TimeSpan _requestTimeout = TimeSpan.FromSeconds(20);

    public string AuctionItemId { get; set; }

    public string BidId { get; set; }
    
    
    public AuctionConfirmAcceptOfferRequestData(string shopperId, 
                                                string sourceURL, 
                                                string orderId, 
                                                string pathway, 
                                                int pageCount,
                                                string auctionItemId,
                                                string bidId) : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      AuctionItemId = auctionItemId;
      BidId = bidId;
      RequestTimeout = _requestTimeout;
    }

    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      throw new Exception("AuctionAcceptCounterOffer is not a cacheable request.");
    }

    #endregion
  }
}
