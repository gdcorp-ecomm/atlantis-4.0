using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.GeoByIP.Interface;
using System.Net;

namespace Atlantis.Framework.GeoByIP.Tests
{
  [TestClass]
  public class GeoByIPTests
  {
    // TODO: need to spin up 26 threads to run multiple requests to the GeoCountry
    // Load test will be to get the timing numbers from that and compare the following implementations:
    // Single memory stream with locking on the seeks
    // No locking (memory stream created per lookup)

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.GeoByIP.Impl.dll")]
    [DeploymentItem("GeoIPv6.dat")]
    [DeploymentItem("GeoLiteCityv6.dat")]
    public void CountryLookupBasic()
    {
      GeoByIPRequestData request = new GeoByIPRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "97.74.104.201", LookupTypeEnum.Country);
      GeoByIPResponseData response = (GeoByIPResponseData)Engine.Engine.ProcessRequest(request, 521);
      Assert.IsTrue(response.CountryFound);

      request = new GeoByIPRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "182.50.145.32", LookupTypeEnum.Country);
      response = (GeoByIPResponseData)Engine.Engine.ProcessRequest(request, 521);
      Assert.IsTrue(response.CountryFound);

      request = new GeoByIPRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "::ffff:614a:68c9", LookupTypeEnum.Country);
      response = (GeoByIPResponseData)Engine.Engine.ProcessRequest(request, 521);
      Assert.IsTrue(response.CountryFound);

      request = new GeoByIPRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "::ffff:b632:9120", LookupTypeEnum.Country);
      response = (GeoByIPResponseData)Engine.Engine.ProcessRequest(request, 521);
      Assert.IsTrue(response.CountryFound);

    }
  }
}
