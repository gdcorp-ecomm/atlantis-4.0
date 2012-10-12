using System.Text.RegularExpressions;
using Atlantis.Framework.ShopperValidator.Interface.Validator;

namespace Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules
{
  public class RegexRule : ValidationRule
  {
    private Regex _regexPattern;
    private bool _isValidIfDoesntMatchRegex;
    public RegexRule(string fieldName, string textToValidate, Regex regexPattern, bool isValidIfDoesntMatchRegex = false)
      : base()
    {
      _regexPattern = regexPattern;
      _isValidIfDoesntMatchRegex = isValidIfDoesntMatchRegex;
      base.ItemToValidate = textToValidate;
      base.ErrorMessage = string.Concat(fieldName, " is an invalid format.");
    }

    public override void Validate()
    {
      base.IsValid = false;
      if (base.ItemToValidate != null && _regexPattern != null)
      {
        bool regexIsMatch = _regexPattern.IsMatch(base.ItemToValidate);
        base.IsValid = _isValidIfDoesntMatchRegex ? !regexIsMatch : regexIsMatch;
      }
    }
  }
}
