using System;
using Atlantis.Framework.ShopperValidator.Interface.RuleConstants;
using Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules;

namespace Atlantis.Framework.ShopperValidator.Interface.Validator
{
  public class ZipRule : RuleContainer
  {
    public ZipRule(string value, string countryCode = "us", string state = "", string fieldName = FieldNames.Zip)
      : base()
    {
      base.RulesToValidate.Add(new MaxLengthRule(fieldName, value, LengthConstants.ZipMaxLength));

      if (countryCode.Equals("us", StringComparison.InvariantCultureIgnoreCase))
      {
        base.RulesToValidate.Add(new RegexRule(fieldName, value, RegexConstants.ZipUS));
      }
      else
      {
        base.RulesToValidate.Add(new InvalidCharactersRule(fieldName, value));
      }

      #region Add required rule if country isn't hong kong
      bool countryIsChinaStateIsHongKong = countryCode.Equals("cn", StringComparison.InvariantCultureIgnoreCase)
        && state.Trim().Equals("hong kong", StringComparison.InvariantCultureIgnoreCase);
      bool countryIsHongKong = countryCode.Equals("hk", StringComparison.InvariantCultureIgnoreCase);

      if (!(countryIsChinaStateIsHongKong || countryIsHongKong))
      {
        base.RulesToValidate.Add(new RequiredRule(fieldName, value));
      }
      #endregion
    }
  }
}