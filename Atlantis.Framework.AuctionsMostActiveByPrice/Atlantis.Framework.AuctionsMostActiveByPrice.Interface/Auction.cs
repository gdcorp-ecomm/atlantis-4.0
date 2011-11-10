using System.Diagnostics;

namespace Atlantis.Framework.AuctionsMostActiveByPrice.Interface
{
  public class Auction
  {
    private int _memberItemId;
    private int _auctionTypeId;
    private string _domainName;
    private string _domainNameAndTld;
    private string _tld;
    private string _currentPrice;
    private string _auctionEndTime;
    private string _timeLeft;
    private int _domainId;
    private int _traffic;
    private int _bidCount;


    #region Properties
    public int MemberItemId
    {
      [DebuggerStepThrough]
      get { return _memberItemId; }
      set { _memberItemId = value; }
    }

    public int AuctionTypeId
    {
      [DebuggerStepThrough]
      get { return _auctionTypeId; }
      set { _auctionTypeId = value; }
    }

    public string DomainName
    {
      [DebuggerStepThrough]
      get { return _domainName; }
      set { _domainName = value; }
    }

    public string DomainNameAndTld
    {
      [DebuggerStepThrough]
      get { return _domainNameAndTld; }
      set { _domainNameAndTld = value; }
    }

    public string Tld
    {
      [DebuggerStepThrough]
      get { return _tld; }
      set { _tld = value; }
    }

    public string CurrentPrice
    {
      [DebuggerStepThrough]
      get { return _currentPrice; }
      set { _currentPrice = value; }
    }

    public string AuctionEndTime
    {
      [DebuggerStepThrough]
      get { return _auctionEndTime; }
      set { _auctionEndTime = value; }
    }

    public string TimeLeft
    {
      [DebuggerStepThrough]
      get { return _timeLeft; }
      set { _timeLeft = value; }
    }

    public int DomainId
    {
      [DebuggerStepThrough]
      get { return _domainId; }
      set { _domainId = value; }
    }

    public int Traffic
    {
      [DebuggerStepThrough]
      get { return _traffic; }
      set { _traffic = value; }
    }

    public int BidCount
    {
      [DebuggerStepThrough]
      get { return _bidCount; }
      set { _bidCount = value; }
    }

    #endregion

    public Auction(int memberItemId,
      int auctionTypeId,
      string domainName,
      string domainNameAndTld,
      string tld,
      string currentPrice,
      string auctionEndTime,
      string timeLeft,
      int domainId,
      int traffic,
      int bidCount)
    {
      _memberItemId = memberItemId;
      _auctionTypeId = auctionTypeId;
      _domainName = domainName;
      _domainNameAndTld = domainNameAndTld;
      _tld = tld;
      _currentPrice = currentPrice;
      _auctionEndTime = auctionEndTime;
      _timeLeft = timeLeft;
      _domainId = domainId;
      _traffic = traffic;
      _bidCount = bidCount;
    }
  }
}
