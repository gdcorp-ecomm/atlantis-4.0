using System;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionConfirmAcceptOffer.Interface
{
  public class AuctionConfirmAcceptOfferResponseData : IResponseData
  {
    private AtlantisException _atlEx;
    private string _auctionAcceptCounterOfferXml = string.Empty;

    public bool IsSuccess { get; private set; }

    public bool IsValid { get; private set; }

    public string AuctionItemId { get; private set; }

    public string ItemDescription { get; private set; }

    public string DomainName { get; private set; }

    public string TimeLeft { get; private set; }

    public string SaleTypeDescription { get; private set; }

    public bool? IsWebsiteIncluded { get; private set; }

    public string ErrorNumber { get; private set; }

    public string RawErrorString { get; private set; }

    public string FriendlyErrorString { get; private set; }


    public AuctionConfirmAcceptOfferResponseData(string responseXml)
    {
      IsWebsiteIncluded = null;
      if (!string.IsNullOrEmpty(responseXml))
      {
        _auctionAcceptCounterOfferXml = responseXml;
        IsSuccess = true;
        _atlEx = null;
        PopulateFromXml(responseXml);
      }
    }

    public AuctionConfirmAcceptOfferResponseData(RequestData oRequestData, Exception ex)
    {
      IsWebsiteIncluded = null;
      _atlEx = new AtlantisException(oRequestData,
        "AuctionConfirmAccceptOfferResponseData",
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
          XElement auctionResponseElement = xDoc.Element("ConfirmAcceptOffer");

          if (auctionResponseElement != null)
          {
            IsValid = (auctionResponseElement.Attribute("Valid") != null ? string.Compare(auctionResponseElement.Attribute("Valid").Value, "true", true) == 0 : false);
            AuctionItemId = (auctionResponseElement.Attribute("MemberItemId") != null ? auctionResponseElement.Attribute("MemberItemId").Value.Trim() : string.Empty);
            ItemDescription = (auctionResponseElement.Attribute("ItemDescription") != null ? auctionResponseElement.Attribute("ItemDescription").Value.Trim() : string.Empty);
            DomainName = (auctionResponseElement.Attribute("DomainName") != null ? auctionResponseElement.Attribute("DomainName").Value.Trim() : string.Empty);
            TimeLeft = (auctionResponseElement.Attribute("TimeLeft") != null ? auctionResponseElement.Attribute("TimeLeft").Value.Trim() : string.Empty);
            SaleTypeDescription = (auctionResponseElement.Attribute("SaleTypeDesc") != null ? auctionResponseElement.Attribute("SaleTypeDesc").Value.Trim() : string.Empty);

            if (auctionResponseElement.Attribute("WebSiteIncludedFlag") != null)
            {
              IsWebsiteIncluded =
                string.Compare(auctionResponseElement.Attribute("WebSiteIncludedFlag").Value, "true", true) == 0;
            }

            if (!IsValid)
            {
              XElement auctionErrorElement = auctionResponseElement.Element("ErrorData");

              if (auctionErrorElement != null)
              {
                ErrorNumber = (auctionErrorElement.Attribute("errornumber") != null ? auctionErrorElement.Attribute("errornumber").Value.Trim() : string.Empty);
                RawErrorString = (auctionErrorElement.Element("RawErrorString") != null ? auctionErrorElement.Element("RawErrorString").Value.Trim() : string.Empty);
                FriendlyErrorString = (auctionErrorElement.Element("FriendlyErrorString") != null ? auctionErrorElement.Element("FriendlyErrorString").Value.Trim() : string.Empty);
              }

            }
          }
        }
      }
    }

    #region Implementation of IResponseData

    public string ToXML()
    {
      return _auctionAcceptCounterOfferXml;
    }

    public AtlantisException GetException()
    {
      return _atlEx;
    }

    #endregion
  }
}
