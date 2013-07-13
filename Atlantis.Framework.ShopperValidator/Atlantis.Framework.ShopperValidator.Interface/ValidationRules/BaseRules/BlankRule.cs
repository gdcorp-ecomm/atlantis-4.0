using Atlantis.Framework.ShopperValidator.Interface.Validator;

namespace Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules
{
  public class BlankRule : ValidationRule
  {
    public BlankRule(bool isValid, string errorMessage = "", string culture = "")
      : base(culture)
    {
      base.IsValid = isValid;
      if (string.IsNullOrEmpty(errorMessage))
      {
        base.ErrorMessage = FetchResource.GetString("invalidData");
      }
      else
      {
        base.ErrorMessage = errorMessage;
      }

    }

    public override void Validate()
    { }
  }
}
