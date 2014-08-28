using System.Text.RegularExpressions;
using Atlantis.Framework.ShopperValidator.Interface.Validator;

namespace Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules
{
  public class RegexRule : ValidationRule
  {
    private Regex _regexPattern;
    private bool _isValidIfDoesntMatchRegex;
    public RegexRule(string fieldName, string textToValidate, Regex regexPattern, bool isValidIfDoesntMatchRegex = false, string culture = "")
      : base(culture)
    {
      _regexPattern = regexPattern;
      _isValidIfDoesntMatchRegex = isValidIfDoesntMatchRegex;
      base.ItemToValidate = textToValidate;
      base.ErrorMessage = string.Format(FetchResource.GetString("invalidFormat"), fieldName);
    }

    public RegexRule(string culture, string fieldName, string textToValidate, Regex regexPattern, bool isValidIfDoesntMatchRegex = false)
      : this(fieldName, textToValidate, regexPattern, isValidIfDoesntMatchRegex, culture)
    { }


    public override void Validate()
    {
      base.IsValid = false;
      base.RuleName = "RegexRule";
      if (base.ItemToValidate != null && _regexPattern != null)
      {
        bool regexIsMatch = _regexPattern.IsMatch(base.ItemToValidate);
        base.IsValid = _isValidIfDoesntMatchRegex ? !regexIsMatch : regexIsMatch;
      }
    }
  }
}
