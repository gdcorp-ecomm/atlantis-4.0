using System.Collections.Generic;
using Atlantis.Framework.Auth.Interface;
using Atlantis.Framework.AuthValidatePassword.Interface;
using Atlantis.Framework.ShopperValidator.Interface.RuleConstants;
using Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules;
using Atlantis.Framework.ShopperValidator.Interface.Validator;
using Atlantis.Framework.ValidateField.Interface;

namespace Atlantis.Framework.ShopperValidator.Interface.ValidationRules
{
  public class PasswordRule : RuleContainer
  {
    private string _fieldName;
    private string _username;
    private string _password;
    private string _requestUrl = string.Empty;
    private string _pathway = string.Empty;
    private int _pageCount = 0;
    private bool _isNewShopper;

    /// <summary>
    /// Should ONLY be used when requestUrl, pathway, and pageCount are undefined.  Otherwise use constructor which accepts these parameters.
    /// </summary>
    public PasswordRule(string value, bool isNewShopper, string username = "", string passwordHint = "", string fieldName = FieldNames.Password)
      : base()
    {
      _fieldName = fieldName;
      _username = username ?? string.Empty;
      _password = value ?? string.Empty;
      _isNewShopper = isNewShopper;

      if (_password.Length > 255) //passwords greater than 255 characters are invalid, but we cannot display them a message (MP# 90509)
      {
        throw new System.InvalidOperationException(string.Format("{0} cannot be greater than {1} characters.  Trim {0} before supplying it to the validator.", fieldName, LengthConstants.PasswordMaxLength));
      }

      base.RulesToValidate.Add(new RequiredRule(fieldName, value));

      #region Not match username
      if (!string.IsNullOrEmpty(_username))
      {
        base.RulesToValidate.Add(new NotMatchRule(fieldName, value, "Username", _username));
      }
      #endregion

      #region Not match new hint
      if (!string.IsNullOrEmpty(passwordHint))
      {
        base.RulesToValidate.Add(new NotMatchRule(fieldName, value, "Password hint", passwordHint));
      }
      #endregion

      BuildCustomRules();
    }

    public PasswordRule(string value, bool isNewShopper, string requestUrl, string pathway, int pageCount, string username = "",
      string passwordHint = "", string fieldName = FieldNames.Password)
      : this(value, isNewShopper, username, passwordHint, fieldName)
    {
      _requestUrl = requestUrl;
      _pageCount = pageCount;
      _pathway = pathway;
    }

    private void BuildCustomRules()
    {
      bool passwordIsValid = PasswordIsValid_ValidateField();//checks for min-length, max-length, capital letter, and number.
      if (passwordIsValid)
      {
        DoAuthValidatePasswordRequest(); //checks against black list, last 30 days, last 5 passwords, and not matching hint.
      }
    }

    #region Custom Rules
    private bool PasswordIsValid_ValidateField()
    {
      var validatorRequest = new ValidateFieldRequestData(string.Empty, _requestUrl, string.Empty, _pathway, _pageCount, "password");
      ValidateFieldResponseData validator = (ValidateFieldResponseData)DataCache.DataCache.GetProcessRequest(validatorRequest, EngineRequestValues.ValidateField);

      List<ValidationFailure> errors;
      bool isPasswordValid = validator.ValidateStringField(_password, out errors);

      if (!isPasswordValid)
      {
        foreach (var failure in errors)
        {
          base.RulesToValidate.Add(new BlankRule(false, failure.GetFormattedDescription(_fieldName)));
        }
      }

      return isPasswordValid;
    }

    private void DoAuthValidatePasswordRequest()
    {
      //we do NOT want to pass a shopper/username into this method b/c  it's a new shopper. 
      //Ecomm's WS will fail b/c it can't find the _username b/c it hasn't been inserted yet..
      string temporaryUserName = _isNewShopper ? string.Empty : _username;
      
      var request = new AuthValidatePasswordRequestData(temporaryUserName, _requestUrl, string.Empty, _pathway, _pageCount, _password);
      var response = Engine.Engine.ProcessRequest(request, EngineRequestValues.AuthValidatePassword) as AuthValidatePasswordResponseData;

      if (response.StatusCode != TwoFactorWebserviceResponseCodes.Success)
      {
        string passwordFailedMessage = string.Empty;
        switch (response.StatusCode)
        {
          case AuthPasswordCodes.PasswordFailBlacklisted:
            passwordFailedMessage = string.Concat(_fieldName, " cannot include a common word or phrase.");
            break;
          case AuthPasswordCodes.PasswordFailLastFive:
            passwordFailedMessage = "Cannot reuse the last 5 passwords.";
            break;
          case AuthPasswordCodes.PasswordFailMatchesHint:
            passwordFailedMessage = string.Concat(_fieldName, " cannot be equal to Password Hint.");
            break;
          case AuthPasswordCodes.PasswordFailThirtyDay:
            passwordFailedMessage = "Cannot use the same password within 30 days.";
            break;
          default:
            passwordFailedMessage = string.Concat(_fieldName, " is invalid. Status Code: ", response.StatusCode.ToString());
            break;
        }

        base.RulesToValidate.Add(new BlankRule(false, passwordFailedMessage));
      }
    }
    #endregion
  }
}
