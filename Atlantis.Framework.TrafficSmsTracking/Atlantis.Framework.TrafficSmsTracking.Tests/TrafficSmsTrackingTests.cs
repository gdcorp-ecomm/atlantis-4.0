using System;
using Atlantis.Framework.TrafficSmsTracking.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.TrafficSmsTracking.Tests
{
  [TestClass]
  public class TrafficSmsTrackingTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void MakeValidSmsTrackingRequest()
    { 
      TrafficSmsTrackingRequestData requestData = new TrafficSmsTrackingRequestData(Guid.NewGuid(),
                                                                                    Guid.NewGuid(), 
                                                                                    "14801234567",
                                                                                    "AT&T",
                                                                                    "Domain Search",
                                                                                    "check somedomaintosearch.com",
                                                                                    "somedomaintosearch.com is available",
                                                                                    DateTime.Now.Subtract(TimeSpan.FromSeconds(2)),
                                                                                    DateTime.Now,
                                                                                    "somedomaintosearch",
                                                                                    "com",
                                                                                    true,
                                                                                    "somedomaintosearch",
                                                                                    "com",
                                                                                    "847235",
                                                                                    "http://sms.godaddymobile.com/sms.ashx",
                                                                                    string.Empty,
                                                                                    string.Empty,
                                                                                    0);

      try
      {
        TrafficSmsTrackingResponseData responseData = (TrafficSmsTrackingResponseData)Engine.Engine.ProcessRequest(requestData, 328);

        Assert.IsTrue(responseData.IsSuccess);
        Console.WriteLine("SmsId: " + requestData.SmsId);
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }
  }
}
