using Atlantis.Framework.ShopperValidator.Interface.RuleConstants;
using Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules;
using Atlantis.Framework.ShopperValidator.Interface.Validator;

namespace Atlantis.Framework.ShopperValidator.Interface.ValidationRules
{
  public class PasswordHintRule : SingleValueRuleContainer
  {
    public PasswordHintRule(string value, string fieldName = FieldNames.PasswordHint, bool isRequired = false)
      : base(value, fieldName, isRequired)
    {
      //base.RulesToValidate.Add(new RequiredRule(fieldName, value));
      base.RulesToValidate.Add(new MaxLengthRule(fieldName, value, LengthConstants.PasswordHintMaxLength));
      base.RulesToValidate.Add(new InvalidCharactersRule(fieldName, value));
    }
  }
}
