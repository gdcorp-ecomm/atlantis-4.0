using System;
using Atlantis.Framework.FastballEventMetrics.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.FastballEventMetrics.Test
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetFastballEventMetricsTests
  {

    private const string _shopperId = "855307";
    private const string _orderId = "1466738";
    private int privatelabelid = 250056;

    private const int _requestType = 579;

    public GetFastballEventMetricsTests()
    {
      //
      // TODO: Add constructor logic here
      //
    }



    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void FastballEventMetricsTest()
    {
      string metricname = "TurnkeyResellerLogsintoControlPanel";
      string metricvalue = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");


      FastballEventMetricsRequestData request = new FastballEventMetricsRequestData(_shopperId
           , string.Empty
           , _orderId
           , string.Empty
           , 0
           , privatelabelid, metricname, metricvalue);
        FastballEventMetricsResponseData response = (FastballEventMetricsResponseData)Engine.Engine.ProcessRequest(request, _requestType);
        Assert.IsTrue(response.IsSuccess && response.ResponseXML.Length > 0, "Service call returned data.");

        System.Diagnostics.Debug.WriteLine(response.ResponseXML);

    }
  }
}
