using Atlantis.Framework.ShopperValidator.Interface.RuleConstants;
using Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules;
using Atlantis.Framework.ShopperValidator.Interface.Validator;
using System;

namespace Atlantis.Framework.ShopperValidator.Interface.ValidationRules
{
  public class BirthDayRule : RuleContainer
  {
    public BirthDayRule(string birthMonth, string birthDay, string fieldName = FieldNames.BirthDay)
      : base()
    {
      BuildCustomRule(birthMonth, birthDay, fieldName);
    }

    #region Custom Rules
    private void BuildCustomRule(string birthMonth, string birthDay, string fieldName)
    {
      bool birthdayIsValid = true;
      string errorMessage = "If providing a birthday you must provide valid values";

      bool hasBirthdayInfo = !string.IsNullOrEmpty(birthMonth) || !string.IsNullOrEmpty(birthDay);

      if (hasBirthdayInfo)
      {
        string month = birthMonth ?? string.Empty;
        string day = birthDay ?? string.Empty;

        string fullBirthDate = string.Concat(month, "/", day, "/2008"); //use 2008 b/c it's a leap year to account if someone chooses Feb 29th for a birthday.
        DateTime dateParser;

        if (!DateTime.TryParse(fullBirthDate, out dateParser))
        {
          birthdayIsValid = false;
        }
      }

      base.RulesToValidate.Add(new BlankRule(birthdayIsValid, errorMessage));
    }
    #endregion
  }
}
