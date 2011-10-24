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
      string registrationId = "d3d4d197-638d-47b3-a379-1a9f094cf502";

      MobilePushShopperGetRequestData requestData = new MobilePushShopperGetRequestData(registrationId,
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
