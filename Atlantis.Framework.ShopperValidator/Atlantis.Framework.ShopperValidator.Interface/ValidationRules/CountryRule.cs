using Atlantis.Framework.ShopperValidator.Interface.RuleConstants;
using Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules;
using Atlantis.Framework.ShopperValidator.Interface.Validator;

namespace Atlantis.Framework.ShopperValidator.Interface.ValidationRules
{
  public class CountryRule : RuleContainer
  {
    public CountryRule(string value, string fieldName = "", bool isRequired = false, string culture = "")
      : base(value, culture)
    {
      var FieldNames = new FieldNames(culture);
      DefaultFieldNameHelper.OverwriteTextIfEmpty(fieldName, FieldNames.Country, out fieldName);

      AddIsRequiredRule(value, fieldName, isRequired);
      base.RulesToValidate.Add(new MaxLengthRule(Culture, fieldName, value, LengthConstants.CountryMaxLength));
    }

    public CountryRule(string culture, string value, string fieldName = "", bool isRequired = false)
      : this(value, fieldName, isRequired, culture)
    { }
  }
}
