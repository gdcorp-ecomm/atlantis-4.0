using Atlantis.Framework.ShopperValidator.Interface.RuleConstants;
using Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules;
using Atlantis.Framework.ShopperValidator.Interface.Validator;

namespace Atlantis.Framework.ShopperValidator.Interface.ValidationRules
{
  public class Address1Rule : SingleValueRuleContainer
  {
    public Address1Rule(string value, string fieldName = FieldNames.Address1, bool isRequired = false)
      : base(value, fieldName, isRequired)
    {
      /*if(requireRule)
        base.RulesToValidate.Add(new RequiredRule(fieldName, value));*/
      base.RulesToValidate.Add(new MaxLengthRule(fieldName, value, LengthConstants.AddressMaxLength));
      base.RulesToValidate.Add(new InvalidCharactersRule(fieldName, value));
    }
  }
}
