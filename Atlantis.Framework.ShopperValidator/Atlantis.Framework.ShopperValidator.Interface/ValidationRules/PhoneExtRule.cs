using Atlantis.Framework.ShopperValidator.Interface.RuleConstants;
using Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules;

namespace Atlantis.Framework.ShopperValidator.Interface.Validator
{
  public class PhoneExtRule : RuleContainer
  {
    public PhoneExtRule(string value, string fieldName = "", bool isRequired = false, string culture = "")
      : base(value, culture)
    {
      var FieldNames = new FieldNames(Culture);
      DefaultFieldNameHelper.OverwriteTextIfEmpty(fieldName, FieldNames.PhoneExtension, out fieldName);

      AddIsRequiredRule(value, fieldName, isRequired);
      base.RulesToValidate.Add(new MaxLengthRule(Culture, fieldName, value, LengthConstants.PhoneExtMaxLength));

      if (!string.IsNullOrEmpty(value))
      {
        base.RulesToValidate.Add(new NumericRule(fieldName, value, Culture));
      }
    }
  }
}
