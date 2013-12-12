using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.Providers.ValidateInput.Interface;
using Atlantis.Framework.Providers.ValidateInput.Interface.ErrorCodes;
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
      var request = new ValidateInputPasswordRequestData(new Dictionary<ValidateInputKeys, string>(1)
      {
        {ValidateInputKeys.PasswordInput, "Abasdf 9980"}
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
      var request = new ValidateInputPasswordRequestData(new Dictionary<ValidateInputKeys, string>(2)
      {
        {ValidateInputKeys.PasswordInput, "Abasdf 9980"},
        {ValidateInputKeys.PasswordInputMatch, "Abasdf 9980"}
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
      var request = new ValidateInputPasswordRequestData(new Dictionary<ValidateInputKeys, string>(2)
      {
        {ValidateInputKeys.PasswordInput, "Abasdf 9980"},
        {ValidateInputKeys.PasswordInputMatch, "iajuiaytajfh562J#*FDS"}
      });
      var validator = (ValidateInputPasswordResponseData)Engine.Engine.ProcessRequest(request, 770);

      var errors = validator.Result.ErrorCodes;
      var isValid = validator.Result.IsSuccess;
      Assert.IsFalse(isValid);
      Assert.IsTrue(HasFailureCode(errors, PasswordErrorCodes.PasswordsNotEqual));
    }

    [TestMethod]
    public void PasswordToShort()
    {
      var request = new ValidateInputPasswordRequestData(new Dictionary<ValidateInputKeys, string>(1)
      {
        {ValidateInputKeys.PasswordInput, "Aab4"}
      });
      var validator = (ValidateInputPasswordResponseData)Engine.Engine.ProcessRequest(request, 770);

      var errors = validator.Result.ErrorCodes;
      var isValid = validator.Result.IsSuccess;
      Assert.IsFalse(isValid);
      Assert.IsTrue(HasFailureCode(errors, PasswordErrorCodes.PasswordLength));
    }

    [TestMethod]
    public void PasswordToLong()
    {
      var request = new ValidateInputPasswordRequestData(new Dictionary<ValidateInputKeys, string>(1)
      {
        {ValidateInputKeys.PasswordInput, "Aab4".PadRight(256, 'x')}
      });
      var validator = (ValidateInputPasswordResponseData)Engine.Engine.ProcessRequest(request, 770);

      var errors = validator.Result.ErrorCodes;
      var isValid = validator.Result.IsSuccess;
      Assert.IsFalse(isValid);
      Assert.IsTrue(HasFailureCode(errors, PasswordErrorCodes.PasswordLength));
    }

    [TestMethod]
    public void PasswordNoUpperCase()
    {
      var request = new ValidateInputPasswordRequestData(new Dictionary<ValidateInputKeys, string>(1)
      {
        {ValidateInputKeys.PasswordInput, "#abkd uzei8k<k>ej%-_"}
      });
      var validator = (ValidateInputPasswordResponseData)Engine.Engine.ProcessRequest(request, 770);

      var errors = validator.Result.ErrorCodes;
      var isValid = validator.Result.IsSuccess;
      Assert.IsFalse(isValid);
      Assert.IsTrue(HasFailureCode(errors, PasswordErrorCodes.PasswordRegEx));
    }

    [TestMethod]
    public void PasswordNoNumber()
    {
      var request = new ValidateInputPasswordRequestData(new Dictionary<ValidateInputKeys, string>(1)
      {
        {ValidateInputKeys.PasswordInput, "#abkd uZeik<k>ej%-_"}
      });
      var validator = (ValidateInputPasswordResponseData)Engine.Engine.ProcessRequest(request, 770);

      var errors = validator.Result.ErrorCodes;
      var isValid = validator.Result.IsSuccess;
      Assert.IsFalse(isValid);
      Assert.IsTrue(HasFailureCode(errors, PasswordErrorCodes.PasswordRegEx));
    }

    [TestMethod]
    public void PasswordMultipleIssuesWithMatch()
    {
      var request = new ValidateInputPasswordRequestData(new Dictionary<ValidateInputKeys, string>(2)
      {
        {ValidateInputKeys.PasswordInput, "ABC"},
        {ValidateInputKeys.PasswordInputMatch, "ABC"}
      });
      var validator = (ValidateInputPasswordResponseData)Engine.Engine.ProcessRequest(request, 770);

      var errors = validator.Result.ErrorCodes;
      var isValid = validator.Result.IsSuccess;
      Assert.IsFalse(isValid);
      Assert.IsTrue(HasFailureCode(errors, PasswordErrorCodes.PasswordRegEx));
      Assert.IsTrue(HasFailureCode(errors, PasswordErrorCodes.PasswordLength));
    }

    [TestMethod]
    public void PasswordMultipleIssuesNoMatch()
    {
      var request = new ValidateInputPasswordRequestData(new Dictionary<ValidateInputKeys, string>(2)
      {
        {ValidateInputKeys.PasswordInput, "ABC"},
        {ValidateInputKeys.PasswordInputMatch, "iajuiaytajfh562J#*FDS"}
      });
      var validator = (ValidateInputPasswordResponseData)Engine.Engine.ProcessRequest(request, 770);

      var errors = validator.Result.ErrorCodes;
      var isValid = validator.Result.IsSuccess;
      Assert.IsFalse(isValid);
      Assert.IsTrue(HasFailureCode(errors, PasswordErrorCodes.PasswordRegEx));
      Assert.IsTrue(HasFailureCode(errors, PasswordErrorCodes.PasswordLength));
      Assert.IsTrue(HasFailureCode(errors, PasswordErrorCodes.PasswordsNotEqual));
    }

    [TestMethod]
    public void PasswordLeadingWhiteSpace()
    {
      var request = new ValidateInputPasswordRequestData(new Dictionary<ValidateInputKeys, string>(1)
      {
        {ValidateInputKeys.PasswordInput, "   1234Adj29"}
      });
      var validator = (ValidateInputPasswordResponseData)Engine.Engine.ProcessRequest(request, 770);

      var errors = validator.Result.ErrorCodes;
      var isValid = validator.Result.IsSuccess;
      Assert.IsFalse(isValid);
      Assert.IsTrue(HasFailureCode(errors, PasswordErrorCodes.PasswordRegEx));
    }

    [TestMethod]
    public void PasswordTrailingWhiteSpace()
    {
      var request = new ValidateInputPasswordRequestData(new Dictionary<ValidateInputKeys, string>(1)
      {
        {ValidateInputKeys.PasswordInput, "1234Adj29   "}
      });
      var validator = (ValidateInputPasswordResponseData)Engine.Engine.ProcessRequest(request, 770);

      var errors = validator.Result.ErrorCodes;
      var isValid = validator.Result.IsSuccess;
      Assert.IsFalse(isValid);
      Assert.IsTrue(HasFailureCode(errors, PasswordErrorCodes.PasswordRegEx));
    }

    [TestMethod]
    public void PasswordEmpty()
    {
      var request = new ValidateInputPasswordRequestData(new Dictionary<ValidateInputKeys, string>(1)
      {
        {ValidateInputKeys.PasswordInput, ""}
      });
      var validator = (ValidateInputPasswordResponseData)Engine.Engine.ProcessRequest(request, 770);

      var errors = validator.Result.ErrorCodes;
      var isValid = validator.Result.IsSuccess;
      Assert.IsFalse(isValid);
      Assert.IsTrue(HasFailureCode(errors, PasswordErrorCodes.PasswordEmpty));
    }
  }
}