using Atlantis.Framework.PromoData.Interface;

namespace Atlantis.Framework.PromoData.Impl
{
  public class PromoCondition : IPromoCondition
  {
    private string _ConditionField;
    public string ConditionField { get { return _ConditionField; } }

    private string _ConditionFieldValue;
    public string conditionFieldValue { get { return _ConditionFieldValue; } }

    private int _productMinQty;
    public int ProductMinQty { get { return _productMinQty; } }

    private string _productExactQty;
    public string ProductExactQty { get { return _productExactQty; } }

    public PromoCondition()
    {
      this._ConditionField = string.Empty;
      this._ConditionFieldValue = string.Empty;
      this._productMinQty = 1;
      this._productExactQty = string.Empty;
    }

    public PromoCondition(int productMinQty, string productExactQty, string conditionField, string conditionFieldValue)
    {
      this._productExactQty = productExactQty;
      this._productMinQty = productMinQty;
      this._ConditionField = conditionField;
      this._ConditionFieldValue = conditionFieldValue;
    }
  }
}
