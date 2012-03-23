using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.ValidateField.Interface;

namespace Atlantis.Framework.ValidateField.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class ValidatePasswordTests
  {
    public ValidatePasswordTests()
    {
      //
      // TODO: Add constructor logic here
      //
    }

    private TestContext testContextInstance;

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext
    {
      get
      {
        return testContextInstance;
      }
      set
      {
        testContextInstance = value;
      }
    }

    #region Additional test attributes
    //
    // You can use the following additional attributes as you write your tests:
    //
    // Use ClassInitialize to run code before running the first test in the class
    // [ClassInitialize()]
    // public static void MyClassInitialize(TestContext testContext) { }
    //
    // Use ClassCleanup to run code after all tests in a class have run
    // [ClassCleanup()]
    // public static void MyClassCleanup() { }
    //
    // Use TestInitialize to run code before running each test 
    // [TestInitialize()]
    // public void MyTestInitialize() { }
    //
    // Use TestCleanup to run code after each test has run
    // [TestCleanup()]
    // public void MyTestCleanup() { }
    //
    #endregion

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.ValidateField.Impl.dll")]
    public void PasswordValid()
    {
      var request = new ValidateFieldRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "password");
      ValidateFieldResponseData validator = (ValidateFieldResponseData)DataCache.DataCache.GetProcessRequest(request, 507);

      List<ValidationFailure> errors;
      bool isValid = validator.ValidateStringField("#abkd uZei8k<k>ej%-_", out errors);
      Assert.IsTrue(isValid);
      Assert.AreEqual(0, errors.Count);
    }

    private bool HasFailureCode(IEnumerable<ValidationFailure> errors, int failureCode)
    {
      bool result = false;
      foreach (ValidationFailure error in errors)
      {
        if (error.FailureCode == failureCode)
        {
          result = true;
          break;
        }
      }
      return result;
    }


    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.ValidateField.Impl.dll")]
    public void PasswordToShort()
    {
      var request = new ValidateFieldRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "password");
      ValidateFieldResponseData validator = (ValidateFieldResponseData)DataCache.DataCache.GetProcessRequest(request, 507);

      List<ValidationFailure> errors;
      bool isValid = validator.ValidateStringField("Aab4", out errors);
      Assert.IsFalse(isValid);
      Assert.IsTrue(HasFailureCode(errors, 2));
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.ValidateField.Impl.dll")]
    public void PasswordToLong()
    {
      var request = new ValidateFieldRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "password");
      ValidateFieldResponseData validator = (ValidateFieldResponseData)DataCache.DataCache.GetProcessRequest(request, 507);

      List<ValidationFailure> errors;
      bool isValid = validator.ValidateStringField("Aab4".PadRight(256,'x'), out errors);
      Assert.IsFalse(isValid);
      Assert.IsTrue(HasFailureCode(errors, 2));
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.ValidateField.Impl.dll")]
    public void PasswordNoUpperCase()
    {
      var request = new ValidateFieldRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "password");
      ValidateFieldResponseData validator = (ValidateFieldResponseData)DataCache.DataCache.GetProcessRequest(request, 507);

      List<ValidationFailure> errors;
      bool isValid = validator.ValidateStringField("#abkd uzei8k<k>ej%-_", out errors);
      Assert.IsFalse(isValid);
      Assert.IsTrue(HasFailureCode(errors, 10));
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.ValidateField.Impl.dll")]
    public void PasswordNoNumber()
    {
      var request = new ValidateFieldRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "password");
      ValidateFieldResponseData validator = (ValidateFieldResponseData)DataCache.DataCache.GetProcessRequest(request, 507);

      List<ValidationFailure> errors;
      bool isValid = validator.ValidateStringField("#abkd uZeik<k>ej%-_", out errors);
      Assert.IsFalse(isValid);
      Assert.IsTrue(HasFailureCode(errors, 10));
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.ValidateField.Impl.dll")]
    public void PasswordNoLowerCase()
    {
      var request = new ValidateFieldRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "password");
      ValidateFieldResponseData validator = (ValidateFieldResponseData)DataCache.DataCache.GetProcessRequest(request, 507);

      List<ValidationFailure> errors;
      bool isValid = validator.ValidateStringField("#ABKD UZEI9K-_", out errors);
      Assert.IsFalse(isValid);
      Assert.IsTrue(HasFailureCode(errors, 10));
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.ValidateField.Impl.dll")]
    public void PasswordMultipleIssues()
    {
      var request = new ValidateFieldRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "password");
      ValidateFieldResponseData validator = (ValidateFieldResponseData)DataCache.DataCache.GetProcessRequest(request, 507);

      List<ValidationFailure> errors;
      bool isValid = validator.ValidateStringField("ABC", out errors);
      Assert.IsFalse(isValid);
      Assert.IsTrue(HasFailureCode(errors, 10));
      Assert.IsTrue(HasFailureCode(errors, 2));
    }

  }
}
