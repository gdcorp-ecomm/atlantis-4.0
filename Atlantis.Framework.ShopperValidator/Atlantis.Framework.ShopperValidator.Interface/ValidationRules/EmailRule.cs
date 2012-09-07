using Atlantis.Framework.ShopperValidator.Interface.RuleConstants;
using Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules;

namespace Atlantis.Framework.ShopperValidator.Interface.Validator
{
  public class EmailRule : RuleContainer
  {
    public EmailRule(string value, string fieldName = FieldNames.Email)
      : base()
    {
      base.RulesToValidate.Add(new RequiredRule(fieldName, value));
      base.RulesToValidate.Add(new MaxLengthRule(fieldName, value, LengthConstants.EmailMaxLength));
      base.RulesToValidate.Add(new RegexRule(fieldName, value, RegexConstants.Email));
    }
  }
}
