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
      shopperToValidate.FirstName.RuleContainer = new FirstNameRule(shopperToValidate.FirstName.Value);
      shopperToValidate.LastName.RuleContainer = new LastNameRule(shopperToValidate.LastName.Value);
      shopperToValidate.BirthDay.RuleContainer = new BirthDayRule(shopperToValidate.BirthMonth.Value, shopperToValidate.BirthDay.Value);
      shopperToValidate.BirthMonth.RuleContainer = shopperToValidate.BirthDay.RuleContainer;
      #endregion

      #region Address Rules
      shopperToValidate.Address1.RuleContainer = new Address1Rule(shopperToValidate.Address1.Value);
      shopperToValidate.Address2.RuleContainer = new Address2Rule(shopperToValidate.Address2.Value);
      shopperToValidate.Email.RuleContainer = new EmailRule(shopperToValidate.Email.Value);
      shopperToValidate.City.RuleContainer = new CityRule(shopperToValidate.City.Value);
      shopperToValidate.State.RuleContainer = new StateRule(shopperToValidate.State.Value);
      shopperToValidate.Zip.RuleContainer = new ZipRule(shopperToValidate.Zip.Value, shopperToValidate.Country.Value, shopperToValidate.State.Value);
      shopperToValidate.Country.RuleContainer = new CountryRule(shopperToValidate.Country.Value);
      #endregion

      #region Phone Rules
      shopperToValidate.PhoneWork.RuleContainer = new AnyPhoneRule(shopperToValidate.PhoneWork.Value, true, shopperToValidate.Country.Value);
      shopperToValidate.PhoneWorkExtension.RuleContainer = new PhoneExtRule(shopperToValidate.PhoneWorkExtension.Value);
      shopperToValidate.PhoneHome.RuleContainer = new AnyPhoneRule(shopperToValidate.PhoneHome.Value, shopperToValidate.Country.Value);
      shopperToValidate.PhoneMobile.RuleContainer = new AnyPhoneRule(shopperToValidate.PhoneMobile.Value, shopperToValidate.Country.Value);
      shopperToValidate.PhoneMobileSurvey.RuleContainer = new AnyPhoneRule(shopperToValidate.PhoneMobileSurvey.Value, shopperToValidate.Country.Value);
      #endregion

      #region Password Rules
      shopperToValidate.Username.RuleContainer = new UsernameRule(shopperToValidate.Username.Value, requestData.SourceURL, 
        requestData.Pathway, requestData.PageCount, requestData.IsNewShopper);
      shopperToValidate.Password.RuleContainer = new PasswordRule(shopperToValidate.Password.Value, requestData.IsNewShopper,
        requestData.SourceURL, requestData.Pathway, requestData.PageCount, shopperToValidate.Username.Value, shopperToValidate.PasswordHint.Value);
      shopperToValidate.PasswordConfirm.RuleContainer = new PasswordConfirmRule(shopperToValidate.PasswordConfirm.Value, shopperToValidate.Password.Value);
      shopperToValidate.PasswordHint.RuleContainer = new PasswordHintRule(shopperToValidate.PasswordHint.Value);
      shopperToValidate.CallInPin.RuleContainer = new CallInPinRule(shopperToValidate.CallInPin.Value);
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
