using Atlantis.Framework.ShopperValidator.Interface.RuleConstants;
using Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules;

namespace Atlantis.Framework.ShopperValidator.Interface.Validator
{
  public class EmailRule : SingleValueRuleContainer
  {
    public EmailRule(string value, string fieldName = FieldNames.Email, bool isRequired = false)
      : base(value, fieldName, isRequired)
    {
      //base.RulesToValidate.Add(new RequiredRule(fieldName, value));
      base.RulesToValidate.Add(new MaxLengthRule(fieldName, value, LengthConstants.EmailMaxLength));
      base.RulesToValidate.Add(new RegexRule(fieldName, value, RegexConstants.Email));
    }
  }
}
