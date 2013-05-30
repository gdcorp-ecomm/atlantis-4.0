using System;
using System.Diagnostics;
using Atlantis.Framework.MobilePushEmailSubscribe.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.MobilePushEmailSubscribe.Tests
{
  [TestClass]
  public class MobilePushEmailSubscribeTests
  {
    private const string EMAIL_VALID = "test1@smccoyforever.com";
    private const string EMAIL_INVALID = "timbo@notvalid.com";

    private const string REGISTRATION_ID = "aa609c71-1d73-4916-8c8b-216e3840f567";

    private const string CALLBACK_URL = "TEST";  // "https://mob.dev.glbt1.gdg/EmailMobilePushService/PushNotification.ashx";

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.MobilePushEmailSubscribe.Impl.dll")]
    public void SubscribeValid()
    {
      MobilePushEmailSubscribeRequestData requestData = new MobilePushEmailSubscribeRequestData(EMAIL_VALID, REGISTRATION_ID, CALLBACK_URL, "847235", "http://www.MobilePushEmailSubscribeTests.com/", string.Empty, Guid.NewGuid().ToString(), 1,true);
      var responseData = (MobilePushEmailSubscribeResponseData)Engine.Engine.ProcessRequest(requestData, 546);

      Console.WriteLine(string.Format("SubscriptionId: {0}", responseData.SubscriptionId));
      Debug.WriteLine(string.Format("SubscriptionId: {0}", responseData.SubscriptionId));
      Assert.IsTrue(responseData.SubscriptionId != "-1");
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.MobilePushEmailSubscribe.Impl.dll")]
    public void SubscribeInValidEmail()
    {
      try
      {
        MobilePushEmailSubscribeRequestData requestData = new MobilePushEmailSubscribeRequestData(EMAIL_INVALID, REGISTRATION_ID, CALLBACK_URL, "847235", "http://www.MobilePushEmailSubscribeTests.com/", string.Empty, Guid.NewGuid().ToString(), 1);
        Engine.Engine.ProcessRequest(requestData, 546);
      }
      catch(Exception ex)
      {
        // Expected exception
        Console.WriteLine(ex.Message);
        Debug.WriteLine(ex.Message);
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.MobilePushEmailSubscribe.Impl.dll")]
    public void SubscribeEmptyEmail()
    {
      try
      {
        MobilePushEmailSubscribeRequestData requestData = new MobilePushEmailSubscribeRequestData(string.Empty, REGISTRATION_ID, CALLBACK_URL, "847235", "http://www.MobilePushEmailSubscribeTests.com/", string.Empty, Guid.NewGuid().ToString(), 1);
        Engine.Engine.ProcessRequest(requestData, 546);
      }
      catch (Exception ex)
      {
        // Expected exception
        Console.WriteLine(ex.Message);
        Debug.WriteLine(ex.Message);
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.MobilePushEmailSubscribe.Impl.dll")]
    public void SubscribeEmptyRegistrationId()
    {
      try
      {
        MobilePushEmailSubscribeRequestData requestData = new MobilePushEmailSubscribeRequestData(EMAIL_VALID, string.Empty, CALLBACK_URL, "847235", "http://www.MobilePushEmailSubscribeTests.com/", string.Empty, Guid.NewGuid().ToString(), 1);
        Engine.Engine.ProcessRequest(requestData, 546);
      }
      catch (Exception ex)
      {
        // Expected exception
        Console.WriteLine(ex.Message);
        Debug.WriteLine(ex.Message);
      }
    }
  }
}
