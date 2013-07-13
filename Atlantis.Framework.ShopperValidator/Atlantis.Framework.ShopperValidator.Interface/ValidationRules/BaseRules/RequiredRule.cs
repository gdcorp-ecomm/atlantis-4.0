using Atlantis.Framework.ShopperValidator.Interface.Validator;

namespace Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules
{
  public class RequiredRule : ValidationRule
  {
    public RequiredRule(string fieldName, string textToValidate, string culture = "")
      : base(culture)
    {
      base.ItemToValidate = textToValidate;
      base.ErrorMessage = string.Format(FetchResource.GetString("required"), fieldName);
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
