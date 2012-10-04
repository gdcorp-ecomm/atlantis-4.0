using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Atlantis.Framework.Auction.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionGetBidHistory.Interface
{
  public class AuctionGetBidHistoryResponseData : IResponseData
  {
    private AtlantisException _atlEx;
    private string _auctionBidHistoryResponseXml = string.Empty;

    private bool _isMemberArea { get; set; }

    public bool IsSuccess { get; private set; }

    public bool IsValidBidHistoryRequest { get; private set; }

    public string ErrorCode { get; private set; }
    
    public string RawErrorMessage { get; private set; }

    public string FriendlyErrorMessage { get; private set; }

    public List<AuctionBidDetail> BidDetails { get; private set; }

    public bool HasBids { get; private set; }


    public AuctionGetBidHistoryResponseData(string auctionXml, bool isMemberArea)
    {
      _isMemberArea = isMemberArea;

      if (!string.IsNullOrEmpty(auctionXml))
      {
        List<AuctionBidDetail> bidDetails = null;
        if (GetBidHistoryDetail(auctionXml, out bidDetails))
        {
          if (bidDetails.Count >= 1)
          {
            HasBids = true;
          }
          BidDetails = bidDetails;
          IsSuccess = true;
          _atlEx = null;
        }
      }
    }

    public AuctionGetBidHistoryResponseData(RequestData oRequestData, Exception ex)
    {
      _atlEx = new AtlantisException(oRequestData,
                                      "AuctionGetBidHistoryResponseData",
                                      ex.Message.ToString(),
                                      oRequestData.ToXML());
    }

    private bool GetBidHistoryDetail(string auctionApiResponse, out List<AuctionBidDetail> bidDetails)
    {
      XDocument bidsXml = XDocument.Parse(auctionApiResponse);
      bidDetails = new List<AuctionBidDetail>();
      bool success = false;
      
      if (bidsXml.Root != null)
      {
        _auctionBidHistoryResponseXml = auctionApiResponse;

        if (!string.IsNullOrEmpty(bidsXml.Root.Attribute("Valid").Value))
        {
          IsValidBidHistoryRequest = (bidsXml.Root.Attribute("Valid") != null ? string.Compare(bidsXml.Root.Attribute("Valid").Value, "true", true) == 0 : true);
        }
        
        if (!String.IsNullOrEmpty(bidsXml.Root.Attribute("Error").Value))
        {
          IsSuccess = true;
          _atlEx = null;
          
          XmlDocument xDoc = new XmlDocument();
          xDoc.LoadXml(auctionApiResponse);

          XmlNode item = xDoc.SelectSingleNode("/GetBidHistoryRsp").FirstChild;

          if (item != null)
          {
            XmlAttributeCollection xAtr = item.Attributes;
            if (xAtr != null)
            {
              ErrorCode = xAtr[0].Value;
            }
            RawErrorMessage = item.ChildNodes[0].InnerText;
            FriendlyErrorMessage = item.ChildNodes[1].InnerText;
          }
        }
        else
        {
          try
          {
            bidDetails = bidsXml.Descendants("Bid").Select(bid => new AuctionBidDetail()
              {
                Id                   = (bid.Attribute("ItemBidId") != null ? bid.Attribute("ItemBidId").Value : string.Empty),
                TimeZone             = (bid.Attribute("TimeZone") != null ? bid.Attribute("TimeZone").Value : string.Empty),
                StartDate            = (bid.Attribute("BidStartDate") != null ? bid.Attribute("BidStartDate").Value : string.Empty),
                EndDate              = (bid.Attribute("BidEndDate") != null ? bid.Attribute("BidEndDate").Value : string.Empty),
                MaskedBidderId       = (bid.Attribute("MaskedBidderID") != null ? bid.Attribute("MaskedBidderID").Value : string.Empty),
                BidAmount            = (bid.Attribute("BidAmount") != null ? bid.Attribute("BidAmount").Value : string.Empty),
                Comment              = (bid.Element("BidComment") != null ? bid.Element("BidComment").Value : string.Empty),
                
                SellerMemberId       = _isMemberArea ? (bid.Attribute("SellerMemberId") != null ? bid.Attribute("SellerMemberId").Value : string.Empty) : null,
                BidderMemberId       = _isMemberArea ? (bid.Attribute("BidderMemberId") != null ? bid.Attribute("BidderMemberId").Value : string.Empty) : null,
                CounterOfferMemberId = _isMemberArea ? (bid.Attribute("CounterOfferMemberId") != null ? bid.Attribute("CounterOfferMemberId").Value : string.Empty) : null,
                IsCounterOffer       = _isMemberArea ? ((bool?)(bid.Attribute("CounterOfferFlag") != null ? string.Compare(bid.Attribute("CounterOfferFlag").Value, "true", true) == 0 : false)) : null,
                DisplayAccept        = _isMemberArea ? ((bool?)(bid.Attribute("DisplayAcceptFlag") != null ? string.Compare(bid.Attribute("DisplayAcceptFlag").Value, "true", true) == 0 : false)) : null,
                DisplayCounter       = _isMemberArea ? ((bool?)(bid.Attribute("DisplayCounterFlag") != null ? string.Compare(bid.Attribute("DisplayCounterFlag").Value, "true", true) == 0 : false)) : null
              }).ToList();
            
            
            success = true;
          }
          catch (Exception)
          {
            success = false;
          }
        }
      }
      return success;
    }

    #region Implementation of IResponseData

    public string ToXML()
    {
      return _auctionBidHistoryResponseXml;
    }

    public AtlantisException GetException()
    {
      return _atlEx;
    }

    #endregion
  }
}
