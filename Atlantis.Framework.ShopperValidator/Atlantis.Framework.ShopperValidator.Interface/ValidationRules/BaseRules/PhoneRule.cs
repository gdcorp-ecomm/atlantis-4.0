using System;
using Atlantis.Framework.ShopperValidator.Interface.RuleConstants;
using Atlantis.Framework.ShopperValidator.Interface.Validator;

namespace Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules
{
  /// <remarks>
  /// If the phone number is US or Canada then it must be 10 digits and not being with 0 or 1.  Otherwise as long is it's a number it's considered valid.
  /// </remarks>
  public class PhoneRule : ValidationRule
  {
    private string _countryCode;
    private bool _isRequired = false;

    public PhoneRule(string value, string fieldName, bool isRequired = false, string countryCode = "us")
      : base()
    {
      base.ItemToValidate = value;
      base.ErrorMessage = string.Concat(fieldName, " is invalid.");
      base.FieldName = fieldName;
      _isRequired = isRequired;
      _countryCode = countryCode ?? "us";
    }

    public PhoneRule(string value, string fieldName, string countryCode = "us")
      : this(value, fieldName, false, countryCode)
    { }

    public override void Validate()
    {
      base.IsValid = false;
      if (base.ItemToValidate != null)
      {
        /*if (base.ItemToValidate.Length > 0)
        {*/
          base.ItemToValidate = RegexConstants.SpecialCharacters.Replace(base.ItemToValidate, string.Empty);
          bool isUsOrCanada = _countryCode.Equals("us", StringComparison.InvariantCultureIgnoreCase)
            || _countryCode.Equals("ca", StringComparison.InvariantCultureIgnoreCase);

          base.IsValid = RegexConstants.NumericOnly.IsMatch(base.ItemToValidate);
          if (base.IsValid)
          {
            if (isUsOrCanada)
            {
              base.IsValid = RegexConstants.PhoneUsCanada.IsMatch(base.ItemToValidate);
              if (!base.IsValid)
              {
                base.ErrorMessage = string.Concat(base.FieldName, " must not start with a 0 or 1.  Must contain ",
                  LengthConstants.PhoneUsCanadaMaxLength, " characters");
              }
            }
            else
            {
              base.IsValid = base.ItemToValidate.Length <= LengthConstants.PhoneInternationalMaxLengh;
              if (!base.IsValid)
              {
                base.ErrorMessage = string.Concat(base.FieldName, " must contain ", LengthConstants.PhoneInternationalMaxLengh, " characters");
              }
            }
          }
          else
          {
            base.ErrorMessage = string.Concat(base.FieldName, " must be numeric");
          }
        /*}
        else
        {
          if (!_isRequired)
          {
            base.IsValid = true;
          }
          else
          {
            base.ErrorMessage = string.Concat(FieldName, " is required");
          }
        }*/
      }
    }
  }
}
