using Atlantis.Framework.PromoData.Interface;

namespace Atlantis.Framework.PromoData.Impl
{
  public class ProductAwardCurrency : IProductAwardCurrency
  {
    private string _transactionalCurrency;
    public string TransactionalCurrency
    {
      get { return _transactionalCurrency; }
    }

    private int _awardAmount;
    public int AwardAmount
    {
      get { return _awardAmount; }
    }

    public ProductAwardCurrency(string transactionalCurrency, int awardAmount)
    {
      this._transactionalCurrency = transactionalCurrency;
      this._awardAmount = awardAmount;
    }
  }
}
