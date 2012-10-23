using Atlantis.Framework.ShopperValidator.Interface.RuleConstants;
using Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules;

namespace Atlantis.Framework.ShopperValidator.Interface.Validator
{
  public class AnyPhoneRule : SingleValueRuleContainer
  {
    public AnyPhoneRule(string value, bool isRequired = false, string countryCode = "us", string fieldName = FieldNames.Phone)
      : base(value, fieldName, isRequired)
    {
      base.RulesToValidate.Add(new PhoneRule(value, fieldName, countryCode));
    }

    public AnyPhoneRule(string value, string countryCode = "us", string fieldName = FieldNames.Phone)
      : base(value, fieldName, false)
    {
      base.RulesToValidate.Add(new PhoneRule(value, fieldName, countryCode));
    }
  }
}
