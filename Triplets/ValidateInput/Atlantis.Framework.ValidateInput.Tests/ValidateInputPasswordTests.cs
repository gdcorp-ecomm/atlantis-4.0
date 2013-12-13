using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.ValidateInput.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.ValidateInput.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.ValidateInput.Impl.dll")]
  public class ValidateInputPasswordTests
  {
    [TestMethod]
    public void PasswordValid()
    {
      var request = new ValidateInputPasswordRequestData(new Dictionary<string, string>(1)
      {
        {"password", "Abasdf 9980"}
      });
      var validator = (ValidateInputPasswordResponseData)Engine.Engine.ProcessRequest(request, 770);

      var errors = validator.Result.ErrorCodes;
      var isValid = validator.Result.IsSuccess;
      Assert.IsTrue(isValid);
      Assert.AreEqual(0, errors.Count);
    }

    [TestMethod]
    public void PasswordValidWithMatch()
    {
      var request = new ValidateInputPasswordRequestData(new Dictionary<string, string>(2)
      {
        {"password", "Abasdf 9980"},
        {"passwordconfirmation", "Abasdf 9980"}
      });
      var validator = (ValidateInputPasswordResponseData)Engine.Engine.ProcessRequest(request, 770);

      var errors = validator.Result.ErrorCodes;
      var isValid = validator.Result.IsSuccess;
      Assert.IsTrue(isValid);
      Assert.AreEqual(0, errors.Count);
    }

    private static bool HasFailureCode(IEnumerable<int> errors, int failureCode)
    {
      return errors.Any(error => error == failureCode);
    }

    [TestMethod]
    public void PasswordValidNoMatch()
    {
      var request = new ValidateInputPasswordRequestData(new Dictionary<string, string>(2)
      {
        {"password", "Abasdf 9980"},
        {"passwordconfirmation", "iajuiaytajfh562J#*FDS"}
      });
      var validator = (ValidateInputPasswordResponseData)Engine.Engine.ProcessRequest(request, 770);

      var errors = validator.Result.ErrorCodes;
      var isValid = validator.Result.IsSuccess;
      Assert.IsFalse(isValid);
      Assert.IsTrue(HasFailureCode(errors, 6));
    }

    [TestMethod]
    public void PasswordToShort()
    {
      var request = new ValidateInputPasswordRequestData(new Dictionary<string, string>(1)
      {
        {"password", "Aab4"}
      });
      var validator = (ValidateInputPasswordResponseData)Engine.Engine.ProcessRequest(request, 770);

      var errors = validator.Result.ErrorCodes;
      var isValid = validator.Result.IsSuccess;
      Assert.IsFalse(isValid);
      Assert.IsTrue(HasFailureCode(errors, 2));
    }

    [TestMethod]
    public void PasswordToLong()
    {
      var request = new ValidateInputPasswordRequestData(new Dictionary<string, string>(1)
      {
        {"password", "Aab4".PadRight(256, 'x')}
      });
      var validator = (ValidateInputPasswordResponseData)Engine.Engine.ProcessRequest(request, 770);

      var errors = validator.Result.ErrorCodes;
      var isValid = validator.Result.IsSuccess;
      Assert.IsFalse(isValid);
      Assert.IsTrue(HasFailureCode(errors, 2));
    }

    [TestMethod]
    public void PasswordNoUpperCase()
    {
      var request = new ValidateInputPasswordRequestData(new Dictionary<string, string>(1)
      {
        {"password", "#abkd uzei8k<k>ej%-_"}
      });
      var validator = (ValidateInputPasswordResponseData)Engine.Engine.ProcessRequest(request, 770);

      var errors = validator.Result.ErrorCodes;
      var isValid = validator.Result.IsSuccess;
      Assert.IsFalse(isValid);
      Assert.IsTrue(HasFailureCode(errors, 10));
    }

    [TestMethod]
    public void PasswordNoNumber()
    {
      var request = new ValidateInputPasswordRequestData(new Dictionary<string, string>(1)
      {
        {"password", "#abkd uZeik<k>ej%-_"}
      });
      var validator = (ValidateInputPasswordResponseData)Engine.Engine.ProcessRequest(request, 770);

      var errors = validator.Result.ErrorCodes;
      var isValid = validator.Result.IsSuccess;
      Assert.IsFalse(isValid);
      Assert.IsTrue(HasFailureCode(errors, 10));
    }

    [TestMethod]
    public void PasswordMultipleIssuesWithMatch()
    {
      var request = new ValidateInputPasswordRequestData(new Dictionary<string, string>(2)
      {
        {"password", "ABC"},
        {"passwordconfirmation", "ABC"}
      });
      var validator = (ValidateInputPasswordResponseData)Engine.Engine.ProcessRequest(request, 770);

      var errors = validator.Result.ErrorCodes;
      var isValid = validator.Result.IsSuccess;
      Assert.IsFalse(isValid);
      Assert.IsTrue(HasFailureCode(errors, 10));
      Assert.IsTrue(HasFailureCode(errors, 2));
    }

    [TestMethod]
    public void PasswordMultipleIssuesNoMatch()
    {
      var request = new ValidateInputPasswordRequestData(new Dictionary<string, string>(2)
      {
        {"password", "ABC"},
        {"passwordconfirmation", "iajuiaytajfh562J#*FDS"}
      });
      var validator = (ValidateInputPasswordResponseData)Engine.Engine.ProcessRequest(request, 770);

      var errors = validator.Result.ErrorCodes;
      var isValid = validator.Result.IsSuccess;
      Assert.IsFalse(isValid);
      Assert.IsTrue(HasFailureCode(errors, 10));
      Assert.IsTrue(HasFailureCode(errors, 2));
      Assert.IsTrue(HasFailureCode(errors, 6));
    }

    [TestMethod]
    public void PasswordLeadingWhiteSpace()
    {
      var request = new ValidateInputPasswordRequestData(new Dictionary<string, string>(1)
      {
        {"password", "   1234Adj29"}
      });
      var validator = (ValidateInputPasswordResponseData)Engine.Engine.ProcessRequest(request, 770);

      var errors = validator.Result.ErrorCodes;
      var isValid = validator.Result.IsSuccess;
      Assert.IsFalse(isValid);
      Assert.IsTrue(HasFailureCode(errors, 10));
    }

    [TestMethod]
    public void PasswordTrailingWhiteSpace()
    {
      var request = new ValidateInputPasswordRequestData(new Dictionary<string, string>(1)
      {
        {"password", "1234Adj29   "}
      });
      var validator = (ValidateInputPasswordResponseData)Engine.Engine.ProcessRequest(request, 770);

      var errors = validator.Result.ErrorCodes;
      var isValid = validator.Result.IsSuccess;
      Assert.IsFalse(isValid);
      Assert.IsTrue(HasFailureCode(errors, 10));
    }

    [TestMethod]
    public void PasswordEmpty()
    {
      var request = new ValidateInputPasswordRequestData(new Dictionary<string, string>(1)
      {
        {"password", ""}
      });
      var validator = (ValidateInputPasswordResponseData)Engine.Engine.ProcessRequest(request, 770);

      var errors = validator.Result.ErrorCodes;
      var isValid = validator.Result.IsSuccess;
      Assert.IsFalse(isValid);
      Assert.IsTrue(HasFailureCode(errors, 5));
    }
  }
}