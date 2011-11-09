using System;
using System.Collections.Generic;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionMostActiveByPrice.Interface
{
  public class AuctionMostActiveByPriceResponseData : IResponseData
  {
    private AtlantisException _atlEx;
    public bool IsSuccess { get; private set; }
    public string ResponseXml { get; private set; }
    public List<AuctionMostActiveDomainByPrice> AuctionList = null;

    public AuctionMostActiveByPriceResponseData(string responseXml)
    {
      if (!string.IsNullOrEmpty(responseXml))
      {
        ResponseXml = responseXml;
        AuctionList = ProcessAuctionListXml;
        IsSuccess = true;
      }
    }

    public AuctionMostActiveByPriceResponseData(RequestData oRequestData, Exception ex)
    {
      _atlEx = new AtlantisException(oRequestData,
        "AuctionMostActiveByPriceResponseData",
        ex.Message,
        oRequestData.ToXML());
    }

    private List<AuctionMostActiveDomainByPrice> ProcessAuctionListXml
    {
      get
      {
        int id = 0;
        List<AuctionMostActiveDomainByPrice> _auctionList = new List<AuctionMostActiveDomainByPrice>();
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(ResponseXml);
        XmlNodeList domainNodes = xmlDoc.SelectNodes("/MostActiveDomainsByPrice/Domain");
        foreach (XmlNode node in domainNodes)
        {
          AuctionMostActiveDomainByPrice auction = new AuctionMostActiveDomainByPrice();

          if(!int.TryParse(node.SelectSingleNode("AuctionTypeID").InnerText,out id))
          {
            id = 0;
          }
          auction.AuctionTypeId = id;
          id = 0;
          if (!int.TryParse(node.SelectSingleNode("MemberItemID").InnerText, out id))
          {
            id = 0;
          }
          auction.MemberItemId = id;

          auction.AuctionLink = node.SelectSingleNode("MemberItemID").Attributes["AuctionLink"].Value;


          auction.DomainName = node.SelectSingleNode("DomainNameTLD").InnerText.Trim().ToLower();
          auction.CurrentPrice = node.SelectSingleNode("CurrentPrice").InnerText;
          auction.StartingBidAmount = node.SelectSingleNode("StartingBidAmount").InnerText;
          auction.AuctionEndTime = node.SelectSingleNode("AuctionEndTime").InnerText.Trim();
          auction.TimeLeft = node.SelectSingleNode("TimeLeft").InnerText.Trim();
          
          if(!int.TryParse(node.SelectSingleNode("TrafficValue").InnerText.Trim(), out id))
          {
            id = 0;
          }
          auction.Traffic = id;

          if (!int.TryParse(node.SelectSingleNode("NumberOfBids").InnerText.Trim(), out id))
          {
            id = 0;
          }
          auction.BidCount = id;

          if (!int.TryParse(node.SelectSingleNode("DomainID").InnerText.Trim(), out id))
          {
            id = 0;
          }
          auction.DomainId = id;


          _auctionList.Add(auction);
        }
        
        return _auctionList;
      }
    }

    #region Implementation of IResponseData

    public string ToXML()
    {
      return ResponseXml;
    }

    public AtlantisException GetException()
    {
      return _atlEx;
    }

    #endregion

  }
}
