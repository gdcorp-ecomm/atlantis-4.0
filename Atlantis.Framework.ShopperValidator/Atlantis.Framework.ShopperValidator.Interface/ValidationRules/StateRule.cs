using Atlantis.Framework.ShopperValidator.Interface.RuleConstants;
using Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules;
using Atlantis.Framework.ShopperValidator.Interface.Validator;

namespace Atlantis.Framework.ShopperValidator.Interface.ValidationRules
{
  public class StateRule : RuleContainer
  {
    public StateRule(string value, string fieldName = "", bool isRequired = false, string culture = "")
      : base(value, culture)
    {
      var FieldNames = new FieldNames(Culture);
      DefaultFieldNameHelper.OverwriteTextIfEmpty(fieldName, FieldNames.State, out fieldName);

      AddIsRequiredRule(value, fieldName, isRequired);
      base.RulesToValidate.Add(new MaxLengthRule(Culture, fieldName, value, LengthConstants.StateMaxLength));
      base.RulesToValidate.Add(new InvalidCharactersRule(fieldName, value, Culture));
    }
    }
}
