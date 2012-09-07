using Atlantis.Framework.ShopperValidator.Interface.RuleConstants;

namespace Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules
{
  public class NumericRule : RegexRule
  {
    public NumericRule(string fieldName, string textToValidate)
      : base(fieldName, textToValidate, RegexConstants.NumericOnly)
    {
      base.ErrorMessage = string.Concat(fieldName, " must contain only numbers.");
    }
  }
}
