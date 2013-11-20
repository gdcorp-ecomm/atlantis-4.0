using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.Localization.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Localization.Tests
{
  /// <summary>
  /// Summary description for MarketMappingsTests
  /// </summary>
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  [DeploymentItem("Atlantis.Framework.Localization.Impl.dll")]
  public class MarketMappingsTests
  {
    private const int _REQUEST_TYPE = 761;

    [TestMethod]
    public void MarketMappingsResponse_BaseTests()
    {
      var request = new MarketMappingsRequestData("en-us");
      var response = (MarketMappingsResponseData)Engine.Engine.ProcessRequest(request, _REQUEST_TYPE);
      Assert.IsTrue(response.IsMappedCountrySite("www", false));
      Assert.IsTrue(response.IsMappedCountrySite("www", true));
      Assert.IsTrue(response.IsPublicMapping("www"));
      Assert.AreEqual("en", response.GetUrlLanguageForCountrySite("www", true).ToLower());
      Assert.AreEqual("en", response.GetUrlLanguageForCountrySite("www", false).ToLower());
      Assert.IsFalse(response.IsMappedCountrySite("xx", false));
      Assert.IsFalse(response.IsMappedCountrySite("xx", true));
      Assert.IsFalse(response.IsPublicMapping("xx"));
      Assert.AreEqual(string.Empty, response.GetUrlLanguageForCountrySite("xx", true).ToLower());
      Assert.AreEqual(string.Empty, response.GetUrlLanguageForCountrySite("xx", false).ToLower());
    }

    [TestMethod]
    public void MarketMappingsResponse_CaseInsensitiveKeys()
    {
      var request = new MarketMappingsRequestData("en-Us");
      var response = (MarketMappingsResponseData)Engine.Engine.ProcessRequest(request, _REQUEST_TYPE);
      Assert.IsTrue(response.IsMappedCountrySite("Www", false));
      Assert.IsTrue(response.IsMappedCountrySite("WWW", true));
      Assert.IsTrue(response.IsPublicMapping("www"));
      Assert.AreEqual("en", response.GetUrlLanguageForCountrySite("www", true).ToLower());
      Assert.AreEqual("en", response.GetUrlLanguageForCountrySite("WwW", false).ToLower());
    }

    [TestMethod]
    public void MarketMappingsResponse_NoMappings()
    {
      var request = new MarketMappingsRequestData("NOMAPPINGS");
      var response = (MarketMappingsResponseData)Engine.Engine.ProcessRequest(request, _REQUEST_TYPE);
      Assert.IsFalse(response.IsMappedCountrySite("www", false));
      Assert.IsFalse(response.IsMappedCountrySite("www", true));
      Assert.IsFalse(response.IsPublicMapping("www"));
      Assert.AreEqual(string.Empty, response.GetUrlLanguageForCountrySite("www", true).ToLower());
      Assert.AreEqual(string.Empty, response.GetUrlLanguageForCountrySite("www", false).ToLower());
    }

    [TestMethod]
    public void MarketMappingsRequestData_CacheKeyUnique()
    {
      var request1 = new MarketMappingsRequestData("en-US");
      var request2 = new MarketMappingsRequestData("pt-BR");
      Assert.IsNotNull(request1);
      Assert.IsNotNull(request2);
      Assert.AreNotEqual(request1, request2);
      Assert.AreNotEqual(request1.GetCacheMD5(), request2.GetCacheMD5());

      request1 = new MarketMappingsRequestData("en-US");
      request2 = new MarketMappingsRequestData("en-us");
      Assert.IsNotNull(request1);
      Assert.IsNotNull(request2);
      Assert.AreNotEqual(request1, request2);
      Assert.AreEqual(request1.GetCacheMD5(), request2.GetCacheMD5());
    }

    [TestMethod]
    public void MarketMappingsResponse_GetUrlLanguageForCountrySite()
    {
      string mappingXml =
        @"<data count=""1"">
        <item catalog_countrySite=""www"" catalog_marketID=""en-US"" languageURLSegment=""en"" isActive=""1"" internalOnly=""0""/>
        <item catalog_countrySite=""rc"" catalog_marketID=""en-US"" languageURLSegment=""rc-en"" isActive=""1"" internalOnly=""0""/>
        <item catalog_countrySite=""xxx"" catalog_marketID=""en-US"" languageURLSegment=""xxx-en"" isActive=""1"" internalOnly=""1""/>
        <item catalog_countrySite=""yyy"" catalog_marketID=""en-US"" languageURLSegment=""yyy-en"" isActive=""1"" internalOnly=""0""/>
      </data>";

      MarketMappingsResponseData response = MarketMappingsResponseData.FromCacheDataXml(mappingXml);
      Assert.AreEqual("rc-en", response.GetUrlLanguageForCountrySite("rc", false).ToLowerInvariant());
      Assert.AreEqual("en", response.GetUrlLanguageForCountrySite("www", false).ToLowerInvariant());
      Assert.AreEqual("yyy-en", response.GetUrlLanguageForCountrySite("yyy", false).ToLowerInvariant());
      Assert.AreEqual("xxx-en", response.GetUrlLanguageForCountrySite("xxx", true).ToLowerInvariant());
      Assert.AreEqual(string.Empty, response.GetUrlLanguageForCountrySite("xxx", false).ToLowerInvariant());
    }

    [TestMethod]
    public void MarketMappingsResponse_GetFirstMappedCountrySiteId()
    {
      string mappingXml =
        @"<data count=""1"">
        <item catalog_countrySite=""xxx"" catalog_marketID=""en-US"" languageURLSegment=""xxx-en"" isActive=""1"" internalOnly=""1""/>
        <item catalog_countrySite=""www"" catalog_marketID=""en-US"" languageURLSegment=""en"" isActive=""1"" internalOnly=""0""/>
        <item catalog_countrySite=""rc"" catalog_marketID=""en-US"" languageURLSegment=""rc-en"" isActive=""1"" internalOnly=""0""/>
        <item catalog_countrySite=""yyy"" catalog_marketID=""en-US"" languageURLSegment=""yyy-en"" isActive=""1"" internalOnly=""0""/>
      </data>";

      MarketMappingsResponseData response = MarketMappingsResponseData.FromCacheDataXml(mappingXml);
      Assert.AreEqual("www", response.GetFirstMappedCountrySiteId(false).ToLowerInvariant());
      Assert.AreEqual("xxx", response.GetFirstMappedCountrySiteId(true).ToLowerInvariant());
      Assert.AreNotEqual("xxx", response.GetFirstMappedCountrySiteId(false).ToLowerInvariant());
    }

    [TestMethod]
    public void MarketMappingsResponse_IsMappedCountrySite()
    {
      string mappingXml =
        @"<data count=""1"">
        <item catalog_countrySite=""xxx"" catalog_marketID=""en-US"" languageURLSegment=""xxx-en"" isActive=""1"" internalOnly=""1""/>
        <item catalog_countrySite=""www"" catalog_marketID=""en-US"" languageURLSegment=""en"" isActive=""1"" internalOnly=""0""/>
        <item catalog_countrySite=""rc"" catalog_marketID=""en-US"" languageURLSegment=""rc-en"" isActive=""1"" internalOnly=""0""/>
        <item catalog_countrySite=""yyy"" catalog_marketID=""en-US"" languageURLSegment=""yyy-en"" isActive=""1"" internalOnly=""0""/>
      </data>";

      MarketMappingsResponseData response = MarketMappingsResponseData.FromCacheDataXml(mappingXml);
      Assert.IsTrue(response.IsMappedCountrySite("rc", false));
      Assert.IsFalse(response.IsMappedCountrySite("xxx", false));
      Assert.IsTrue(response.IsMappedCountrySite("xxx", true));
    }

    [TestMethod]
    [ExpectedException(typeof(System.ArgumentNullException))]
    public void MarketMappingsResponse_NullXml()
    {
      MarketMappingsResponseData response = MarketMappingsResponseData.FromCacheDataXml(null);
    }

    [TestMethod]
    [ExpectedException(typeof(System.Xml.XmlException))]
    public void MarketMappingsResponse_InvalidXml()
    {
      MarketMappingsResponseData response = MarketMappingsResponseData.FromCacheDataXml("<dataItem>");
    }
  }
}
