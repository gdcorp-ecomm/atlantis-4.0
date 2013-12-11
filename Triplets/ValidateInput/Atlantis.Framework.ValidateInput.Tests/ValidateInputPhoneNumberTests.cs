using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.Providers.ValidateInput.Interface;
using Atlantis.Framework.Providers.ValidateInput.Interface.ErrorCodes;
using Atlantis.Framework.ValidateInput.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhoneNumbers;

namespace Atlantis.Framework.ValidateInput.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.ValidateInput.Impl.dll")]
  public class ValidateInputPhoneNumberTests
  {
    [TestMethod]
    public void PhoneNumberValid()
    {
      var request = new ValidateInputPhoneNumberRequestData(new Dictionary<ValidateInputKeys, string>(2)
      {
        {ValidateInputKeys.PhoneNumberInput, "0236618300"},
        {ValidateInputKeys.PhoneNumberRegionCode, RegionCode.IT}
      });
      var validator = (ValidateInputPhoneNumberResponseData)Engine.Engine.ProcessRequest(request, 769);

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
    public void PhoneNumberInvalid()
    {
      var request = new ValidateInputPhoneNumberRequestData(new Dictionary<ValidateInputKeys, string>(2)
      {
        {ValidateInputKeys.PhoneNumberInput, "2530000"},
        {ValidateInputKeys.PhoneNumberRegionCode, RegionCode.US}
      });
      var validator = (ValidateInputPhoneNumberResponseData)Engine.Engine.ProcessRequest(request, 769);

      var errors = validator.Result.ErrorCodes;
      var isValid = validator.Result.IsSuccess;
      Assert.IsFalse(isValid);
      Assert.IsTrue(HasFailureCode(errors, PhoneNumberErrorCodes.InvalidPhoneNumber));
    }

    [TestMethod]
    public void PhoneNumberTooShort()
    {
      var request = new ValidateInputPhoneNumberRequestData(new Dictionary<ValidateInputKeys, string>(2)
      {
        {ValidateInputKeys.PhoneNumberInput, "65025"},
        {ValidateInputKeys.PhoneNumberRegionCode, RegionCode.US}
      });
      var validator = (ValidateInputPhoneNumberResponseData)Engine.Engine.ProcessRequest(request, 769);

      var errors = validator.Result.ErrorCodes;
      var isValid = validator.Result.IsSuccess;
      Assert.IsFalse(isValid);
      Assert.IsTrue(HasFailureCode(errors, PhoneNumberErrorCodes.TooShort));
    }

    [TestMethod]
    public void PhoneNumberTooLong()
    {
      var request = new ValidateInputPhoneNumberRequestData(new Dictionary<ValidateInputKeys, string>(2)
      {
        {ValidateInputKeys.PhoneNumberInput, "65025300001"},
        {ValidateInputKeys.PhoneNumberRegionCode, RegionCode.US}
      });
      var validator = (ValidateInputPhoneNumberResponseData)Engine.Engine.ProcessRequest(request, 769);

      var errors = validator.Result.ErrorCodes;
      var isValid = validator.Result.IsSuccess;
      Assert.IsFalse(isValid);
      Assert.IsTrue(HasFailureCode(errors, PhoneNumberErrorCodes.TooLong));
    }

    [TestMethod]
    public void PhoneNumberEmpty()
    {
      var request = new ValidateInputPhoneNumberRequestData(new Dictionary<ValidateInputKeys, string>(2)
      {
        {ValidateInputKeys.PhoneNumberInput, ""},
        {ValidateInputKeys.PhoneNumberRegionCode, RegionCode.US}
      });
      var validator = (ValidateInputPhoneNumberResponseData)Engine.Engine.ProcessRequest(request, 769);

      var errors = validator.Result.ErrorCodes;
      var isValid = validator.Result.IsSuccess;
      Assert.IsFalse(isValid);
      Assert.IsTrue(HasFailureCode(errors, PhoneNumberErrorCodes.EmptyPhoneNumber));
    }

    [TestMethod]
    public void PhoneNumberInvalidCountryCode()
    {
      var request = new ValidateInputPhoneNumberRequestData(new Dictionary<ValidateInputKeys, string>(2)
      {
        {ValidateInputKeys.PhoneNumberInput, "2530000"},
        {ValidateInputKeys.PhoneNumberCountryCallingCode, "0"}
      });
      var validator = (ValidateInputPhoneNumberResponseData)Engine.Engine.ProcessRequest(request, 769);

      var errors = validator.Result.ErrorCodes;
      var isValid = validator.Result.IsSuccess;
      Assert.IsFalse(isValid);
      Assert.IsTrue(HasFailureCode(errors, PhoneNumberErrorCodes.InvalidCountryCode));
    }

    [TestMethod]
    public void PhoneNumberNoRegionOrCountryCode()
    {
      var request = new ValidateInputPhoneNumberRequestData(new Dictionary<ValidateInputKeys, string>(1)
      {
        {ValidateInputKeys.PhoneNumberInput, "2530000"}
      });
      var validator = (ValidateInputPhoneNumberResponseData)Engine.Engine.ProcessRequest(request, 769);

      var errors = validator.Result.ErrorCodes;
      var isValid = validator.Result.IsSuccess;
      Assert.IsFalse(isValid);
      Assert.IsTrue(HasFailureCode(errors, PhoneNumberErrorCodes.InvalidCountryCode));
    }
  }
}