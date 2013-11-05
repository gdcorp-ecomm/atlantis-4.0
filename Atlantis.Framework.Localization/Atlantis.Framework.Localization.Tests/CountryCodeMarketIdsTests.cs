using Atlantis.Framework.Localization.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Localization.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  [DeploymentItem("Atlantis.Framework.Localization.Impl.dll")]
  public class CountryCodeMarketIdsTests
  {
    private const int REQUEST_TYPE = 759;

    [TestMethod]
    public void CountryCodeWithMarkets()
    {
      var request = new CountryCodeMarketIdsRequestData("US");
      var response = (CountryCodeMarketIdsResponseData)Engine.Engine.ProcessRequest(request, REQUEST_TYPE);

      Assert.IsTrue(response.HasMarketIds);
      foreach (var marketId in response.MarketIds)
      {
        Assert.IsTrue(marketId.IndexOf('-').Equals(2));
      }
    }

    [TestMethod]
    public void CountryCodeWithNoMarkets()
    {
      var request = new CountryCodeMarketIdsRequestData("XX");
      var response = (CountryCodeMarketIdsResponseData)Engine.Engine.ProcessRequest(request, REQUEST_TYPE);

      Assert.IsFalse(response.HasMarketIds);
    }

    [TestMethod]
    public void EmptyStringCacheResponse()
    {
      var response = CountryCodeMarketIdsResponseData.FromCacheDataXml(string.Empty);
      Assert.IsFalse(response.HasMarketIds);
      Assert.IsTrue(string.IsNullOrEmpty(response.ToXML()));
    }

    [TestMethod]
    public void InvalidCacheResponse()
    {
      var response = CountryCodeMarketIdsResponseData.FromCacheDataXml("Here's a bunch of crap");
      Assert.IsFalse(response.HasMarketIds);
    }
  }
}
