
namespace Atlantis.Framework.RegDomainsDbsCheck.Interface
{
  public class DbsAttributes
  {
    private bool _isDbsCapable;
    public bool IsDbsCapable
    {
      get { return _isDbsCapable; }
    }

    private bool _isAuctionActive;
    public bool IsAuctionActive
    {
      get { return _isAuctionActive; }
    }

    private string _auctionLink;
    public string AuctionLink
    {
      get { return _auctionLink; }
    }

    public DbsAttributes(bool isDbsCapable, bool isAuctionActive, string auctionLink)
    {
      this._isDbsCapable = isDbsCapable;
      this._isAuctionActive = isAuctionActive;
      this._auctionLink = auctionLink;
    }
  }
}
