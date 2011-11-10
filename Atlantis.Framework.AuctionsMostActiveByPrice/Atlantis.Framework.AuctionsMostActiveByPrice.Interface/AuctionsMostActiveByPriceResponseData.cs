using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionsMostActiveByPrice.Interface
{
  public class AuctionsMostActiveByPriceResponseData : IResponseData
  {
    private string _responseXML = null;
    private AtlantisException _exAtlantis = null;
    private bool _success = false;
    private List<Auction> _auctions = null;

    public List<Auction> GetAuctionsList() 
    { 
      return new List<Auction>(_auctions); 
    } 

    public string ResponseXML { get { return _responseXML; } }

    public bool IsSuccess { get { return _success; } }

    public AuctionsMostActiveByPriceResponseData(string responseXML)
    {
      int memberItemId;
      int auctionTypeId;
      string domainName;
      string domainNameAndTld;
      string tld = string.Empty;    //Currently unused
      string currentPrice;
      string auctionEndTime;
      string timeLeft;
      int domainId = 0;             //Currently unused
      int traffic;
      int bidCount;

      _auctions = new List<Auction>();

      XmlDocument xmlDoc = new XmlDocument();
      xmlDoc.LoadXml(responseXML);
      XmlNodeList domainNodes = xmlDoc.SelectNodes("/MostActiveDomainsByPrice/Domain");
      int id = 0;
      //int rowCount = 0;
      foreach (XmlNode node in domainNodes)
      {
        int.TryParse(node.SelectSingleNode("AuctionTypeID").InnerText, out id);

        if (IsInvalidAuctionTypeId(id))
          continue;
        
        auctionTypeId = id; 
        id = 0;

        int.TryParse(node.SelectSingleNode("MemberItemID").InnerText.Trim(), out id);
        memberItemId = id; 
        id = 0;

        domainName = node.SelectSingleNode("DomainNameTLD").InnerText.Trim().ToLower();
        domainNameAndTld = domainName;

        currentPrice = node.SelectSingleNode("CurrentPrice").InnerText;
        auctionEndTime = node.SelectSingleNode("AuctionEndTime").InnerText.Trim();

        try
        {
          timeLeft = FormatTimeLeft(Convert.ToDateTime(auctionEndTime));
        }
        catch
        {
          timeLeft = "not available";
        }

        traffic = 0;
        int.TryParse(node.SelectSingleNode("TrafficValue").InnerText.Trim(), out traffic);
        bidCount = 0;
        int.TryParse(node.SelectSingleNode("NumberOfBids").InnerText.Trim(), out bidCount);

        _auctions.Add(new Auction(memberItemId,
          auctionTypeId,
          domainName,
          domainNameAndTld,
          tld,
          currentPrice,
          auctionEndTime,
          timeLeft,
          domainId,
          traffic,
          bidCount));
      }

      _responseXML = responseXML;
      _success = true;

    }

    public AuctionsMostActiveByPriceResponseData(string responseXML, AtlantisException exAtlantis)
    {
      _responseXML = responseXML;
      _exAtlantis = exAtlantis;
    }

    public AuctionsMostActiveByPriceResponseData(string responseXML, RequestData requestData, Exception ex)
    {
      _responseXML = responseXML;
      _exAtlantis = new AtlantisException(requestData,
                                          "AuctionsMostActiveByPriceResponseData",
                                          ex.Message.ToString(),
                                          requestData.ToString());
    }

    #region Helper Methods

    private bool IsInvalidAuctionTypeId(int auctionTypeId)
    {
      bool isInvalid = false;

      switch (auctionTypeId)
      {
        case 0:
        case 1:
        case 4:
        case 5:
        case 7:
        case 12:
        case 19:
        case 22:
          isInvalid = true;
          break;
        default:
          isInvalid = false;
          break;
      }
      return isInvalid;
    }

    private string FormatTimeLeft(DateTime endDate)
    {
      DateTime now = DateTime.Now;
      TimeSpan timeLeft = endDate.Subtract(now);

      if (timeLeft <= TimeSpan.Zero)
        return "0 mins";

      StringBuilder timeFormat = new StringBuilder();

      if (timeLeft.Days > 0)
        timeFormat.AppendFormat("{0} day{1}", timeLeft.Days, timeLeft.Days > 1 ? "s" : string.Empty);

      if (timeLeft.Hours > 0)
      {
        timeFormat.AppendFormat(", {0} hour{1}", timeLeft.Hours, timeLeft.Hours > 1 ? "s" : string.Empty);
      }

      if (timeLeft.Minutes > 0)
      {
        timeFormat.AppendFormat(", {0} min{1}", timeLeft.Minutes, timeLeft.Minutes > 1 ? "s" : string.Empty);
      }

      if (timeFormat.Length == 0)
        timeFormat.Append("0 mins");

      return timeFormat.ToString().Trim().Trim(',');
    }

    #endregion

    #region IResponseData Members

    public string ToXML()
    {
      return _responseXML;
    }

    public AtlantisException GetException()
    {
      return _exAtlantis;
    }

    #endregion
  }
}
