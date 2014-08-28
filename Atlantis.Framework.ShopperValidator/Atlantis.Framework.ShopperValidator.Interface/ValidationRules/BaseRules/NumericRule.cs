using Atlantis.Framework.ShopperValidator.Interface.RuleConstants;

namespace Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules
{
  public class NumericRule : RegexRule
  {
    public NumericRule(string fieldName, string textToValidate, string culture = "")
      : base(fieldName, textToValidate, RegexConstants.NumericOnly, false, culture)
    {
      base.ErrorMessage = string.Format(FetchResource.GetString("numericRule"), fieldName);
      base.RuleName = "NumericRule";
    }

  }
}
