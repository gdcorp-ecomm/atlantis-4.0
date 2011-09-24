using System;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionConfirmCounterOffer.Interface
{
  public class AuctionConfirmCounterOfferResponseData : IResponseData
  {
    private AtlantisException _atlEx;
    private string _auctionConfirmCounterOfferXml = string.Empty;

    public bool IsSuccess { get; private set; }
    
    public bool IsValid { get; private set; }
    
    public string Error { get; private set; }
    
    public string AuctionItemId { get; private set; }
    
    public string ItemDescription { get; private set; }
    
    public string DomainName { get; private set; }
    
    public string TimeLeft { get; private set; }
    
    public string SaleTypeDescription { get; private set; }

    public bool? IsWebsiteIncluded { get; private set; }

    public string OfferAmount { get; private set; }


    public AuctionConfirmCounterOfferResponseData(string responseXml)
    {
      IsWebsiteIncluded = null;
      if (!string.IsNullOrEmpty(responseXml))
      {
        _auctionConfirmCounterOfferXml = responseXml;
        IsSuccess = true;
        _atlEx = null;
        PopulateFromXml(responseXml);
      }
    }

    public AuctionConfirmCounterOfferResponseData(RequestData oRequestData, Exception ex)
    {
      IsWebsiteIncluded = null;
      _atlEx = new AtlantisException(oRequestData,
        "AuctionConfirmCounterOfferResponseData",
      ex.Message,
      oRequestData.ToXML());
    }


    private void PopulateFromXml(string responsXml)
    {
      if (!string.IsNullOrEmpty(responsXml))
      {
        XDocument xDoc = XDocument.Parse(responsXml);

        if (xDoc.Root != null)
        {
          XElement auctionResponseElement = xDoc.Element("ConfirmCounterOffer");

          if (auctionResponseElement != null)
          {
            IsValid = (auctionResponseElement.Attribute("Valid") != null ? string.Compare(auctionResponseElement.Attribute("Valid").Value, "true", true) == 0 : false);
            Error = (auctionResponseElement.Attribute("Error") != null ? auctionResponseElement.Attribute("Error").Value.Trim() : string.Empty);
            AuctionItemId = (auctionResponseElement.Attribute("MemberItemId") != null ? auctionResponseElement.Attribute("MemberItemId").Value.Trim() : string.Empty);
            ItemDescription = (auctionResponseElement.Attribute("ItemDescription") != null ? auctionResponseElement.Attribute("ItemDescription").Value.Trim() : string.Empty);
            DomainName = (auctionResponseElement.Attribute("DomainName") != null ? auctionResponseElement.Attribute("DomainName").Value.Trim() : string.Empty);
            TimeLeft = (auctionResponseElement.Attribute("TimeLeft") != null ? auctionResponseElement.Attribute("TimeLeft").Value.Trim() : string.Empty);
            SaleTypeDescription = (auctionResponseElement.Attribute("SaleTypeDesc") != null ? auctionResponseElement.Attribute("SaleTypeDesc").Value.Trim() : string.Empty);
            OfferAmount = (auctionResponseElement.Attribute("OfferAmount") != null ? auctionResponseElement.Attribute("OfferAmount").Value.Trim() : string.Empty);

            if (auctionResponseElement.Attribute("WebSiteIncludedFlag") != null)
            {
              IsWebsiteIncluded = string.Compare(auctionResponseElement.Attribute("WebSiteIncludedFlag").Value, "true", true) == 0;
            }
          }
        }
      }
    }

    #region Implementation of IResponseData

    public string ToXML()
    {
      return _auctionConfirmCounterOfferXml;
    }

    public AtlantisException GetException()
    {
      return _atlEx;
    }

    #endregion
  }
}
