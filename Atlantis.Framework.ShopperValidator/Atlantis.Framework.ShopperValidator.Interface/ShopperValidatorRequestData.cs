using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ShopperValidator.Interface.RuleConstants;
using Atlantis.Framework.ShopperValidator.Interface.ShopperValidation;
using Atlantis.Framework.ValidateField.Interface;

namespace Atlantis.Framework.ShopperValidator.Interface
{
 public class ShopperValidatorRequestData: RequestData
  {
   public ShopperToValidate ShopperToValidate;
   public bool IsNewShopper;

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

   public ShopperValidatorRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, string username, string password, string confirmPassword, string pin, string email)
     : base(shopperId, sourceURL, orderId, pathway, pageCount)
   {
     ShopperBaseModel = new Dictionary<string, Dictionary<string, string>>(1);
     ShopperBaseModel.Add(ModelConstants.MODEL_ID_SHOPPERVALID, new Dictionary<string, string>
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
                            {ModelConstants.FACT_INVALID_CHARACTERS_REGEX, RegexConstants.InvalidCharactersPattern}
                          });
   }

   public ShopperValidatorRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, ShopperToValidate shopperToValidate, bool isNewShopper)
     : base(shopperId, sourceURL, orderId, pathway, pageCount)
   {
     ShopperToValidate = shopperToValidate;
     IsNewShopper = isNewShopper;
   }

   public override string GetCacheMD5()
   {
     throw new NotImplementedException();
   }
  }
}
