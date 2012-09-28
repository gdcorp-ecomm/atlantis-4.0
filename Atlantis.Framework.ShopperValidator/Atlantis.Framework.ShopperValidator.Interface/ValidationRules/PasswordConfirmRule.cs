using Atlantis.Framework.ShopperValidator.Interface.RuleConstants;
using Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules;
using Atlantis.Framework.ShopperValidator.Interface.Validator;

namespace Atlantis.Framework.ShopperValidator.Interface.ValidationRules
{
  public class PasswordConfirmRule : RuleContainer
  {
    private const string ErrorFormat = "{0} cannot be greater than {1} characters.  Trim {0} before supplying it to the validator.";

    public PasswordConfirmRule(string value, string password, string fieldName = FieldNames.PasswordConfirm)
      : base()
    {
      //passwords greater than 255 characters are invalid, but we cannot display them a message (MP# 90509)
      if (value.Length > 255)
      {
        throw new System.InvalidOperationException(string.Format(ErrorFormat, fieldName, LengthConstants.PasswordMaxLength));
      }
      else if (password.Length > 255)
      {
        throw new System.InvalidOperationException(string.Format(ErrorFormat, "Password", LengthConstants.PasswordMaxLength));
      }

      base.RulesToValidate.Add(new MatchRule(fieldName, value, "Password", password));
    }
  }
}
