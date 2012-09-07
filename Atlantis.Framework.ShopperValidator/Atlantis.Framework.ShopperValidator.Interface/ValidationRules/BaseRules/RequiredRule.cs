using Atlantis.Framework.ShopperValidator.Interface.Validator;

namespace Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules
{
 public class RequiredRule: ValidationRule
  {
   public RequiredRule(string fieldName, string textToValidate)
      : base()
    {
      base.ItemToValidate = textToValidate;
      base.ErrorMessage = string.Concat(fieldName, " is required");
    }

    public override void Validate()
    {
      base.IsValid = false;
      if (base.ItemToValidate != null)
      {
        base.IsValid = base.ItemToValidate.Trim().Length > 0;
      }
    }
  }
}
