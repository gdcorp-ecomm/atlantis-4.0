using Atlantis.Framework.Localization.Interface;
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
  public class CountrySiteMarketMappingsTests
  {
    private const int _REQUEST_TYPE = 730;

    [TestMethod]
    public void CountrySiteMarketMappingsResponse_DefaultMappings()
    {
      var request = new CountrySiteMarketMappingsRequestData("www");
      var response = (CountrySiteMarketMappingsResponseData)Engine.Engine.ProcessRequest(request, _REQUEST_TYPE);

      string marketId;

      Assert.IsTrue(response.IsValidUrlLanguageForCountrySite("en", false));
      Assert.IsTrue(response.TryGetMarketIdByCountrySiteAndUrlLanguage("en", out marketId));
      Assert.AreEqual("en-US", marketId);
      Assert.IsTrue(response.IsValidUrlLanguageForCountrySite("es", false));
      Assert.IsTrue(response.TryGetMarketIdByCountrySiteAndUrlLanguage("es", out marketId));
      Assert.AreEqual("es-US", marketId);
      Assert.IsFalse(response.IsValidUrlLanguageForCountrySite("qa-qa", false));
      Assert.IsTrue(response.IsValidUrlLanguageForCountrySite("qa-qa", true));
      Assert.IsTrue(response.TryGetMarketIdByCountrySiteAndUrlLanguage("qa-qa", out marketId));
      Assert.AreEqual("qa-QA", marketId);
      Assert.IsFalse(response.IsValidUrlLanguageForCountrySite("qa-ps", false));
      Assert.IsTrue(response.IsValidUrlLanguageForCountrySite("qa-ps", true));
      Assert.IsTrue(response.TryGetMarketIdByCountrySiteAndUrlLanguage("qa-ps", out marketId));
      Assert.AreEqual("qa-PS", marketId);

      request = new CountrySiteMarketMappingsRequestData("ca");
      response = (CountrySiteMarketMappingsResponseData)Engine.Engine.ProcessRequest(request, _REQUEST_TYPE);
      Assert.IsTrue(response.IsValidUrlLanguageForCountrySite("en", false));
      Assert.IsTrue(response.TryGetMarketIdByCountrySiteAndUrlLanguage("en", out marketId));
      Assert.AreEqual("en-CA", marketId);

      request = new CountrySiteMarketMappingsRequestData("uk");
      response = (CountrySiteMarketMappingsResponseData)Engine.Engine.ProcessRequest(request, _REQUEST_TYPE);
      Assert.IsTrue(response.IsValidUrlLanguageForCountrySite("en", true));
      Assert.IsTrue(response.TryGetMarketIdByCountrySiteAndUrlLanguage("en", out marketId));
      Assert.AreEqual("en-GB", marketId);

      request = new CountrySiteMarketMappingsRequestData("au");
      response = (CountrySiteMarketMappingsResponseData)Engine.Engine.ProcessRequest(request, _REQUEST_TYPE);
      Assert.IsTrue(response.IsValidUrlLanguageForCountrySite("en", false));
      Assert.IsTrue(response.TryGetMarketIdByCountrySiteAndUrlLanguage("en", out marketId));
      Assert.AreEqual("en-AU", marketId);

      request = new CountrySiteMarketMappingsRequestData("in");
      response = (CountrySiteMarketMappingsResponseData)Engine.Engine.ProcessRequest(request, _REQUEST_TYPE);
      Assert.IsTrue(response.IsValidUrlLanguageForCountrySite("en", true));
      Assert.IsTrue(response.TryGetMarketIdByCountrySiteAndUrlLanguage("en", out marketId));
      Assert.AreEqual("en-IN", marketId);
    }

    [TestMethod]
    public void CountrySiteMarketMappingsResponse_CaseInsensitiveKeys()
    {
      var request = new CountrySiteMarketMappingsRequestData("ca");
      var response = (CountrySiteMarketMappingsResponseData)Engine.Engine.ProcessRequest(request, _REQUEST_TYPE);

      string marketId;

      Assert.IsTrue(response.IsValidUrlLanguageForCountrySite("en", false));
      Assert.IsTrue(response.TryGetMarketIdByCountrySiteAndUrlLanguage("en", out marketId));
      Assert.AreEqual("en-CA", marketId);
      Assert.IsTrue(response.IsValidUrlLanguageForCountrySite("EN", false));
      Assert.IsTrue(response.TryGetMarketIdByCountrySiteAndUrlLanguage("EN", out marketId));
      Assert.AreEqual("en-CA", marketId);
      Assert.IsTrue(response.IsValidUrlLanguageForCountrySite("en", true));
      Assert.IsTrue(response.TryGetMarketIdByCountrySiteAndUrlLanguage("en", out marketId));
      Assert.AreEqual("en-CA", marketId);
      Assert.IsTrue(response.IsValidUrlLanguageForCountrySite("eN", true));
      Assert.IsTrue(response.TryGetMarketIdByCountrySiteAndUrlLanguage("En", out marketId));
      Assert.AreEqual("en-CA", marketId);
      Assert.IsTrue(response.IsValidUrlLanguageForCountrySite("EN", false));
      Assert.IsTrue(response.TryGetMarketIdByCountrySiteAndUrlLanguage("en", out marketId));
      Assert.AreEqual("en-CA", marketId);
    }

    [TestMethod]
    public void CountrySiteMarketMappingsResponse_UnknownKeys()
    {
      var request = new CountrySiteMarketMappingsRequestData("xx");
      var response = (CountrySiteMarketMappingsResponseData)Engine.Engine.ProcessRequest(request, _REQUEST_TYPE);

      string marketId;

      Assert.IsFalse(response.IsValidUrlLanguageForCountrySite("en", true));
      Assert.IsFalse(response.TryGetMarketIdByCountrySiteAndUrlLanguage("en", out marketId));
      Assert.AreEqual(string.Empty, marketId);

      request = new CountrySiteMarketMappingsRequestData("www");
      response = (CountrySiteMarketMappingsResponseData)Engine.Engine.ProcessRequest(request, _REQUEST_TYPE);
      Assert.IsFalse(response.IsValidUrlLanguageForCountrySite("xx", true));
      Assert.IsFalse(response.TryGetMarketIdByCountrySiteAndUrlLanguage("xx", out marketId));
      Assert.AreEqual(string.Empty, marketId);

    }

    #region Miscellaneous tests

    [TestMethod]
    public void CountrySiteMarketMappingsRequestData_ConstructorGeneratesNewRequestDataObject()
    {
      var request = new CountrySiteMarketMappingsRequestData("www");
      Assert.IsNotNull(request);
    }

    [TestMethod]
    public void CountrySiteMarketMappingsRequestData_CacheKeyUnique()
    {
      var request1 = new CountrySiteMarketMappingsRequestData("www");
      var request2 = new CountrySiteMarketMappingsRequestData("ca");
      Assert.IsNotNull(request1);
      Assert.IsNotNull(request2);
      Assert.AreNotEqual(request1, request2);
      Assert.AreNotEqual(request1.GetCacheMD5(), request2.GetCacheMD5());
    }

    [TestMethod]
    public void CountrySiteMarketMappingsRequestData_ReturnsValidResponse()
    {
      var request = new CountrySiteMarketMappingsRequestData("www");
      var response = (CountrySiteMarketMappingsResponseData)Engine.Engine.ProcessRequest(request, _REQUEST_TYPE);
      Assert.IsNotNull(response);
      string marketId;
      response.TryGetMarketIdByCountrySiteAndUrlLanguage("en", out marketId);
      Assert.AreEqual("EN-US", marketId.ToUpperInvariant());
    }

    [TestMethod]
    public void CountrySiteMarketMappingsResponseData_ToXMLReturnsEmptyString()
    {
      var request = new CountrySiteMarketMappingsRequestData("www");
      var response = (CountrySiteMarketMappingsResponseData)Engine.Engine.ProcessRequest(request, _REQUEST_TYPE);
      Assert.AreEqual(string.Empty, response.ToXML());
    }

    [TestMethod]
    public void CountrySiteMarketMappingsResponseData_InvalidXml_ReturnsNoMappingsResponse()
    {
      const string invalidXml =
@"<data count=""3"">
	<item catalog_countrySite=""www"" catalog_marketID=""en-US"" languageURLSegment=""en"" isActive=""1"" internalOnly=""0"" />
	<item catalog_countrySite=""www"" catalog_marketID="""" languageURLSegment=""es"" isActive=""1"" internalOnly=""0"" />
	<item catalog_countrySite=""ca"" languageURLSegment=""fr"" isActive=""1"" internalOnly=""0"" />
</data>";

      CountrySiteMarketMappingsResponseData response = CountrySiteMarketMappingsResponseData.FromCacheDataXml(invalidXml);
      Assert.AreSame(CountrySiteMarketMappingsResponseData.NoMappingsResponse, response);

      response = CountrySiteMarketMappingsResponseData.FromCacheDataXml(null);
      Assert.AreSame(CountrySiteMarketMappingsResponseData.NoMappingsResponse, response);

      response = CountrySiteMarketMappingsResponseData.FromCacheDataXml(string.Empty);
      Assert.AreSame(CountrySiteMarketMappingsResponseData.NoMappingsResponse, response);
    }

    [TestMethod]
    public void CountrySiteMarketMappingsResponseData_InvalidKeyValues_NotAddedToResponseData()
    {
      const string invalidXml =
@"<data count=""8"">
	<item catalog_countrySite=""www"" catalog_marketID=""en-US"" languageURLSegment="""" isActive=""1"" internalOnly=""0"" />
	<item catalog_countrySite=""www"" catalog_marketID=""es-US"" languageURLSegment=""es"" isActive=""1"" internalOnly=""0"" />
	<item catalog_countrySite=""www"" catalog_marketID=""en-US"" languageURLSegment=""en"" isActive=""1"" internalOnly=""0"" />
</data>";

      CountrySiteMarketMappingsResponseData response = CountrySiteMarketMappingsResponseData.FromCacheDataXml(invalidXml);

      string marketId;
      Assert.IsTrue(response.IsValidUrlLanguageForCountrySite("en", false));
      Assert.IsTrue(response.TryGetMarketIdByCountrySiteAndUrlLanguage("en", out marketId));
      Assert.AreEqual("en-US", marketId);
      Assert.IsTrue(response.IsValidUrlLanguageForCountrySite("es", false));
      Assert.IsTrue(response.TryGetMarketIdByCountrySiteAndUrlLanguage("es", out marketId));
      Assert.AreEqual("es-US", marketId);
    }

    [TestMethod]
    public void CountrySiteMarketMappingsResponseData_DuplicateKeyValues_LastOneWins()
    {
      const string invalidXml =
@"<data count=""8"">
	<item catalog_countrySite=""www"" catalog_marketID=""en-XX"" languageURLSegment=""en"" isActive=""1"" internalOnly=""0"" />
	<item catalog_countrySite=""www"" catalog_marketID=""es-XX"" languageURLSegment=""es"" isActive=""1"" internalOnly=""0"" />
	<item catalog_countrySite=""www"" catalog_marketID=""en-US"" languageURLSegment=""en"" isActive=""1"" internalOnly=""0"" />
	<item catalog_countrySite=""www"" catalog_marketID=""es-US"" languageURLSegment=""es"" isActive=""1"" internalOnly=""0"" />
</data>";

      CountrySiteMarketMappingsResponseData response = CountrySiteMarketMappingsResponseData.FromCacheDataXml(invalidXml);

      string marketId;
      Assert.IsTrue(response.IsValidUrlLanguageForCountrySite("en", false));
      Assert.IsTrue(response.TryGetMarketIdByCountrySiteAndUrlLanguage("en", out marketId));
      Assert.AreEqual("en-US", marketId);
      Assert.IsTrue(response.IsValidUrlLanguageForCountrySite("es", false));
      Assert.IsTrue(response.TryGetMarketIdByCountrySiteAndUrlLanguage("es", out marketId));
      Assert.AreEqual("es-US", marketId);
    }

    #endregion

    #region Exception mapping tests

    [TestMethod]
    public void CountrySiteMarketMappingsResponseData_FromCountrySiteAndMarketId_ReturnsExceptionDefaultMapping()
    {
      CountrySiteMarketMappingsResponseData response =
        CountrySiteMarketMappingsResponseData.FromCountrySiteAndMarketId("xx", "yy-ZZ");

      string marketId;
      Assert.IsTrue(response.IsValidUrlLanguageForCountrySite("yy", true));
      Assert.IsTrue(response.IsValidUrlLanguageForCountrySite("aa", true));
      Assert.IsTrue(response.IsValidUrlLanguageForCountrySite("bb", true));
      Assert.IsTrue(response.TryGetMarketIdByCountrySiteAndUrlLanguage("yy", out marketId));
      Assert.AreEqual("yy-ZZ", marketId);
      Assert.IsTrue(response.TryGetMarketIdByCountrySiteAndUrlLanguage("aa", out marketId));
      Assert.AreEqual("yy-ZZ", marketId);
      Assert.IsTrue(response.TryGetMarketIdByCountrySiteAndUrlLanguage("bb", out marketId));
      Assert.AreEqual("yy-ZZ", marketId);
    }

    #endregion
  }
}
