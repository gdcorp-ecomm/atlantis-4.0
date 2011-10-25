using System.Collections.Generic;
using Atlantis.Framework.PromoData.Interface;

namespace Atlantis.Framework.PromoData.Impl
{
  public class ProductAward : IProductAward
  {
    private string _awardType;
    public string AwardType
    {
      get { return _awardType; }
    }

    private int _quantityLimit;
    public int QuantityLimit
    {
      get { return _quantityLimit; }
    }

    private int _productId;
    public int ProductId
    {
      get { return _productId; }
    }

    private string _awardField = string.Empty;
    public string AwardField
    {
      get { return _awardField; }
      set { _awardField = value; }
    }

    private string _awardFieldValue = string.Empty;
    public string AwardFieldValue
    {
      get { return _awardFieldValue; }
      set { _awardFieldValue = value; }
    }

    private IPromoCondition _promoCondition;
    public IPromoCondition PromoCondition
    {
      get { return _promoCondition;  }
    }

    private List<IProductAwardCurrency> _productAwardCurrencies;

    public ProductAward(string awardType, int productId, int quantityLimit, IPromoCondition promoCondition)
    {
      this._awardType = awardType;
      this._productId = productId;
      this._quantityLimit = quantityLimit;
      this._promoCondition = promoCondition;
      this._productAwardCurrencies = new List<IProductAwardCurrency>();
    }

    public void AddProductAwardCurrency(IProductAwardCurrency productAwardCurrency)
    {
      this._productAwardCurrencies.Add(productAwardCurrency);
    }

    public int GetAwardAmount(string currencyType)
    {
      int amount = 0;

      IProductAwardCurrency award 
        = _productAwardCurrencies.Find((IProductAwardCurrency item) => (item.TransactionalCurrency.Equals(currencyType,
        System.StringComparison.InvariantCultureIgnoreCase) || item.TransactionalCurrency.Equals("{all}")));

      if (award != null)
      {
        amount = award.AwardAmount;
      }

      return amount;
    }
  }
}
