using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Atlantis.Framework.Auction.Interface {
  [DataContract]
  [Serializable]
  public class Auction {
    [DataMember]
    public string Id { get; set; }

    [DataMember]
    public string TimeZone { get; set; }

    [DataMember]
    public string EndTime { get; set; }

    [DataMember]
    public string TypeId { get; set; }

    [DataMember]
    public string DomainName { get; set; }

    [DataMember]
    public string StartPrice { get; set; }

    [DataMember]
    public string CurrentPrice { get; set; }

    [DataMember]
    public string ReservedPrice { get; set; }

    [DataMember]
    public string PrivateCode { get; set; }

    [DataMember]
    public string IsFeatured { get; set; }

    [DataMember]
    public string OnSalePercent { get; set; }

    [DataMember]
    public string IsBuyNow { get; set; }

    [DataMember]
    public string Bids { get; set; }

    [DataMember]
    public string SellerMemberId { get; set; }

    [DataMember]
    public string SellerShopperId { get; set; }

    [DataMember]
    public string MemberHasWatch { get; set; }

    [DataMember]
    public string AppraisalId { get; set; }

    [DataMember]
    public string Traffic { get; set; }

    [DataMember]
    public string ValuationPrice { get; set; }

    [DataMember]
    public string BuyNowPrice { get; set; }

    [DataMember]
    public string HighestBidderId { get; set; }

    [DataMember]
    public string HighestBidderShopperId { get; set; }

    [DataMember]
    public string AuctionModel { get; set; }

    [DataMember]
    public string MinBid { get; set; }

    [DataMember]
    public string SaleType { get; set; }

    public AuctionDetail _details = new AuctionDetail();
    [DataMember]
    public AuctionDetail Details
    {
      get { return _details; }
      set { _details = value; }
    }

    public List<AuctionBidDetail> _bids = new List<AuctionBidDetail>();
    [DataMember]
    public List<AuctionBidDetail> BidHistory {
      get { return _bids; }
      set { _bids = value; }
    }


    public Auction() { }

    public Auction(string id, string timeZone, string endTime, string typeId, string domainName, string startPrice,
      string currentPrice, string reservedPrice, string privateCode, string isFeatured, string onSalePercent, string isBuyNow,
      string bids, string sellerMemberId, string sellerShopperId, string memberHasWatch, string appraisalId, string traffic,
      string valuationPrice, string buyNowPrice, string highestBidderId, string highestBidderShopperId, string auctionModel,
      string minBid, string saleType) {
      Id = id;
      TimeZone = timeZone;
      EndTime = endTime;
      TypeId = typeId;
      DomainName = domainName;
      StartPrice = startPrice;
      CurrentPrice = currentPrice;
      ReservedPrice = reservedPrice;
      PrivateCode = privateCode;
      IsFeatured = isFeatured;
      OnSalePercent = onSalePercent;
      IsBuyNow = isBuyNow;
      Bids = bids;
      SellerMemberId = sellerMemberId;
      SellerShopperId = sellerShopperId;
      MemberHasWatch = memberHasWatch;
      AppraisalId = appraisalId;
      Traffic = traffic;
      ValuationPrice = valuationPrice;
      BuyNowPrice = buyNowPrice;
      HighestBidderId = highestBidderId;
      HighestBidderShopperId = highestBidderShopperId;
      AuctionModel = auctionModel;
      MinBid = minBid;
      SaleType = saleType;
    }
  }
}