using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.ValidateField.Interface;

namespace Atlantis.Framework.ValidateField.Tests
{
  [TestClass]
  public class ValidatePasswordTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.ValidateField.Impl.dll")]
    public void PasswordValid()
    {
      var request = new ValidateFieldRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "password");
      ValidateFieldResponseData validator = (ValidateFieldResponseData)DataCache.DataCache.GetProcessRequest(request, 507);

      List<ValidationFailure> errors;
      bool isValid = validator.ValidateStringField("Abasdf 9980", out errors);
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

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.ValidateField.Impl.dll")]
    public void PasswordLeadingWhiteSpace()
    {
      var request = new ValidateFieldRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "password");
      ValidateFieldResponseData validator = (ValidateFieldResponseData)DataCache.DataCache.GetProcessRequest(request, 507);

      List<ValidationFailure> errors;
      bool isValid = validator.ValidateStringField("   1234Adj29", out errors);
      Assert.IsFalse(isValid);
      Assert.IsTrue(HasFailureCode(errors, 10));
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.ValidateField.Impl.dll")]
    public void PasswordTrailingWhiteSpace()
    {
      var request = new ValidateFieldRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "password");
      ValidateFieldResponseData validator = (ValidateFieldResponseData)DataCache.DataCache.GetProcessRequest(request, 507);

      List<ValidationFailure> errors;
      bool isValid = validator.ValidateStringField("1234Adj29   ", out errors);
      Assert.IsFalse(isValid);
      Assert.IsTrue(HasFailureCode(errors, 10));
    }

  }
}
