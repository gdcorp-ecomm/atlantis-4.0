using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Atlantis.Framework.RuleEngine.Results;
using Atlantis.Framework.ShopperValidator.Interface;
using Atlantis.Framework.ShopperValidator.Interface.ShopperValidation;
using Atlantis.Framework.ShopperValidator.Interface.ValidationRules;
using Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules;
using Atlantis.Framework.ShopperValidator.Interface.Validator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.ShopperValidator.Test
{
  [TestClass]
  [DeploymentItem("Atlantis.Framework.ShopperValidator.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.SearchShoppers.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.ValidateField.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.AuthValidatePassword.Impl.dll")]
  public class UnitTest1
  {
    private const string _emailAddress = "Seth";
    private const string _address1 = "This is valid";
    readonly string _address2 = _overMaxLength;
    private const string _firstName = "Seth";
    private const string _lastName = "";
    private const string _zip = "#@j2asdf";
    private const string _state = "SomeSate";
    private const string _city = "city";
    private const string _workPhoneInvalidStart = "15082415881";
    private const string _workPhoneValid = "5082415881";
    private const string _workPhoneInvalidChars = "asddf825881";
    private const string _workPhoneValidIntl = "1";


    public static string _overMaxLength
    {
      get
      {
        var exceededLength = string.Empty;
        for (int i = 1; i < 500; i++)
        {
          exceededLength += i.ToString();
        }

        return exceededLength;
      }
    }

    readonly HashSet<RuleContainer> rules = new HashSet<RuleContainer>();

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestIndividualRules()
    {
      var emailRule = new EmailRule(_emailAddress);
      var zipRuleUs = new ZipRule(_zip, "us", "notstate");
      var zipRuleOther = new ZipRule(_zip, "aj", "notstate");
      var lastName = new LastNameRule(_lastName);
      var firstName = new FirstNameRule(_firstName);
      var address1 = new Address1Rule(_address1);
      var address2 = new Address2Rule(_address2);
      var stateRule = new StateRule(_state);
      var workInvalidStart = new AnyPhoneRule(_workPhoneInvalidStart, true);
      var workPhoneInvalidChars = new AnyPhoneRule(_workPhoneInvalidChars, true);
      var workPhoneValid = new AnyPhoneRule(_workPhoneValid, true);
      var workPhoneUsFail = new AnyPhoneRule(_workPhoneValidIntl, true);
      var workPhoneIntl = new AnyPhoneRule(_workPhoneValidIntl, true, "aj");
      var workPhoneMissing = new AnyPhoneRule(string.Empty, true);
      var workPhoneMissingValid = new AnyPhoneRule(string.Empty, "work phone missing valid");
      var passwordruleBlacklist = new PasswordRule("Password1234", false);
      var passwordruleNoCapital = new PasswordRule("thisisnotvalid1234", false);
      var passwordruleNoNumber = new PasswordRule("Thisisnotvalid", false);
      var passwordruleMinLength = new PasswordRule("short", false);

      var blewChunks = false;
      try
      {
        var passwordruleMaxLength = new PasswordRule(_overMaxLength, false);
      }
      catch(System.InvalidOperationException e)
      {
        blewChunks = true;
      }
      Assert.IsTrue(blewChunks);

      var passwordruleNotMatchCurrentHint = new PasswordRule("Seth456sethseth", false, "867900");

      var mybr = new BlankRule(false, "error");

      var passwordRuleNotMatchNewHint = new PasswordRule("shouldstopbeforeothervalidations", false, string.Empty, "shouldstopbeforeothervalidations");

      AddAllRules(emailRule, zipRuleUs, zipRuleOther, lastName, firstName, address1, address2, stateRule,
        workInvalidStart, workPhoneInvalidChars, workPhoneValid, workPhoneUsFail, workPhoneIntl, workPhoneMissing, workPhoneMissingValid,
        passwordruleBlacklist, passwordruleNoCapital, passwordruleNoNumber, passwordruleMinLength, passwordruleNotMatchCurrentHint,
        passwordRuleNotMatchNewHint);

      var validator = new ShopperRuleValidator(rules);
      validator.ValidateAllRules();

      SendToDebug(rules);
      var p = "a";
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestOneRule()
    {
      var zr = new ZipRule(" E2L 4V8", "ca", "nostate");
      zr.Validate();
      var cipr = new CallInPinRule("1234");
      cipr.Validate();
      var unr = new UsernameRule("myusername");
      unr.Validate();

      var pwRule = new PasswordRule("PASSWORD12340", true, "jasdfj29nask3", "asdf");
      pwRule.Validate();
      bool b = pwRule.IsValid;

      var pwHintrule = new PasswordHintRule("lert");
      pwHintrule.Validate();
      Debug.WriteLine("pwHintrule.Isvalid:     " + pwHintrule.IsValid.ToString());

    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestUsingShopperClass()
    {

      var shopper = new ShopperToValidate("www.requesturl.com", "pathway", 0);

      shopper.FirstName.Value = "Seth";
      shopper.LastName.Value = "Yukna";
      shopper.Address1.Value = "123 any street";
      shopper.Address2.Value = _overMaxLength;
      shopper.CallInPin.Value = "1423";
      shopper.City.Value = "Lakewood";
      shopper.Country.Value = "ca"; 
      shopper.Email.Value = "seth";
      shopper.Password.Value = "Seth1seth";
      shopper.PasswordConfirm.Value = shopper.Password.Value;
      shopper.PhoneHome.Value = "5";
      shopper.PhoneMobile.Value = "508-241-5881";
      shopper.PhoneMobileSurvey.Value = "";
      shopper.PhoneWork.Value = "1-508-241-5881";
      shopper.PhoneWorkExtension.Value = "7";
      shopper.State.Value = "CO";
      shopper.BirthMonth.Value = "9";
      shopper.AccountUsageType.Value = "1";
      shopper.Username.Value = "syukna";
      shopper.Zip.Value = "E2L 4V8";
      shopper.BirthDay.Value = "23";


      var isNewShopper = true;
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, shopper, isNewShopper);
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      foreach (var prop in response.ValidatedShopper.AllShopperProperties)
      {
        if (prop.HasValidationRules && !prop.RuleContainer.IsValid)
        {
          Debug.WriteLine("IsValid: " + prop.RuleContainer.IsValid.ToString() + "   ErrorMessage: " + prop.RuleContainer.ErrorMessage);
        }
      }

      string x = "pause";
    }

    public void AddAllRules(params RuleContainer[] list)
    {
      foreach (RuleContainer rule in list)
      {
        rules.Add(rule);
      }
    }

    public void SendToDebug(HashSet<RuleContainer> rules)
    {
      foreach (RuleContainer rule in rules)
      {
        Debug.WriteLine("IsValid: " + rule.IsValid.ToString() + "   ErrorMessage: " + rule.ErrorMessage);
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [ExpectedException(typeof(System.InvalidOperationException), "Password cannot be greater than 255 characters.  Trim Password before supplying it to the validator.")]
    public void TestOverMaxLengthPassword()
    {
      PasswordRule passwordruleMaxLength = new PasswordRule(_overMaxLength, false);
      AddAllRules(passwordruleMaxLength);

      ShopperRuleValidator validator = new ShopperRuleValidator(rules);
      validator.ValidateAllRules();
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestAllRulesBlank()
    {
      ShopperToValidate shopper = new ShopperToValidate();

      #region location
      shopper.Address1.Value = "";
      shopper.Address2.Value = "";
      shopper.City.Value = "";
      shopper.State.Value = "";
      shopper.Country.Value = "";
      shopper.Zip.Value = "";
      #endregion

      #region contact
      shopper.Email.Value = "";
      shopper.PhoneHome.Value = "";
      shopper.PhoneMobile.Value = "";
      shopper.PhoneMobileSurvey.Value = "";
      shopper.PhoneWork.Value = "";
      shopper.PhoneWorkExtension.Value = "";
      #endregion

      #region personal
      shopper.FirstName.Value = "";
      shopper.LastName.Value = "";
      shopper.BirthDay.Value = "";
      shopper.BirthMonth.Value = "";
      shopper.AccountUsageType.Value = "";
      #endregion

      #region credentials
      shopper.Username.Value = "";
      shopper.Password.Value = "";
      shopper.PasswordConfirm.Value = shopper.Password.Value;
      shopper.PasswordHint.Value = "";
      shopper.CallInPin.Value = "";
      #endregion

      bool isNewShopper = true;
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, shopper, isNewShopper);
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      foreach (ShopperProperty prop in response.ValidatedShopper.AllShopperProperties)
      {
        if (prop.HasValidationRules && !prop.RuleContainer.IsValid)
        {
          Debug.WriteLine("IsValid: " + prop.RuleContainer.IsValid.ToString() + "   ErrorMessage: " + prop.RuleContainer.ErrorMessage);
        }
      }

      Assert.IsTrue(response.IsSuccess);
    }

    #region Tests with Required Fields

    private void EvaluateValid(IModelResult model, params string[] failFactKeys)
    {
      Assert.IsNotNull(model);

      Assert.IsTrue(model.ContainsInvalids ^ !failFactKeys.Any());

      foreach (var fact in model.Facts)
      {
        if (failFactKeys.Contains(fact.FactKey))
        {
          Assert.IsTrue(fact.Status == ValidationResultStatus.InValid);
          Assert.IsTrue(fact.Messages.Count > 0);
        }
        else
        {
          Assert.IsTrue(fact.Status == ValidationResultStatus.Valid);
        }
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestRuleEngineSlimShopperValid()
    {
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "firstname", "lastname", "username", "P4ssW0rd!", "P4ssW0rd!", "1254", "email@email.com");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestStandardShopperMinimumRequired()
    {
      ShopperToValidate shopper = new ShopperToValidate("www.requesturl.com", "pathway", 0);
      shopper.InitStandardRequiredFields();

      #region location
      shopper.Address1.Value = "123 any street";
      //shopper.Address2.Value = "";
      shopper.City.Value = "Lakewood";
      shopper.State.Value = "CO";
      shopper.Country.Value = "ca";
      shopper.Zip.Value = "E2L 4V8";
      #endregion

      #region contact
      shopper.Email.Value = "seth@sdr.coskm";
      //shopper.PhoneHome.Value = "5";
      //shopper.PhoneMobile.Value = "508-241-5881";
      //shopper.PhoneMobileSurvey.Value = "";
      shopper.PhoneWork.Value = "508-241-5881";
      //shopper.PhoneWorkExtension.Value = "7";
      #endregion

      #region personal
      shopper.FirstName.Value = "Seth";
      shopper.LastName.Value = "Yukna";
      //shopper.BirthDay.Value = "23";
      //shopper.BirthMonth.Value = "9";
      //shopper.AccountUsageType.Value = "1";
      #endregion

      #region credentials
      shopper.Username.Value = "dfewcasdrwq";
      shopper.Password.Value = "P4ssW0rd!";
      shopper.PasswordConfirm.Value = shopper.Password.Value;
      shopper.PasswordHint.Value = "idk this?";
      shopper.CallInPin.Value = "1423";
      #endregion

      bool isNewShopper = true;
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, shopper, isNewShopper);
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      foreach (ShopperProperty prop in response.ValidatedShopper.AllShopperProperties)
      {
        if (prop.HasValidationRules && !prop.RuleContainer.IsValid)
        {
          Debug.WriteLine("IsValid: " + prop.RuleContainer.IsValid.ToString() + "   ErrorMessage: " + prop.RuleContainer.ErrorMessage);
        }
        if (prop.HasValidationRules)
          Assert.IsTrue(prop.RuleContainer.IsValid);
      }

      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestSlimShopperMinimumRequired()
    {
      ShopperToValidate shopper = new ShopperToValidate("www.requesturl.com", "pathway", 0);
      shopper.InitSlimRequiredFields();

      #region location
      shopper.Address1.Value = "123 any street";
      //shopper.Address2.Value = "";
      //shopper.City.Value = "Lakewood";
      //shopper.State.Value = "CO";
      //shopper.Country.Value = "ca";
      //shopper.Zip.Value = "E2L 4V8";
      #endregion

      #region contact
      shopper.Email.Value = "seth@sdr.coskm";
      //shopper.PhoneHome.Value = "5";
      //shopper.PhoneMobile.Value = "508-241-5881";
      //shopper.PhoneMobileSurvey.Value = "";
      //shopper.PhoneWork.Value = "508-241-5881";
      //shopper.PhoneWorkExtension.Value = "7";
      #endregion

      #region personal
      shopper.FirstName.Value = "Seth";
      shopper.LastName.Value = "Yukna";
      //shopper.BirthDay.Value = "23";
      //shopper.BirthMonth.Value = "9";
      //shopper.AccountUsageType.Value = "1";
      #endregion

      #region credentials
      shopper.Username.Value = "dfewcasdrwq";
      shopper.Password.Value = "P4ssW0rd!";
      shopper.PasswordConfirm.Value = shopper.Password.Value;
      shopper.PasswordHint.Value = "idk this?";
      shopper.CallInPin.Value = "1423";
      #endregion

      bool isNewShopper = true;
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, shopper, isNewShopper);
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      foreach (ShopperProperty prop in response.ValidatedShopper.AllShopperProperties)
      {
        if (prop.HasValidationRules && !prop.RuleContainer.IsValid)
        {
          Debug.WriteLine("IsValid: " + prop.RuleContainer.IsValid.ToString() + "   ErrorMessage: " + prop.RuleContainer.ErrorMessage);
        }
        if (prop.HasValidationRules)
          Assert.IsTrue(prop.RuleContainer.IsValid);
      }

      Assert.IsTrue(response.IsSuccess);
    }

    #endregion


    #region Supplemental Rule Engine Properties

    #region First Name Properties
    private const int _firstNameMaxLength = 30;
    private string FirstNameMaxLength { get { return Convert.ToString(_firstNameMaxLength); } }
    #endregion

    #region Last Name Properties
    private const int _lastNameMaxLength = 50;
    private string LastNameMaxLength { get { return Convert.ToString(_lastNameMaxLength); } }
    #endregion

    #region Username Properties
    private const int _usernameMaxLength = 30;
    private string UsernameMaxLength { get { return Convert.ToString(_usernameMaxLength); } }
    #endregion

    #region Password Properties
    private const int _passwordMaxLength = 255;
    private string PasswordMaxLength { get { return Convert.ToString(_passwordMaxLength); } }

    private const int _passwordMinLength = 8;
    private string PasswordMinLength { get { return Convert.ToString(_passwordMinLength); } }

    private const string _passwordRegex = @"(?=^\S).*(?=.*[A-Z])(?=.*\d).*(?=\S$)";
    private string PasswordRegex { get { return _passwordRegex; } }
    #endregion

    #region Pin Properties
    private const int _callInPinMaxLength = 4;
    private string CallInPinMaxLength { get { return Convert.ToString(_callInPinMaxLength); } }

    private const int _callInPinMinLength = 4;
    private string CallInPinMinLength { get { return Convert.ToString(_callInPinMinLength); } }
    #endregion

    #region Email Properties
    private const int _emailMaxLength = 100;
    private string EmailMaxLength { get { return Convert.ToString(_emailMaxLength); } }

    private const string _emailRegex = @"^[a-zA-Z0-9._%+\-]+@[a-zA-Z0-9.\-]+\.[a-zA-Z]{2,6}$";
    private string EmailRegex { get { return _emailRegex; } }
    #endregion

    #region General Properties
    private const string _numericOnlyRegex = @"^[0-9]*$";
    private string NumericOnlyRegex { get { return _numericOnlyRegex; } }

    private const string _invalidCharactersRegex = @"[^\x20-\x27\x2A-\x3A\x3F-\x7E]";
    private string InvalidCharactersRegex { get { return _invalidCharactersRegex; } }
    #endregion

    #endregion

    #region Rule Engine Test Methods
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREGoodShopper()
    {
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "firstname", "lastname", "username", "P4ssW0rd!", "P4ssW0rd!", "1254", "email@email.com");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel);
    }
    
    #region First Name Tests (count = 4)
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadFirstNameEmpty()
    {
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, string.Empty, "lastname", "username", "P4ssW0rd!", "P4ssW0rd!", "1254", "email@email.com");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_FIRST_NAME);
    }
    
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TesREtBadFirstNameNull()
    {
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, null, "lastname", "username", "P4ssW0rd!", "P4ssW0rd!", "1254", "email@email.com");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_FIRST_NAME);
    }
    
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadFirstNameMaxLength()
    {
      var longFirstName = "";
      while (longFirstName.Length < _firstNameMaxLength + 1)
      {
        longFirstName += "A";
      }

      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, longFirstName, "lastname", "username", "P4ssW0rd!", "P4ssW0rd!", "1254", "email@email.com");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_FIRST_NAME);
    }
    
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadFirstNameInvalidChars()
    {
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "first;name", "lastname", "username", "P4ssW0rd!", "P4ssW0rd!", "1254", "email@email.com");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_FIRST_NAME);
    }
    #endregion
    
    
    #region Last Name Tests (count = 4)
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadLastNameEmpty()
    {
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "firstname", string.Empty, "username", "P4ssW0rd!", "P4ssW0rd!", "1254", "email@email.com");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_LAST_NAME);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadLastNameNull()
    {
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "firstname", null, "username", "P4ssW0rd!", "P4ssW0rd!", "1254", "email@email.com");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_LAST_NAME);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadLastNameMaxLength()
    {
      var longLastName = "";
      while (longLastName.Length < _lastNameMaxLength + 1)
      {
        longLastName += "A";
      }

      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "firstname", longLastName, "username", "P4ssW0rd!", "P4ssW0rd!", "1254", "email@email.com");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_LAST_NAME);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadLastNameInvalidChars()
    {
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "firstname", "last;name", "username", "P4ssW0rd!", "P4ssW0rd!", "1254", "email@email.com");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_LAST_NAME);
    }
    #endregion
    
    
    #region Username Tests (count = 5)
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadUsernameEmpty()
    {
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "firstname", "lastname", string.Empty, "P4ssW0rd!", "P4ssW0rd!", "1254", "email@email.com");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_USERNAME);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadUsernameNull()
    {
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "firstname", "lastname", null, "P4ssW0rd!", "P4ssW0rd!", "1254", "email@email.com");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_USERNAME);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadUsernameMaxLength()
    {
      var longUsername = "";
      while (longUsername.Length < _usernameMaxLength + 1)
      {
        longUsername += "A";
      }

      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "firstname", "lastname", longUsername, "P4ssW0rd!", "P4ssW0rd!", "1254", "email@email.com");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_USERNAME);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadUsernameAllNumeric()
    {
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "firstname", "lastname", "123456789", "P4ssW0rd!", "P4ssW0rd!", "1254", "email@email.com");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_USERNAME);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadUsernameInvalidChars()
    {
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "firstname", "lastname", "user;name", "P4ssW0rd!", "P4ssW0rd!", "1254", "email@email.com");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_USERNAME);
    }
    #endregion


    #region Password Tests (count = 12)
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBothBadPasswordsEmpty()
    {
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "firstname", "lastname", "username", string.Empty, string.Empty, "1254", "email@email.com");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_PASSWORD, ModelConstants.FACT_PASSWORD2);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBothBadPasswordsNull()
    {
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "firstname", "lastname", "username", null, null, "1254", "email@email.com");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_PASSWORD, ModelConstants.FACT_PASSWORD2);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadPasswordEmpty()
    {
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "firstname", "lastname", "username", string.Empty, "P4ssW0rd!", "1254", "email@email.com");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_PASSWORD, ModelConstants.FACT_PASSWORD2);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadPassword2Empty()
    {
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "firstname", "lastname", "username", "P4ssW0rd!", string.Empty, "1254", "email@email.com");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_PASSWORD2);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadPasswordNull()
    {
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "firstname", "lastname", "username", null, "P4ssW0rd!", "1254", "email@email.com");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_PASSWORD, ModelConstants.FACT_PASSWORD2);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadPassword2Null()
    {
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "firstname", "lastname", "username", "P4ssW0rd!", null, "1254", "email@email.com");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_PASSWORD2);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadPasswordRegexNoNumbers()
    {
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "firstname", "lastname", "username", "PassWord!", "PassWord!", "1254", "email@email.com");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_PASSWORD);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadPasswordRegexNoCaps()
    {
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "firstname", "lastname", "username", "p4ssw0rd!", "p4ssw0rd!", "1254", "email@email.com");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_PASSWORD);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadPasswordRegexBeginningSpace()
    {
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "firstname", "lastname", "username", " P4ssW0rd!", " P4ssW0rd!", "1254", "email@email.com");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_PASSWORD);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadPasswordMinLength()
    {
      var shortPassword = "P4s!";
      while (shortPassword.Length < _passwordMinLength - 1)
      {
        shortPassword += "A";
      }

      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "firstname", "lastname", "username", shortPassword, shortPassword, "1254", "email@email.com");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_PASSWORD);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadPasswordMaxLength()
    {
      var longPassword = "P4s!";
      while (longPassword.Length < _passwordMaxLength + 1)
      {
        longPassword += "A";
      }

      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "firstname", "lastname", "username", longPassword, longPassword, "1254", "email@email.com");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_PASS_MAX_LENGTH);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadPasswordMatch()
    {
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "firstname", "lastname", "username", "P4ssW0rd!", "P4ssW0rd!!", "1254", "email@email.com");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_PASSWORD2);
    }
    #endregion
    

    #region PIN Tests (count = 7)
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadPINEmpty()
    {
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "firstname", "lastname", "username", "P4ssW0rd!", "P4ssW0rd!", string.Empty, "email@email.com");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_PIN);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadPINNull()
    {
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "firstname", "lastname", "username", "P4ssW0rd!", "P4ssW0rd!", null, "email@email.com");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_PIN);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadPINMinLength()
    {
      var shortPIN = "7";
      while (shortPIN.Length < _callInPinMinLength - 1)
      {
        shortPIN += "1";
      }

      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "firstname", "lastname", "username", "P4ssW0rd!", "P4ssW0rd!", shortPIN, "email@email.com");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_PIN);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadPINMaxLength()
    {
      var longPIN = "7";
      while (longPIN.Length < _callInPinMaxLength + 1)
      {
        longPIN += "1";
      }

      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "firstname", "lastname", "username", "P4ssW0rd!", "P4ssW0rd!", longPIN, "email@email.com");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_PIN);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadPINNotNumericOnly()
    {
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "firstname", "lastname", "username", "P4ssW0rd!", "P4ssW0rd!", "123f", "email@email.com");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_PIN);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadPINAscSequence()
    {
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "firstname", "lastname", "username", "P4ssW0rd!", "P4ssW0rd!", "1234", "email@email.com");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_PIN);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadPINDescSequence()
    {
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "firstname", "lastname", "username", "P4ssW0rd!", "P4ssW0rd!", "8765", "email@email.com");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_PIN);
    }
    #endregion


    #region Email Tests (count = 4)
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadEmailEmpty()
    {
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "firstname", "lastname", "username", "P4ssW0rd!", "P4ssW0rd!", "1254", string.Empty);
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_EMAIL);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadEmailNull()
    {
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "firstname", "lastname", "username", "P4ssW0rd!", "P4ssW0rd!", "1254", null);
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_EMAIL);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadEmailMaxLength()
    {
      var longEmail = "a@x.";
      while (longEmail.Length < _emailMaxLength + 1)
      {
        longEmail += "c";
      }

      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "firstname", "lastname", "username", "P4ssW0rd!", "P4ssW0rd!", "1254", longEmail);
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_EMAIL);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadEmailRegex()
    {
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "firstname", "lastname", "username", "P4ssW0rd!", "P4ssW0rd!", "1254", "email@emailcom");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_EMAIL);
    }
    #endregion

    #endregion

  }
}
