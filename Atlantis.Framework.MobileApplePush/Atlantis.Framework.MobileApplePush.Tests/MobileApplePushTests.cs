using System;
using System.Diagnostics;
using Atlantis.Framework.MobileApplePush.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.MobileApplePush.Tests
{
  [TestClass]
  public class MobileApplePushTests
  {
    private void WriteToConsole(string message)
    {
      Console.WriteLine(message);
      Debug.WriteLine(message);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void SendMultipleValidNotifications()
    {
      string deviceToken = "ad4d631c9574beac6781ae2dceb58eca6047db91dea43981d9e0a27afd8902e8"; // Jonah's phone, he will get this push notification
      MobileApplePushRequestData requestData = new MobileApplePushRequestData(deviceToken, 
                                                                              "MobileApplePushTests", 
                                                                              "847235", 
                                                                              "http://Atlantis.Framework.MobileApplePush.Tests", 
                                                                              string.Empty, 
                                                                              Guid.NewGuid().ToString(), 
                                                                              1);

      requestData.Notification.Payload.AddCustomString("v", "1");
      requestData.Notification.Payload.AddCustomString("i", "danica");
      requestData.Notification.Payload.AddCustomInt("f", 123456);

      try
      {
        MobileApplePushResponseData responseData = (MobileApplePushResponseData)Engine.Engine.ProcessRequest(requestData, 437);
        WriteToConsole(requestData.Notification.Payload.ToJson());
        Assert.IsTrue(responseData.IsSuccess);
      }
      catch (Exception ex)
      {
        WriteToConsole(ex.Message);
        Assert.Fail(ex.Message);
      }

      MobileApplePushRequestData requestData2 = new MobileApplePushRequestData(deviceToken,
                                                                              "MobileApplePushTests2",
                                                                              "847235",
                                                                              "http://Atlantis.Framework.MobileApplePush.Tests",
                                                                              string.Empty,
                                                                              Guid.NewGuid().ToString(),
                                                                              1);

      requestData2.Notification.Payload.AddCustomString("v", "1");
      requestData2.Notification.Payload.AddCustomString("i", "danica");
      requestData2.Notification.Payload.AddCustomInt("f", 123456);

      try
      {
        MobileApplePushResponseData responseData2 = (MobileApplePushResponseData)Engine.Engine.ProcessRequest(requestData2, 437);
        WriteToConsole(requestData2.Notification.Payload.ToJson());
        Assert.IsTrue(responseData2.IsSuccess);
      }
      catch (Exception ex)
      {
        WriteToConsole(ex.Message);
        Assert.Fail(ex.Message);
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void InvalidDeviceTokenNotification()
    {
      string deviceToken = "asdf";
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void PayloadTooLargeNotification()
    {
      string deviceToken = "068adfb999630b00f2d1b1f464c29ba132f5f2164eb9c1ea3e0aa672242626d2"; // Jonah's phone, he will get this push notification

    }
  }
}
