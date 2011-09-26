using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Atlantis.Framework.Auction.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionPlaceBidBulk.Interface
{
  public class AuctionPlaceBidBulkResponseData : IResponseData
  {
    #region Members

    private AtlantisException _atlEx;
    private string _auctionBulkBidResponseXml = string.Empty;

    #endregion


    #region Properties

    public bool IsSuccess { get; private set; }

    public bool IsValidPlaceBidBulkRequest { get; private set; }

    public int ErrorCode { get; private set; }

    public string RawErrorMessage { get; private set; }

    public string FriendlyErrorMessage { get; private set; }

    public bool DidBidsHaveErrors { get; private set; }

    public List<AuctionBidAttributes> AuctionBidAttributesList { get; private set; }

    #endregion


    #region Constructors

    public AuctionPlaceBidBulkResponseData(string auctionXml)
    {
      if (!string.IsNullOrEmpty(auctionXml))
      {
        _auctionBulkBidResponseXml = auctionXml;
        List<AuctionBidAttributes> bidResponseAttributes = null;
        if (GetBidResponses(auctionXml, out bidResponseAttributes))
        {
          AuctionBidAttributesList = bidResponseAttributes;
          IsSuccess = true;
          _atlEx = null;
        }
      }
    }

    public AuctionPlaceBidBulkResponseData(RequestData oRequestData, Exception ex)
    {
      _atlEx = new AtlantisException(oRequestData,
                                     "AuctionPlaceBidBulkResponseData",
                                     ex.Message,
                                     oRequestData.ToXML());
    }

    #endregion


    #region Methods

    private bool GetBidResponses(string auctionXml, out List<AuctionBidAttributes> bidResponseAttributes)
    {
      bool success = false;
      bidResponseAttributes = new List<AuctionBidAttributes>();

      XDocument xDoc = XDocument.Parse(auctionXml);

      if (xDoc.Root != null)
      {
        if (!string.IsNullOrEmpty(xDoc.Root.Attribute("Valid").Value))
        {
          IsValidPlaceBidBulkRequest = string.Compare(xDoc.Root.Attribute("Valid").Value, "true", true) == 0;
          
          if (!IsValidPlaceBidBulkRequest)
          {
            IEnumerable<XElement> errorData = xDoc.Root.Descendants("ErrorData");

            if (errorData.Attributes().FirstOrDefault().Name == "errornumber")
            {
              int parse = -1;
              if (Int32.TryParse(errorData.Attributes().FirstOrDefault().Value, out parse))
              {
                ErrorCode = parse;
              }

              foreach (XElement errorElement in errorData.Descendants())
              {
                switch (errorElement.Name.ToString())
                {
                  case "RawErrorString":
                    RawErrorMessage = errorElement.Value;
                    break;
                  case "FriendlyErrorString":
                    FriendlyErrorMessage = errorElement.Value;
                    break;
                }
              }
            }
          }
          else
          {
            bidResponseAttributes = xDoc.Descendants("ConfirmBid").Select(bid => new AuctionBidAttributes()
                      {
                        IsBidValid = (bid.Attribute("Valid") != null ? string.Compare(bid.Attribute("Valid").Value, "true", true) == 0 : false),
                        Error = (bid.Attribute("Error") != null ? bid.Attribute("Error").Value : string.Empty),
                        AuctionItemId = (bid.Attribute("MemberItemID") != null ? bid.Attribute("MemberItemID").Value : string.Empty),
                        ItemDescription = (bid.Attribute("ItemDescription") != null ? bid.Attribute("ItemDescription").Value : string.Empty),
                        DomainName = (bid.Attribute("DomainName") != null ? bid.Attribute("DomainName").Value : string.Empty),
                        DomainNameContainsNumber = (bid.Attribute("DomainNameContainsNumber") != null ? string.Compare(bid.Attribute("DomainNameContainsNumber").Value, "true", true) == 0 : false),
                        TimeLeft = (bid.Attribute("TimeLeft") != null ? bid.Attribute("TimeLeft").Value : string.Empty),
                        SaleTypeDesc = (bid.Attribute("SaleTypeDesc") != null ? bid.Attribute("SaleTypeDesc").Value : string.Empty),
                        IsWebsiteIncluded = (bid.Attribute("WebSiteIncludedFlag") != null ? string.Compare(bid.Attribute("WebSiteIncludedFlag").Value, "true", true) == 0 : false),
                        IsHighBid = (bid.Attribute("HighBid") != null ? string.Compare(bid.Attribute("HighBid").Value, "true", true) == 0 : false),
                      }).ToList();

            foreach (AuctionBidAttributes bidResponse in bidResponseAttributes)
            {
              if (!bidResponse.IsBidValid)
              {
                DidBidsHaveErrors = true;
                break;
              }
            }
            success = true;
          }
        }
      }
      return success;
    }

    #endregion


    #region Implementation of IResponseData

    public string ToXML()
    {
      return _auctionBulkBidResponseXml;
    }

    public AtlantisException GetException()
    {
      return _atlEx;
    }

    #endregion
  }
}
