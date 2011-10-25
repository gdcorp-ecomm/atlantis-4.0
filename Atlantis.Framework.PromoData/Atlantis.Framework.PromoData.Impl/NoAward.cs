using Atlantis.Framework.PromoData.Interface;

namespace Atlantis.Framework.PromoData.Impl
{
  class NoAward : IProductAward
  {
    public string AwardType
    {
      get { return string.Empty; }
    }

    public int QuantityLimit
    {
      get { return -1; }
    }

    private int _productId;
    public int ProductId
    {
      get { return _productId; }
    }
    public string AwardField
    {
      get { return string.Empty; }
    }
    public string AwardFieldValue
    {
      get { return string.Empty; }
    }

    public IPromoCondition PromoCondition
    {
      get { return new PromoCondition(); }
    }

    public NoAward(int productId)
    {
      this._productId = productId;
    }

    public int GetAwardAmount(string currencyType)
    {
      return 0;
    }
  }
}
