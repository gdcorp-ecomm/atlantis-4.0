using System;
using System.Collections.Generic;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionRecommendations.Interface
{
  public class AuctionRecommendationsResponseData : IResponseData
  {
    private AtlantisException _ex;
    private string _auctionReponseXML = string.Empty;
    Dictionary<string, DomainAuctionAttributes> _attributes = null;
    public bool _isSuccess = false;

    public AuctionRecommendationsResponseData(string auctionsXML)
    {
      if (!string.IsNullOrEmpty(auctionsXML))
        _auctionReponseXML = auctionsXML;
      _isSuccess = true;
      _ex = null;
    }

    public AuctionRecommendationsResponseData(RequestData oRequestData, Exception ex)
    {
      _ex = new AtlantisException(oRequestData,
                                "AuctionRecommendationsResponseData",
                                ex.Message.ToString(),
                                oRequestData.ToXML());
    }

    #region IResponseData Members

    public string ToXML()
    {
      throw new NotImplementedException();
    }

    public AtlantisException GetException()
    {
      return _ex;
    }

    public Dictionary<string, DomainAuctionAttributes> AuctionAttributes
    {
      get
      {
        return PopulateFromXML();
      }
    }

    private Dictionary<string, DomainAuctionAttributes> PopulateFromXML()
    {
      _attributes = new Dictionary<string, DomainAuctionAttributes>();
      XmlDocument xDoc = new XmlDocument();
      xDoc.LoadXml(_auctionReponseXML);
      XmlNodeList xnlAuctionItems = xDoc.SelectNodes("/recommendedauctions/auctionlist/auction");
      string auctionIDValue = string.Empty;
      string domainNameValue = string.Empty;
      string auctionEndTimeValue = string.Empty;
      string memberIDValue = string.Empty;
      string typeValue = string.Empty;
      string auctionTypeIdValue = string.Empty;
      string auctionTypeValue = string.Empty;
      string priceValue = string.Empty;
      string urlValue = string.Empty;
      int listingCount = 0;

      foreach (XmlElement xlAuction in xnlAuctionItems)
      {
        auctionIDValue = xlAuction.GetAttribute("auctionid");
        domainNameValue = xlAuction.GetAttribute("domainname");
        auctionEndTimeValue = xlAuction.GetAttribute("auctionendtime");
        memberIDValue = xlAuction.GetAttribute("memberid");
        typeValue = xlAuction.GetAttribute("type");
        auctionTypeIdValue = xlAuction.GetAttribute("auctiontypeid");
        auctionTypeValue = xlAuction.GetAttribute("auctiontype");
        priceValue = xlAuction.GetAttribute("price");
        urlValue = xlAuction.GetAttribute("url");
        listingCount++;

        _attributes[auctionIDValue] = new DomainAuctionAttributes(auctionIDValue, domainNameValue, auctionEndTimeValue,
        memberIDValue, typeValue, auctionTypeIdValue, auctionTypeValue, priceValue, urlValue, listingCount);
      }

      return _attributes;
    }

    #endregion
  }
}
