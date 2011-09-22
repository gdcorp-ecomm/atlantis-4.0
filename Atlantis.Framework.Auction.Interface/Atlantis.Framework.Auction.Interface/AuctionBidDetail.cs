using System;
using System.Runtime.Serialization;

namespace Atlantis.Framework.Auction.Interface
{
  [DataContract]
  public class AuctionBidDetail
  {
    [DataMember]
    public string Id { get; set; }

    [DataMember]
    public string TimeZone { get; set; }

    [DataMember]
    public string StartDate { get; set; }

    [DataMember]
    public string EndDate { get; set; }

    public DateTime? FormattedStartDate
    {
      get
      {
        DateTime date;
        return DateTime.TryParse(StartDate, out date) ? (DateTime?) date : null;
      }
    }

    public DateTime? FormattedEndDate {
      get {
        DateTime date;
        return DateTime.TryParse(EndDate, out date) ? (DateTime?)date : null;
      }
    }

    [DataMember]
    public string MaskedBidderId { get; set; }

    [DataMember]
    public string BidAmount { get; set; }

    [DataMember]
    public string Comment { get; set; }

    [DataMember]
    public string SellerMemberId { get; set; }

    [DataMember]
    public string BidderMemberId { get; set; }

    [DataMember]
    public string CounterOfferMemberId { get; set; }

    [DataMember]
    public bool? IsCounterOffer { get; set; }

    [DataMember]
    public bool? DisplayAccept { get; set; }

    [DataMember]
    public bool? DisplayCounter { get; set; }


    public AuctionBidDetail() { }

    public AuctionBidDetail(string id,
                            string timeZone,
                            string startDate,
                            string endDate,
                            string maskedBidderId,
                            string bidAmount,
                            string comment)
    {
      Id = id;
      TimeZone = timeZone;

      StartDate = startDate;
      EndDate = endDate;

      MaskedBidderId = maskedBidderId;
      BidAmount = bidAmount;
      Comment = comment;

      SellerMemberId = null;
      BidderMemberId = null;
      CounterOfferMemberId = null;
      IsCounterOffer = null;
      DisplayAccept = null;
      DisplayCounter = null;
    }

    public AuctionBidDetail(string id,
                            string timeZone,
                            string startDate,
                            string endDate,
                            string maskedBidderId,
                            string bidAmount,
                            string comment,
                            string sellerMemberId,
                            string bidderMemberId,
                            string counterOfferMemberId,
                            string counterOfferFlag,
                            string displayAcceptFlag,
                            string displayCounterFlag)
    {
      Id = id;

      TimeZone = timeZone;

      StartDate = startDate;
      EndDate = endDate;

      MaskedBidderId = maskedBidderId;
      BidAmount = bidAmount;
      Comment = comment;
      SellerMemberId = sellerMemberId;
      BidderMemberId = bidderMemberId;
      CounterOfferMemberId = counterOfferMemberId;

      IsCounterOffer = string.Compare(counterOfferFlag, "true", true) == 0;
      DisplayAccept = string.Compare(displayAcceptFlag, "true", true) == 0;
      DisplayCounter = string.Compare(displayCounterFlag, "true", true) == 0;
    }
  }
}
