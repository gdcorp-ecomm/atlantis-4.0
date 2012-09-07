using Atlantis.Framework.ShopperValidator.Interface.Validator;
using System;

namespace Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules
{
  public class MatchRule : ValidationRule
  {
    private StringComparison _comparisonType;
    private string _compareText;

    public MatchRule(string fieldName, string textToValidate, string compareFieldName, string compareText, StringComparison comparisionType = StringComparison.InvariantCultureIgnoreCase)
      : base()
    {
      base.ItemToValidate = textToValidate;
      base.ErrorMessage = string.Concat(fieldName, " must be equal to ", compareFieldName);
      _comparisonType = comparisionType;
      _compareText = compareText;
    }

    public override void Validate()
    {
      base.IsValid = false;
      if (base.ItemToValidate != null && _compareText != null)
      {
        base.IsValid = base.ItemToValidate.Equals(_compareText, _comparisonType);
      }
    }
  }
}
