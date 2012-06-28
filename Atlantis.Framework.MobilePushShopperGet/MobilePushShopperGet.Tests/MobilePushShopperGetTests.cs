using System;
using System.Diagnostics;
using Atlantis.Framework.Engine;
using Atlantis.Framework.MobilePushShopperGet.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MobilePushShopperGet.Tests
{
  [TestClass]
  public class MobilePushShopperGetTests
  {
    private void WriteMessageToConsole(string message)
    {
      Console.WriteLine(message);
      Debug.WriteLine(message);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void ValidShopperGetNotificationByShopperId()
    {
      MobilePushShopperGetRequestData requestData = new MobilePushShopperGetRequestData("847235",
                                                                                        MobilePushShopperGetType.Shopper,
                                                                                        "847235",
                                                                                        "http://www.MobilePushShopperAddTests.com",
                                                                                        string.Empty,
                                                                                        Guid.NewGuid().ToString(),
                                                                                        1);

      try
      {
        MobilePushShopperGetResponseData responseData = (MobilePushShopperGetResponseData)Engine.ProcessRequest(requestData, 429);
        WriteMessageToConsole(responseData.Xml);
        Assert.IsTrue(responseData.IsSuccess);
        Assert.IsTrue(responseData.Records.Count > 0);
      }
      catch (Exception ex)
      {
        WriteMessageToConsole(ex.Message + " " + ex.StackTrace);
        Assert.Fail();
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void ValidShopperGetNotificationByPushEmail()
    {
      MobilePushShopperGetRequestData requestData = new MobilePushShopperGetRequestData("trwalker@godaddy.com",
                                                                                        MobilePushShopperGetType.Email,
                                                                                        "847235",
                                                                                        "http://www.MobilePushShopperAddTests.com",
                                                                                        string.Empty,
                                                                                        Guid.NewGuid().ToString(),
                                                                                        1);

      try
      {
        MobilePushShopperGetResponseData responseData = (MobilePushShopperGetResponseData)Engine.ProcessRequest(requestData, 429);
        WriteMessageToConsole(responseData.Xml);
        Assert.IsTrue(responseData.IsSuccess);
        Assert.IsTrue(responseData.Records.Count > 0);
      }
      catch (Exception ex)
      {
        WriteMessageToConsole(ex.Message + " " + ex.StackTrace);
        Assert.Fail();
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void ValidShopperGetNotificationByRegistrationId()
    {
      string registrationId = "ad4d631c9574beac6781ae2dceb58eca6047db91dea43981d9e0a27afd8902e8";

      MobilePushShopperGetRequestData requestData = new MobilePushShopperGetRequestData(registrationId,
                                                                                        MobilePushShopperGetType.RegistrationId,
                                                                                        "847235",
                                                                                        "http://www.MobilePushShopperAddTests.com",
                                                                                        string.Empty,
                                                                                        Guid.NewGuid().ToString(),
                                                                                        1);

      try
      {
        MobilePushShopperGetResponseData responseData = (MobilePushShopperGetResponseData)Engine.ProcessRequest(requestData, 429);
        WriteMessageToConsole(responseData.Xml);
        Assert.IsTrue(responseData.IsSuccess);
        Assert.IsTrue(responseData.Records.Count > 0);
      }
      catch (Exception ex)
      {
        WriteMessageToConsole(ex.Message + " " + ex.StackTrace);
        Assert.Fail();
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void InValidShopperGetNotificationByShopperId()
    {
      MobilePushShopperGetRequestData requestData = new MobilePushShopperGetRequestData("56984521",
                                                                                        MobilePushShopperGetType.Shopper,
                                                                                        "56984521",
                                                                                        "http://www.MobilePushShopperAddTests.com",
                                                                                        string.Empty,
                                                                                        Guid.NewGuid().ToString(),
                                                                                        1);

      try
      {
        MobilePushShopperGetResponseData responseData = (MobilePushShopperGetResponseData)Engine.ProcessRequest(requestData, 429);
        WriteMessageToConsole(responseData.Xml);
        Assert.IsTrue(responseData.IsSuccess);
        Assert.IsTrue(responseData.Records.Count == 0);
      }
      catch (Exception ex)
      {
        WriteMessageToConsole(ex.Message + " " + ex.StackTrace);
        Assert.Fail();
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void InValidShopperGetNotificationByRegistrationId()
    {
      string registrationId = "asdf";

      MobilePushShopperGetRequestData requestData = new MobilePushShopperGetRequestData(registrationId,
                                                                                        MobilePushShopperGetType.RegistrationId,
                                                                                        "847235",
                                                                                        "http://www.MobilePushShopperAddTests.com",
                                                                                        string.Empty,
                                                                                        Guid.NewGuid().ToString(),
                                                                                        1);

      try
      {
        MobilePushShopperGetResponseData responseData = (MobilePushShopperGetResponseData)Engine.ProcessRequest(requestData, 429);
        WriteMessageToConsole(responseData.Xml);
        Assert.IsTrue(responseData.IsSuccess);
        Assert.IsTrue(responseData.Records.Count == 0);
      }
      catch (Exception ex)
      {
        WriteMessageToConsole(ex.Message + " " + ex.StackTrace);
        Assert.Fail();
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void InValidShopperGetNotificationNoShopperOrRegistrationId()
    {
      string registrationId = "asdf";

      MobilePushShopperGetRequestData requestData = new MobilePushShopperGetRequestData(string.Empty,
                                                                                        MobilePushShopperGetType.Shopper,
                                                                                        string.Empty,
                                                                                        "http://www.MobilePushShopperAddTests.com",
                                                                                        string.Empty,
                                                                                        Guid.NewGuid().ToString(),
                                                                                        1);

      try
      {
        MobilePushShopperGetResponseData responseData = (MobilePushShopperGetResponseData)Engine.ProcessRequest(requestData, 429);

        // The line above should have thrown an exception
        Assert.Fail();
      }
      catch (Exception ex)
      {
        WriteMessageToConsole(ex.Message + " " + ex.StackTrace);
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void BadDataShopperGetNotification()
    {
      MobilePushShopperGetRequestData requestData = new MobilePushShopperGetRequestData("sfkjsjklfsal",
                                                                                        MobilePushShopperGetType.Shopper,
                                                                                        "sfkjsjklfsal",
                                                                                        "http://www.MobilePushShopperAddTests.com",
                                                                                        string.Empty,
                                                                                        Guid.NewGuid().ToString(),
                                                                                        1);

      try
      {
        MobilePushShopperGetResponseData responseData = (MobilePushShopperGetResponseData)Engine.ProcessRequest(requestData, 429);
        WriteMessageToConsole(responseData.Xml);
        Assert.IsFalse(responseData.IsSuccess);
      }
      catch (Exception ex)
      {
        WriteMessageToConsole(ex.Message + " " + ex.StackTrace);
        Assert.Fail();
      }
    }
  }
}
