namespace Atlantis.Framework.PromoData.Interface
{
  public interface IPromoCondition
  {
    string ConditionField { get; }
    string conditionFieldValue { get; }
    int ProductMinQty { get; }
    string ProductExactQty { get; }
  }
}
