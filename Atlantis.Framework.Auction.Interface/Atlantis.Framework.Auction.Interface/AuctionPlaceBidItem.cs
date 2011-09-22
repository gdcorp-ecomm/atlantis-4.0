using System.Runtime.Serialization;

namespace Atlantis.Framework.Auction.Interface
{
  [DataContract]
  public class AuctionPlaceBidItem
  {
    [DataMember]
    public string AuctionItemId { get; set; }

    [DataMember]
    public string BidderShopperId { get; set; }

    [DataMember]
    public string BidAmount { get; set; }

    [DataMember]
    public string Comments { get; set; }


    public AuctionPlaceBidItem() {}

    public AuctionPlaceBidItem(string auctionItemId, string bidderShopperId, string bidAmount, string comments)
    {
      AuctionItemId = auctionItemId;
      BidderShopperId = bidderShopperId;
      BidAmount = bidAmount;
      Comments = comments;
    }

    public AuctionPlaceBidItem(string auctionItemId, string bidAmount)
    {
      AuctionItemId = auctionItemId;
      BidAmount = bidAmount;
      BidderShopperId = null;
      Comments = null;
    }
  }
}
