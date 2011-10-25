namespace Atlantis.Framework.PromoData.Interface
{
  public interface IProductAwardCurrency
  {
    string TransactionalCurrency { get; }
    int AwardAmount { get; }
  }
}
