using Atlantis.Framework.ShopperValidator.Interface.RuleConstants;
using Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules;
using Atlantis.Framework.ShopperValidator.Interface.Validator;

namespace Atlantis.Framework.ShopperValidator.Interface.ValidationRules
{
  public class Address2Rule : RuleContainer
  {
    public Address2Rule(string value, string fieldName = "", bool isRequired = false, string culture = "")
      : base(value,culture)
    {
      var FieldNames = new FieldNames(culture);
      DefaultFieldNameHelper.OverwriteTextIfEmpty(fieldName, FieldNames.Address2, out fieldName);

      AddIsRequiredRule(value, fieldName, isRequired);
      base.RulesToValidate.Add(new MaxLengthRule(Culture, fieldName, value, LengthConstants.AddressMaxLength));
      base.RulesToValidate.Add(new InvalidCharactersRule(fieldName, value, Culture));
    }
  }
}
