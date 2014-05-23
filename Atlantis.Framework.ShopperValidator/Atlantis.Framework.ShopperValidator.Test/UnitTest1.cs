using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;
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
  [DeploymentItem("altanis.config")]
  [DeploymentItem("Atlantis.Framework.ShopperValidator.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.Shopper.Impl.dll")]
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
      var workPhoneMissingValid = new AnyPhoneRule("work phone missing valid");
      var passwordruleBlacklist = new PasswordRule("Password1234", false);
      var passwordruleNoCapital = new PasswordRule("thisisnotvalid1234", false);
      var passwordruleNoNumber = new PasswordRule("Thisisnotvalid", false);
      var passwordruleMinLength = new PasswordRule("short", false);

      var blewChunks = false;
      try
      {
        var passwordruleMaxLength = new PasswordRule(_overMaxLength, false);
      }
      catch (System.InvalidOperationException e)
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

      shopper.FirstName.Value = "";
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

      shopper.InitStandardRequiredFields();
      var isNewShopper = true;
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, shopper, isNewShopper, "esdX");
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


    private string _culture = "es-US";
    private string _invalidChars = "<script>alert<%=Message%>";
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestLanguageErrorMessages()
    {
      var blankRule = new BlankRule(false, culture: _culture);
      var equalRule = new EqualLength(_culture, "equal", "four", 3, true);
      var invalidChar = new InvalidCharactersRule("invalidchar", "<script>$M<@$%M<A", _culture);
      var match = new MatchRule(_culture, "match", "asdf", "match2nd", "isjdf");
      var maxLength = new MaxLengthRule(_culture, "maxLength", "thisislonger", 4);
      var minLength = new MinLengthRule(_culture, "minlength", "asdfasdf", 25);
      var notMatch = new NotMatchRule(_culture, "notmatch", "asdf", "notmatch2nd", "asdf");
      var numeric = new NumericRule("numeric", "asdf", _culture);
      var phone = new PhoneRule("asdf", "phone", culture: _culture);
      var regex = new RegexRule(_culture, "reggy", "asdf", new Regex(@"^\S+@\S+\.\S+$"));
      var required = new RequiredRule("required", "", _culture);
      var xss = new XssRule("xss", "<script alert('hi');</script>", _culture);

      var emailRule = new EmailRule("fasdf", culture: _culture);
      var zipRuleUs = new ZipRule("234", "us", "notstate", culture: _culture);
      var zipRuleOther = new ZipRule("1234", "aj", "notstate", culture: _culture);
      var lastName = new LastNameRule(_overMaxLength, culture: _culture);
      var firstName = new FirstNameRule(_overMaxLength, culture: _culture);
      var address1 = new Address1Rule(_overMaxLength, culture: _culture);
      var address2 = new Address2Rule(_overMaxLength, culture: _culture);
      var stateRule = new StateRule(_overMaxLength, culture: _culture);
      var countryRule = new CountryRule(_overMaxLength, culture: _culture);
      var birthday = new BirthDayRule("234", "23423", culture: _culture);
      var callInPin = new CallInPinRule("12341234", culture: _culture);
      var confirm = new PasswordConfirmRule("asdfaw", "39r", culture: _culture);
      var cityRule = new CityRule(_invalidChars, culture: _culture);
      var hint = new PasswordHintRule(_overMaxLength, culture: _culture);
      var username = new UsernameRule("syukna", true, culture: _culture);
      var username2 = new UsernameRule("asdf-3f089nasd0fnas0df9n309fna0sdfnasd0f9asdfn0493nfasdf", culture: _culture);
      var phoneExt = new PhoneExtRule("3093093n90ansdf09asn0f9n309fn039anf09anw09asndf0asnf0934nf0a93n0a93fna09nf03n", culture: _culture);
      var workInvalidStart = new AnyPhoneRule(_workPhoneInvalidStart, true, culture: _culture);
      var workPhoneInvalidChars = new AnyPhoneRule(_workPhoneInvalidChars, true, culture: _culture);
      var workPhoneValid = new AnyPhoneRule(_workPhoneValid, true, culture: _culture);
      var workPhoneUsFail = new AnyPhoneRule(_workPhoneValidIntl, true, culture: _culture);
      var workPhoneIntl = new AnyPhoneRule(_workPhoneValidIntl, true, "aj", culture: _culture);
      var workPhoneMissing = new AnyPhoneRule(string.Empty, true, culture: _culture);
      var workPhoneMissingValid = new AnyPhoneRule("work phone missing valid", culture: _culture);
      var passwordruleBlacklist = new PasswordRule("Password1234", false, culture: _culture);
      var passwordruleNoCapital = new PasswordRule("thisisnotvalid1234", false, culture: _culture);
      var passwordruleNoNumber = new PasswordRule("Thisisnotvalid", false, culture: _culture);
      var passwordruleMinLength = new PasswordRule("short", false, culture: _culture);
      var passwordruleNotMatchCurrentHint = new PasswordRule("Seth456sethseth", false, "867900", culture: _culture);
      var passwordRuleNotMatchNewHint = new PasswordRule("shouldstopbeforeothervalidations", false, string.Empty, "shouldstopbeforeothervalidations", culture: _culture);

      var shopper = new ShopperToValidate("www.requesturl.com", "pathway", 0);
      shopper.FirstName.Value = firstName.Value;
      shopper.LastName.Value = lastName.Value;
      shopper.Address1.Value = address1.Value;
      shopper.Address2.Value = address2.Value;
      shopper.CallInPin.Value = callInPin.Value;
      shopper.City.Value = cityRule.Value;
      shopper.Country.Value = countryRule.Value;
      shopper.Email.Value = emailRule.Value;
      shopper.Password.Value = passwordruleBlacklist.Value;
      shopper.PasswordConfirm.Value = confirm.Value;
      shopper.PasswordHint.Value = hint.Value;
      shopper.PhoneHome.Value = workPhoneInvalidChars.Value;
      shopper.PhoneMobile.Value = workPhoneInvalidChars.Value;
      shopper.PhoneMobileSurvey.Value = workPhoneInvalidChars.Value;
      shopper.PhoneWork.Value = workPhoneInvalidChars.Value;
      shopper.PhoneWorkExtension.Value = phoneExt.Value;
      shopper.State.Value = stateRule.Value;
      shopper.BirthMonth.Value = "sdasfasdf";
      shopper.BirthDay.Value = "8923";
      shopper.AccountUsageType.Value = "081";
      shopper.Username.Value = username.Value;
      shopper.Zip.Value = _invalidChars;

      ProcessBaseRules(blankRule, equalRule, invalidChar, match, maxLength, minLength, notMatch, numeric, phone, regex,
                  required, xss);

      ProcessRuleContainers(emailRule, zipRuleUs, zipRuleOther, lastName, firstName, address1, address2, stateRule, countryRule,
        birthday, callInPin, confirm, hint, username, username2, phoneExt, cityRule,
        workInvalidStart, workPhoneInvalidChars, workPhoneValid, workPhoneUsFail, workPhoneIntl, workPhoneMissing, workPhoneMissingValid,
        passwordruleBlacklist, passwordruleNoCapital, passwordruleNoNumber, passwordruleMinLength, passwordruleNotMatchCurrentHint,
        passwordRuleNotMatchNewHint);

      ProcessRequestData(shopper);


      string p = "pause";
    }

    [TestMethod]
    public void TestCallInPinWithStringThatPassesIntTryParse()
    {
      var pinRule = new CallInPinRule(" 122");
      pinRule.Validate();
      Assert.IsFalse(pinRule.IsValid);
    }

    [TestMethod]
    public void TestCallInPinValid()
    {
      var pinRule = new CallInPinRule("9122");
      pinRule.Validate();
      Assert.IsTrue(pinRule.IsValid);
    }

    [TestMethod]
    public void TestCallInPinSequence()
    {
      var pinRule = new CallInPinRule("1111");
      pinRule.Validate();
      Assert.IsFalse(pinRule.IsValid);

      var pinRule2 = new CallInPinRule("1234");
      pinRule2.Validate();
      Assert.IsFalse(pinRule2.IsValid);
    }

    [TestMethod]
    public void TestCallInPinLength()
    {
      var pinRule = new CallInPinRule("92");
      pinRule.Validate();
      Assert.IsFalse(pinRule.IsValid);

      var pinRule2 = new CallInPinRule("900002");
      pinRule2.Validate();
      Assert.IsFalse(pinRule2.IsValid);
    }

    private void ProcessBaseRules(params ValidationRule[] vRules)
    {
      foreach (ValidationRule rule in vRules)
      {
        rule.Validate();
        WriteMessage(rule.IsValid, rule.ErrorMessage);
        bool errorMessageBlank = string.IsNullOrEmpty(rule.ErrorMessage);
        Assert.IsFalse(errorMessageBlank);
      }
    }

    private void ProcessRuleContainers(params RuleContainer[] ruleContainers)
    {
      foreach (RuleContainer rule in ruleContainers)
      {
        rule.Validate();
        WriteMessage(rule.IsValid, rule.ErrorMessage, rule.Value);
        bool messageIsMissing = !rule.IsValid && string.IsNullOrEmpty(rule.ErrorMessage);
        Assert.IsFalse(messageIsMissing);
      }
    }

    private void ProcessRequestData(ShopperToValidate shopper)
    {
      Debug.WriteLine("");
      Debug.WriteLine(" ----  SHOPPER VALIDATOR -------");
      Debug.WriteLine("");
      shopper.InitStandardRequiredFields();
      var isNewShopper = true;
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, shopper, isNewShopper, _culture);
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      foreach (var prop in response.ValidatedShopper.AllShopperProperties)
      {
        if (prop.HasValidationRules)
        {
          WriteMessage(prop.RuleContainer.IsValid, prop.RuleContainer.ErrorMessage, prop.Value);
          bool messageIsMissing = !prop.RuleContainer.IsValid && string.IsNullOrEmpty(prop.RuleContainer.ErrorMessage);
          Assert.IsFalse(messageIsMissing);
        }
      }
    }

    private void WriteMessage(bool isvalid, string errormessage, string value = "")
    {
      Debug.WriteLine("IsValid: " + isvalid.ToString() + "   ErrorMessage: " + errormessage + "   [Value: " + value.PadRight(50).Substring(0, 49));
    }
  }
}
