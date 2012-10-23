using System.Collections.Generic;
using System.Diagnostics;
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
    string _emailAddress = "Seth";
    string _address1 = "This is valid";
    string _address2 = _overMaxLength;
    string _firstName = "Seth";
    string _lastName = "";
    string _zip = "#@j2asdf";
    string _state = "SomeSate";
    string _city = "city";
    string _workPhoneInvalidStart = "15082415881";
    string _workPhoneValid = "5082415881";
    string _workPhoneInvalidChars = "asddf825881";
    string _workPhoneValidIntl = "1";


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

    HashSet<RuleContainer> rules = new HashSet<RuleContainer>();

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestIndividualRules()
    {
      EmailRule emailRule = new EmailRule(_emailAddress);
      ZipRule zipRuleUs = new ZipRule(_zip, "us", "notstate");
      ZipRule zipRuleOther = new ZipRule(_zip, "aj", "notstate");
      LastNameRule lastName = new LastNameRule(_lastName);
      FirstNameRule firstName = new FirstNameRule(_firstName);
      Address1Rule address1 = new Address1Rule(_address1);
      Address2Rule address2 = new Address2Rule(_address2);
      StateRule stateRule = new StateRule(_state);
      AnyPhoneRule workInvalidStart = new AnyPhoneRule(_workPhoneInvalidStart, true);
      AnyPhoneRule workPhoneInvalidChars = new AnyPhoneRule(_workPhoneInvalidChars, true);
      AnyPhoneRule workPhoneValid = new AnyPhoneRule(_workPhoneValid, true);
      AnyPhoneRule workPhoneUsFail = new AnyPhoneRule(_workPhoneValidIntl, true);
      AnyPhoneRule workPhoneIntl = new AnyPhoneRule(_workPhoneValidIntl, true, "aj");
      AnyPhoneRule workPhoneMissing = new AnyPhoneRule(string.Empty, true);
      AnyPhoneRule workPhoneMissingValid = new AnyPhoneRule(string.Empty, "work phone missing valid");
      PasswordRule passwordruleBlacklist = new PasswordRule("Password1234", false);
      PasswordRule passwordruleNoCapital = new PasswordRule("thisisnotvalid1234", false);
      PasswordRule passwordruleNoNumber = new PasswordRule("Thisisnotvalid", false);
      PasswordRule passwordruleMinLength = new PasswordRule("short", false);

      bool blewChunks = false;
      try
      {
        PasswordRule passwordruleMaxLength = new PasswordRule(_overMaxLength, false);
      }
      catch(System.InvalidOperationException e)
      {
        blewChunks = true;
      }
      Assert.IsTrue(blewChunks);

      PasswordRule passwordruleNotMatchCurrentHint = new PasswordRule("Seth456sethseth", false, "867900");

      BlankRule mybr = new BlankRule(false, "error");

      PasswordRule passwordRuleNotMatchNewHint = new PasswordRule("shouldstopbeforeothervalidations", false, string.Empty, "shouldstopbeforeothervalidations");

      AddAllRules(emailRule, zipRuleUs, zipRuleOther, lastName, firstName, address1, address2, stateRule,
        workInvalidStart, workPhoneInvalidChars, workPhoneValid, workPhoneUsFail, workPhoneIntl, workPhoneMissing, workPhoneMissingValid,
        passwordruleBlacklist, passwordruleNoCapital, passwordruleNoNumber, passwordruleMinLength, passwordruleNotMatchCurrentHint,
        passwordRuleNotMatchNewHint);

      ShopperRuleValidator validator = new ShopperRuleValidator(rules);
      validator.ValidateAllRules();

      SendToDebug(rules);
      var p = "a";
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestOneRule()
    {
      ZipRule zr = new ZipRule(" E2L 4V8", "ca", "nostate");
      zr.Validate();
      CallInPinRule cipr = new CallInPinRule("1234");
      cipr.Validate();
      UsernameRule unr = new UsernameRule("myusername");
      unr.Validate();

      PasswordRule pwRule = new PasswordRule("PASSWORD12340", true, "jasdfj29nask3", "asdf");
      pwRule.Validate();
      bool b = pwRule.IsValid;

      PasswordHintRule pwHintrule = new PasswordHintRule("lert");
      pwHintrule.Validate();
      Debug.WriteLine("pwHintrule.Isvalid:     " + pwHintrule.IsValid.ToString());

    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestUsingShopperClass()
    {

      ShopperToValidate shopper = new ShopperToValidate("www.requesturl.com", "pathway", 0);

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

  }
}
