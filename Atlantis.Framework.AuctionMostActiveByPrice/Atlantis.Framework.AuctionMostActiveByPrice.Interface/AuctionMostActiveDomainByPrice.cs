
namespace Atlantis.Framework.AuctionMostActiveByPrice.Interface
{
  public class AuctionMostActiveDomainByPrice
  {

    string _auctionLink;
    public string AuctionLink
    {
      get { return _auctionLink; }
      set { _auctionLink = value; }
    }
    
    int _memberItemId;
    public int MemberItemId
    {
      get { return _memberItemId; }
      set { _memberItemId = value; }
    }

    int _auctionTypeId;
    public int AuctionTypeId
    {
      get { return _auctionTypeId; }
      set { _auctionTypeId = value; }
    }

    string _domainName;
    public string DomainName
    {
      get { return _domainName; }
      set { _domainName = value; }
    }

    string _currentPrice;
    public string CurrentPrice
    {
      get { return _currentPrice; }
      set { _currentPrice = value; }
    }

    string _startingBidAmount;
    public string StartingBidAmount
    {
      get { return _startingBidAmount; }
      set { _startingBidAmount = value; }
    }

    string _auctionEndTime;
    public string AuctionEndTime
    {
      get { return _auctionEndTime; }
      set { _auctionEndTime = value; }
    }

    string _timeLeft;
    public string TimeLeft
    {
      get { return _timeLeft; }
      set { _timeLeft = value; }
    }

    int domainId;
    public int DomainId
    {
      get { return domainId; }
      set { domainId = value; }
    }

    int _traffic;
    public int Traffic
    {
      get { return _traffic; }
      set { _traffic = value; }
    }

    int _bidCount;
    public int BidCount
    {
      get { return _bidCount; }
      set { _bidCount = value; }
    }

  }
}
