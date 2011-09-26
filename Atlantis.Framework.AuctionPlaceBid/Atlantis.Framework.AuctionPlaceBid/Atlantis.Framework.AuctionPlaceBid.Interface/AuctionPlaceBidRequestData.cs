using System;
using Atlantis.Framework.AuctionSearch.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionPlaceBid.Interface
{
  public class AuctionPlaceBidRequestData : RequestData
  {
    private static readonly TimeSpan _requestTimeout = TimeSpan.FromSeconds(20);
    
    public RequestorInformation RequestorInformation { get; set; }

    public string AuctionItemId { get; set; }

    public string BidAmount { get; set; }

    public string Comments { get; set; }

    public TimeSpan RequestTimeout { get; set; }
    
    public AuctionPlaceBidRequestData(RequestorInformation requestorInformation,
                                      string auctionItemId, 
                                      string bidAmount, 
                                      string comments,
                                      string shopperId, 
                                      string sourceUrl, 
                                      string orderId, 
                                      string pathway, 
                                      int pageCount) : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestorInformation = requestorInformation;
      AuctionItemId = auctionItemId;
      BidAmount = bidAmount;
      Comments = comments;
      RequestTimeout = _requestTimeout;
    }

    

    public override string GetCacheMD5()
    {
      throw new Exception("AuctionPlaceBid is not a cacheable request.");
    }
  }
}
