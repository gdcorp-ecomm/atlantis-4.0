using Atlantis.Framework.ShopperValidator.Interface.RuleConstants;
using Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules;
using Atlantis.Framework.ShopperValidator.Interface.Validator;

namespace Atlantis.Framework.ShopperValidator.Interface.ValidationRules
{
  public class Address2Rule : SingleValueRuleContainer
  {
    public Address2Rule(string value, string fieldName = FieldNames.Address2, bool isRequired = false)
      : base(value, fieldName, isRequired)
    {
      base.RulesToValidate.Add(new MaxLengthRule(fieldName, value, LengthConstants.AddressMaxLength));
      base.RulesToValidate.Add(new InvalidCharactersRule(fieldName, value));
    }
  }
}
