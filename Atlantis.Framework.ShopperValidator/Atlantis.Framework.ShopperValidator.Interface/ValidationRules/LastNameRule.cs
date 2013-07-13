using Atlantis.Framework.ShopperValidator.Interface.RuleConstants;
using Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules;
using Atlantis.Framework.ShopperValidator.Interface.Validator;

namespace Atlantis.Framework.ShopperValidator.Interface.ValidationRules
{
  public class LastNameRule : RuleContainer
  {
    public LastNameRule(string value, string fieldName = "", bool isRequired = false, string culture = "")
      : base(value, culture)
    {
      var FieldNames = new FieldNames(culture);
      DefaultFieldNameHelper.OverwriteTextIfEmpty(fieldName, FieldNames.LastName, out fieldName);
      
      AddIsRequiredRule(value, fieldName, isRequired);
      base.RulesToValidate.Add(new MaxLengthRule(Culture, fieldName, value, LengthConstants.LastNameMaxLength));
      base.RulesToValidate.Add(new InvalidCharactersRule(fieldName, value, Culture));
    }
  }
}
