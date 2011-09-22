using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Atlantis.Framework.Auction.Interface
{
  [DataContract]
  public class AuctionDetail
  {
    [DataMember]
    public string AuctionItemId { get; set; }

    [DataMember]
    public string MemberId { get; set; }

    [DataMember]
    public string ShopperId { get; set; }

    [DataMember]
    public string AuctionTypeId { get; set; }

    [DataMember]
    public string StatusCodeId { get; set; }

    [DataMember]
    public string DomainExtensionId { get; set; }

    [DataMember]
    public string DomainName { get; set; }

    [DataMember]
    public string FullDomainName { get; set; }

    [DataMember]
    public string ItemDescription { get; set; }

    [DataMember]
    public string CurrentPrice { get; set; }

    [DataMember]
    public string StartingBidAmount { get; set; }

    [DataMember]
    public string BidIncrementAmount { get; set; }

    [DataMember]
    public string BuyItNowAmount { get; set; }

    [DataMember]
    public string ReservedPriceAmount { get; set; }

    [DataMember]
    public string TimeZone { get; set; }

    [DataMember]
    public string AuctionListDate { get; set; }

    [DataMember]
    public string AuctionStartTime { get; set; }

    [DataMember]
    public string AuctionEndTime { get; set; }

    [DataMember]
    public bool IsWebsiteIncluded { get; set; }

    [DataMember]
    public bool EmailToFriend { get; set; }

    [DataMember]
    public bool IsFeatureListing { get; set; }

    [DataMember]
    public bool IsCategoryFeatureListing { get; set; }

    [DataMember]
    public bool IsSubCategoryFeatureListing { get; set; }

    [DataMember]
    public bool IsBuyItNow { get; set; }

    [DataMember]
    public bool IsAddlCategoryListing { get; set; }

    [DataMember]
    public bool HasReservedPrice { get; set; }

    [DataMember]
    public bool IsAdultListing { get; set; }

    [DataMember]
    public bool HasBidAccepted { get; set; }

    [DataMember]
    public string BidAcceptedDate { get; set; }

    [DataMember]
    public string BillingResourceId { get; set; }

    [DataMember]
    public string HighestBidderId { get; set; }

    [DataMember]
    public string HighestBidderShopperId { get; set; }

    [DataMember]
    public string FulfillmentDataId { get; set; }

    [DataMember]
    public string FulfillmentShopperId { get; set; }

    [DataMember]
    public string PayPalAddress { get; set; }

    [DataMember]
    public string Bids { get; set; }

    [DataMember]
    public string BackorderTypeId { get; set; }

    [DataMember]
    public string BackorderMemberId { get; set; }

    [DataMember]
    public string BackorderShopperId { get; set; }

    [DataMember]
    public string DomainId { get; set; }

    [DataMember]
    public string Rank { get; set; }

    [DataMember]
    public string AppraisalId { get; set; }

    [DataMember]
    public string HasTraffic { get; set; }

    [DataMember]
    public string Last14DaysTraffic { get; set; }

    [DataMember]
    public string OnSalePercent { get; set; }

    [DataMember]
    public string PrivateCode { get; set; }

    [DataMember]
    public bool HasAuthenticationEmailSent { get; set; }

    [DataMember]
    public string EasyPushStatusCode { get; set; }

    [DataMember]
    public string LastBidOfferDtm { get; set; }

    [DataMember]
    public string BackorderId { get; set; }

    [DataMember]
    public string SourceId { get; set; }

    [DataMember]
    public string EscrowId { get; set; }

    [DataMember]
    public string VendorId { get; set; }

    [DataMember]
    public string ValuationPrice { get; set; }

    [DataMember]
    public bool IsHide { get; set; }

    [DataMember]
    public string Tld { get; set; }

    [DataMember]
    public string AuctionModel { get; set; }

    [DataMember]
    public string SaleType { get; set; }

    [DataMember]
    public List<string> Categories { get; set; }

    [DataMember]
    public string ReserveMet { get; set; }

    [DataMember]
    public string ViewCount { get; set; }

    [DataMember]
    public string AppraisedValue { get; set; }

    [DataMember]
    public string AuctionExpireDate { get; set; }

    [DataMember]
    public string CancelReason { get; set; }

    [DataMember]
    public bool SentToEscrow { get; set; }

    [DataMember]
    public string MemberHasWatch { get; set; }

    public AuctionDetail()
    {
      AuctionItemId = string.Empty;
      MemberId = string.Empty;
      ShopperId = string.Empty;
      AuctionTypeId = string.Empty;
      StatusCodeId = string.Empty;
      DomainExtensionId = string.Empty;
      DomainName = string.Empty;
      FullDomainName = string.Empty;
      ItemDescription = string.Empty;
      CurrentPrice = string.Empty;
      StartingBidAmount = string.Empty;
      BidIncrementAmount = string.Empty;
      BuyItNowAmount = string.Empty;
      ReservedPriceAmount = string.Empty;
      TimeZone = string.Empty;
      AuctionListDate = string.Empty;
      AuctionStartTime = string.Empty;
      AuctionEndTime = string.Empty;
      BidAcceptedDate = string.Empty;
      BillingResourceId = string.Empty;
      HighestBidderId = string.Empty;
      HighestBidderShopperId = string.Empty;
      FulfillmentDataId = string.Empty;
      FulfillmentShopperId = string.Empty;
      PayPalAddress = string.Empty;
      Bids = string.Empty;
      BackorderTypeId = string.Empty;
      BackorderMemberId = string.Empty;
      BackorderShopperId = string.Empty;
      DomainId = string.Empty;
      Rank = string.Empty;
      AppraisalId = string.Empty;
      HasTraffic = string.Empty;
      Last14DaysTraffic = string.Empty;
      OnSalePercent = string.Empty;
      PrivateCode = string.Empty;
      EasyPushStatusCode = string.Empty;
      LastBidOfferDtm = string.Empty;
      BackorderId = string.Empty;
      SourceId = string.Empty;
      EscrowId = string.Empty;
      VendorId = string.Empty;
      ValuationPrice = string.Empty;
      Tld = string.Empty;
      AuctionModel = string.Empty;
      SaleType = string.Empty;
      ReserveMet = string.Empty;
      ViewCount = string.Empty;
      AppraisedValue = string.Empty;
      AuctionExpireDate = string.Empty;
      CancelReason = string.Empty;
      MemberHasWatch = string.Empty;
    }
  }
}
