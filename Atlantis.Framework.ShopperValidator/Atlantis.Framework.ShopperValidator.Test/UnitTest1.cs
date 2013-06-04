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
  }
}
