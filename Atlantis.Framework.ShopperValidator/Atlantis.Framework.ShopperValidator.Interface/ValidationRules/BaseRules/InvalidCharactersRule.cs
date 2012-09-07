using Atlantis.Framework.ShopperValidator.Interface.RuleConstants;

namespace Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules
{
  public class InvalidCharactersRule : RegexRule
  {
    public InvalidCharactersRule(string fieldName, string textToValidate)
      : base(fieldName, textToValidate, RegexConstants.InvalidCharacters)
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
