using System;
using Atlantis.Framework.TrafficMobileTracking.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.TrafficMobileTracking.Tests
{
  [TestClass]
  public class TrafficMobileTrackingTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void MobileTrafficTrackingRequestWithoutCiCode()
    {
      TrafficMobileTrackingRequestData requestData = new TrafficMobileTrackingRequestData("847235",
                                                                                          1,
                                                                                          Guid.NewGuid().ToString(),
                                                                                          1,
                                                                                          "3",
                                                                                          "iPhone",
                                                                                          "3.0",
                                                                                          "iPhone",
                                                                                          "Mobile",
                                                                                          "Unit Test MobileTrafficTrackingRequestWithoutCiCode",
                                                                                          string.Empty,
                                                                                          string.Empty,
                                                                                          string.Empty,
                                                                                          string.Empty,
                                                                                          string.Empty,
                                                                                          string.Empty,
                                                                                          1);

      requestData.RequestTimeout = TimeSpan.FromSeconds(30);

      try
      {
        TrafficMobileTrackingResponseData responseData = (TrafficMobileTrackingResponseData)Engine.Engine.ProcessRequest(requestData, 203);
        Assert.IsTrue(responseData.IsSuccess);
      }
      catch (Exception ex)
      {
       Assert.Fail(ex.Message); 
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void MobileTrafficTrackingRequestWithCiCode()
    {
      TrafficMobileTrackingRequestData requestData = new TrafficMobileTrackingRequestData("847235",
                                                                                          1,
                                                                                          Guid.NewGuid().ToString(),
                                                                                          1,
                                                                                          "3",
                                                                                          "iPhone",
                                                                                          "3.0",
                                                                                          "iPhone",
                                                                                          "Mobile",
                                                                                          "Unit Test MobileTrafficTrackingRequestWithoutCiCode",
                                                                                          string.Empty,
                                                                                          string.Empty,
                                                                                          string.Empty,
                                                                                          1234,
                                                                                          string.Empty,
                                                                                          string.Empty,
                                                                                          string.Empty,
                                                                                          1);

      requestData.RequestTimeout = TimeSpan.FromSeconds(30);

      try
      {
        TrafficMobileTrackingResponseData responseData = (TrafficMobileTrackingResponseData)Engine.Engine.ProcessRequest(requestData, 203);
        Assert.IsTrue(responseData.IsSuccess);
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }
  }
}
