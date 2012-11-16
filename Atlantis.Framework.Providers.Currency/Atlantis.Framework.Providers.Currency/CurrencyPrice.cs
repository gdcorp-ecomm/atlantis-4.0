using Atlantis.Framework.Providers.Interface.Currency;
using System;

namespace Atlantis.Framework.Providers.Currency
{
  public class CurrencyPrice : ICurrencyPrice, IEquatable<ICurrencyPrice>
  {
    private int _price;
    private ICurrencyInfo _currencyInfo;
    private CurrencyPriceType _type;

    public CurrencyPrice(int price, ICurrencyInfo currencyInfo, CurrencyPriceType type)
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

    #endregion

    #region Operator Overloads

    public override bool Equals(object obj)
    {
      bool result = false;

      ICurrencyPrice price = obj as ICurrencyPrice;
      if ((price != null) && (this.CurrencyInfo.Equals(price.CurrencyInfo)))
      {
        result = (Price == price.Price);
      }

      return result;
    }

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }

    #endregion

    #region IEquatable<ICurrencyPrice> Members

    public bool Equals(ICurrencyPrice other)
    {
      bool result = false;

      if ((other != null) && (this.CurrencyInfo.Equals(other.CurrencyInfo)))
      {
        result = (Price == other.Price);
      }

      return result;
    }

    #endregion
  }
}
