using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    [DeploymentItem("GeoIPv6.dat")]
    [DeploymentItem("GeoLiteCityv6.dat")]
    public void TestMethod1()
    {
    }
  }
}
