using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionConfirmCounterOffer.Interface
{
  public class AuctionConfirmCounterOfferRequestData : RequestData
  {
    private static readonly TimeSpan _requestTimeout = TimeSpan.FromSeconds(20);

    public string AuctionItemId { get; set; }
    
    public string BidId { get; set; }
    
    public string BidAmount { get; set; }
    
    public string  Comments { get; set; }


    public AuctionConfirmCounterOfferRequestData(string shopperId, 
                                                 string sourceURL,
                                                 string orderId,
                                                 string pathway,
                                                 int pageCount,
                                                 string auctionItemId,
                                                 string bidId,
                                                 string bidAmount,
                                                 string comments) : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      AuctionItemId = auctionItemId;
      BidId = bidId;
      BidAmount = bidAmount;
      Comments = comments;
      RequestTimeout = _requestTimeout;
    }

    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      throw new Exception("AuctionConfirmCounterOffer is not a cacheable request.");
    }

    #endregion
  }
}
