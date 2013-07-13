using Atlantis.Framework.ShopperValidator.Interface.RuleConstants;
using Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules;

namespace Atlantis.Framework.ShopperValidator.Interface.Validator
{
  public class AnyPhoneRule : RuleContainer
  {
    public AnyPhoneRule(string value, bool isRequired = false, string countryCode = "us", string fieldName = "", string culture = "en")
      : base(value, culture)
    {
      var FieldNames = new FieldNames(culture);
      DefaultFieldNameHelper.OverwriteTextIfEmpty(fieldName, FieldNames.Phone, out fieldName);
      
      AddIsRequiredRule(value, fieldName, isRequired);
      base.RulesToValidate.Add(new PhoneRule( value, fieldName, isRequired, countryCode, Culture));
    }
  }
}
