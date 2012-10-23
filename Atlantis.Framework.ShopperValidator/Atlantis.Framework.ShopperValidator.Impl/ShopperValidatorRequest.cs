using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ShopperValidator.Interface;
using Atlantis.Framework.ShopperValidator.Interface.ShopperValidation;
using Atlantis.Framework.ShopperValidator.Interface.ValidationRules;
using Atlantis.Framework.ShopperValidator.Interface.Validator;

namespace Atlantis.Framework.ShopperValidator.Impl
{
  public class ShopperValidatorRequest : IRequest
  {

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      ShopperValidatorResponseData responseData;

      try
      {
        var request = (ShopperValidatorRequestData)requestData;

        if (request.ShopperToValidate == null)
        {
          throw new AtlantisException(requestData, "ShopperValidator::RequestHandler", "ShopperToValidate cannot be null", "--input data--");
        }

        ValidateShopper(request);
        
        responseData = new ShopperValidatorResponseData(request.ShopperToValidate);
      }
      catch (AtlantisException aex)
      {
        responseData = new ShopperValidatorResponseData(aex);
      }
      catch (Exception ex)
      {
        string message = ex.Message + Environment.NewLine + ex.StackTrace;
        AtlantisException aex = new AtlantisException(requestData, "ShopperValidator::RequestHandler", message, string.Empty);
        responseData = new ShopperValidatorResponseData(aex);
      }

      return responseData;
    }

    #region Shopper Validation Functionality

    public void ValidateShopper(ShopperValidatorRequestData requestData)
    {
      ShopperToValidate shopperToValidate = requestData.ShopperToValidate;

      #region Create Rules
      #region Name and Birthday Rules
      if (shopperToValidate.FirstName.Value != null)
        shopperToValidate.FirstName.RuleContainer = new FirstNameRule(shopperToValidate.FirstName.Value, isRequired: shopperToValidate.FirstName.IsRequired);
      if (shopperToValidate.LastName.Value != null)
        shopperToValidate.LastName.RuleContainer = new LastNameRule(shopperToValidate.LastName.Value, isRequired: shopperToValidate.LastName.IsRequired);
      if (shopperToValidate.BirthDay.Value != null)
        shopperToValidate.BirthDay.RuleContainer = new BirthDayRule(shopperToValidate.BirthMonth.Value, shopperToValidate.BirthDay.Value,
          monthIsRequired: shopperToValidate.BirthMonth.IsRequired, dayIsRequired: shopperToValidate.BirthDay.IsRequired);
      if (shopperToValidate.BirthMonth.Value != null)
        shopperToValidate.BirthMonth.RuleContainer = shopperToValidate.BirthDay.RuleContainer;
      #endregion

      #region Address Rules
      if (shopperToValidate.Address1.Value != null)
        shopperToValidate.Address1.RuleContainer = new Address1Rule(shopperToValidate.Address1.Value, isRequired: shopperToValidate.Address1.IsRequired);
      if (shopperToValidate.Address2.Value != null)
        shopperToValidate.Address2.RuleContainer = new Address2Rule(shopperToValidate.Address2.Value, isRequired: shopperToValidate.Address2.IsRequired);
      if (shopperToValidate.Email.Value != null)
        shopperToValidate.Email.RuleContainer = new EmailRule(shopperToValidate.Email.Value, isRequired: shopperToValidate.Email.IsRequired);
      if (shopperToValidate.City.Value != null)
        shopperToValidate.City.RuleContainer = new CityRule(shopperToValidate.City.Value, isRequired: shopperToValidate.City.IsRequired);
      if (shopperToValidate.State.Value != null)
        shopperToValidate.State.RuleContainer = new StateRule(shopperToValidate.State.Value, isRequired: shopperToValidate.State.IsRequired);
      if (shopperToValidate.Zip.Value != null)
        shopperToValidate.Zip.RuleContainer = new ZipRule(shopperToValidate.Zip.Value, shopperToValidate.Country.Value, shopperToValidate.State.Value, isRequired: shopperToValidate.Zip.IsRequired);
      if (shopperToValidate.Country.Value != null)
        shopperToValidate.Country.RuleContainer = new CountryRule(shopperToValidate.Country.Value, isRequired: shopperToValidate.Country.IsRequired);
      #endregion

      #region Phone Rules
      if (shopperToValidate.PhoneWork.Value != null)
        shopperToValidate.PhoneWork.RuleContainer = new AnyPhoneRule(shopperToValidate.PhoneWork.Value, isRequired: shopperToValidate.PhoneWork.IsRequired, countryCode: shopperToValidate.Country.Value);
      if (shopperToValidate.PhoneWorkExtension.Value != null)
        shopperToValidate.PhoneWorkExtension.RuleContainer = new PhoneExtRule(shopperToValidate.PhoneWorkExtension.Value, isRequired: shopperToValidate.PhoneWorkExtension.IsRequired);
      if (shopperToValidate.PhoneHome.Value != null)
        shopperToValidate.PhoneHome.RuleContainer = new AnyPhoneRule(shopperToValidate.PhoneHome.Value, isRequired: shopperToValidate.PhoneHome.IsRequired, countryCode: shopperToValidate.Country.Value);
      if (shopperToValidate.PhoneMobile.Value != null)
        shopperToValidate.PhoneMobile.RuleContainer = new AnyPhoneRule(shopperToValidate.PhoneMobile.Value, isRequired: shopperToValidate.PhoneMobile.IsRequired, countryCode: shopperToValidate.Country.Value);
      if (shopperToValidate.PhoneMobileSurvey.Value != null)
        shopperToValidate.PhoneMobileSurvey.RuleContainer = new AnyPhoneRule(shopperToValidate.PhoneMobileSurvey.Value, isRequired: shopperToValidate.PhoneMobileSurvey.IsRequired, countryCode: shopperToValidate.Country.Value);
      #endregion

      #region Password Rules
      if (shopperToValidate.Username.Value != null)
        shopperToValidate.Username.RuleContainer = new UsernameRule(shopperToValidate.Username.Value, requestData.SourceURL,
          requestData.Pathway, requestData.PageCount, requestData.IsNewShopper, isRequired: shopperToValidate.Username.IsRequired);
      if (shopperToValidate.Password.Value != null)
        shopperToValidate.Password.RuleContainer = new PasswordRule(shopperToValidate.Password.Value, requestData.IsNewShopper,
          requestData.SourceURL, requestData.Pathway, requestData.PageCount, shopperToValidate.Username.Value, shopperToValidate.PasswordHint.Value,
          isRequired: shopperToValidate.Password.IsRequired);
      if (shopperToValidate.PasswordConfirm.Value != null)
        shopperToValidate.PasswordConfirm.RuleContainer = new PasswordConfirmRule(shopperToValidate.PasswordConfirm.Value, shopperToValidate.Password.Value);
      if (shopperToValidate.PasswordHint.Value != null)
        shopperToValidate.PasswordHint.RuleContainer = new PasswordHintRule(shopperToValidate.PasswordHint.Value, isRequired: shopperToValidate.PasswordHint.IsRequired);
      if (shopperToValidate.CallInPin.Value != null)
        shopperToValidate.CallInPin.RuleContainer = new CallInPinRule(shopperToValidate.CallInPin.Value, isRequired: shopperToValidate.CallInPin.IsRequired);
      #endregion

      HashSet<RuleContainer> shopperRules = CreateShopperRuleContainerList(shopperToValidate);
      #endregion

      ShopperRuleValidator shopperValidator = new ShopperRuleValidator(shopperRules);
      shopperValidator.ValidateAllRules();
    }
    private HashSet<RuleContainer> CreateShopperRuleContainerList(ShopperToValidate shopperToValidate)
    {
      HashSet<RuleContainer> shopperRules = new HashSet<RuleContainer>();

      foreach (ShopperProperty shopperProperty in shopperToValidate.AllShopperProperties)
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
