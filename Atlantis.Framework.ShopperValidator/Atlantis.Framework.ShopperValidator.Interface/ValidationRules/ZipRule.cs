using System;
using Atlantis.Framework.ShopperValidator.Interface.RuleConstants;
using Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules;

namespace Atlantis.Framework.ShopperValidator.Interface.Validator
{
  public class ZipRule : RuleContainer
  {
    public ZipRule(string value, string countryCode, string state, string fieldName = "", bool isRequired = false, string culture = "")
      : base(value, culture)
    {
      var FieldNames = new FieldNames(Culture);
      DefaultFieldNameHelper.OverwriteTextIfEmpty(fieldName, FieldNames.Zip, out fieldName);

      base.RulesToValidate.Add(new MaxLengthRule(Culture, fieldName, value, LengthConstants.ZipMaxLength));

      if (countryCode.Equals("us", StringComparison.InvariantCultureIgnoreCase))
      {
        base.RulesToValidate.Add(new RegexRule(Culture, fieldName, value, RegexConstants.ZipUS));
      }
      else
      {
        base.RulesToValidate.Add(new InvalidCharactersRule(fieldName, value, Culture));
      }

      bool countryIsChinaStateIsHongKong = countryCode.Equals("cn", StringComparison.InvariantCultureIgnoreCase)
        && state.Trim().Equals("hong kong", StringComparison.InvariantCultureIgnoreCase);
      bool countryIsHongKong = countryCode.Equals("hk", StringComparison.InvariantCultureIgnoreCase);

      if (!(countryIsChinaStateIsHongKong || countryIsHongKong) && isRequired)
      {
        base.RulesToValidate.Add(new RequiredRule(fieldName, value, Culture));
      }
    }
  }
}