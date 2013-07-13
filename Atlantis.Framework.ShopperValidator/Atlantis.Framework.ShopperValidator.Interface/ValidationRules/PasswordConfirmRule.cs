using Atlantis.Framework.ShopperValidator.Interface.RuleConstants;
using Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules;
using Atlantis.Framework.ShopperValidator.Interface.Validator;

namespace Atlantis.Framework.ShopperValidator.Interface.ValidationRules
{
  public class PasswordConfirmRule : RuleContainer
  {
    public PasswordConfirmRule(string value, string password, string fieldName = "", string culture = "")
      : base(value, culture)
    {
      var FieldNames = new FieldNames(Culture);
      DefaultFieldNameHelper.OverwriteTextIfEmpty(fieldName, FieldNames.PasswordConfirm, out fieldName);
      string errorFormat = FetchResource.GetString("passwordConfirm");
      //passwords greater than 255 characters are invalid, but we cannot display them a message (MP# 90509)
      if (value.Length > 255)
      {
        throw new System.InvalidOperationException(string.Format(errorFormat, fieldName, LengthConstants.PasswordMaxLength));
      }
      else if (password.Length > 255)
      {
        throw new System.InvalidOperationException(string.Format(errorFormat, fieldName, LengthConstants.PasswordMaxLength));
      }

      base.RulesToValidate.Add(new MatchRule(Culture, fieldName, value, fieldName, password));
    }
  }
}
