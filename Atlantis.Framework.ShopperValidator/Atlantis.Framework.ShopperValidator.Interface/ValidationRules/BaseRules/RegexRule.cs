using System.Text.RegularExpressions;
using Atlantis.Framework.ShopperValidator.Interface.Validator;

namespace Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules
{
  public class RegexRule : ValidationRule
  {
    private Regex _regexPattern;
    public RegexRule(string fieldName, string textToValidate, Regex regexPattern)
      : base()
    {
      _regexPattern = regexPattern;
      base.ItemToValidate = textToValidate;
      base.ErrorMessage = string.Concat(fieldName, " is an invalid format.");
    }

    public override void Validate()
    {
      base.IsValid = false;
      if (base.ItemToValidate != null && _regexPattern != null)
      {
        base.IsValid = _regexPattern.IsMatch(base.ItemToValidate);
      }
    }
  }
}
