using Atlantis.Framework.ShopperValidator.Interface.Validator;

namespace Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules
{
  public class MaxLengthRule : ValidationRule
  {
    public int MaxLength { get; private set; }

    public MaxLengthRule(string fieldName, string textToValidate, int maxLength = int.MaxValue)
      : base()
    {
      MaxLength = maxLength;
      base.ItemToValidate = textToValidate;
      base.ErrorMessage = string.Concat(fieldName, " must be less than ", MaxLength.ToString(), " characters.");
    }

    public override void Validate()
    {
      base.IsValid = false;
      if (base.ItemToValidate != null)
      {
        base.IsValid = base.ItemToValidate.Length <= MaxLength;
      }
    }
  }
}
