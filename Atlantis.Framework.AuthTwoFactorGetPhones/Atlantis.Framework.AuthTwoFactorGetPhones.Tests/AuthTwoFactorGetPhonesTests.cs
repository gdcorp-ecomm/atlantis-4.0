using System;
using Atlantis.Framework.Auth.Interface;
using Atlantis.Framework.AuthTwoFactorGetPhones.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.AuthTwoFactorGetPhones.Tests
{
  [TestClass]
  public class AuthTwoFactorGetPhonesTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetPhonesInValidShopper()
    {
      AuthTwoFactorGetPhonesRequestData requestData = new AuthTwoFactorGetPhonesRequestData(string.Empty,
                                                                                            "www.AuthTwoFactorGetPhones.com/tests",
                                                                                            string.Empty,
                                                                                            Guid.NewGuid().ToString(),
                                                                                            1);

      AuthTwoFactorGetPhonesResponseData responseData = (AuthTwoFactorGetPhonesResponseData)Engine.Engine.ProcessRequest(requestData, 520);

      Assert.IsTrue(responseData.StatusCode == TwoFactorWebserviceResponseCodes.Error);
      Assert.IsTrue(responseData.ValidationCodes.Contains(AuthValidationCodes.ValidateShopperIdRequired));
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetPhonesValid()
    {
      AuthTwoFactorGetPhonesRequestData requestData = new AuthTwoFactorGetPhonesRequestData("847235",
                                                                                            "www.AuthTwoFactorGetPhones.com/tests",
                                                                                            string.Empty,
                                                                                            Guid.NewGuid().ToString(),
                                                                                            1);

      AuthTwoFactorGetPhonesResponseData responseData = (AuthTwoFactorGetPhonesResponseData)Engine.Engine.ProcessRequest(requestData, 520);

      Assert.IsTrue(responseData.StatusCode == TwoFactorWebserviceResponseCodes.Success);
      Assert.IsTrue(responseData.PrimaryPhone != null);
    }
  }
}
