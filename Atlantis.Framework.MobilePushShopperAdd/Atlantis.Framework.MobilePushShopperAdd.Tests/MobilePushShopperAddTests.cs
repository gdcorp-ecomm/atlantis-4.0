﻿using System;
using System.Diagnostics;
using Atlantis.Framework.MobilePushShopperAdd.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.MobilePushShopperAdd.Tests
{
  [TestClass]
  public class MobilePushShopperAddTests
  {
    private void WriteMessageToConsole(string message)
    {
      Console.WriteLine(message);
      Debug.WriteLine(message);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void ValidShopperAddNotification()
    {
      string registrationId = "d3d4d197-638d-47b3-a379-1a9f094cf502";
      string deviceId = "5d022bdf-e080-45b2-8712-4f12eef79922";
      string mobileAppId = "1"; //iPhone

      MobilePushShopperAddRequestData requestData = new MobilePushShopperAddRequestData(registrationId,
                                                                                        mobileAppId,
                                                                                        deviceId,
                                                                                        string.Empty,
                                                                                        0,
                                                                                        "847235", 
                                                                                        "http://www.MobilePushShopperAddTests.com", 
                                                                                        string.Empty, 
                                                                                        Guid.NewGuid().ToString(), 
                                                                                        1);

      try
      {
        MobilePushShopperAddResponseData responseData = (MobilePushShopperAddResponseData)Engine.Engine.ProcessRequest(requestData, 426);
        WriteMessageToConsole(responseData.Xml);
        Assert.IsTrue(responseData.IsSuccess);
      }
      catch (Exception ex)
      {
        WriteMessageToConsole(ex.Message + " " + ex.StackTrace);
        Assert.Fail();
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void ValidShopperAddNotification2()
    {
      string registrationId = "d3d4d197-638d-47b3-a379-1a9f094cf504";
      string deviceId = "5d022bdf-e080-45b2-8712-4f12eef79923";
      string mobileAppId = "1"; //iPhone

      MobilePushShopperAddRequestData requestData = new MobilePushShopperAddRequestData(registrationId,
                                                                                        mobileAppId,
                                                                                        deviceId,
                                                                                        string.Empty,
                                                                                        0,
                                                                                        "840820",
                                                                                        "http://www.MobilePushShopperAddTests.com",
                                                                                        string.Empty,
                                                                                        Guid.NewGuid().ToString(),
                                                                                        1);

      try
      {
        MobilePushShopperAddResponseData responseData = (MobilePushShopperAddResponseData)Engine.Engine.ProcessRequest(requestData, 426);
        WriteMessageToConsole(responseData.Xml);
        Assert.IsTrue(responseData.IsSuccess);
      }
      catch (Exception ex)
      {
        WriteMessageToConsole(ex.Message + " " + ex.StackTrace);
        Assert.Fail();
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void InValidShopperAddNotification()
    {
      string registrationId = "d3d4d197-638d-47b3-a379-1a9f094cf502";
      string deviceId = "5d022bdf-e080-45b2-8712-4f12eef79922";
      string mobileAppId = "1"; //iPhone

      MobilePushShopperAddRequestData requestData = new MobilePushShopperAddRequestData(registrationId,
                                                                                        mobileAppId,
                                                                                        deviceId,
                                                                                        string.Empty,
                                                                                        0,
                                                                                        "asdfjsdafkfjk12341k23jjk1l23klj",
                                                                                        "http://www.MobilePushShopperAddTests.com",
                                                                                        string.Empty,
                                                                                        Guid.NewGuid().ToString(),
                                                                                        1);

      try
      {
        MobilePushShopperAddResponseData responseData = (MobilePushShopperAddResponseData)Engine.Engine.ProcessRequest(requestData, 426);
        WriteMessageToConsole(responseData.Xml);
        Assert.IsTrue(!responseData.IsSuccess);
      }
      catch (Exception ex)
      {
        WriteMessageToConsole(ex.Message + " " + ex.StackTrace);
        Assert.Fail();
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void ShopperDoesNotExistShopperAddNotification()
    {
      string registrationId = "d3d4d197-638d-47b3-a379-1a9f094cf502";
      string deviceId = "5d022bdf-e080-45b2-8712-4f12eef79922";
      string mobileAppId = "1"; //iPhone

      MobilePushShopperAddRequestData requestData = new MobilePushShopperAddRequestData(registrationId,
                                                                                        mobileAppId,
                                                                                        deviceId,
                                                                                        string.Empty,
                                                                                        0,
                                                                                        "84723556234",
                                                                                        "http://www.MobilePushShopperAddTests.com",
                                                                                        string.Empty,
                                                                                        Guid.NewGuid().ToString(),
                                                                                        1);

      try
      {
        MobilePushShopperAddResponseData responseData = (MobilePushShopperAddResponseData)Engine.Engine.ProcessRequest(requestData, 426);
        WriteMessageToConsole(responseData.Xml);
        Assert.IsTrue(!responseData.IsSuccess);
      }
      catch (Exception ex)
      {
        WriteMessageToConsole(ex.Message + " " + ex.StackTrace);
        Assert.Fail();
      }
    }
  }
}
