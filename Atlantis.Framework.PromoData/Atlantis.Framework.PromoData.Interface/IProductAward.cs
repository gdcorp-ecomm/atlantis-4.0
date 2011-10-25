namespace Atlantis.Framework.PromoData.Interface
{
  public interface IProductAward
  {
    string AwardType { get; }
    int QuantityLimit { get; }
    int ProductId { get; }
    string AwardField { get; }
    string AwardFieldValue { get; }
    IPromoCondition PromoCondition { get; }
    int GetAwardAmount(string currencyType);
  }
}
