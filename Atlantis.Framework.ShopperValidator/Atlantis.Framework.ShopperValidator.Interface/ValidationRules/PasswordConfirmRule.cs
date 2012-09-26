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
      //we never want to display a pw max length error of 255 so we truncate it.
      if (value.Length > 255)
      {
        value = value.Substring(0, 255);
      }

      if (password.Length > 255)
      {
        password = password.Substring(0, 255);
      }

      base.RulesToValidate.Add(new MatchRule(fieldName, value, "Password", password));
    }
  }
}
