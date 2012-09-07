using Atlantis.Framework.ShopperValidator.Interface.Validator;

namespace Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules
{
  public class MinLengthRule : ValidationRule
  {
    public int MinLength { get; private set; }

    public MinLengthRule(string fieldName, string textToValidate, int minLength = 0)
      : base()
    {
      MinLength = minLength;
      base.ItemToValidate = textToValidate;
      base.ErrorMessage = string.Concat(fieldName, " must be greater than ", MinLength.ToString(), " characters.");
    }

    public override void Validate()
    {
      base.IsValid = false;
      if (base.ItemToValidate != null)
      {
        base.IsValid = base.ItemToValidate.Length >= MinLength;
      }
    }
  }
}
