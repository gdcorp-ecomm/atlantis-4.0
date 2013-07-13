using System.Reflection;
using System.Resources;
using Atlantis.Framework.ShopperValidator.Interface.Validator;

namespace Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules
{
  public class MinLengthRule : ValidationRule
  {
    public int MinLength { get; private set; }

    public MinLengthRule(string fieldName, string textToValidate, int minLength = 0, string culture = "")
      : base(culture)
    {
      MinLength = minLength;
      base.ItemToValidate = textToValidate;

      base.ErrorMessage = string.Format(FetchResource.GetString("minLengthError"), fieldName, MinLength.ToString());
    }

    public MinLengthRule(string culture, string fieldName, string textToValidate, int minLength = 0)
      : this(fieldName, textToValidate, minLength, culture)
    {}

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
