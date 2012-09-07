using Atlantis.Framework.ShopperValidator.Interface.RuleConstants;

namespace Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules
{
  public class XssRule: RegexRule
  {
    public XssRule(string fieldName, string textToValidate)
      :base(fieldName, textToValidate, RegexConstants.InvalidXssTags)
    {
      base.ErrorMessage = string.Concat(fieldName, " contains invalid characters");
    }

    public override void Validate()
    {
      base.Validate();
      this.IsValid = !base.IsValid;
    }
  }

}
