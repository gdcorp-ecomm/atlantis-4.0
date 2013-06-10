using System;
using System.Diagnostics;
using System.Linq;
using Atlantis.Framework.RuleEngine.Results;
using Atlantis.Framework.ShopperValidator.Interface;
using Atlantis.Framework.ShopperValidator.Interface.ShopperValidation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.ShopperValidator.Test
{
  [TestClass]
  public class RuleEngineUnitTests
  {
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
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "username", "P4ssW0rd!", "P4ssW0rd!", "email@email.com");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel);
    }

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
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "username", "P4ssW0rd!", "P4ssW0rd!", "email@email.com");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel);
    }

    #region Username Tests (count = 5)
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadUsernameEmpty()
    {
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, string.Empty, "P4ssW0rd!", "P4ssW0rd!", "email@email.com");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_USERNAME);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadUsernameNull()
    {
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, null, "P4ssW0rd!", "P4ssW0rd!", "email@email.com");
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

      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, longUsername, "P4ssW0rd!", "P4ssW0rd!", "email@email.com");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_USERNAME);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadUsernameAllNumeric()
    {
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "123456789", "P4ssW0rd!", "P4ssW0rd!", "email@email.com");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_USERNAME);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadUsernameInvalidChars()
    {
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "user;name", "P4ssW0rd!", "P4ssW0rd!", "email@email.com");
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
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "username", string.Empty, string.Empty, "email@email.com");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_PASSWORD, ModelConstants.FACT_PASSWORD2);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBothBadPasswordsNull()
    {
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "username", null, null, "email@email.com");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_PASSWORD, ModelConstants.FACT_PASSWORD2);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadPasswordEmpty()
    {
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "username", string.Empty, "P4ssW0rd!", "email@email.com");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_PASSWORD, ModelConstants.FACT_PASSWORD2);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadPassword2Empty()
    {
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "username", "P4ssW0rd!", string.Empty, "email@email.com");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_PASSWORD2);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadPasswordNull()
    {
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "username", null, "P4ssW0rd!", "email@email.com");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_PASSWORD, ModelConstants.FACT_PASSWORD2);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadPassword2Null()
    {
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "username", "P4ssW0rd!", null, "email@email.com");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_PASSWORD2);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadPasswordRegexNoNumbers()
    {
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "username", "PassWord!", "PassWord!", "email@email.com");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_PASSWORD);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadPasswordRegexNoCaps()
    {
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "username", "p4ssw0rd!", "p4ssw0rd!", "email@email.com");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_PASSWORD);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadPasswordRegexBeginningSpace()
    {
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "username", " P4ssW0rd!", " P4ssW0rd!", "email@email.com");
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

      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "username", shortPassword, shortPassword, "email@email.com");
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

      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "username", longPassword, longPassword, "email@email.com");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_PASS_MAX_LENGTH);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadPasswordMatch()
    {
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "username", "P4ssW0rd!", "P4ssW0rd!!", "email@email.com");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_PASSWORD2);
    }
    #endregion


    #region Email Tests (count = 4)
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadEmailEmpty()
    {
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "username", "P4ssW0rd!", "P4ssW0rd!", string.Empty);
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_EMAIL);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadEmailNull()
    {
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "username", "P4ssW0rd!", "P4ssW0rd!", null);
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

      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "username", "P4ssW0rd!", "P4ssW0rd!", longEmail);
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_EMAIL);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestREBadEmailRegex()
    {
      var request = new ShopperValidatorRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "username", "P4ssW0rd!", "P4ssW0rd!", "email@emailcom");
      var response = Engine.Engine.ProcessRequest(request, 588) as ShopperValidatorResponseData;

      Assert.IsTrue(response.IsSuccess);

      EvaluateValid(response.ValidatedModel, ModelConstants.FACT_EMAIL);
    }
    #endregion

    #endregion
  }
}
