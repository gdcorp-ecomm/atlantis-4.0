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
      ZipRule zipRuleUs = new ZipRule(_zip, "us");
      ZipRule zipRuleOther = new ZipRule(_zip, "aj");
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
      PasswordRule passwordruleMaxLength = new PasswordRule(_overMaxLength, false);
      PasswordRule passwordruleNotMatchCurrentHint = new PasswordRule("Seth456sethseth", false, "867900");

      BlankRule mybr = new BlankRule(false, "error");

      PasswordRule passwordRuleNotMatchNewHint = new PasswordRule("shouldstopbeforeothervalidations", false, string.Empty, "shouldstopbeforeothervalidations");

      AddAllRules(emailRule, zipRuleUs, zipRuleOther, lastName, firstName, address1, address2, stateRule,
        workInvalidStart, workPhoneInvalidChars, workPhoneValid, workPhoneUsFail, workPhoneIntl, workPhoneMissing, workPhoneMissingValid,
        passwordruleBlacklist, passwordruleNoCapital, passwordruleNoNumber, passwordruleMinLength, passwordruleMaxLength, passwordruleNotMatchCurrentHint,
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
      ZipRule zr = new ZipRule("", "hk");
      zr.Validate();
      CallInPinRule cipr = new CallInPinRule("1234");
      cipr.Validate();
      UsernameRule unr = new UsernameRule("myusername");
      unr.Validate();

      PasswordRule pwRule = new PasswordRule(_overMaxLength, true, "jasdfj29nask3", "asdf");
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
      shopper.Country.Value = "us"; 
      shopper.Email.Value = "seth";
      shopper.Password.Value = _overMaxLength;
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
      shopper.Zip.Value = "j";
      shopper.BirthDay.Value = "23";


      bool isNewShopper = true;
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, shopper, isNewShopper);
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      string xu = response.GetException().Message;
      foreach (ShopperProperty prop in response.ValidatedShopper.AllShopperProperties)
      {
        if (prop.HasValidationRules && !prop.RuleContainer.IsValid)
        {
          Debug.WriteLine("IsValid: " + prop.RuleContainer.IsValid.ToString() + "   ErrorMessage: " + prop.RuleContainer.ErrorMessage);
        }
      }
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
  }
}
