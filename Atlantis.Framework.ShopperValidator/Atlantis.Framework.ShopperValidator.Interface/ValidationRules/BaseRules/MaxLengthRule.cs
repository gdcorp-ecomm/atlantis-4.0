using Atlantis.Framework.ShopperValidator.Interface.Validator;

namespace Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules
{
  public class MaxLengthRule : ValidationRule
  {
    public int MaxLength { get; private set; }

    public MaxLengthRule(string fieldName, string textToValidate, int maxLength = int.MaxValue, string culture = "")
      : base(culture)
    {
      var maxLengthError = FetchResource.GetString("maxLengthError");

      MaxLength = maxLength;
      base.ItemToValidate = textToValidate;
      base.ErrorMessage = string.Format(maxLengthError, fieldName, MaxLength.ToString());
    }

    public MaxLengthRule(string culture, string fieldName, string textToValidate, int maxLength = int.MaxValue)
      : this(fieldName, textToValidate, maxLength, culture)
    { }

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
