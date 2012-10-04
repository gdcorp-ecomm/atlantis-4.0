using Atlantis.Framework.Providers.Interface.Currency;

namespace Atlantis.Framework.ResourcePricing.Interface
{
  public class LockedPrice:ICurrencyPrice
  {
    private int _price;
    private ICurrencyInfo _currencyInfo;
    private CurrencyPriceType _type;

    public LockedPrice(int price,ICurrencyInfo currencyInfo, CurrencyPriceType type)
    {
      _price = price;
      _currencyInfo = currencyInfo;
      _type = type;

    }

    #region ICurrencyPrice Members

    public int Price
    {
      get { return _price; }
    }

    public ICurrencyInfo CurrencyInfo
    {
      get { return _currencyInfo; }
    }

    public CurrencyPriceType Type
    {
      get { return _type; }
    }

    #endregion ICurrencyPrice Members
  }
}
