using Atlantis.Framework.ShopperValidator.Interface.RuleConstants;
using Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules;

namespace Atlantis.Framework.ShopperValidator.Interface.Validator
{
  public class AnyPhoneRule : RuleContainer
  {
    public AnyPhoneRule(string value, bool isRequired = false, string countryCode = "us", string fieldName = FieldNames.Phone)
      : base()
    {
      base.RulesToValidate.Add(new PhoneRule(value, fieldName, isRequired, countryCode));
    }

    public AnyPhoneRule(string value, string countryCode = "us", string fieldName = FieldNames.Phone)
      : base()
    {
      base.RulesToValidate.Add(new PhoneRule(value, fieldName, false, countryCode));
    }
  }
}
