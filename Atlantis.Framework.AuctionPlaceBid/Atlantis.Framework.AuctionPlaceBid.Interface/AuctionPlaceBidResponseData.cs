using System;
using System.Xml;
using Atlantis.Framework.Auction.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionPlaceBid.Interface
{
  public class AuctionPlaceBidResponseData : IResponseData
  {
    private AtlantisException _atlEx;
    private string _auctionBidResponseXML = string.Empty;
    private AuctionBidAttributes _bidAttributes;

    public bool IsSuccess { get; private set; }

    public AuctionPlaceBidResponseData(string auctionXml)
    {
      if (!string.IsNullOrEmpty(auctionXml))
      {
        _auctionBidResponseXML = auctionXml;
        IsSuccess = true;
        _atlEx = null;
      }
    }

    public AuctionPlaceBidResponseData(RequestData oRequestData, Exception ex)
    {
      _atlEx = new AtlantisException(oRequestData,
        "AuctionPlaceBidResponseData",
      ex.Message,
      oRequestData.ToXML());
    }

    public AuctionBidAttributes AuctionBidAttributes
    {
      get { return PopulateFromXml(); }
    }

    private AuctionBidAttributes PopulateFromXml()
    {
      if (_bidAttributes == null)
      {
        if (!String.IsNullOrEmpty(_auctionBidResponseXML))
        {
          bool isBidValid = false;
          string error = string.Empty;
          string auctionItemId = string.Empty;
          string itemDescription = string.Empty;
          string domainName = string.Empty;
          bool domainNameContainsNumber = false;
          string timeLeft = string.Empty;
          string saleTypeDesc = string.Empty;
          bool isWebsiteIncluded = false;
          bool isHighBid = false;

          XmlDocument xDoc = new XmlDocument();
          xDoc.LoadXml(_auctionBidResponseXML);

          var item = xDoc.SelectSingleNode("/ConfirmBid");
          if (item != null)
          {
            XmlAttributeCollection xAtr = item.Attributes;
            if (xAtr != null)
            {
              foreach (XmlAttribute xAtrNode in xAtr)
              {
                if (!string.IsNullOrEmpty(xAtrNode.Value))
                {
                  switch (xAtrNode.Name)
                  {
                    case "Valid":
                      isBidValid = string.Compare(xAtrNode.Value, "true", true) == 0;
                      break;
                    case "Error":
                      error = xAtrNode.Value;
                      break;
                    case "MemberItemID":
                      auctionItemId = xAtrNode.Value;
                      break;
                    case "ItemDescription":
                      itemDescription = xAtrNode.Value;
                      break;
                    case "DomainName":
                      domainName = xAtrNode.Value;
                      break;
                    case "DomainNameContainsNumber":
                      domainNameContainsNumber = string.Compare(xAtrNode.Value,"true", true) == 0;
                      break;
                    case "TimeLeft":
                      timeLeft = xAtrNode.Value;
                      break;
                    case "SaleTypeDesc":
                      saleTypeDesc = xAtrNode.Value;
                      break;
                    case "WebSiteIncludeFlag":
                      isWebsiteIncluded = string.Compare(xAtrNode.Value, "true", true) == 0;
                      break;
                    case "HighBid":
                      isHighBid = string.Compare(xAtrNode.Value, "true", true) == 0;
                      break;
                  }
                }
              }
            }
          }

          _bidAttributes = new AuctionBidAttributes(isBidValid, error, auctionItemId, itemDescription, domainName, domainNameContainsNumber, timeLeft, saleTypeDesc, isWebsiteIncluded, isHighBid);
        }
      }

      return _bidAttributes;
    }

    #region Implementation of IResponseData

    public string ToXML()
    {
      throw new NotImplementedException();
    }

    public AtlantisException GetException()
    {
      return _atlEx;
    }

    #endregion
  }
}
