using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using Atlantis.Framework.Auth.Interface;
using Atlantis.Framework.AuthValidatePassword.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.RuleEngine.Results;
using Atlantis.Framework.Shopper.Interface;
using Atlantis.Framework.ShopperValidator.Interface;
using Atlantis.Framework.ShopperValidator.Interface.LanguageResources;
using Atlantis.Framework.ShopperValidator.Interface.RuleConstants;
using Atlantis.Framework.ShopperValidator.Interface.ShopperValidation;
using Atlantis.Framework.ShopperValidator.Interface.ValidationRules;
using Atlantis.Framework.ShopperValidator.Interface.Validator;

namespace Atlantis.Framework.ShopperValidator.Impl
{
  public class ShopperValidatorRequest : IRequest
  {
    private IRuleEngineResult _engineResults;

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      ShopperValidatorResponseData responseData;

      try
      {
        var request = (ShopperValidatorRequestData)requestData;

        if (request.ShopperToValidate == null && request.ShopperBaseModel == null)
        {
          throw new AtlantisException("ShopperValidator::RequestHandler", 0, "Both ShopperToValidate and ShopperBaseModel cannot be null", "--input data--");
        }

        ValidateShopper(request);

        responseData = request.ShopperBaseModel != null ? new ShopperValidatorResponseData(_engineResults) : new ShopperValidatorResponseData(request.ShopperToValidate);
      }
      catch (AtlantisException aex)
      {
        responseData = new ShopperValidatorResponseData(aex);
      }
      catch (Exception ex)
      {
        string message = ex.Message + Environment.NewLine + ex.StackTrace;
        var aex = new AtlantisException("ShopperValidator::RequestHandler", 0, message, string.Empty);
        responseData = new ShopperValidatorResponseData(aex);
      }

      return responseData;
    }

    #region Shopper Validation Functionality

    public void ValidateShopper(ShopperValidatorRequestData requestData)
    {
      if (requestData.ShopperBaseModel != null)
      {
        ValidateShopperWithRuleEngine(requestData);
      }
      else
      {
        ShopperToValidate shopperToValidate = requestData.ShopperToValidate;

        #region Create Rules
        #region Name and Birthday Rules
        if (shopperToValidate.FirstName.Value != null)
          shopperToValidate.FirstName.RuleContainer = new FirstNameRule(shopperToValidate.FirstName.Value, isRequired: shopperToValidate.FirstName.IsRequired, culture: requestData.Culture);
        if (shopperToValidate.LastName.Value != null)
          shopperToValidate.LastName.RuleContainer = new LastNameRule(shopperToValidate.LastName.Value, isRequired: shopperToValidate.LastName.IsRequired, culture: requestData.Culture);
        if (shopperToValidate.BirthDay.Value != null)
          shopperToValidate.BirthDay.RuleContainer = new BirthDayRule(shopperToValidate.BirthMonth.Value, shopperToValidate.BirthDay.Value,
            monthIsRequired: shopperToValidate.BirthMonth.IsRequired, dayIsRequired: shopperToValidate.BirthDay.IsRequired, culture: requestData.Culture);
        if (shopperToValidate.BirthMonth.Value != null)
          shopperToValidate.BirthMonth.RuleContainer = shopperToValidate.BirthDay.RuleContainer;
        #endregion

        #region Address Rules
        if (shopperToValidate.Address1.Value != null)
          shopperToValidate.Address1.RuleContainer = new Address1Rule(shopperToValidate.Address1.Value, isRequired: shopperToValidate.Address1.IsRequired, culture: requestData.Culture);
        if (shopperToValidate.Address2.Value != null)
          shopperToValidate.Address2.RuleContainer = new Address2Rule(shopperToValidate.Address2.Value, isRequired: shopperToValidate.Address2.IsRequired, culture: requestData.Culture);
        if (shopperToValidate.Email.Value != null)
          shopperToValidate.Email.RuleContainer = new EmailRule(shopperToValidate.Email.Value, isRequired: shopperToValidate.Email.IsRequired, culture: requestData.Culture);
        if (shopperToValidate.City.Value != null)
          shopperToValidate.City.RuleContainer = new CityRule(shopperToValidate.City.Value, isRequired: shopperToValidate.City.IsRequired, culture: requestData.Culture);
        if (shopperToValidate.State.Value != null)
          shopperToValidate.State.RuleContainer = new StateRule(shopperToValidate.State.Value, isRequired: shopperToValidate.State.IsRequired, culture: requestData.Culture);
        if (shopperToValidate.Zip.Value != null)
          shopperToValidate.Zip.RuleContainer = new ZipRule(shopperToValidate.Zip.Value, shopperToValidate.Country.Value, shopperToValidate.State.Value, isRequired: shopperToValidate.Zip.IsRequired, culture: requestData.Culture);
        if (shopperToValidate.Country.Value != null)
          shopperToValidate.Country.RuleContainer = new CountryRule(shopperToValidate.Country.Value, isRequired: shopperToValidate.Country.IsRequired, culture: requestData.Culture);
        #endregion

        #region Phone Rules
        if (shopperToValidate.PhoneWork.Value != null)
          shopperToValidate.PhoneWork.RuleContainer = new AnyPhoneRule(shopperToValidate.PhoneWork.Value, shopperToValidate.PhoneWork.IsRequired, shopperToValidate.Country.Value, culture: requestData.Culture);
        if (shopperToValidate.PhoneWorkExtension.Value != null)
          shopperToValidate.PhoneWorkExtension.RuleContainer = new PhoneExtRule(shopperToValidate.PhoneWorkExtension.Value, isRequired: shopperToValidate.PhoneWorkExtension.IsRequired, culture: requestData.Culture);
        if (shopperToValidate.PhoneHome.Value != null)
          shopperToValidate.PhoneHome.RuleContainer = new AnyPhoneRule(shopperToValidate.PhoneHome.Value, shopperToValidate.PhoneHome.IsRequired, shopperToValidate.Country.Value, culture: requestData.Culture);
        if (shopperToValidate.PhoneMobile.Value != null)
          shopperToValidate.PhoneMobile.RuleContainer = new AnyPhoneRule(shopperToValidate.PhoneMobile.Value, shopperToValidate.PhoneMobile.IsRequired, shopperToValidate.Country.Value, culture: requestData.Culture);
        if (shopperToValidate.PhoneMobileSurvey.Value != null)
          shopperToValidate.PhoneMobileSurvey.RuleContainer = new AnyPhoneRule(shopperToValidate.PhoneMobileSurvey.Value, shopperToValidate.PhoneMobileSurvey.IsRequired, shopperToValidate.Country.Value, culture: requestData.Culture);
        #endregion

        #region Password Rules
        if (shopperToValidate.Username.Value != null)
          shopperToValidate.Username.RuleContainer = new UsernameRule(shopperToValidate.Username.Value, requestData.SourceURL,
            requestData.Pathway, requestData.PageCount, requestData.IsNewShopper, isRequired: shopperToValidate.Username.IsRequired, culture: requestData.Culture);
        if (shopperToValidate.Password.Value != null)
          shopperToValidate.Password.RuleContainer = new PasswordRule(shopperToValidate.Password.Value, requestData.IsNewShopper,
            requestData.SourceURL, requestData.Pathway, requestData.PageCount, shopperToValidate.Username.Value, shopperToValidate.PasswordHint.Value,
            isRequired: shopperToValidate.Password.IsRequired, culture: requestData.Culture);
        if (shopperToValidate.PasswordConfirm.Value != null)
          shopperToValidate.PasswordConfirm.RuleContainer = new PasswordConfirmRule(shopperToValidate.PasswordConfirm.Value, shopperToValidate.Password.Value, culture: requestData.Culture);
        if (shopperToValidate.PasswordHint.Value != null)
          shopperToValidate.PasswordHint.RuleContainer = new PasswordHintRule(shopperToValidate.PasswordHint.Value, isRequired: shopperToValidate.PasswordHint.IsRequired, culture: requestData.Culture);
        if (shopperToValidate.CallInPin.Value != null)
          shopperToValidate.CallInPin.RuleContainer = new CallInPinRule(shopperToValidate.CallInPin.Value, isRequired: shopperToValidate.CallInPin.IsRequired, culture: requestData.Culture);
        #endregion

        IEnumerable<RuleContainer> shopperRules = CreateShopperRuleContainerList(shopperToValidate);
        #endregion

        var shopperValidator = new ShopperRuleValidator(shopperRules);
        shopperValidator.ValidateAllRules();
      }
    }

    private bool LoadValidationRulesXml(out XmlDocument validationRulesXml)
    {
      validationRulesXml = new XmlDocument();
      var success = false;
      foreach (string resource in Assembly.GetExecutingAssembly().GetManifestResourceNames())
      {
        if (resource.EndsWith("SlimShopperValidation.xml"))
        {
          using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource))
          {
            try
            {
              if (stream != null)
              {
                validationRulesXml.Load(stream);
                success = true;
              }
            }
            catch (XmlException)
            {
              success = false;
            }
          }
          break;
        }
      }

      return success;
    }

    private void ValidateShopperWithRuleEngine(ShopperValidatorRequestData requestData)
    {
      XmlDocument slimShopperRulesDoc;
      var success = LoadValidationRulesXml(out slimShopperRulesDoc);

      if (!success)
      {
        throw new AtlantisException("ShopperValidator::ValidateShopperWithRuleEngine", 0, "Error reading SlimShopperValidation.xml", "--input data--");
      }

      var model = requestData.ShopperBaseModel;
      _engineResults = RuleEngine.RuleEngine.EvaluateRules(model, slimShopperRulesDoc);

      var validatedModel = _engineResults.ValidationResults.FirstOrDefault(m => m.ModelId == ModelConstants.MODEL_ID_SHOPPERVALID);

      if (!FactIsInvalid(validatedModel, ModelConstants.FACT_PASSWORD, ModelConstants.FACT_PASSWORD2, ModelConstants.FACT_PASS_MAX_LENGTH, ModelConstants.FACT_PASS_MIN_LENGTH))
      {
        DoAuthValidatePasswordRequest(requestData);
      }
      if (!FactIsInvalid(validatedModel, ModelConstants.FACT_USERNAME, ModelConstants.FACT_USERNAME_MAX_LENGTH))
      {
        DoValidateUsernameSearch(requestData);
      }
      if (!FactIsInvalid(validatedModel, ModelConstants.FACT_PIN, ModelConstants.FACT_PIN_MAX_LENGTH, ModelConstants.FACT_PIN_MIN_LENGTH))
      {
        DoValidatePinAllSameNumbers(requestData);
      }
    }

    private bool FactIsInvalid(IModelResult validatedModel, params string[] factKeys)
    {
      return validatedModel.Facts.Where(fact => factKeys.Contains(fact.FactKey)).Any(fact => fact.Status == ValidationResultStatus.InValid);
    }

    private void DoValidateUsernameSearch(ShopperValidatorRequestData requestData)
    {
      string username = requestData.ShopperBaseModel[ModelConstants.MODEL_ID_SHOPPERVALID][ModelConstants.FACT_USERNAME];

      var searchFields = new Dictionary<string, string>();
      var returnFields = new List<string>();

      searchFields["loginName"] = username;
      returnFields.Add("loginName");

      SearchShoppersRequestData request = new SearchShoppersRequestData(string.Empty, "ShopperValidator::UsernameRule", searchFields, returnFields);
      SearchShoppersResponseData loginResponse = (SearchShoppersResponseData)Engine.Engine.ProcessRequest(request, EngineRequestValues.ShopperSearch);

      if (loginResponse.Count > 0)
      {
        foreach (var model in _engineResults.ValidationResults)
        {
          if (model.ModelId == ModelConstants.MODEL_ID_SHOPPERVALID)
          {
            foreach (var fact in model.Facts)
            {
              if (fact.FactKey == ModelConstants.FACT_USERNAME)
              {
                FetchResource fetcher = new FetchResource(ResourceNamespace.ShopperValidator, requestData.Culture);
                fact.Status = ValidationResultStatus.InValid;
                fact.Messages.Add(fetcher.GetString("usernameExists"));
                model.ContainsInvalids = true;
                break;
              }
            }
            break;
          }
        }
      }
    }

    private void DoValidatePinAllSameNumbers(ShopperValidatorRequestData requestData)
    {
      string callInPin = requestData.ShopperBaseModel[ModelConstants.MODEL_ID_SHOPPERVALID][ModelConstants.FACT_PIN];
      int callInPinNumeric = Convert.ToInt16(callInPin);
      string baseUniformNumber = "";
      for (int i = 0; i < callInPin.Length; i++)
      {
        baseUniformNumber += "1";
      }
      if (callInPinNumeric == 0 || callInPinNumeric % Convert.ToInt16(baseUniformNumber) == 0)
      {
        foreach (var model in _engineResults.ValidationResults)
        {
          if (model.ModelId == ModelConstants.MODEL_ID_SHOPPERVALID)
          {
            foreach (var fact in model.Facts)
            {
              if (fact.FactKey == ModelConstants.FACT_PIN)
              {
                FetchResource fetcher = new FetchResource(ResourceNamespace.ShopperValidator, requestData.Culture);
                fact.Status = ValidationResultStatus.InValid;
                fact.Messages.Add(fetcher.GetString("pinOneDigit"));
                model.ContainsInvalids = true;
                break;
              }
            }
            break;
          }
        }
      }
    }

    private void DoAuthValidatePasswordRequest(ShopperValidatorRequestData requestData)
    {
      //we do NOT want to pass a shopper/username into this method b/c  it's a new shopper. 
      //Ecomm's WS will fail b/c it can't find the _username b/c it hasn't been inserted yet..
      string temporaryUserName = string.Empty;

      var request = new AuthValidatePasswordRequestData(temporaryUserName, requestData.SourceURL, string.Empty, requestData.Pathway, requestData.PageCount, requestData.ShopperBaseModel[ModelConstants.MODEL_ID_SHOPPERVALID][ModelConstants.FACT_PASSWORD]);
      var response = Engine.Engine.ProcessRequest(request, EngineRequestValues.AuthValidatePassword) as AuthValidatePasswordResponseData;

      if (response != null && response.StatusCode != TwoFactorWebserviceResponseCodes.Success)
      {
        FetchResource fetcher = new FetchResource(ResourceNamespace.ShopperValidator, requestData.Culture);
        string password = fetcher.GetString("password");

        string passwordFailedMessage;
        switch (response.StatusCode)
        {
          case AuthPasswordCodes.PasswordFailBlacklisted:
            passwordFailedMessage = string.Format(fetcher.GetString("commonPhrase"), password);
            break;
          case AuthPasswordCodes.PasswordFailLastFive:
            passwordFailedMessage = fetcher.GetString("lastFive");
            break;
          case AuthPasswordCodes.PasswordFailMatchesHint:
            passwordFailedMessage = string.Format(fetcher.GetString("matchesHint"), password);
            break;
          case AuthPasswordCodes.PasswordFailThirtyDay:
            passwordFailedMessage = fetcher.GetString("samePassword");
            break;
          default:
            passwordFailedMessage = string.Format(fetcher.GetString("isInvalidStatusCode"), password, Convert.ToString(response.StatusCode));
            break;
        }

        foreach (var model in _engineResults.ValidationResults)
        {
          if (model.ModelId == ModelConstants.MODEL_ID_SHOPPERVALID)
          {
            foreach (var fact in model.Facts)
            {
              if (fact.FactKey == ModelConstants.FACT_PASSWORD)
              {
                fact.Status = ValidationResultStatus.InValid;
                fact.Messages.Add(passwordFailedMessage);
                model.ContainsInvalids = true;
                break;
              }
            }
            break;
          }
        }
      }
    }

    private IEnumerable<RuleContainer> CreateShopperRuleContainerList(ShopperToValidate shopperToValidate)
    {
      var shopperRules = new HashSet<RuleContainer>();

      foreach (var shopperProperty in shopperToValidate.AllShopperProperties)
      {
        if (shopperProperty.HasValidationRules)
        {
          shopperRules.Add(shopperProperty.RuleContainer);
        }
      }

      return shopperRules;
    }

    #endregion
  }
}
