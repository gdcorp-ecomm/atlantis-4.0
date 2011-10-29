using System;
using System.Diagnostics;
using System.Text;
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
      //string deviceToken = "ad4d631c9574beac6781ae2dceb58eca6047db91dea43981d9e0a27afd8902e8"; // Jonah's phone, he will get this push notification
      string deviceToken = "badd631c9574beac6781ae2dceb58eca6047db91dea43981d9e0a27afd8902e8"; // Dummy valid token
      MobileApplePushRequestData requestData = new MobileApplePushRequestData("MobileApplePushTests",
                                                                              deviceToken,
                                                                              "847235", 
                                                                              "http://Atlantis.Framework.MobileApplePush.Tests", 
                                                                              string.Empty, 
                                                                              Guid.NewGuid().ToString(), 
                                                                              1);

      // View mapping "v" key, 4 digit mapping max
      requestData.Notification.Payload.AddCustomInt("v", 1234);

      // ISC code, 15 characters max OR fbiOfferId (fastball), 15 digit max
      //requestData.Notification.Payload.AddCustomString("i", "danica123456789");
      requestData.Notification.Payload.AddCustomInt("f", 123456789012345);

      // ci (click impression), 15 digit max
      requestData.Notification.Payload.AddCustomInt("c", 123456789012345);

      try
      {
        MobileApplePushResponseData responseData = (MobileApplePushResponseData)Engine.Engine.ProcessRequest(requestData, 437);
        WriteToConsole(requestData.Notification.Payload.ToJson());
        WriteToConsole(string.Format("Payload bytes: {0}", Encoding.UTF8.GetBytes(requestData.Notification.Payload.ToJson()).Length));
        Assert.IsTrue(responseData.IsSuccess);
      }
      catch (Exception ex)
      {
        WriteToConsole(ex.Message);
        Assert.Fail(ex.Message);
      }

      MobileApplePushRequestData requestData2 = new MobileApplePushRequestData("123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789 1234567890 1234567890 123456789",
                                                                              9999,
                                                                              deviceToken,
                                                                              "847235",
                                                                              "http://Atlantis.Framework.MobileApplePush.Tests",
                                                                              string.Empty,
                                                                              Guid.NewGuid().ToString(),
                                                                              1);

      // View mapping "v" key, 4 digit mapping max
      requestData2.Notification.Payload.AddCustomInt("v", 1234);

      // ISC code, 15 characters max OR fbiOfferId (fastball), 15 digit max
      requestData2.Notification.Payload.AddCustomString("i", "danica123456789");
      //requestData2.Notification.Payload.AddCustomInt("f", 123456789012345);

      // ci (click impression), 15 digit max
      requestData2.Notification.Payload.AddCustomInt("c", 123456789012345);

      try
      {
        MobileApplePushResponseData responseData2 = (MobileApplePushResponseData)Engine.Engine.ProcessRequest(requestData2, 437);
        WriteToConsole(requestData2.Notification.Payload.ToJson());
        WriteToConsole(string.Format("Payload bytes: {0}", Encoding.UTF8.GetBytes(requestData2.Notification.Payload.ToJson()).Length));
        Assert.IsTrue(responseData2.IsSuccess);
      }
      catch (Exception ex)
      {
        WriteToConsole(ex.Message);
        Assert.Fail(ex.Message);
      }

      MobileApplePushRequestData requestData3 = new MobileApplePushRequestData(9999,
                                                                              deviceToken,
                                                                              "847235",
                                                                              "http://Atlantis.Framework.MobileApplePush.Tests",
                                                                              string.Empty,
                                                                              Guid.NewGuid().ToString(),
                                                                              1);

      // View mapping "v" key, 4 digit mapping max
      requestData3.Notification.Payload.AddCustomInt("v", 1234);

      // ISC code, 15 characters max OR fbiOfferId (fastball), 15 digit max
      requestData3.Notification.Payload.AddCustomString("i", "danica123456789");
      //requestData3.Notification.Payload.AddCustomInt("f", 123456789012345);

      // ci (click impression), 15 digit max
      requestData3.Notification.Payload.AddCustomInt("c", 123456789012345);

      try
      {
        MobileApplePushResponseData responseData3 = (MobileApplePushResponseData)Engine.Engine.ProcessRequest(requestData3, 437);
        WriteToConsole(requestData3.Notification.Payload.ToJson());
        WriteToConsole(string.Format("Payload bytes: {0}", Encoding.UTF8.GetBytes(requestData3.Notification.Payload.ToJson()).Length));
        Assert.IsTrue(responseData3.IsSuccess);
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
