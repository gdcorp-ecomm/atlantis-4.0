using System;
using System.Diagnostics;
using Atlantis.Framework.MobileAndroidPush.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.MobileAndroidPush.Tests
{
  [TestClass]
  public class MobileAndroidPushNotificationTests
  {
    private const string EVO_REGISTRATION_ID = "APA91bHYT0Q8md4HR2mpHEIOmXk58XXlOVxSBR4c0vKLy8-oljvt_rtjXqhr05Jz6qY9E8r1bLMCvygo9vbugJy-mHDrHNYywfLDepLwwuctoBVu9vvwHvc"; // Mark Weaver's Phone
    private const string DROID_X_REGISTRATION_ID = "APA91bEaKrm5ibQwvKG6hJc2JNNSwD0Cxxy0YLz9BNxaeITXj6-eAYYOeCEfA93tEkhr5668wY1fCITNv7GicfGadwI7acbgeW-res7kA71n-bQT9LGn9WqoO_12jbQ0l1Af4IMRX28q5kcA6u45gJd-zeKlYW9GQg"; // Tim Walker's Phone
    private const string BAD_REGISTRATION_ID = "58XXlOVxSBR4c0vKLy8-oljvt_rtjXqhr05Jz6qY9E8r1bLMCvy";

    private static void WriteResults(MobileAndroidPushResponseData responseData)
    {
      Console.WriteLine(string.Format("PushId: {0}", responseData.PushId));
      Console.WriteLine(string.Format("Error: {0}", responseData.ErrorType));

      Debug.WriteLine(string.Format("PushId: {0}", responseData.PushId));
      Debug.WriteLine(string.Format("Error: {0}", responseData.ErrorType));
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("App.config")]
    public void PushNotificationValid()
    {
      MobileAndroidPushRequestData requestData = new MobileAndroidPushRequestData(DROID_X_REGISTRATION_ID,
                                                                                  "MobileAndroidPushNotification Test",
                                                                                  "MobileAndroidPushNotification.Tests",
                                                                                  "847235",
                                                                                  "http://www.MobileAndroidPushNotificationTests.com/",
                                                                                  string.Empty,
                                                                                  Guid.NewGuid().ToString(),
                                                                                  1);

      requestData.Notification.AddItem("v", "view 1");
      requestData.Notification.AddItem("i", "danica");
      requestData.Notification.AddItem("c", "165789");

      try
      {
        MobileAndroidPushResponseData responseData = (MobileAndroidPushResponseData)Engine.Engine.ProcessRequest(requestData, 443);
        WriteResults(responseData);
        Assert.IsTrue(responseData.IsSuccess);
      }
      catch(Exception ex)
      {
        Assert.Fail(ex.Message);   
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("App.config")]
    public void PushNotificationValidMultipleRequests()
    {
      MobileAndroidPushRequestData requestData = new MobileAndroidPushRequestData(EVO_REGISTRATION_ID,
                                                                                  "MobileAndroidPushNotification Test",
                                                                                  "MobileAndroidPushNotification.Tests",
                                                                                  "847235",
                                                                                  "http://www.MobileAndroidPushNotificationTests.com/",
                                                                                  string.Empty,
                                                                                  Guid.NewGuid().ToString(),
                                                                                  1);

      requestData.Notification.AddItem("v", "view 1");
      requestData.Notification.AddItem("i", "danica");
      requestData.Notification.AddItem("c", "165789");

      try
      {
        MobileAndroidPushResponseData responseData = (MobileAndroidPushResponseData)Engine.Engine.ProcessRequest(requestData, 443);
        WriteResults(responseData);
        Assert.IsTrue(responseData.IsSuccess);
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }

      MobileAndroidPushRequestData requestData2 = new MobileAndroidPushRequestData(EVO_REGISTRATION_ID,
                                                                                  "MobileAndroidPushNotification Test",
                                                                                  "MobileAndroidPushNotification.Tests",
                                                                                  "847235",
                                                                                  "http://www.MobileAndroidPushNotificationTests.com/",
                                                                                  string.Empty,
                                                                                  Guid.NewGuid().ToString(),
                                                                                  1);

      requestData2.Notification.AddItem("v", "view 1");
      requestData2.Notification.AddItem("i", "danica");
      requestData2.Notification.AddItem("c", "165789");

      try
      {
        MobileAndroidPushResponseData responseData = (MobileAndroidPushResponseData)Engine.Engine.ProcessRequest(requestData2, 443);
        WriteResults(responseData);
        Assert.IsTrue(responseData.IsSuccess);
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("App.config")]
    public void PushNotificationEmptyNamespace()
    {
      MobileAndroidPushRequestData requestData = new MobileAndroidPushRequestData(EVO_REGISTRATION_ID,
                                                                                  "MobileAndroidPushNotification Test",
                                                                                  string.Empty,
                                                                                  "847235",
                                                                                  "http://www.MobileAndroidPushNotificationTests.com/",
                                                                                  string.Empty,
                                                                                  Guid.NewGuid().ToString(),
                                                                                  1);

      requestData.Notification.AddItem("v", "view 1");
      requestData.Notification.AddItem("i", "danica");
      requestData.Notification.AddItem("c", "165789");

      try
      {
        MobileAndroidPushResponseData responseData = (MobileAndroidPushResponseData)Engine.Engine.ProcessRequest(requestData, 443);
        WriteResults(responseData);
        Assert.IsFalse(responseData.IsSuccess);
        Assert.AreEqual(responseData.ErrorType, MobileAndroidPushErrorTypes.MissingCollapseKey);
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("App.config")]
    public void PushNotificationBadRegistrationId()
    {
      MobileAndroidPushRequestData requestData = new MobileAndroidPushRequestData(BAD_REGISTRATION_ID,
                                                                                  "MobileAndroidPushNotification Test",
                                                                                  "MobileAndroidPushNotification.Tests",
                                                                                  "847235",
                                                                                  "http://www.MobileAndroidPushNotificationTests.com/",
                                                                                  string.Empty,
                                                                                  Guid.NewGuid().ToString(),
                                                                                  1);

      requestData.Notification.AddItem("v", "view 1");
      requestData.Notification.AddItem("i", "danica");
      requestData.Notification.AddItem("c", "165789");

      try
      {
        MobileAndroidPushResponseData responseData = (MobileAndroidPushResponseData)Engine.Engine.ProcessRequest(requestData, 443);
        WriteResults(responseData);
        Assert.IsFalse(responseData.IsSuccess);
        Assert.AreEqual(responseData.ErrorType, MobileAndroidPushErrorTypes.InvalidRegistration);
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }
  }
}
