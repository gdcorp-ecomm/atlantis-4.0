using System.Runtime.Serialization;

namespace Atlantis.Framework.Auction.Interface
{
  [DataContract]
  public class AuctionBidAttributes
  {
    [DataMember]
    public bool IsBidValid { get; set; }

    [DataMember]
    public string Error { get; set; }

    [DataMember]
    public string AuctionItemId { get; set; }

    [DataMember]
    public string ItemDescription { get; set; }

    [DataMember]
    public string DomainName { get; set; }

    [DataMember]
    public bool DomainNameContainsNumber { get; set; }

    [DataMember]
    public string TimeLeft { get; set; }

    [DataMember]
    public string SaleTypeDesc { get; set; }

    [DataMember]
    public bool IsWebsiteIncluded { get; set; }

    [DataMember]
    public bool IsHighBid { get; set; }

    public AuctionBidAttributes() { }

    public AuctionBidAttributes(bool isBidValid,
                                string error,
                                string auctionItemId,
                                string itemDescription,
                                string domainName,
                                bool domainNameContainsNumber,
                                string timeLeft,
                                string saleTypeDesc,
                                bool isWebsiteIncluded,
                                bool isHighBid)
    {
      IsBidValid = isBidValid;
      Error = error;
      AuctionItemId = auctionItemId;
      ItemDescription = itemDescription;
      DomainName = domainName;
      DomainNameContainsNumber = domainNameContainsNumber;
      TimeLeft = timeLeft;
      SaleTypeDesc = saleTypeDesc;
      IsWebsiteIncluded = isWebsiteIncluded;
      IsHighBid = isHighBid;
    }
  }
}
