using Atlantis.Framework.Localization.Interface;
using Atlantis.Framework.Localization.Tests.Mocks;
using Atlantis.Framework.Providers.Localization.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Localization.Tests
{
  /// <summary>
  /// Summary description for MarketsActiveTests
  /// </summary>
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  [DeploymentItem("Atlantis.Framework.Localization.Impl.dll")]
  public class MarketsActiveTests
  {
    private const int _REQUEST_TYPE = 729;

    [TestMethod]
    public void MarketsActiveResponse_DefaultActiveMarkets_IEnumerable()
    {
      MarketsActiveResponseData response = MarketsActiveResponseData.DefaultMarkets;
      Assert.IsNotNull(response);

      int count = 0;
      foreach (IMarket m in response.Markets)
      {
        count++;

        switch (m.Id)
        {
          case "en-US":
            Assert.AreEqual("English (United States)", m.Description);
            Assert.AreEqual("en-US", m.MsCulture);
            Assert.AreEqual(false, m.IsInternalOnly);
            break;
          case "es-US":
            Assert.AreEqual("Spanish (United States)", m.Description);
            Assert.AreEqual("es-US", m.MsCulture);
            Assert.AreEqual(false, m.IsInternalOnly);
            break;
          case "en-CA":
            Assert.AreEqual("English (Canada)", m.Description);
            Assert.AreEqual("en-CA", m.MsCulture);
            Assert.AreEqual(false, m.IsInternalOnly);
            break;
          case "en-GB":
            Assert.AreEqual("English (United Kingdom)", m.Description);
            Assert.AreEqual("en-GB", m.MsCulture);
            Assert.AreEqual(false, m.IsInternalOnly);
            break;
          case "en-AU":
            Assert.AreEqual("English (Australia)", m.Description);
            Assert.AreEqual("en-AU", m.MsCulture);
            Assert.AreEqual(false, m.IsInternalOnly);
            break;
          case "en-IN":
            Assert.AreEqual("English (India)", m.Description);
            Assert.AreEqual("en-IN", m.MsCulture);
            Assert.AreEqual(false, m.IsInternalOnly);
            break;
          case "qa-QA":
            Assert.AreEqual("QA (Show Tags)", m.Description);
            Assert.AreEqual("en-US", m.MsCulture);
            Assert.AreEqual(true, m.IsInternalOnly);
            break;
          case "qa-PS":
            Assert.AreEqual("QA (Pseudo)", m.Description);
            Assert.AreEqual("en-US", m.MsCulture);
            Assert.AreEqual(true, m.IsInternalOnly);
            break;
          case "qa-PZ":
            Assert.AreEqual("QA (Zs)", m.Description);
            Assert.AreEqual("en-US", m.MsCulture);
            Assert.AreEqual(true, m.IsInternalOnly);
            break;
          default:
            Assert.Fail("Unknown default Market {0}", m.Id);
            break;
        }
      }
      Assert.AreEqual(9, count);
    }

    [TestMethod]
    public void MarketsACtiveResponse_IndexedByCaseInsensitiveCountrySite()
    {
      MarketsActiveResponseData response = MarketsActiveResponseData.DefaultMarkets;
      Assert.IsNotNull(response);

      IMarket m;
      Assert.IsTrue(response.TryGetMarketById("en-us", out m));
      Assert.IsNotNull(m);
      Assert.AreEqual("en-US", m.Id);

      IMarket m2;
      Assert.IsTrue(response.TryGetMarketById("en-Us", out m2));
      Assert.IsNotNull(m);
      Assert.AreEqual("en-US", m.Id);
      Assert.AreSame(m, m2);

      Assert.IsTrue(response.TryGetMarketById("EN-US", out m));
      Assert.IsNotNull(m);
      Assert.AreEqual("en-US", m.Id);
    }

    [TestMethod]
    public void MarketsActiveResponse_UnknownCountrySite_ReturnsNull()
    {
      MarketsActiveResponseData response = MarketsActiveResponseData.DefaultMarkets;
      Assert.IsNotNull(response);

      IMarket m;
      Assert.IsFalse(response.TryGetMarketById("xx-11", out m));
    }

    #region Miscellaneous tests

    [TestMethod]
    public void MarketsActiveRequestDataConstructorGeneratesNewRequestDataObject()
    {
      var request = new MarketsActiveRequestData();
      Assert.IsNotNull(request);
    }

    [TestMethod]
    public void MarketsActiveRequestDataCacheKeySame()
    {
      var request1 = new MarketsActiveRequestData();
      var request2 = new MarketsActiveRequestData();
      Assert.AreNotEqual(request1, request2);
      Assert.IsNotNull(request1);
      Assert.IsNotNull(request2);
      Assert.AreEqual(request1.GetCacheMD5(), request2.GetCacheMD5());
    }

    [TestMethod]
    public void MarketsActiveRequestReturnsValidResponse()
    {
      var request = new MarketsActiveRequestData();
      var response = (MarketsActiveResponseData)Engine.Engine.ProcessRequest(request, _REQUEST_TYPE);
      Assert.IsNotNull(response);
    }

    [TestMethod]
    public void MarketsActiveResponseToXMLReturnsEmptyString()
    {
      var response = MarketsActiveResponseData.DefaultMarkets;
      Assert.AreEqual(string.Empty, response.ToXML());
    }

    [TestMethod]
    public void MarketsActiveResponse_InvalidXml_ReturnsEmptyResponseData()
    {
      const string invalidXml = 
@"<data count=""8"">
	<item marketID=""en-US"" marketDescription=""English - United States"" msCulture=""en-US"" isActive=""-1"" internalOnly=""0"" />
	<item marketID=""es-US"" marketDescription=""Spanish - United States"" isActive=""-1"" internalOnly=""0"" />
</data>";

      MarketsActiveResponseData response = MarketsActiveResponseData.FromCacheDataXml(invalidXml);
      Assert.AreSame(MarketsActiveResponseData.DefaultMarkets, response);

      response = MarketsActiveResponseData.FromCacheDataXml(null);
      Assert.AreSame(MarketsActiveResponseData.DefaultMarkets, response);

      response = MarketsActiveResponseData.FromCacheDataXml(string.Empty);
      Assert.AreSame(MarketsActiveResponseData.DefaultMarkets, response);
    }

    [TestMethod]
    public void MarketsActiveResponse_InvalidMarketIds_NotAddedToCollection()
    {
      const string invalidXml =
@"<data count=""8"">
	<item marketID=""en-US"" marketDescription=""English - United States"" msCulture=""en-US"" isActive=""-1"" internalOnly=""0"" />
	<item marketID="""" marketDescription=""Spanish - United States"" msCulture=""en-US"" isActive=""-1"" internalOnly=""0"" />
  <item marketID=""es-US"" marketDescription=""Spanish - United States"" msCulture=""en-US"" isActive=""-1"" internalOnly=""0"" />
  <item marketDescription=""English - Canada"" msCulture=""en-CA"" isActive=""-1"" internalOnly=""0"" />
  <item marketID=""fr-CA"" marketDescription=""French - Canada"" msCulture=""fr-CA"" isActive=""-1"" internalOnly=""0"" />
</data>";

      MarketsActiveResponseData response = MarketsActiveResponseData.FromCacheDataXml(invalidXml);
      Assert.AreEqual(3, response.Count);
    }
    #endregion
  }
}
