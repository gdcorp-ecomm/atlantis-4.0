﻿using System;
using System.Diagnostics;
using Atlantis.Framework.Auth.Interface;
using Atlantis.Framework.AuthTwoFactorEnable.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.AuthTwoFactorEnable.Tests
{
  [TestClass]
  public class AuthTwoFactorEnableTests
  {
    private const string VALID_DEV_SHOPPER = "870808";
    private const string VALID_DEV_SHOPPER_PASSWORD = "passsword";
    private const string VALID_IP = "172.16.44.792";
    private const string VALID_HOSTNAME = "idp.godaddy.com";
    private const string VALID_PHONE = "14807605267";
    private const string VALID_CARRIER = "verizon";


    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void EnableTwoFactorEmptyShopper()
    {
      AuthTwoFactorEnableRequestData requestData = new AuthTwoFactorEnableRequestData(string.Empty,
                                                                                      VALID_DEV_SHOPPER_PASSWORD,
                                                                                      1,
                                                                                      VALID_PHONE,
                                                                                      VALID_CARRIER,
                                                                                      VALID_HOSTNAME,
                                                                                      VALID_IP,
                                                                                      "wwww.authtwofactorenabletests.com",
                                                                                      string.Empty,
                                                                                      Guid.NewGuid().ToString(),
                                                                                      1);

      AuthTwoFactorEnableResponseData responseData = (AuthTwoFactorEnableResponseData)Engine.Engine.ProcessRequest(requestData, 516);

      Assert.IsTrue(responseData.ValidationCodes.Count == 1);
      Assert.IsTrue(responseData.ValidationCodes.Contains(AuthValidationCodes.ValidateShopperIdRequired));
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void EnableTwoFactorEmptyPassword()
    {
      AuthTwoFactorEnableRequestData requestData = new AuthTwoFactorEnableRequestData(VALID_DEV_SHOPPER,
                                                                                      string.Empty,
                                                                                      1,
                                                                                      VALID_PHONE,
                                                                                      VALID_CARRIER,
                                                                                      VALID_HOSTNAME,
                                                                                      VALID_IP,
                                                                                      "wwww.authtwofactorenabletests.com",
                                                                                      string.Empty,
                                                                                      Guid.NewGuid().ToString(),
                                                                                      1);

      AuthTwoFactorEnableResponseData responseData = (AuthTwoFactorEnableResponseData)Engine.Engine.ProcessRequest(requestData, 516);

      Assert.IsTrue(responseData.ValidationCodes.Count == 1);
      Assert.IsTrue(responseData.ValidationCodes.Contains(AuthValidationCodes.ValidatePasswordRequired));
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void EnableTwoFactorEmptyPhone()
    {
      AuthTwoFactorEnableRequestData requestData = new AuthTwoFactorEnableRequestData(VALID_DEV_SHOPPER,
                                                                                      VALID_DEV_SHOPPER_PASSWORD,
                                                                                      1,
                                                                                      string.Empty,
                                                                                      VALID_CARRIER,
                                                                                      VALID_HOSTNAME,
                                                                                      VALID_IP,
                                                                                      "wwww.authtwofactorenabletests.com",
                                                                                      string.Empty,
                                                                                      Guid.NewGuid().ToString(),
                                                                                      1);

      AuthTwoFactorEnableResponseData responseData = (AuthTwoFactorEnableResponseData)Engine.Engine.ProcessRequest(requestData, 516);

      Assert.IsTrue(responseData.ValidationCodes.Count == 1);
      Assert.IsTrue(responseData.ValidationCodes.Contains(AuthValidationCodes.ValidatePhoneRequired));
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void EnableTwoFactorEmptyCarrier()
    {
      AuthTwoFactorEnableRequestData requestData = new AuthTwoFactorEnableRequestData(VALID_DEV_SHOPPER,
                                                                                      VALID_DEV_SHOPPER_PASSWORD,
                                                                                      1,
                                                                                      VALID_PHONE,
                                                                                      string.Empty,
                                                                                      VALID_HOSTNAME,
                                                                                      VALID_IP,
                                                                                      "wwww.authtwofactorenabletests.com",
                                                                                      string.Empty,
                                                                                      Guid.NewGuid().ToString(),
                                                                                      1);

      AuthTwoFactorEnableResponseData responseData = (AuthTwoFactorEnableResponseData)Engine.Engine.ProcessRequest(requestData, 516);

      Assert.IsTrue(responseData.ValidationCodes.Count == 1);
      Assert.IsTrue(responseData.ValidationCodes.Contains(AuthValidationCodes.ValidateCarrierRequired));
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void EnableTwoFactorEmptyHost()
    {
      AuthTwoFactorEnableRequestData requestData = new AuthTwoFactorEnableRequestData(VALID_DEV_SHOPPER,
                                                                                      VALID_DEV_SHOPPER_PASSWORD,
                                                                                      1,
                                                                                      VALID_PHONE,
                                                                                      VALID_CARRIER,
                                                                                      string.Empty,
                                                                                      VALID_IP,
                                                                                      "wwww.authtwofactorenabletests.com",
                                                                                      string.Empty,
                                                                                      Guid.NewGuid().ToString(),
                                                                                      1);

      AuthTwoFactorEnableResponseData responseData = (AuthTwoFactorEnableResponseData)Engine.Engine.ProcessRequest(requestData, 516);

      Assert.IsTrue(responseData.ValidationCodes.Count == 1);
      Assert.IsTrue(responseData.ValidationCodes.Contains(AuthValidationCodes.ValidateHostNameRequired));
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void EnableTwoFactorEmptyIp()
    {
      AuthTwoFactorEnableRequestData requestData = new AuthTwoFactorEnableRequestData(VALID_DEV_SHOPPER,
                                                                                      VALID_DEV_SHOPPER_PASSWORD,
                                                                                      1,
                                                                                      VALID_PHONE,
                                                                                      VALID_CARRIER,
                                                                                      VALID_HOSTNAME,
                                                                                      string.Empty,
                                                                                      "wwww.authtwofactorenabletests.com",
                                                                                      string.Empty,
                                                                                      Guid.NewGuid().ToString(),
                                                                                      1);

      AuthTwoFactorEnableResponseData responseData = (AuthTwoFactorEnableResponseData)Engine.Engine.ProcessRequest(requestData, 516);

      Assert.IsTrue(responseData.ValidationCodes.Count == 1);
      Assert.IsTrue(responseData.ValidationCodes.Contains(AuthValidationCodes.ValidateIpAddressRequired));
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void EnableTwoFactorValid()
    {
      AuthTwoFactorEnableRequestData requestData = new AuthTwoFactorEnableRequestData(VALID_DEV_SHOPPER,
                                                                                      VALID_DEV_SHOPPER_PASSWORD,
                                                                                      1,
                                                                                      VALID_PHONE,
                                                                                      VALID_CARRIER,
                                                                                      VALID_HOSTNAME,
                                                                                      VALID_IP,
                                                                                      "wwww.authtwofactorenabletests.com",
                                                                                      string.Empty,
                                                                                      Guid.NewGuid().ToString(),
                                                                                      1);

      AuthTwoFactorEnableResponseData responseData = (AuthTwoFactorEnableResponseData)Engine.Engine.ProcessRequest(requestData, 516);

      Debug.WriteLine("StatusCode: " + responseData.StatusCode);
      Console.WriteLine("StatusCode: " + responseData.StatusCode);

      Assert.IsTrue(responseData.ValidationCodes.Count == 0);
      Assert.IsTrue(responseData.StatusCode == TwoFactorWebserviceResponseCodes.Success);
    }
  }
}
