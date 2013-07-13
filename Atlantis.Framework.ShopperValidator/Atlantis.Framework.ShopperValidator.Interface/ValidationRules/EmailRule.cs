using Atlantis.Framework.ShopperValidator.Interface.RuleConstants;
using Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules;

namespace Atlantis.Framework.ShopperValidator.Interface.Validator
{
  public class EmailRule : RuleContainer
  {
    public EmailRule(string value, string fieldName = "", bool isRequired = false, string culture = "")
      : base(value, culture)
    {
      var FieldNames = new FieldNames(culture);
      DefaultFieldNameHelper.OverwriteTextIfEmpty(fieldName, FieldNames.Email, out fieldName);

      AddIsRequiredRule(value, fieldName, isRequired);
      base.RulesToValidate.Add(new MaxLengthRule(Culture, fieldName, value, LengthConstants.EmailMaxLength));
      base.RulesToValidate.Add(new RegexRule(Culture, fieldName, value, RegexConstants.Email));
    }
  }
}
