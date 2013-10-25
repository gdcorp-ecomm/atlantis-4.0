using System.Collections.Generic;
using Atlantis.Framework.RuleEngine.Results;
using Atlantis.Framework.ShopperValidator.Interface;
using Atlantis.Framework.ShopperValidator.Interface.RuleConstants;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.ShopperValidator.Test
{
  // ReSharper disable InconsistentNaming

  [TestClass]
  [DeploymentItem("Atlantis.Framework.ShopperValidator.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.SearchShoppers.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.ValidateField.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.AuthValidatePassword.Impl.dll")]
  public class RuleEngineUnitTests
  {
    private const string _CULTURE = "en-US";

    private IModelResult _model;
    private IModelResult Model
    {
      get
      {
        if (_model == null)
        {
          _model = new ModelResult();
        }
        return _model;
      }
      set { _model = value; }
    }

    private IList<string> _expectedInvalidFacts;
    private IList<string> ExpectedInvalidFacts
    {
      get
      {
        if (_expectedInvalidFacts == null)
        {
          _expectedInvalidFacts = new List<string>(0);
        }
        return _expectedInvalidFacts;
      }
      set { _expectedInvalidFacts = value; }
    }

    private IDictionary<string, IList<string>> _expectedErrorMessages;
    private IDictionary<string, IList<string>> ExpectedErrorMessages
    {
      get
      {
        if (_expectedErrorMessages == null)
        {
          _expectedErrorMessages = new Dictionary<string, IList<string>>(0);
        }
        return _expectedErrorMessages;
      }
      set { _expectedErrorMessages = value; }
    }

    private ShopperValidatorResponseData _response;
    private ShopperValidatorResponseData Response
    {
      get
      {
        if (_response == null)
        {
          _response = new ShopperValidatorResponseData(new RuleEngineResult(RuleEngineResultStatus.Invalid));
        }
        return _response;
      }
      set { _response = value; }
    }

    [TestInitialize]
    public void Initialize()
    {
      Response = null;
      Model = null;
      ExpectedInvalidFacts = null;
      ExpectedErrorMessages = null;
    }

    [TestCleanup]
    public void AssertAllTheThings()
    {

      foreach (var fact in Model.Facts)
      {
        if (ExpectedInvalidFacts.Contains(fact.FactKey))
        {
          Assert.IsTrue(fact.Status == ValidationResultStatus.InValid);
        }

        if (fact.Messages.Count > 0)
        {
          Assert.IsTrue(ExpectedErrorMessages.ContainsKey(fact.FactKey));
          foreach (var errorMessage in fact.Messages)
          {
            Assert.IsTrue(ExpectedErrorMessages[fact.FactKey].Contains(errorMessage));
          }
        }
      }
    }

    public void ValidateShopper(string username, string password, string password2, string pin, string email, string culture = _CULTURE)
    {
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, username, password, password2, pin, email, culture);
      Response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Model = Response.ValidatedModel;
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestRuleEngineSlimShopperValid()
    {
      ValidateShopper("username", "P4ssW0rd!", "P4ssW0rd!", "1223", "email@email.com", _CULTURE);
    }

    #region Rule Engine Test Methods
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREGoodShopper()
    {
      ValidateShopper("username", "P4ssW0rd!", "P4ssW0rd!", "1223", "email@email.com", _CULTURE);
    }

    #region Username Tests (count = 5)
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadUsernameEmpty()
    {
      ValidateShopper(string.Empty, "P4ssW0rd!", "P4ssW0rd!", "1223", "email@email.com", _CULTURE);

      ExpectedInvalidFacts.Add(ModelConstants.FACT_USERNAME);
      ExpectedErrorMessages[ModelConstants.FACT_USERNAME] = new[] { ValidationErrorMessages.Error_Username_Required_en };
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadUsernameNull()
    {
      ValidateShopper(null, "P4ssW0rd!", "P4ssW0rd!", "1223", "email@email.com", _CULTURE);

      ExpectedInvalidFacts.Add(ModelConstants.FACT_USERNAME);
      ExpectedErrorMessages[ModelConstants.FACT_USERNAME] = new[] { ValidationErrorMessages.Error_Username_Required_en };
    }
    
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadUsernameMaxLength()
    {
      var longUsername = "";
      while (longUsername.Length < LengthConstants.UsernameMaxLength + 1)
      {
        longUsername += "A";
      }

      ValidateShopper(longUsername, "P4ssW0rd!", "P4ssW0rd!", "1223", "email@email.com", _CULTURE);

      ExpectedInvalidFacts.Add(ModelConstants.FACT_USERNAME);
      ExpectedErrorMessages[ModelConstants.FACT_USERNAME] = new[] { ValidationErrorMessages.Error_Username_MaxLength_en };
    }
    
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadUsernameAllNumeric()
    {
      ValidateShopper("123456789", "P4ssW0rd!", "P4ssW0rd!", "1223", "email@email.com", _CULTURE);

      ExpectedInvalidFacts.Add(ModelConstants.FACT_USERNAME);
      ExpectedErrorMessages[ModelConstants.FACT_USERNAME] = new[] { ValidationErrorMessages.Error_Username_NonNumericOnly_en };
    }
    
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadUsernameInvalidChars()
    {
      ValidateShopper("user;name", "P4ssW0rd!", "P4ssW0rd!", "1223", "email@email.com", _CULTURE);
      
      ExpectedInvalidFacts.Add(ModelConstants.FACT_USERNAME);
      ExpectedErrorMessages[ModelConstants.FACT_USERNAME] = new[] { ValidationErrorMessages.Error_Username_InvalidChars_en };
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadUsernameDuplicate()
    {
      ValidateShopper("devgdalan09", "P4ssW0rd!", "P4ssW0rd!", "1223", "email@email.com", _CULTURE);

      ExpectedInvalidFacts.Add(ModelConstants.FACT_USERNAME);
      ExpectedErrorMessages[ModelConstants.FACT_USERNAME] = new[] { ValidationErrorMessages.Error_Username_Exists_en };
    }
    #endregion

    
    #region Password Tests (count = 12)
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBothBadPasswordsEmpty()
    {
      ValidateShopper("username", string.Empty, string.Empty, "1223", "email@email.com", _CULTURE);

      ExpectedInvalidFacts.Add(ModelConstants.FACT_PASSWORD);
      ExpectedInvalidFacts.Add(ModelConstants.FACT_PASSWORD2);
      ExpectedErrorMessages[ModelConstants.FACT_PASSWORD] = new[] { ValidationErrorMessages.Error_Password_Required_en, ValidationErrorMessages.Error_Password_MinLength_en, ValidationErrorMessages.Error_Password_InvalidFormat_en };
      ExpectedErrorMessages[ModelConstants.FACT_PASSWORD2] = new[] { ValidationErrorMessages.Error_Password2_Required_en };
    }
    
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBothBadPasswordsNull()
    {
      ValidateShopper("username", null, null, "1223", "email@email.com", _CULTURE);

      ExpectedInvalidFacts.Add(ModelConstants.FACT_PASSWORD);
      ExpectedInvalidFacts.Add(ModelConstants.FACT_PASSWORD2);
      ExpectedErrorMessages[ModelConstants.FACT_PASSWORD] = new[] { ValidationErrorMessages.Error_Password_Required_en, ValidationErrorMessages.Error_Password_MinLength_en, ValidationErrorMessages.Error_Password_InvalidFormat_en };
      ExpectedErrorMessages[ModelConstants.FACT_PASSWORD2] = new[] { ValidationErrorMessages.Error_Password2_Required_en };
    }
    
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadPasswordEmpty()
    {
      ValidateShopper("username", string.Empty, "P4ssW0rd!", "1223", "email@email.com", _CULTURE);

      ExpectedInvalidFacts.Add(ModelConstants.FACT_PASSWORD);
      ExpectedInvalidFacts.Add(ModelConstants.FACT_PASSWORD2);
      ExpectedErrorMessages[ModelConstants.FACT_PASSWORD] = new[] { ValidationErrorMessages.Error_Password_Required_en, ValidationErrorMessages.Error_Password_MinLength_en, ValidationErrorMessages.Error_Password_InvalidFormat_en };
      ExpectedErrorMessages[ModelConstants.FACT_PASSWORD2] = new[] { ValidationErrorMessages.Error_Password2_Mismatch_en };
    }
    
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadPassword2Empty()
    {
      ValidateShopper("username", "P4ssW0rd!", string.Empty, "1223", "email@email.com", _CULTURE);

      ExpectedInvalidFacts.Add(ModelConstants.FACT_PASSWORD2);
      ExpectedErrorMessages[ModelConstants.FACT_PASSWORD2] = new[] { ValidationErrorMessages.Error_Password2_Required_en, ValidationErrorMessages.Error_Password2_Mismatch_en };
    }
    
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadPasswordNull()
    {
      ValidateShopper("username", null, "P4ssW0rd!", "1223", "email@email.com", _CULTURE);

      ExpectedInvalidFacts.Add(ModelConstants.FACT_PASSWORD);
      ExpectedInvalidFacts.Add(ModelConstants.FACT_PASSWORD2);
      ExpectedErrorMessages[ModelConstants.FACT_PASSWORD] = new[] { ValidationErrorMessages.Error_Password_Required_en, ValidationErrorMessages.Error_Password_MinLength_en, ValidationErrorMessages.Error_Password_InvalidFormat_en };
      ExpectedErrorMessages[ModelConstants.FACT_PASSWORD2] = new[] { ValidationErrorMessages.Error_Password2_Mismatch_en };
    }
    
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadPassword2Null()
    {
      ValidateShopper("username", "P4ssW0rd!", null, "1223", "email@email.com", _CULTURE);

      ExpectedInvalidFacts.Add(ModelConstants.FACT_PASSWORD2);
      ExpectedErrorMessages[ModelConstants.FACT_PASSWORD2] = new[] { ValidationErrorMessages.Error_Password2_Required_en, ValidationErrorMessages.Error_Password2_Mismatch_en };
    }
    
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadPasswordRegexNoNumbers()
    {
      ValidateShopper("username", "PassWord!", "PassWord!", "1223", "email@email.com", _CULTURE);

      ExpectedInvalidFacts.Add(ModelConstants.FACT_PASSWORD);
      ExpectedErrorMessages[ModelConstants.FACT_PASSWORD] = new[] { ValidationErrorMessages.Error_Password_InvalidFormat_en };
    }
    
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadPasswordRegexNoCaps()
    {
      ValidateShopper("username", "p4ssw0rd!", "p4ssw0rd!", "1223", "email@email.com", _CULTURE);

      ExpectedInvalidFacts.Add(ModelConstants.FACT_PASSWORD);
      ExpectedErrorMessages[ModelConstants.FACT_PASSWORD] = new[] { ValidationErrorMessages.Error_Password_InvalidFormat_en };
    }
    
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadPasswordRegexBeginningSpace()
    {
      ValidateShopper("username", " P4ssW0rd!", " P4ssW0rd!", "1223", "email@email.com", _CULTURE);

      ExpectedInvalidFacts.Add(ModelConstants.FACT_PASSWORD);
      ExpectedErrorMessages[ModelConstants.FACT_PASSWORD] = new[] { ValidationErrorMessages.Error_Password_InvalidFormat_en };
    }
    
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadPasswordMinLength()
    {
      var shortPassword = "P4s!";
      while (shortPassword.Length < LengthConstants.PasswordMinLength - 1)
      {
        shortPassword += "A";
      }

      ValidateShopper("username", shortPassword, shortPassword, "1223", "email@email.com", _CULTURE);

      ExpectedInvalidFacts.Add(ModelConstants.FACT_PASSWORD);
      ExpectedErrorMessages[ModelConstants.FACT_PASSWORD] = new[] { ValidationErrorMessages.Error_Password_MinLength_en };
    }
    
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadPasswordMaxLength()
    {
      var longPassword = "P4s!";
      while (longPassword.Length < LengthConstants.PasswordMaxLength + 1)
      {
        longPassword += "A";
      }

      ValidateShopper("username", longPassword, longPassword, "1223", "email@email.com", _CULTURE);

      ExpectedInvalidFacts.Add(ModelConstants.FACT_PASS_MAX_LENGTH);
      ExpectedErrorMessages[ModelConstants.FACT_PASS_MAX_LENGTH] = new[] { ValidationErrorMessages.Error_PasswordMaxLength_MaxLength_en };
    }
    
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadPasswordMatch()
    {
      ValidateShopper("username", "P4ssW0rd!", "P4ssW0rd!!", "1223", "email@email.com", _CULTURE);

      ExpectedInvalidFacts.Add(ModelConstants.FACT_PASSWORD2);
      ExpectedErrorMessages[ModelConstants.FACT_PASSWORD2] = new[] { ValidationErrorMessages.Error_Password2_Mismatch_en };
    }
    #endregion

    
    #region Email Tests (count = 4)
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadEmailEmpty()
    {
      ValidateShopper("username", "P4ssW0rd!", "P4ssW0rd!", "1223", string.Empty, _CULTURE);
      
      ExpectedInvalidFacts.Add(ModelConstants.FACT_EMAIL);
      ExpectedErrorMessages[ModelConstants.FACT_EMAIL] = new[] { ValidationErrorMessages.Error_Email_Required_en, ValidationErrorMessages.Error_Email_InvalidFormat_en };
    }
    
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadEmailNull()
    {
      ValidateShopper("username", "P4ssW0rd!", "P4ssW0rd!", "1223", null, _CULTURE);

      ExpectedInvalidFacts.Add(ModelConstants.FACT_EMAIL);
      ExpectedErrorMessages[ModelConstants.FACT_EMAIL] = new[] { ValidationErrorMessages.Error_Email_Required_en, ValidationErrorMessages.Error_Email_InvalidFormat_en };
    }
    
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadEmailMaxLength()
    {
      var longEmail = "a@x.";
      while (longEmail.Length < LengthConstants.EmailMaxLength + 1)
      {
        longEmail += "c";
      }

      ValidateShopper("username", "P4ssW0rd!", "P4ssW0rd!", "1223", longEmail, _CULTURE);

      ExpectedInvalidFacts.Add(ModelConstants.FACT_EMAIL);
      ExpectedErrorMessages[ModelConstants.FACT_EMAIL] = new[] { ValidationErrorMessages.Error_Email_MaxLength_en, ValidationErrorMessages.Error_Email_InvalidFormat_en };
    }
    
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadEmailRegex()
    {
      ValidateShopper("username", "P4ssW0rd!", "P4ssW0rd!", "1223", "email@emailcom", _CULTURE);

      ExpectedInvalidFacts.Add(ModelConstants.FACT_EMAIL);
      ExpectedErrorMessages[ModelConstants.FACT_EMAIL] = new[] { ValidationErrorMessages.Error_Email_InvalidFormat_en };
    }
    #endregion

    #region PIN Tests (count = 4)
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadPinEmpty()
    {
      ValidateShopper("username", "P4ssW0rd!", "P4ssW0rd!", string.Empty, "email@email.com", _CULTURE);

      ExpectedInvalidFacts.Add(ModelConstants.FACT_PIN);
      ExpectedErrorMessages[ModelConstants.FACT_PIN] = new[] { ValidationErrorMessages.Error_PIN_Required_en };
    }
    
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadPinNull()
    {
      ValidateShopper("username", "P4ssW0rd!", "P4ssW0rd!", null, "email@email.com", _CULTURE);

      ExpectedInvalidFacts.Add(ModelConstants.FACT_PIN);
      ExpectedErrorMessages[ModelConstants.FACT_PIN] = new[] { ValidationErrorMessages.Error_PIN_Required_en };
    }
    
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadPinMaxLength()
    {
      ValidateShopper("username", "P4ssW0rd!", "P4ssW0rd!", "12235", "email@email.com", _CULTURE);

      ExpectedInvalidFacts.Add(ModelConstants.FACT_PIN);
      ExpectedErrorMessages[ModelConstants.FACT_PIN] = new[] { ValidationErrorMessages.Error_PIN_MaxLength_en };
    }
    
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadPinMinLength()
    {
      ValidateShopper("username", "P4ssW0rd!", "P4ssW0rd!", "122", "email@email.com", _CULTURE);

      ExpectedInvalidFacts.Add(ModelConstants.FACT_PIN);
      ExpectedErrorMessages[ModelConstants.FACT_PIN] = new[] { ValidationErrorMessages.Error_PIN_MinLength_en };
    }
    
    #endregion

    #region Error Message Validation

    protected static class ValidationErrorMessages
    {
      public static string Error_Username_Exists_en
      {
        get { return "Username already exists"; }
      }

      public static string Error_Username_Required_en
      {
        get { return "Username is required."; }
      }

      public static string Error_Username_MaxLength_en
      {
        get { return string.Format("Username cannot be longer than {0} characters long.", LengthConstants.UsernameMaxLength); }
      }

      public static string Error_Username_InvalidChars_en
      {
        get { return "Username contains invalid characters."; }
      }

      public static string Error_Username_NonNumericOnly_en
      {
        get { return "Username cannot be only numbers."; }
      }

      public static string Error_PasswordMaxLength_MaxLength_en
      {
        get { return string.Format("Password cannot be longer than {0} characters long.", LengthConstants.PasswordMaxLength); }
      }

      public static string Error_Password_Required_en
      {
        get { return "Password is required."; }
      }

      public static string Error_Password_MinLength_en
      {
        get { return string.Format("Password must be at least {0} characters long.", LengthConstants.PasswordMinLength); }
      }

      public static string Error_Password_InvalidFormat_en
      {
        get { return "Password must contain at least 1 uppercase letter, 1 number, and cannot begin or end with a space."; }
      }

      public static string Error_Password2_Required_en
      {
        get { return "Confirm Password is required."; }
      }

      public static string Error_Password2_Mismatch_en
      {
        get { return "Passwords must match."; }
      }

      public static string Error_PIN_Required_en
      {
        get { return "PIN is required."; }
      }

      public static string Error_PIN_MaxLength_en
      {
        get { return string.Format("PIN cannot be longer than {0} characters long.", LengthConstants.CallInPinMaxLength); }
      }

      public static string Error_PIN_MinLength_en
      {
        get { return string.Format("PIN must be at least {0} characters long.", LengthConstants.CallInPinMinLength); }
      }

      public static string Error_PIN_NumericOnly_en
      {
        get { return "PIN must contain only numbers."; }
      }

      public static string Error_PIN_Sequential_en
      {
        get { return "PIN cannot be any straight numerical sequence of digits."; }
      }

      public static string Error_Email_Required_en
      {
        get { return "Email is required."; }
      }

      public static string Error_Email_MaxLength_en
      {
        get { return string.Format("Email cannot be longer than {0} characters long.", LengthConstants.EmailMaxLength); }
      }

      public static string Error_Email_InvalidFormat_en
      {
        get { return "Email is an invalid format."; }
      }
    }

    #endregion

    #endregion
    // ReSharper restore InconsistentNaming
  }
}
