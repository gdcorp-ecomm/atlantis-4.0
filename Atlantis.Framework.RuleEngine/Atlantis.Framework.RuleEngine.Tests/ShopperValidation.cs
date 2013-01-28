using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Atlantis.Framework.RuleEngine.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.RuleEngine.Tests
{

  [TestClass]
  public class ShopperValidation
  {
    #region Constants
    private const string MODEL_ID_SHOPPERVALID = "Shopper";

    private const string FACT_FIRST_NAME = "txtFirstName";
    private const string FACT_LAST_NAME = "txtLastName";
    private const string FACT_USERNAME = "txtUsername";
    private const string FACT_PASSWORD = "txtCreatePassword";
    private const string FACT_PASSWORD2 = "txtCreatePassword2";
    private const string FACT_PIN = "txtPin";
    private const string FACT_EMAIL = "txtEmail";

    private const string FACT_FIRST_NAME_MAX_LENGTH = "firstNameMaxLength";
    private const string FACT_LAST_NAME_MAX_LENGTH = "lastNameMaxLength";
    private const string FACT_USERNAME_MAX_LENGTH = "usernameMaxLength";
    private const string FACT_PASS_MAX_LENGTH = "passwordMaxLength";
    private const string FACT_PASS_MIN_LENGTH = "passwordMinLength";
    private const string FACT_PASS_REGEX = "passwordRegex";
    private const string FACT_PIN_MAX_LENGTH = "pinMaxLength";
    private const string FACT_PIN_MIN_LENGTH = "pinMinLength";
    private const string FACT_EMAIL_REGEX = "emailRegex";
    private const string FACT_EMAIL_MAX_LENGTH = "emailMaxLength";
    private const string FACT_NUMERIC_ONLY_REGEX = "numericOnlyRegex";
    private const string FACT_INVALID_CHARACTERS_REGEX = "invalidCharsRegex";
    #endregion

    #region Supplemental Properties

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

    #region Supplemental Methods
    Dictionary<string, Dictionary<string, string>> BuildModel(string firstName, string lastName,
      string userName, string password, string password2, string pin, string email)
    {
      var model = new Dictionary<string, Dictionary<string, string>>();

      model.Add("Shopper", new Dictionary<string, string>
                            {
                              { FACT_FIRST_NAME, firstName },
                              { FACT_LAST_NAME, lastName },
                              { FACT_USERNAME, userName },
                              { FACT_PASSWORD, password },
                              { FACT_PASSWORD2, password2 },
                              { FACT_PIN, pin },
                              { FACT_EMAIL, email },
                              { FACT_FIRST_NAME_MAX_LENGTH, FirstNameMaxLength},
                              { FACT_LAST_NAME_MAX_LENGTH, LastNameMaxLength},
                              { FACT_USERNAME_MAX_LENGTH, UsernameMaxLength},
                              { FACT_PASS_MAX_LENGTH, PasswordMaxLength },
                              { FACT_PASS_MIN_LENGTH, PasswordMinLength },
                              { FACT_PASS_REGEX, PasswordRegex },
                              { FACT_PIN_MAX_LENGTH, CallInPinMaxLength },
                              { FACT_PIN_MIN_LENGTH, CallInPinMinLength },
                              { FACT_EMAIL_REGEX, EmailRegex },
                              { FACT_EMAIL_MAX_LENGTH, EmailMaxLength },
                              { FACT_NUMERIC_ONLY_REGEX, NumericOnlyRegex },
                              { FACT_INVALID_CHARACTERS_REGEX, InvalidCharactersRegex }
                            });

      return model;
    }
    #endregion

    #region Test Methods
    [TestMethod]
    [DeploymentItem("ShopperValidation.xml")]
    public void TestGoodShopper()
    {
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"ShopperValidation.xml");
      
      var rules = new XmlDocument();
      rules.Load(directory);
      var model = BuildModel("firstname", "lastname", "username", "P4ssW0rd!", "P4ssW0rd!", "1254", "email@email.com");
      var engineResult = RuleEngine.EvaluateRules(model, rules);
      var modelResults = engineResult.ValidationResults;

      EvaluateValid(MODEL_ID_SHOPPERVALID, engineResult, modelResults);
    }

    #region First Name Tests
    [TestMethod]
    [DeploymentItem("ShopperValidation.xml")]
    public void TestBadFirstNameEmpty()
    {
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"ShopperValidation.xml");

      var rules = new XmlDocument();
      rules.Load(directory);

      var model = BuildModel(string.Empty, "lastname", "username", "P4ssW0rd!", "P4ssW0rd!", "1254", "email@email.com");
      var engineResult = RuleEngine.EvaluateRules(model, rules);
      var modelResults = engineResult.ValidationResults;

      EvaluateValid(MODEL_ID_SHOPPERVALID, engineResult, modelResults, FACT_FIRST_NAME);
    }

    [TestMethod]
    [DeploymentItem("ShopperValidation.xml")]
    public void TestBadFirstNameNull()
    {
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"ShopperValidation.xml");

      var rules = new XmlDocument();
      rules.Load(directory);

      var model = BuildModel(null, "lastname", "username", "P4ssW0rd!", "P4ssW0rd!", "1254", "email@email.com");
      var engineResult = RuleEngine.EvaluateRules(model, rules);
      var modelResults = engineResult.ValidationResults;

      EvaluateValid(MODEL_ID_SHOPPERVALID, engineResult, modelResults, FACT_FIRST_NAME);
    }

    [TestMethod]
    [DeploymentItem("ShopperValidation.xml")]
    public void TestBadFirstNameMaxLength()
    {
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"ShopperValidation.xml");

      var rules = new XmlDocument();
      rules.Load(directory);

      var longFirstName = "";
      while (longFirstName.Length < _firstNameMaxLength + 1)
      {
        longFirstName += "A";
      }

      var model = BuildModel(longFirstName, "lastname", "username", "P4ssW0rd!", "P4ssW0rd!", "1254", "email@email.com");
      var engineResult = RuleEngine.EvaluateRules(model, rules);
      var modelResults = engineResult.ValidationResults;

      EvaluateValid(MODEL_ID_SHOPPERVALID, engineResult, modelResults, FACT_FIRST_NAME);
    }

    [TestMethod]
    [DeploymentItem("ShopperValidation.xml")]
    public void TestBadFirstNameInvalidChars()
    {
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"ShopperValidation.xml");

      var rules = new XmlDocument();
      rules.Load(directory);

      var model = BuildModel("first;name", "lastname", "username", "P4ssW0rd!", "P4ssW0rd!", "1254", "email@email.com");
      var engineResult = RuleEngine.EvaluateRules(model, rules);
      var modelResults = engineResult.ValidationResults;

      EvaluateValid(MODEL_ID_SHOPPERVALID, engineResult, modelResults, FACT_FIRST_NAME);
    }
    #endregion

    #region Last Name Tests
    [TestMethod]
    [DeploymentItem("ShopperValidation.xml")]
    public void TestBadLastNameEmpty()
    {
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"ShopperValidation.xml");

      var rules = new XmlDocument();
      rules.Load(directory);

      var model = BuildModel("firstname", string.Empty, "username", "P4ssW0rd!", "P4ssW0rd!", "1254", "email@email.com");
      var engineResult = RuleEngine.EvaluateRules(model, rules);
      var modelResults = engineResult.ValidationResults;

      EvaluateValid(MODEL_ID_SHOPPERVALID, engineResult, modelResults, FACT_LAST_NAME);
    }

    [TestMethod]
    [DeploymentItem("ShopperValidation.xml")]
    public void TestBadLastNameNull()
    {
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"ShopperValidation.xml");

      var rules = new XmlDocument();
      rules.Load(directory);

      var model = BuildModel("firstname", null, "username", "P4ssW0rd!", "P4ssW0rd!", "1254", "email@email.com");
      var engineResult = RuleEngine.EvaluateRules(model, rules);
      var modelResults = engineResult.ValidationResults;

      EvaluateValid(MODEL_ID_SHOPPERVALID, engineResult, modelResults, FACT_LAST_NAME);
    }

    [TestMethod]
    [DeploymentItem("ShopperValidation.xml")]
    public void TestBadLastNameMaxLength()
    {
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"ShopperValidation.xml");

      var rules = new XmlDocument();
      rules.Load(directory);

      var longLastName = "";
      while (longLastName.Length < _lastNameMaxLength + 1)
      {
        longLastName += "A";
      }

      var model = BuildModel("firstname", longLastName, "username", "P4ssW0rd!", "P4ssW0rd!", "1254", "email@email.com");
      var engineResult = RuleEngine.EvaluateRules(model, rules);
      var modelResults = engineResult.ValidationResults;

      EvaluateValid(MODEL_ID_SHOPPERVALID, engineResult, modelResults, FACT_LAST_NAME);
    }

    [TestMethod]
    [DeploymentItem("ShopperValidation.xml")]
    public void TestBadLastNameInvalidChars()
    {
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"ShopperValidation.xml");

      var rules = new XmlDocument();
      rules.Load(directory);

      var model = BuildModel("firstname", "last;name", "username", "P4ssW0rd!", "P4ssW0rd!", "1254", "email@email.com");
      var engineResult = RuleEngine.EvaluateRules(model, rules);
      var modelResults = engineResult.ValidationResults;

      EvaluateValid(MODEL_ID_SHOPPERVALID, engineResult, modelResults, FACT_LAST_NAME);
    }
    #endregion

    #region Username Tests
    [TestMethod]
    [DeploymentItem("ShopperValidation.xml")]
    public void TestBadUsernameEmpty()
    {
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"ShopperValidation.xml");

      var rules = new XmlDocument();
      rules.Load(directory);

      var model = BuildModel("firstname", "lastname", string.Empty, "P4ssW0rd!", "P4ssW0rd!", "1254", "email@email.com");
      var engineResult = RuleEngine.EvaluateRules(model, rules);
      var modelResults = engineResult.ValidationResults;

      EvaluateValid(MODEL_ID_SHOPPERVALID, engineResult, modelResults, FACT_USERNAME);
    }

    [TestMethod]
    [DeploymentItem("ShopperValidation.xml")]
    public void TestBadUsernameNull()
    {
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"ShopperValidation.xml");

      var rules = new XmlDocument();
      rules.Load(directory);

      var model = BuildModel("firstname", "lastname", null, "P4ssW0rd!", "P4ssW0rd!", "1254", "email@email.com");
      var engineResult = RuleEngine.EvaluateRules(model, rules);
      var modelResults = engineResult.ValidationResults;

      EvaluateValid(MODEL_ID_SHOPPERVALID, engineResult, modelResults, FACT_USERNAME);
    }

    [TestMethod]
    [DeploymentItem("ShopperValidation.xml")]
    public void TestBadUsernameMaxLength()
    {
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"ShopperValidation.xml");

      var rules = new XmlDocument();
      rules.Load(directory);

      var longUsername = "";
      while (longUsername.Length < _usernameMaxLength + 1)
      {
        longUsername += "A";
      }

      var model = BuildModel("firstname", "lastname", longUsername, "P4ssW0rd!", "P4ssW0rd!", "1254", "email@email.com");
      var engineResult = RuleEngine.EvaluateRules(model, rules);
      var modelResults = engineResult.ValidationResults;

      EvaluateValid(MODEL_ID_SHOPPERVALID, engineResult, modelResults, FACT_USERNAME);
    }

    [TestMethod]
    [DeploymentItem("ShopperValidation.xml")]
    public void TestBadUsernameAllNumeric()
    {
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"ShopperValidation.xml");

      var rules = new XmlDocument();
      rules.Load(directory);

      var model = BuildModel("firstname", "lastname", "123456789", "P4ssW0rd!", "P4ssW0rd!", "1254", "email@email.com");
      var engineResult = RuleEngine.EvaluateRules(model, rules);
      var modelResults = engineResult.ValidationResults;

      EvaluateValid(MODEL_ID_SHOPPERVALID, engineResult, modelResults, FACT_USERNAME);
    }

    [TestMethod]
    [DeploymentItem("ShopperValidation.xml")]
    public void TestBadUsernameInvalidChars()
    {
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"ShopperValidation.xml");

      var rules = new XmlDocument();
      rules.Load(directory);

      var model = BuildModel("firstname", "lastname", "user;name", "P4ssW0rd!", "P4ssW0rd!", "1254", "email@email.com");
      var engineResult = RuleEngine.EvaluateRules(model, rules);
      var modelResults = engineResult.ValidationResults;

      EvaluateValid(MODEL_ID_SHOPPERVALID, engineResult, modelResults, FACT_USERNAME);
    }
    #endregion

    #region Password Tests
    [TestMethod]
    [DeploymentItem("ShopperValidation.xml")]
    public void TestBothBadPasswordsEmpty()
    {
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"ShopperValidation.xml");

      var rules = new XmlDocument();
      rules.Load(directory);
      var model = BuildModel("firstname", "lastname", "username", string.Empty, string.Empty, "1254", "email@email.com");
      var engineResult = RuleEngine.EvaluateRules(model, rules);
      var modelResults = engineResult.ValidationResults;

      EvaluateValid(MODEL_ID_SHOPPERVALID, engineResult, modelResults, FACT_PASSWORD, FACT_PASSWORD2);
    }

    [TestMethod]
    [DeploymentItem("ShopperValidation.xml")]
    public void TestBothBadPasswordsNull()
    {
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"ShopperValidation.xml");

      var rules = new XmlDocument();
      rules.Load(directory);
      var model = BuildModel("firstname", "lastname", "username", null, null, "1254", "email@email.com");
      var engineResult = RuleEngine.EvaluateRules(model, rules);
      var modelResults = engineResult.ValidationResults;

      EvaluateValid(MODEL_ID_SHOPPERVALID, engineResult, modelResults, FACT_PASSWORD, FACT_PASSWORD2);
    }

    [TestMethod]
    [DeploymentItem("ShopperValidation.xml")]
    public void TestBadPasswordEmpty()
    {
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"ShopperValidation.xml");

      var rules = new XmlDocument();
      rules.Load(directory);
      var model = BuildModel("firstname", "lastname", "username", string.Empty, "P4ssW0rd!", "1254", "email@email.com");
      var engineResult = RuleEngine.EvaluateRules(model, rules);
      var modelResults = engineResult.ValidationResults;

      EvaluateValid(MODEL_ID_SHOPPERVALID, engineResult, modelResults, FACT_PASSWORD, FACT_PASSWORD2);
    }

    [TestMethod]
    [DeploymentItem("ShopperValidation.xml")]
    public void TestBadPassword2Empty()
    {
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"ShopperValidation.xml");

      var rules = new XmlDocument();
      rules.Load(directory);
      var model = BuildModel("firstname", "lastname", "username", "P4ssW0rd!", string.Empty, "1254", "email@email.com");
      var engineResult = RuleEngine.EvaluateRules(model, rules);
      var modelResults = engineResult.ValidationResults;

      EvaluateValid(MODEL_ID_SHOPPERVALID, engineResult, modelResults, FACT_PASSWORD2);
    }

    [TestMethod]
    [DeploymentItem("ShopperValidation.xml")]
    public void TestBadPasswordNull()
    {
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"ShopperValidation.xml");

      var rules = new XmlDocument();
      rules.Load(directory);
      var model = BuildModel("firstname", "lastname", "username", null, "P4ssW0rd!", "1254", "email@email.com");
      var engineResult = RuleEngine.EvaluateRules(model, rules);
      var modelResults = engineResult.ValidationResults;

      EvaluateValid(MODEL_ID_SHOPPERVALID, engineResult, modelResults, FACT_PASSWORD, FACT_PASSWORD2);
    }

    [TestMethod]
    [DeploymentItem("ShopperValidation.xml")]
    public void TestBadPassword2Null()
    {
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"ShopperValidation.xml");

      var rules = new XmlDocument();
      rules.Load(directory);
      var model = BuildModel("firstname", "lastname", "username", "P4ssW0rd!", null, "1254", "email@email.com");
      var engineResult = RuleEngine.EvaluateRules(model, rules);
      var modelResults = engineResult.ValidationResults;

      EvaluateValid(MODEL_ID_SHOPPERVALID, engineResult, modelResults, FACT_PASSWORD2);
    }

    [TestMethod]
    [DeploymentItem("ShopperValidation.xml")]
    public void TestBadPasswordRegexNoNumbers()
    {
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"ShopperValidation.xml");

      var rules = new XmlDocument();
      rules.Load(directory);
      var model = BuildModel("firstname", "lastname", "username", "PassWord!", "PassWord!", "1254", "email@email.com");
      var engineResult = RuleEngine.EvaluateRules(model, rules);
      var modelResults = engineResult.ValidationResults;

      EvaluateValid(MODEL_ID_SHOPPERVALID, engineResult, modelResults, FACT_PASSWORD);
    }

    [TestMethod]
    [DeploymentItem("ShopperValidation.xml")]
    public void TestBadPasswordRegexNoCaps()
    {
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"ShopperValidation.xml");

      var rules = new XmlDocument();
      rules.Load(directory);
      var model = BuildModel("firstname", "lastname", "username", "p4ssw0rd!", "p4ssw0rd!", "1254", "email@email.com");
      var engineResult = RuleEngine.EvaluateRules(model, rules);
      var modelResults = engineResult.ValidationResults;

      EvaluateValid(MODEL_ID_SHOPPERVALID, engineResult, modelResults, FACT_PASSWORD);
    }

    [TestMethod]
    [DeploymentItem("ShopperValidation.xml")]
    public void TestBadPasswordRegexBeginningSpace()
    {
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"ShopperValidation.xml");

      var rules = new XmlDocument();
      rules.Load(directory);
      var model = BuildModel("firstname", "lastname", "username", " P4ssW0rd!", " P4ssW0rd!", "1254", "email@email.com");
      var engineResult = RuleEngine.EvaluateRules(model, rules);
      var modelResults = engineResult.ValidationResults;

      EvaluateValid(MODEL_ID_SHOPPERVALID, engineResult, modelResults, FACT_PASSWORD);
    }

    [TestMethod]
    [DeploymentItem("ShopperValidation.xml")]
    public void TestBadPasswordMinLength()
    {
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"ShopperValidation.xml");

      var rules = new XmlDocument();
      rules.Load(directory);

      var shortPassword = "P4s!";
      while (shortPassword.Length < _passwordMinLength-1)
      {
        shortPassword += "A";
      }

      var model = BuildModel("firstname", "lastname", "username", shortPassword, shortPassword, "1254", "email@email.com");
      var engineResult = RuleEngine.EvaluateRules(model, rules);
      var modelResults = engineResult.ValidationResults;

      EvaluateValid(MODEL_ID_SHOPPERVALID, engineResult, modelResults, FACT_PASSWORD);
    }

    [TestMethod]
    [DeploymentItem("ShopperValidation.xml")]
    public void TestBadPasswordMaxLength()
    {
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"ShopperValidation.xml");

      var rules = new XmlDocument();
      rules.Load(directory);

      var longPassword = "P4s!";
      while (longPassword.Length < _passwordMaxLength+1)
      {
        longPassword += "A";
      }

      var model = BuildModel("firstname", "lastname", "username", longPassword, longPassword, "1254", "email@email.com");
      var engineResult = RuleEngine.EvaluateRules(model, rules);
      var modelResults = engineResult.ValidationResults;

      EvaluateValid(MODEL_ID_SHOPPERVALID, engineResult, modelResults, FACT_PASS_MAX_LENGTH);
    }

    [TestMethod]
    [DeploymentItem("ShopperValidation.xml")]
    public void TestBadPasswordMatch()
    {
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"ShopperValidation.xml");

      var rules = new XmlDocument();
      rules.Load(directory);

      var model = BuildModel("firstname", "lastname", "username", "P4ssW0rd!", "P4ssW0rd!!", "1254", "email@email.com");
      var engineResult = RuleEngine.EvaluateRules(model, rules);
      var modelResults = engineResult.ValidationResults;

      EvaluateValid(MODEL_ID_SHOPPERVALID, engineResult, modelResults, FACT_PASSWORD2);
    }
    #endregion

    #region PIN Tests
    [TestMethod]
    [DeploymentItem("ShopperValidation.xml")]
    public void TestBadPINEmpty()
    {
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"ShopperValidation.xml");

      var rules = new XmlDocument();
      rules.Load(directory);
      var model = BuildModel("firstname", "lastname", "username", "P4ssW0rd!", "P4ssW0rd!", string.Empty, "email@email.com");
      var engineResult = RuleEngine.EvaluateRules(model, rules);
      var modelResults = engineResult.ValidationResults;

      EvaluateValid(MODEL_ID_SHOPPERVALID, engineResult, modelResults, FACT_PIN);
    }

    [TestMethod]
    [DeploymentItem("ShopperValidation.xml")]
    public void TestBadPINNull()
    {
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"ShopperValidation.xml");

      var rules = new XmlDocument();
      rules.Load(directory);
      var model = BuildModel("firstname", "lastname", "username", "P4ssW0rd!", "P4ssW0rd!", null, "email@email.com");
      var engineResult = RuleEngine.EvaluateRules(model, rules);
      var modelResults = engineResult.ValidationResults;

      EvaluateValid(MODEL_ID_SHOPPERVALID, engineResult, modelResults, FACT_PIN);
    }

    [TestMethod]
    [DeploymentItem("ShopperValidation.xml")]
    public void TestBadPINMinLength()
    {
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"ShopperValidation.xml");

      var rules = new XmlDocument();
      rules.Load(directory);

      var shortPIN = "7";
      while (shortPIN.Length < _callInPinMinLength - 1)
      {
        shortPIN += "1";
      }

      var model = BuildModel("firstname", "lastname", "username", "P4ssW0rd!", "P4ssW0rd!", shortPIN, "email@email.com");
      var engineResult = RuleEngine.EvaluateRules(model, rules);
      var modelResults = engineResult.ValidationResults;

      EvaluateValid(MODEL_ID_SHOPPERVALID, engineResult, modelResults, FACT_PIN);
    }

    [TestMethod]
    [DeploymentItem("ShopperValidation.xml")]
    public void TestBadPINMaxLength()
    {
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"ShopperValidation.xml");

      var rules = new XmlDocument();
      rules.Load(directory);

      var longPIN = "7";
      while (longPIN.Length < _callInPinMaxLength + 1)
      {
        longPIN += "1";
      }

      var model = BuildModel("firstname", "lastname", "username", "P4ssW0rd!", "P4ssW0rd!", longPIN, "email@email.com");
      var engineResult = RuleEngine.EvaluateRules(model, rules);
      var modelResults = engineResult.ValidationResults;

      EvaluateValid(MODEL_ID_SHOPPERVALID, engineResult, modelResults, FACT_PIN);
    }

    [TestMethod]
    [DeploymentItem("ShopperValidation.xml")]
    public void TestBadPINNotNumericOnly()
    {
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"ShopperValidation.xml");

      var rules = new XmlDocument();
      rules.Load(directory);

      var model = BuildModel("firstname", "lastname", "username", "P4ssW0rd!", "P4ssW0rd!", "123f", "email@email.com");
      var engineResult = RuleEngine.EvaluateRules(model, rules);
      var modelResults = engineResult.ValidationResults;

      EvaluateValid(MODEL_ID_SHOPPERVALID, engineResult, modelResults, FACT_PIN);
    }

    [TestMethod]
    [DeploymentItem("ShopperValidation.xml")]
    public void TestBadPINAscSequence()
    {
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"ShopperValidation.xml");

      var rules = new XmlDocument();
      rules.Load(directory);

      var model = BuildModel("firstname", "lastname", "username", "P4ssW0rd!", "P4ssW0rd!", "1234", "email@email.com");
      var engineResult = RuleEngine.EvaluateRules(model, rules);
      var modelResults = engineResult.ValidationResults;

      EvaluateValid(MODEL_ID_SHOPPERVALID, engineResult, modelResults, FACT_PIN);
    }

    [TestMethod]
    [DeploymentItem("ShopperValidation.xml")]
    public void TestBadPINDescSequence()
    {
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"ShopperValidation.xml");

      var rules = new XmlDocument();
      rules.Load(directory);

      var model = BuildModel("firstname", "lastname", "username", "P4ssW0rd!", "P4ssW0rd!", "8765", "email@email.com");
      var engineResult = RuleEngine.EvaluateRules(model, rules);
      var modelResults = engineResult.ValidationResults;

      EvaluateValid(MODEL_ID_SHOPPERVALID, engineResult, modelResults, FACT_PIN);
    }
    #endregion

    #region Email Tests
    [TestMethod]
    [DeploymentItem("ShopperValidation.xml")]
    public void TestBadEmailEmpty()
    {
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"ShopperValidation.xml");

      var rules = new XmlDocument();
      rules.Load(directory);
      var model = BuildModel("firstname", "lastname", "username", "P4ssW0rd!", "P4ssW0rd!", "1254", string.Empty);
      var engineResult = RuleEngine.EvaluateRules(model, rules);
      var modelResults = engineResult.ValidationResults;

      EvaluateValid(MODEL_ID_SHOPPERVALID, engineResult, modelResults, FACT_EMAIL);
    }

    [TestMethod]
    [DeploymentItem("ShopperValidation.xml")]
    public void TestBadEmailNull()
    {
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"ShopperValidation.xml");

      var rules = new XmlDocument();
      rules.Load(directory);
      var model = BuildModel("firstname", "lastname", "username", "P4ssW0rd!", "P4ssW0rd!", "1254", null);
      var engineResult = RuleEngine.EvaluateRules(model, rules);
      var modelResults = engineResult.ValidationResults;

      EvaluateValid(MODEL_ID_SHOPPERVALID, engineResult, modelResults, FACT_EMAIL);
    }

    [TestMethod]
    [DeploymentItem("ShopperValidation.xml")]
    public void TestBadEmailMaxLength()
    {
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"ShopperValidation.xml");

      var rules = new XmlDocument();
      rules.Load(directory);

      var longEmail = "a@x.";
      while (longEmail.Length < _emailMaxLength + 1)
      {
        longEmail += "c";
      }

      var model = BuildModel("firstname", "lastname", "username", "P4ssW0rd!", "P4ssW0rd!", "1254", longEmail);
      var engineResult = RuleEngine.EvaluateRules(model, rules);
      var modelResults = engineResult.ValidationResults;

      EvaluateValid(MODEL_ID_SHOPPERVALID, engineResult, modelResults, FACT_EMAIL);
    }

    [TestMethod]
    [DeploymentItem("ShopperValidation.xml")]
    public void TestBadEmailRegex()
    {
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"ShopperValidation.xml");

      var rules = new XmlDocument();
      rules.Load(directory);
      var model = BuildModel("firstname", "lastname", "username", "P4ssW0rd!", "P4ssW0rd!", "1254", "email@emailcom");
      var engineResult = RuleEngine.EvaluateRules(model, rules);
      var modelResults = engineResult.ValidationResults;

      EvaluateValid(MODEL_ID_SHOPPERVALID, engineResult, modelResults, FACT_EMAIL);
    }
    #endregion

    #endregion

    #region Evaluation (Assert methods)
    private void EvaluateValid(string modelId, IRuleEngineResult engineResult, IList<IModelResult> modelResults, params string[] failFactKeys)
    {
      Assert.IsTrue(engineResult.Status == RuleEngineResultStatus.Valid);

      var facts = modelResults.FirstOrDefault(m => m.ModelId == modelId);

      Assert.IsNotNull(facts);

      Assert.IsTrue(facts.ContainsInvalids ^ !failFactKeys.Any());

      foreach (var fact in facts.Facts)
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

    private void EvaluateInvalid(IRuleEngineResult engineResult)
    {
      // TODO: Handle internal model validity?
      Assert.IsTrue(engineResult.Status == RuleEngineResultStatus.Invalid);
    }

    private void EvaluateException(IRuleEngineResult engineResult)
    {
      // TODO: HAndle internal model validity?
      Assert.IsTrue(engineResult.Status == RuleEngineResultStatus.Exception);
    }
    #endregion
  }
}
