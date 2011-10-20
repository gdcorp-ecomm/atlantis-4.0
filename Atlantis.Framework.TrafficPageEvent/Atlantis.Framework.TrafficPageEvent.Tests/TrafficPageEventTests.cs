using System;
using Atlantis.Framework.TrafficPageEvent.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.TrafficPageEvent.Tests
{
  [TestClass]
  public class TrafficPageEventTests
  {
    [TestMethod]
    public void TrafficPageEventValidInput()
    {
      TrafficPageEventRequestData requestData = new TrafficPageEventRequestData("Atlantis.Framework.TestPage.aspx",
                                                                                1,
                                                                                "Test Run",
                                                                                "847235",
                                                                                string.Empty,
                                                                                string.Empty,
                                                                                Guid.NewGuid().ToString(),
                                                                                1);

      requestData.RequestTimeout = new TimeSpan(0, 0, 30);

      requestData.AddCiImpression(2);
      requestData.AddCiImpression(3);
      requestData.AddKeyValuePair("test1", "test1Value");
      requestData.AddKeyValuePair("test2", "test2Value");

      try
      {
        TrafficPageEventResponseData trafficPageEventResponseData = (TrafficPageEventResponseData)Engine.Engine.ProcessRequest(requestData, 166);
        Assert.IsTrue(trafficPageEventResponseData.IsSuccess);
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }

    [TestMethod]
    public void TrafficPageEventInValidInput()
    {
      TrafficPageEventRequestData requestData = new TrafficPageEventRequestData("Atlantis.Framework.TestPage.aspx",
                                                                                -4,
                                                                                "Invalid Test Run",
                                                                                "847235",
                                                                                string.Empty,
                                                                                string.Empty,
                                                                                Guid.NewGuid().ToString(),
                                                                                1);

      requestData.RequestTimeout = new TimeSpan(0, 0, 30);

      requestData.AddCiImpression(-1);
      requestData.AddCiImpression(-2);
      requestData.AddKeyValuePair(null, null);
      requestData.AddKeyValuePair(string.Empty, string.Empty);
    }
  }
}
