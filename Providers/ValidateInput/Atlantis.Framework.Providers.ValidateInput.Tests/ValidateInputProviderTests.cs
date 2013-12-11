using System.Collections.Generic;
using Atlantis.Framework.Providers.ValidateInput.Interface;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.ValidateInput.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.ValidateInput.Impl.dll")]
  public class ValidateInputProviderTests
  {
    private static MockProviderContainer NewValidateInputProvider()
    {
      var container = new MockProviderContainer();
      container.RegisterProvider<IValidateInputProvider, ValidateInputProvider>();

      return container;
    }

    [TestMethod]
    public void ValidateInputPasswordSuccess()
    {
      var container = NewValidateInputProvider();
      var provider = container.Resolve<IValidateInputProvider>();

      var result = provider.ValidateInput(ValidateInputTypes.Password, new Dictionary<ValidateInputKeys, string>(1)
      {
        {ValidateInputKeys.PasswordInput, "Abasdf 9980"}
      });

      var errors = result.ErrorCodes;
      var isValid = result.IsSuccess;
      Assert.IsTrue(isValid);
      Assert.AreEqual(0, errors.Count);
    }

    [TestMethod]
    public void ValidateInputPhoneNumberSuccess()
    {
      var container = NewValidateInputProvider();
      var provider = container.Resolve<IValidateInputProvider>();

      var result = provider.ValidateInput(ValidateInputTypes.PhoneNumber, new Dictionary<ValidateInputKeys, string>(2)
      {
        {ValidateInputKeys.PhoneNumberInput, "0236618300"},
        {ValidateInputKeys.PhoneNumberRegionCode, "IT"}
      });

      var errors = result.ErrorCodes;
      var isValid = result.IsSuccess;
      Assert.IsTrue(isValid);
      Assert.AreEqual(0, errors.Count);
    }

    [TestMethod]
    public void ValidateInputPasswordFailure()
    {
      var container = NewValidateInputProvider();
      var provider = container.Resolve<IValidateInputProvider>();

      var result = provider.ValidateInput(ValidateInputTypes.Password, new Dictionary<ValidateInputKeys, string>(2)
      {
        {ValidateInputKeys.PasswordInput, "ABC"},
        {ValidateInputKeys.PasswordInputMatch, "iajuiaytajfh562J#*FDS"}
      });

      var errors = result.ErrorCodes;
      var isValid = result.IsSuccess;
      Assert.IsFalse(isValid);
      Assert.AreNotEqual(0, errors.Count);
    }

    [TestMethod]
    public void ValidateInputPhoneNumberFailure()
    {
      var container = NewValidateInputProvider();
      var provider = container.Resolve<IValidateInputProvider>();

      var result = provider.ValidateInput(ValidateInputTypes.PhoneNumber, new Dictionary<ValidateInputKeys, string>(2)
      {
        {ValidateInputKeys.PhoneNumberInput, "2530000"},
        {ValidateInputKeys.PhoneNumberRegionCode, "US"}
      });

      var errors = result.ErrorCodes;
      var isValid = result.IsSuccess;
      Assert.IsFalse(isValid);
      Assert.AreNotEqual(0, errors.Count);
    }
  }
}