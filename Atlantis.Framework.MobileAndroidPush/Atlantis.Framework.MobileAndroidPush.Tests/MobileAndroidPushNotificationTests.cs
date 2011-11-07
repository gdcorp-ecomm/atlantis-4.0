using System;
using System.Diagnostics;
using Atlantis.Framework.MobileAndroidPush.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.MobileAndroidPush.Tests
{
  [TestClass]
  public class MobileAndroidPushNotificationTests
  {
    private const string EVO_REGISTRATION_ID = "APA91bHYT0Q8md4HR2mpHEIOmXk58XXlOVxSBR4c0vKLy8-oljvt_rtjXqhr05Jz6qY9E8r1bLMCvygo9vbugJy-mHDrHNYywfLDepLwwuctoBVu9vvwHvc";
    private const string BAD_REGISTRATION_ID = "58XXlOVxSBR4c0vKLy8-oljvt_rtjXqhr05Jz6qY9E8r1bLMCvy";
    private const string AUTH_TOKEN = "DQAAAMIAAABhQZiwtn02Ii0rtJP2kUNDZUOrBjugxpPESL6dptNxTxArCfkFJB-emJJAlrYQdvzB5hHecL2u-VYnbJ_k2x-Aw3hGM1kF_W1UTDBKaJYJ_KUp7OVJaInTNc0Zzgde6ktHdK7RCe0URFuPfInKgjr_psDGu5FlqgeQIq6BYwqHrg55E7BHdzrEa32FY29RpfAngk0kO99OQm4VH9-58v4TPC4hPYjvbIVvV2sYePMhjSFwHuITELUR8iQsPcuVT-DYRWkyncg-rRIqvdt-Ts9Q";
    private const string BAD_AUTH_TOKEN = "JJAlrYQdvzB5hHecL2u-VYnbJ_k2x-Aw3hGM1kF_W1UTDBKaJYJ_KUp7O";

    private static void WriteResults(MobileAndroidPushResponseData responseData)
    {
      Console.WriteLine(string.Format("PushId: {0}", responseData.PushId));
      Console.WriteLine(string.Format("Error: {0}", responseData.ErrorType));
      Console.WriteLine(string.Format("Authentication Error: {0}", responseData.AuthenticationError ? "true" : "false"));
      Console.WriteLine(string.Format("Service Unavailable: {0}", responseData.ServiceUnavailable ? "true" : "false"));

      Debug.WriteLine(string.Format("PushId: {0}", responseData.PushId));
      Debug.WriteLine(string.Format("Error: {0}", responseData.ErrorType));
      Debug.WriteLine(string.Format("Authentication Error: {0}", responseData.AuthenticationError ? "true" : "false"));
      Debug.WriteLine(string.Format("Service Unavailable: {0}", responseData.ServiceUnavailable ? "true" : "false"));
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void PushNotificationValid()
    {
      MobileAndroidPushRequestData requestData = new MobileAndroidPushRequestData(AUTH_TOKEN,
                                                                                  EVO_REGISTRATION_ID,
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
    public void PushNotificationBadClientAuth()
    {
      MobileAndroidPushRequestData requestData = new MobileAndroidPushRequestData(BAD_AUTH_TOKEN,
                                                                                  EVO_REGISTRATION_ID,
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
        Assert.IsTrue(responseData.AuthenticationError);
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void PushNotificationEmptyNamespace()
    {
      MobileAndroidPushRequestData requestData = new MobileAndroidPushRequestData(AUTH_TOKEN,
                                                                                  EVO_REGISTRATION_ID,
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
    public void PushNotificationBadRegistrationId()
    {
      MobileAndroidPushRequestData requestData = new MobileAndroidPushRequestData(AUTH_TOKEN,
                                                                                  BAD_REGISTRATION_ID,
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
