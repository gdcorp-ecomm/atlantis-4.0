using Atlantis.Framework.ShopperValidator.Interface.RuleConstants;
using Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules;
using Atlantis.Framework.ShopperValidator.Interface.Validator;

namespace Atlantis.Framework.ShopperValidator.Interface.ValidationRules
{
  public class LastNameRule : SingleValueRuleContainer
  {
    public LastNameRule(string value, string fieldName = FieldNames.LastName, bool isRequired = false)
      : base(value, fieldName, isRequired)
    {
      //base.RulesToValidate.Add(new RequiredRule(fieldName, value));
      base.RulesToValidate.Add(new MaxLengthRule(fieldName, value, LengthConstants.LastNameMaxLength));
      base.RulesToValidate.Add(new InvalidCharactersRule(fieldName, value));
    }
  }
}
