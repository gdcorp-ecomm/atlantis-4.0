﻿using System.Collections.Generic;
using System.Linq;
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
      var request = new ValidateInputPhoneNumberRequestData(new Dictionary<string, string>(2)
      {
        {"phonenumber", "0236618300"},
        {"regioncode", RegionCode.IT}
      });
      var validator = (ValidateInputPhoneNumberResponseData)Engine.Engine.ProcessRequest(request, 769);

      var errors = validator.Result.ErrorCodes;
      var isValid = validator.Result.IsSuccess;
      Assert.IsTrue(isValid);
      Assert.AreEqual(0, errors.Count);
    }

    [TestMethod]
    public void PhoneNumberValidNoDefaultRegion()
    {
      var request = new ValidateInputPhoneNumberRequestData(new Dictionary<string, string>(1)
      {
        {"phonenumber", "+442083661177"}
      });
      var validator = (ValidateInputPhoneNumberResponseData)Engine.Engine.ProcessRequest(request, 769);

      var errors = validator.Result.ErrorCodes;
      var isValid = validator.Result.IsSuccess;
      Assert.IsTrue(isValid);
      Assert.AreEqual(0, errors.Count);
    }

    [TestMethod]
    public void PhoneNumberValidNoDefaultRegionNoPlus()
    {
      var request = new ValidateInputPhoneNumberRequestData(new Dictionary<string, string>(1)
      {
        {"phonenumber", "442083661177"}
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
      var request = new ValidateInputPhoneNumberRequestData(new Dictionary<string, string>(2)
      {
        {"phonenumber", "2530000"},
        {"regioncode", RegionCode.US}
      });
      var validator = (ValidateInputPhoneNumberResponseData)Engine.Engine.ProcessRequest(request, 769);

      var errors = validator.Result.ErrorCodes;
      var isValid = validator.Result.IsSuccess;
      Assert.IsFalse(isValid);
      Assert.IsTrue(HasFailureCode(errors, 7));
    }

    [TestMethod]
    public void PhoneNumberTooShort()
    {
      var request = new ValidateInputPhoneNumberRequestData(new Dictionary<string, string>(2)
      {
        {"phonenumber", "65025"},
        {"regioncode", RegionCode.US}
      });
      var validator = (ValidateInputPhoneNumberResponseData)Engine.Engine.ProcessRequest(request, 769);

      var errors = validator.Result.ErrorCodes;
      var isValid = validator.Result.IsSuccess;
      Assert.IsFalse(isValid);
      Assert.IsTrue(HasFailureCode(errors, 5));
    }

    [TestMethod]
    public void PhoneNumberTooLong()
    {
      var request = new ValidateInputPhoneNumberRequestData(new Dictionary<string, string>(2)
      {
        {"phonenumber", "65025300001"},
        {"regioncode", RegionCode.US}
      });
      var validator = (ValidateInputPhoneNumberResponseData)Engine.Engine.ProcessRequest(request, 769);

      var errors = validator.Result.ErrorCodes;
      var isValid = validator.Result.IsSuccess;
      Assert.IsFalse(isValid);
      Assert.IsTrue(HasFailureCode(errors, 6));
    }

    [TestMethod]
    public void PhoneNumberEmpty()
    {
      var request = new ValidateInputPhoneNumberRequestData(new Dictionary<string, string>(2)
      {
        {"phonenumber", ""},
        {"regioncode", RegionCode.US}
      });
      var validator = (ValidateInputPhoneNumberResponseData)Engine.Engine.ProcessRequest(request, 769);

      var errors = validator.Result.ErrorCodes;
      var isValid = validator.Result.IsSuccess;
      Assert.IsFalse(isValid);
      Assert.IsTrue(HasFailureCode(errors, 2));
    }

    [TestMethod]
    public void PhoneNumberInvalidCountryCode()
    {
      var request = new ValidateInputPhoneNumberRequestData(new Dictionary<string, string>(2)
      {
        {"phonenumber", "2530000"},
        {"countrycallingcode", "0"}
      });
      var validator = (ValidateInputPhoneNumberResponseData)Engine.Engine.ProcessRequest(request, 769);

      var errors = validator.Result.ErrorCodes;
      var isValid = validator.Result.IsSuccess;
      Assert.IsFalse(isValid);
      Assert.IsTrue(HasFailureCode(errors, 8));
    }

    [TestMethod]
    public void PhoneNumberNoRegionInvalidCountryCode()
    {
      var request = new ValidateInputPhoneNumberRequestData(new Dictionary<string, string>(1)
      {
        {"phonenumber", "0236618300"}
      });
      var validator = (ValidateInputPhoneNumberResponseData)Engine.Engine.ProcessRequest(request, 769);

      var errors = validator.Result.ErrorCodes;
      var isValid = validator.Result.IsSuccess;
      Assert.IsFalse(isValid);
      Assert.IsTrue(HasFailureCode(errors, 8));
    }
  }
}