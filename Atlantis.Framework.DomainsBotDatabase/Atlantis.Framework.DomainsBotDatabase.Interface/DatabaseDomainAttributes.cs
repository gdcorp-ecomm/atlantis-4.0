using System;

namespace Atlantis.Framework.DomainsBotDatabase.Interface
{
  public class DatabaseDomainAttributes
  {
    private int _price = 0;
    public int Price
    {
      get { return _price; }
    }

    private int _commission = 0;
    public int Commission
    {
      get { return _commission; }
    }

    private DateTime _auctionEndTime = DateTime.MinValue;
    public DateTime AuctionEndTime
    {
      get { return _auctionEndTime; }
    }

    public DatabaseDomainAttributes(int price, int commission, DateTime auctionEndTime)
    {
      this._price = price;
      this._commission = commission;
      this._auctionEndTime = auctionEndTime;
    }
  }
}
