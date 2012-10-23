using Atlantis.Framework.ShopperValidator.Interface.RuleConstants;
using Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules;

namespace Atlantis.Framework.ShopperValidator.Interface.Validator
{
  public class PhoneExtRule : SingleValueRuleContainer
  {
    public PhoneExtRule(string value, string fieldName = FieldNames.PhoneExtension, bool isRequired = false)
      : base(value, fieldName, isRequired)
    {
      base.RulesToValidate.Add(new MaxLengthRule(fieldName, value, LengthConstants.PhoneExtMaxLength));

      if (!string.IsNullOrEmpty(value))
      {
        base.RulesToValidate.Add(new NumericRule(fieldName, value));
      }
    }
  }
}
