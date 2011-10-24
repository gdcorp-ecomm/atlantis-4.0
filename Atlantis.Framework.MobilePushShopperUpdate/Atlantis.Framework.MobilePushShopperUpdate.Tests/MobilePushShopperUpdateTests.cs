using System;
using System.Diagnostics;
using Atlantis.Framework.MobilePushShopperUpdate.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.MobilePushShopperUpdate.Tests
{
  [TestClass]
  public class MobilePushShopperUpdateTests
  {
    private void WriteMessageToConsole(string message)
    {
      Console.WriteLine(message);
      Debug.WriteLine(message);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void ValidShopperUpdateNotification()
    {
      int shopperPushId = 13;
      string registrationId = "d3d4d197-638d-47b3-a379-1a9f094cf502";
      string deviceId = "5d022bdf-e080-45b2-8712-4f12eef79922";
      string mobileAppId = "1"; //iPhone

      MobilePushShopperUpdateRequestData requestData = new MobilePushShopperUpdateRequestData(shopperPushId,
                                                                                              registrationId,
                                                                                              mobileAppId,
                                                                                              deviceId,
                                                                                              "847235",
                                                                                              "http://www.MobilePushShopperAddTests.com",
                                                                                              string.Empty,
                                                                                              Guid.NewGuid().ToString(),
                                                                                              1);

      try
      {
        MobilePushShopperUpdateResponseData responseData = (MobilePushShopperUpdateResponseData)Engine.Engine.ProcessRequest(requestData, 430);
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
    public void InValidShopperUpdateNotificationBadId()
    {
      int shopperPushId = -1;
      string registrationId = string.Empty;
      string deviceId = string.Empty;
      string mobileAppId = "1"; //iPhone

      MobilePushShopperUpdateRequestData requestData = new MobilePushShopperUpdateRequestData(shopperPushId,
                                                                                              registrationId,
                                                                                              mobileAppId,
                                                                                              deviceId,
                                                                                              string.Empty,
                                                                                              "http://www.MobilePushShopperAddTests.com",
                                                                                              string.Empty,
                                                                                              Guid.NewGuid().ToString(),
                                                                                              1);

      try
      {
        MobilePushShopperUpdateResponseData responseData = (MobilePushShopperUpdateResponseData)Engine.Engine.ProcessRequest(requestData, 430);
        WriteMessageToConsole(responseData.Xml);
        Assert.IsTrue(responseData.IsSuccess);
      }
      catch (Exception ex)
      {
        WriteMessageToConsole(ex.Message + " " + ex.StackTrace);
        Assert.Fail();
      }
    }
  }
}
