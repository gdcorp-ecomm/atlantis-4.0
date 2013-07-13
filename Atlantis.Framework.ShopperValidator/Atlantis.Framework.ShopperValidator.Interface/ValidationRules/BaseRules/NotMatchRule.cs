using System;
using Atlantis.Framework.ShopperValidator.Interface.Validator;

namespace Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules
{
  public class NotMatchRule : ValidationRule
  {
    private StringComparison _comparisonType;
    private string _compareText;

    public NotMatchRule(string fieldName, string textToValidate, string compareFieldName, string compareText, StringComparison comparisionType = StringComparison.InvariantCultureIgnoreCase, string culture = "")
      : base(culture)
    {
      base.ItemToValidate = textToValidate;
      base.ErrorMessage = string.Format(FetchResource.GetString("notMatch"), fieldName, compareFieldName);
      _comparisonType = comparisionType;
      _compareText = compareText;
    }

    public NotMatchRule(string culture, string fieldName, string textToValidate, string compareFieldName, string compareText, StringComparison comparisionType = StringComparison.InvariantCultureIgnoreCase)
      : this(fieldName, textToValidate, compareFieldName, compareText, comparisionType, culture)
    { }

    public override void Validate()
    {
      base.IsValid = false;
      if (base.ItemToValidate != null && _compareText != null)
      {
        base.IsValid = !base.ItemToValidate.Equals(_compareText, _comparisonType);
      }
    }
  }
}
