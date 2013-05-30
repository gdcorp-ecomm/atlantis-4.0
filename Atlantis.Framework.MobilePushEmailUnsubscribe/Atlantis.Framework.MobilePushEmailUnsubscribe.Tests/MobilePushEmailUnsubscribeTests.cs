using System;
using System.Diagnostics;
using Atlantis.Framework.MobilePushEmailUnsubscribe.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.MobilePushEmailUnsubscribe.Tests
{
  [TestClass]
  public class MobilePushEmailUnsubscribeTests
  {
    private const string EMAIL_VALID = "test1@smccoyforever.com";
    private const string EMAIL_INVALID = "timbo@notvalid.com";

    private const long SUBSCRIPTION_ID_VALID = 482;
    private const long SUBSCRIPTION_ID_INVALID = -2;

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void UnsubscribeValid()
    {
      // NOTE: You have to do a subscribe before running this to get a valid subscriptionId
      MobilePushEmailUnsubscribeRequestData requestData = new MobilePushEmailUnsubscribeRequestData(EMAIL_VALID, SUBSCRIPTION_ID_VALID, "847235", "http://www.MobilePushEmailSubscribeTests.com/", string.Empty, Guid.NewGuid().ToString(), 1);
      MobilePushEmailUnsubscribeResponseData responseData = (MobilePushEmailUnsubscribeResponseData)Engine.Engine.ProcessRequest(requestData, 547);

      Assert.IsTrue(responseData.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void UnsubscribeInValidEmail()
    {
      try
      {
        MobilePushEmailUnsubscribeRequestData requestData = new MobilePushEmailUnsubscribeRequestData(EMAIL_INVALID, SUBSCRIPTION_ID_VALID, "847235", "http://www.MobilePushEmailSubscribeTests.com/", string.Empty, Guid.NewGuid().ToString(), 1);
        MobilePushEmailUnsubscribeResponseData responseData = (MobilePushEmailUnsubscribeResponseData)Engine.Engine.ProcessRequest(requestData, 547);
      }
      catch (Exception ex)
      {
        // Expected
        Debug.WriteLine(ex.Message);
        Console.WriteLine(ex.Message);
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void UnsubscribeInValidSubscriptionId()
    {
      try
      {
        MobilePushEmailUnsubscribeRequestData requestData = new MobilePushEmailUnsubscribeRequestData(EMAIL_VALID, SUBSCRIPTION_ID_INVALID, "847235", "http://www.MobilePushEmailSubscribeTests.com/", string.Empty, Guid.NewGuid().ToString(), 1);
        MobilePushEmailUnsubscribeResponseData responseData = (MobilePushEmailUnsubscribeResponseData)Engine.Engine.ProcessRequest(requestData, 547);
      }
      catch (Exception ex)
      {
        // Expected
        Debug.WriteLine(ex.Message);
        Console.WriteLine(ex.Message);
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void UnsubscribeEmptyEmail()
    {
      try
      {
        MobilePushEmailUnsubscribeRequestData requestData = new MobilePushEmailUnsubscribeRequestData(string.Empty, SUBSCRIPTION_ID_INVALID, "847235", "http://www.MobilePushEmailSubscribeTests.com/", string.Empty, Guid.NewGuid().ToString(), 1);
        MobilePushEmailUnsubscribeResponseData responseData = (MobilePushEmailUnsubscribeResponseData)Engine.Engine.ProcessRequest(requestData, 547);
      }
      catch (Exception ex)
      {
        // Expected
        Debug.WriteLine(ex.Message);
        Console.WriteLine(ex.Message);
      }
    }
  }
}
