using Atlantis.Framework.ShopperValidator.Interface.Validator;

namespace Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules
{
  public class BlankRule : ValidationRule
  {
    public BlankRule(bool isValid, string errorMessage = "Invalid data")
      : base()
    {
      base.IsValid = isValid;
      base.ErrorMessage = errorMessage;
    }

    public override void Validate()
    { }
  }
}
