
namespace Atlantis.Framework.AuctionRecommendations.Interface
{
  public class DomainAuctionAttributes
  {
    string auctionIDValue;
    string domainNameValue;
    string auctionEndTimeValue;
    string memberIDValue;
    string typeValue;
    string auctionTypeIdValue;
    string auctionTypeValue;
    string priceValue;
    string urlValue;
    int listingCountValue;

    public DomainAuctionAttributes(string auctionID, string domain, string auctionEndtime,
        string memberID, string type, string auctionTypeID, string auctionType, string price, string url, int count)
    {
      auctionIDValue = auctionID;
      domainNameValue = domain;
      auctionEndTimeValue = auctionEndtime;
      memberIDValue = memberID;
      typeValue = type;
      auctionTypeIdValue = auctionTypeID;
      auctionTypeValue = auctionType;
      priceValue = price;
      urlValue = url;
      listingCountValue = count;
    }

    public string AuctionID
    {
      get { return auctionIDValue; }
    }

    public string Domain
    {
      get { return domainNameValue; }
    }

    public string AuctionEndTime
    {
      get { return auctionEndTimeValue; }
    }

    public string MemberID
    {
      get { return memberIDValue; }
    }

    public string Type
    {
      get { return typeValue; }
    }

    public string AuctionTypeID
    {
      get { return auctionTypeIdValue; }
    }

    public string AuctionType
    {
      get { return auctionTypeValue; }
    }

    public string Price
    {
      get { return priceValue; }
    }

    public string URL
    {
      get { return urlValue; }
    }

    public int Count
    {
      get { return listingCountValue; }
    }
  }
}
