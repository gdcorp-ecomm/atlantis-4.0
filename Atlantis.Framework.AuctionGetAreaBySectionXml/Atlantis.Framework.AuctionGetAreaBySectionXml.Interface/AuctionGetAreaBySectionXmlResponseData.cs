using System;
using System.Collections.Generic;
using System.Xml;
using Atlantis.Framework.Auction.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionGetAreaBySectionXml.Interface
{
  public class AuctionGetAreaBySectionXmlResponseData : IResponseData
  {
    private AtlantisException _ex;
    private string _responseXml;

    public string Error { get; private set; }

    private List<AuctionMemberSection> _results;
    public List<AuctionMemberSection> Results
    {
      get
      {
        if (_results == null)
        {
          _results = new List<AuctionMemberSection>();
        }
        return _results;
      }
      private set { _results = value; }
    }

    public bool IsSuccess { get; private set; }

    public AuctionGetAreaBySectionXmlResponseData(string xmlResponse)
    {
      _responseXml = xmlResponse;
      _ex = null;
      if (ParseXmlResponse())
      {
        IsSuccess = true;
      }
      else
      {
        IsSuccess = false;
        Results = new List<AuctionMemberSection>();
      }
    }

    private bool ParseXmlResponse()
    {
      bool success;
      var doc = new XmlDocument();
      try
      {
        doc.LoadXml(_responseXml);

        if (doc.SelectSingleNode("//Error") != null)
        {
          success = false;
          Error = doc.SelectSingleNode("//Error").InnerText;
        }
        else
        {
          var sections = doc.SelectNodes("//Section");
          foreach (XmlNode section in sections)
          {
            var item = new AuctionMemberSection();
            item.SectionName = section.Attributes["SectionName"].Value ?? string.Empty;
            XmlNodeList auctionNodes = section.SelectNodes("Auction");
            foreach (XmlNode auction in auctionNodes)
            {
              var auctionItem = new Auction.Interface.Auction
              {
                Id = auction.SelectSingleNode("auctionID").InnerText ?? string.Empty,
                TypeId = auction.SelectSingleNode("auctionTypeID").InnerText ?? string.Empty,
                Bids = auction.SelectSingleNode("bids").InnerText ?? string.Empty,
                CurrentPrice = auction.SelectSingleNode("currentPrice").InnerText ?? string.Empty,
                DomainName = auction.SelectSingleNode("domainName").InnerText ?? string.Empty,
                HighestBidderId = auction.SelectSingleNode("highestBidderID").InnerText ?? string.Empty,
                IsBuyNow = auction.SelectSingleNode("BuyItNowFlag").InnerText ?? string.Empty,
                OnSalePercent = auction.SelectSingleNode("onSalePercent").InnerText ?? string.Empty,
                PrivateCode = auction.SelectSingleNode("privateCode").InnerText ?? string.Empty,
                ReservedPrice = auction.SelectSingleNode("ReservedPriceAmount").InnerText ?? string.Empty,
                SaleType = auction.SelectSingleNode("saletypeid").InnerText ?? string.Empty,
                Traffic = auction.SelectSingleNode("monthlyTraffic").InnerText ?? string.Empty
              };

              auctionItem.Details = new AuctionDetail
              {
                AuctionItemId = auctionItem.Id,
                AppraisedValue = auction.SelectSingleNode("AppraisedValue").InnerText ?? string.Empty,
                AuctionEndTime = auction.SelectSingleNode("auctionEndTime").InnerText ?? string.Empty,
                AuctionExpireDate = auction.SelectSingleNode("expireDate").InnerText ?? string.Empty,
                AuctionListDate = auction.SelectSingleNode("auctionListDate").InnerText ?? string.Empty,
                AuctionStartTime = auction.SelectSingleNode("auctionStartTime").InnerText ?? string.Empty,
                AuctionTypeId = auction.SelectSingleNode("auctionTypeID").InnerText ?? string.Empty,
                BackorderMemberId = auction.SelectSingleNode("backorderMemberID").InnerText ?? string.Empty,
                BackorderTypeId = auction.SelectSingleNode("BackOrderTypeID").InnerText ?? string.Empty,
                BidAcceptedDate = auction.SelectSingleNode("bidAcceptedDate").InnerText ?? string.Empty,
                CancelReason = auction.SelectSingleNode("cancelReason").InnerText ?? string.Empty,
                EasyPushStatusCode = auction.SelectSingleNode("easyPushStatusCode").InnerText ?? string.Empty,
                HasAuthenticationEmailSent = bool.Parse((string.IsNullOrEmpty(auction.SelectSingleNode("authenticationEmailSent").InnerText) ? false.ToString() : auction.SelectSingleNode("authenticationEmailSent").InnerText) ?? false.ToString()),
                HasReservedPrice = bool.Parse((string.IsNullOrEmpty(auction.SelectSingleNode("ReservedPriceFlag").InnerText) ? false.ToString() : auction.SelectSingleNode("ReservedPriceFlag").InnerText) ?? false.ToString()),
                IsBuyItNow = bool.Parse((string.IsNullOrEmpty(auction.SelectSingleNode("BuyItNowFlag").InnerText) ? false.ToString() : auction.SelectSingleNode("BuyItNowFlag").InnerText) ?? false.ToString()),
                ItemDescription = auction.SelectSingleNode("itemDescription").InnerText ?? string.Empty,
                LastBidOfferDtm = auction.SelectSingleNode("LastBidOfferDTM").InnerText ?? string.Empty,
                ReservedPriceAmount = auction.SelectSingleNode("ReservedPriceAmount").InnerText ?? string.Empty,
                SentToEscrow = bool.Parse((string.IsNullOrEmpty(auction.SelectSingleNode("sentToEscrow").InnerText) ? false.ToString() : auction.SelectSingleNode("sentToEscrow").InnerText) ?? false.ToString()),
                StartingBidAmount = auction.SelectSingleNode("startingBidAmount").InnerText ?? string.Empty,
                StatusCodeId = auction.SelectSingleNode("statusCodeID").InnerText ?? string.Empty,
                Tld = auction.SelectSingleNode("TLD").InnerText ?? string.Empty,
              };

              XmlNodeList bids = auction.SelectNodes("ItemBids/ItemBid");
              int iBidCounter = 0;
              Dictionary<string, string> bidders = new Dictionary<string, string>();
              string maskedBidderId;

              foreach (XmlNode bidDetail in bids)
              {
                
                string bidderMemberId = bidDetail.SelectSingleNode("bidderMemberID").InnerText;
                if (!bidders.ContainsKey(bidderMemberId)) {
                  iBidCounter++;
                  maskedBidderId = string.Format("Bidder{0}", iBidCounter);
                  bidders.Add(bidderMemberId, maskedBidderId);
                } else
                {
                  maskedBidderId = bidders[bidderMemberId];
                }


                var bid = new AuctionBidDetail()
                            {

                              BidAmount = bidDetail.SelectSingleNode("bidAmount").InnerText ?? string.Empty,
                              BidderMemberId = bidderMemberId,
                              Comment = bidDetail.SelectSingleNode("comments").InnerText ?? string.Empty,
                              CounterOfferMemberId = bidDetail.SelectSingleNode("counterOfferMemberID").InnerText ?? string.Empty,
                              DisplayAccept = bool.Parse((string.IsNullOrEmpty(bidDetail.SelectSingleNode("displayAcceptButton").InnerText) ? false.ToString() : bidDetail.SelectSingleNode("displayAcceptButton").InnerText) ?? false.ToString()),
                              DisplayCounter = bool.Parse((string.IsNullOrEmpty(bidDetail.SelectSingleNode("displayCounterButton").InnerText) ? false.ToString() : bidDetail.SelectSingleNode("displayCounterButton").InnerText) ?? false.ToString()),
                              EndDate = bidDetail.SelectSingleNode("bidEndDate").InnerText ?? string.Empty,
                              Id = bidDetail.SelectSingleNode("itemBidID").InnerText ?? string.Empty,
                              IsCounterOffer = bool.Parse((string.IsNullOrEmpty(bidDetail.SelectSingleNode("counterOfferFlag").InnerText) ? false.ToString() : bidDetail.SelectSingleNode("counterOfferFlag").InnerText) ?? false.ToString()),
                              MaskedBidderId = maskedBidderId,
                              SellerMemberId = bidDetail.SelectSingleNode("sellerMemberID").InnerText ?? string.Empty,
                              StartDate = bidDetail.SelectSingleNode("bidStartDate").InnerText ?? string.Empty
                            };


                auctionItem.BidHistory.Add(bid);
              }

              item.Auctions.Add(auctionItem);
            }
            Results.Add(item);
          }
          success = true;
        }
      }
      catch (Exception ex)
      {
        success = false;
        IsSuccess = false;
        _ex = new AtlantisException(null, "ParseXmlResponse", ex.Message, string.Empty, ex);
      }

      return success;

    }

    public AuctionGetAreaBySectionXmlResponseData(RequestData requestData, Exception ex)
    {
      _responseXml = string.Empty;
      _ex = new AtlantisException(requestData, "AuctionGetAreaBySectionXmlResponseData", ex.Message, string.Empty, ex);
    }

    #region Implementation of IResponseData

    public string ToXML()
    {
      return _responseXml ?? string.Empty;
    }

    public AtlantisException GetException()
    {
      return _ex;
    }

    #endregion
  }
}
