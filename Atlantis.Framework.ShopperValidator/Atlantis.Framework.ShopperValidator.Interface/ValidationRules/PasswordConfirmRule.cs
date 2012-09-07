using Atlantis.Framework.ShopperValidator.Interface.RuleConstants;
using Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules;
using Atlantis.Framework.ShopperValidator.Interface.Validator;

namespace Atlantis.Framework.ShopperValidator.Interface.ValidationRules
{
  public class PasswordConfirmRule : RuleContainer
  {
    public PasswordConfirmRule(string value, string password, string fieldName = FieldNames.PasswordConfirm)
      : base()
    {
      base.RulesToValidate.Add(new MatchRule(fieldName, value, "Password", password));
    }
  }
}
