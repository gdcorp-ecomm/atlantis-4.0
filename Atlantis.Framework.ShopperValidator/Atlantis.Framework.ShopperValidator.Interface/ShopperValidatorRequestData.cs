using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ShopperValidator.Interface.LanguageResources;
using Atlantis.Framework.ShopperValidator.Interface.RuleConstants;
using Atlantis.Framework.ShopperValidator.Interface.ShopperValidation;
using Atlantis.Framework.ValidateField.Interface;

namespace Atlantis.Framework.ShopperValidator.Interface
{
  public class ShopperValidatorRequestData : RequestData
  {
    public ShopperToValidate ShopperToValidate;
    public bool IsNewShopper;

    private const string _DEFAULT_CULTURE = "en";
    private string _culture;
    public string Culture
    {
      get
      {
        if (string.IsNullOrEmpty(_culture))
        {
          _culture = _DEFAULT_CULTURE;
        }
        return _culture;
      }

      set { _culture = value; }
    }

    private ValidateFieldResponseData _passwordValidator;
    protected ValidateFieldResponseData PasswordValidator
    {
      get
      {
        if (_passwordValidator == null)
        {
          var validatorRequest = new ValidateFieldRequestData(string.Empty, SourceURL, string.Empty, Pathway, PageCount, "password");
          _passwordValidator = (ValidateFieldResponseData)DataCache.DataCache.GetProcessRequest(validatorRequest, EngineRequestValues.ValidateField);
        }
        return _passwordValidator;
      }
    }

    public Dictionary<string, Dictionary<string, string>> ShopperBaseModel;

    [Obsolete("use culture-aware method")]
    public ShopperValidatorRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, string username, string password, string confirmPassword, string pin, string email)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      SetUpShopperBaseModal(username, password, confirmPassword, pin, email);
    }

    public ShopperValidatorRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, string username, string password, string confirmPassword, string pin, string email, string culture)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      Culture = culture;
      SetUpShopperBaseModal(username, password, confirmPassword, pin, email);
    }

    public ShopperValidatorRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, ShopperToValidate shopperToValidate, bool isNewShopper)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      ShopperToValidate = shopperToValidate;
      IsNewShopper = isNewShopper;
    }

    public ShopperValidatorRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, ShopperToValidate shopperToValidate, bool isNewShopper, string culture)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      ShopperToValidate = shopperToValidate;
      IsNewShopper = isNewShopper;
      Culture = culture;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }

    private void SetUpShopperBaseModal(string username, string password, string confirmPassword, string pin,
                                       string email)
    {
      var modelValues = new Dictionary<string, string>
        {
          {ModelConstants.FACT_USERNAME, username},
          {ModelConstants.FACT_PASSWORD, password},
          {ModelConstants.FACT_PASSWORD2, confirmPassword},
          {ModelConstants.FACT_PIN, pin},
          {ModelConstants.FACT_EMAIL, email},
          {ModelConstants.FACT_USERNAME_MAX_LENGTH, Convert.ToString(LengthConstants.UsernameMaxLength)},
          {ModelConstants.FACT_PASS_MAX_LENGTH, Convert.ToString(LengthConstants.PasswordMaxLength)},
          {ModelConstants.FACT_PASS_MIN_LENGTH, Convert.ToString(LengthConstants.PasswordMinLength)},
          {ModelConstants.FACT_PASS_REGEX, PasswordValidator.ExpressionRule.ServerPattern},
          {ModelConstants.FACT_PIN_MAX_LENGTH, Convert.ToString(LengthConstants.CallInPinMaxLength)},
          {ModelConstants.FACT_PIN_MIN_LENGTH, Convert.ToString(LengthConstants.CallInPinMinLength)},
          {ModelConstants.FACT_EMAIL_REGEX, RegexConstants.EmailPattern},
          {ModelConstants.FACT_EMAIL_MAX_LENGTH, Convert.ToString(LengthConstants.EmailMaxLength)},
          {ModelConstants.FACT_NUMERIC_ONLY_REGEX, RegexConstants.NumericOnlyPattern},
          {ModelConstants.FACT_INVALID_CHARACTERS_REGEX, RegexConstants.InvalidCharactersPattern},
          
          {ModelConstants.FACT_ERROR_PIN_REQUIRED, FetchResource.GetString(ModelConstants.FACT_ERROR_PIN_REQUIRED)},
          {ModelConstants.FACT_ERROR_PIN_NUMERIC, FetchResource.GetString(ModelConstants.FACT_ERROR_PIN_NUMERIC)},
          {ModelConstants.FACT_ERROR_PIN_SEQUENTIAL, FetchResource.GetString(ModelConstants.FACT_ERROR_PIN_SEQUENTIAL)},
          {ModelConstants.FACT_ERROR_EMAIL_REQUIRED, FetchResource.GetString(ModelConstants.FACT_ERROR_EMAIL_REQUIRED)},
          {ModelConstants.FACT_ERROR_EMAIL_INVALID_FORMAT, FetchResource.GetString(ModelConstants.FACT_ERROR_EMAIL_INVALID_FORMAT)},
          {ModelConstants.FACT_ERROR_PASSWORD_REQUIRED, FetchResource.GetString(ModelConstants.FACT_ERROR_PASSWORD_REQUIRED)},
          {ModelConstants.FACT_ERROR_PASSWORD_MISMATCH, FetchResource.GetString(ModelConstants.FACT_ERROR_PASSWORD_MISMATCH)},
          {ModelConstants.FACT_ERROR_PASSWORD_INVALID_FORMAT, FetchResource.GetString(ModelConstants.FACT_ERROR_PASSWORD_INVALID_FORMAT)},
          {ModelConstants.FACT_ERROR_PASSWORD2_REQUIRED, FetchResource.GetString(ModelConstants.FACT_ERROR_PASSWORD2_REQUIRED)},
          {ModelConstants.FACT_ERROR_USERNAME_INVALID_CHARS, FetchResource.GetString(ModelConstants.FACT_ERROR_USERNAME_INVALID_CHARS)},
          {ModelConstants.FACT_ERROR_USERNAME_NUMERIC_ONLY, FetchResource.GetString(ModelConstants.FACT_ERROR_USERNAME_NUMERIC_ONLY)},
          {ModelConstants.FACT_ERROR_USERNAME_REQUIRED, FetchResource.GetString(ModelConstants.FACT_ERROR_USERNAME_REQUIRED)},

          {ModelConstants.FACT_ERROR_PIN_MAX_LENGTH, string.Format(FetchResource.GetString(ModelConstants.FACT_ERROR_PIN_MAX_LENGTH), LengthConstants.CallInPinMaxLength)},
          {ModelConstants.FACT_ERROR_PIN_MIN_LENGTH, string.Format(FetchResource.GetString(ModelConstants.FACT_ERROR_PIN_MIN_LENGTH), LengthConstants.CallInPinMinLength)},
          {ModelConstants.FACT_ERROR_EMAIL_MAX_LENGTH, string.Format(FetchResource.GetString(ModelConstants.FACT_ERROR_EMAIL_MAX_LENGTH), LengthConstants.EmailMaxLength)},
          {ModelConstants.FACT_ERROR_PASSWORD_MAX_LENGTH, string.Format(FetchResource.GetString(ModelConstants.FACT_ERROR_PASSWORD_MAX_LENGTH), LengthConstants.PasswordMaxLength)},
          {ModelConstants.FACT_ERROR_PASSWORD_MIN_LENGTH, string.Format(FetchResource.GetString(ModelConstants.FACT_ERROR_PASSWORD_MIN_LENGTH), LengthConstants.PasswordMinLength)},
          {ModelConstants.FACT_ERROR_USERNAME_MAX_LENGTH, string.Format(FetchResource.GetString(ModelConstants.FACT_ERROR_USERNAME_MAX_LENGTH), LengthConstants.UsernameMaxLength)},
        };

      ShopperBaseModel = new Dictionary<string, Dictionary<string, string>>(1);
      ShopperBaseModel.Add(ModelConstants.MODEL_ID_SHOPPERVALID, modelValues);
    }

    private FetchResource _fetchResource;
    protected FetchResource FetchResource
    {
      get
      {
        if (_fetchResource == null)
        {
          _fetchResource = new FetchResource(ResourceNamespace.ShopperValidator, Culture);
        }

        return _fetchResource;
      }
    }
  }
}