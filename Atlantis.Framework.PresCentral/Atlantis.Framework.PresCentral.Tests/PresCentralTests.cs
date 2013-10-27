using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime;
using System.Runtime.Remoting.Messaging;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.PresCentral.Interface;
using Atlantis.Framework.PresCentral.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.PresCentral.Tests
{
  [TestClass]
  public class PresCentralTests
  {
    private NameValueCollection GetRequestParams1()
    {
      var result = new NameValueCollection();
      result[PCQueryKeyNames.Manifest] = "standardheaderfooter";
      result[PCQueryKeyNames.PrivateLabelId] = "1";
      result[PCQueryKeyNames.Application] = "sales";
      result[PCQueryKeyNames.Bot] = "false";
      result[PCQueryKeyNames.CountrySite] = "www";
      result[PCQueryKeyNames.CurrencyType] = "USD";
      result[PCQueryKeyNames.DocType] = "XHTML";
      result[PCQueryKeyNames.Https] = "false";
      result[PCQueryKeyNames.Split] = "1";

      return result;
    }

    private NameValueCollection GetRequestParams2()
    {
      var result = new NameValueCollection();
      result[PCQueryKeyNames.CountrySite] = "www";
      result[PCQueryKeyNames.PrivateLabelId] = "1";
      result[PCQueryKeyNames.Split] = "1";
      result[PCQueryKeyNames.Application] = "sales";
      result[PCQueryKeyNames.Bot] = "false";
      result[PCQueryKeyNames.CurrencyType] = "USD";
      result[PCQueryKeyNames.DocType] = "XHTML";
      result[PCQueryKeyNames.Manifest] = "standardheaderfooter";
      result[PCQueryKeyNames.Https] = "false";

      return result;
    }


    [TestMethod]
    public void DetermineCacheKeyBasic()
    {
      var request = new PCDetermineCacheKeyRequestData();
      request.AddQueryParameters(GetRequestParams1());

      var response = (PCDetermineCacheKeyResponseData)Engine.Engine.ProcessRequest(request, 542);
      Assert.AreEqual(0, response.Data.ResultCode);
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void DetermineCacheKeyBasicBadManifest()
    {
      var request = new PCDetermineCacheKeyRequestData();
      request.AddQueryParameters(GetRequestParams1());
      request.AddQueryParameter(PCQueryKeyNames.Manifest, "thismanifestnotthere");

      var response = (PCDetermineCacheKeyResponseData)Engine.Engine.ProcessRequest(request, 542);
    }

    [TestMethod]
    public void DuplicateQueryItems()
    {
      var request1 = new PCDetermineCacheKeyRequestData();
      request1.AddQueryParameters(GetRequestParams1());

      var request2 = new PCDetermineCacheKeyRequestData();
      request2.AddQueryParameters(GetRequestParams2());

      Assert.AreEqual(request1.GetQuery(), request2.GetQuery());
    }

    [TestMethod]
    public void DetermineCacheKeyDataCache()
    {
      var request1 = new PCDetermineCacheKeyRequestData();
      request1.AddQueryParameters(GetRequestParams1());

      var request2 = new PCDetermineCacheKeyRequestData();
      request2.AddQueryParameters(GetRequestParams2());

      Assert.AreEqual(request1.GetCacheMD5(), request2.GetCacheMD5());
    }

    [TestMethod]
    public void GenerateContentNoCacheBasic()
    {
      var request2 = new PCGenerateContentNoCacheRequestData();
      request2.AddQueryParameters(GetRequestParams2());

      var response2 = (PCGenerateContentNoCacheResponseData)Engine.Engine.ProcessRequest(request2, 544);

      var item = response2.Data.FindContentByName("header");
      Assert.IsNotNull(item);
    }

    [TestMethod]
    public void GenerateContentBasic()
    {
      var request1 = new PCDetermineCacheKeyRequestData();
      request1.AddQueryParameters(GetRequestParams1());

      var response1 = (PCDetermineCacheKeyResponseData)Engine.Engine.ProcessRequest(request1, 542);

      var request2 = new PCGenerateContentRequestData(response1.Data.CacheKey);
      request2.AddQueryParameters(GetRequestParams2());

      var response2 = (PCGenerateContentResponseData)Engine.Engine.ProcessRequest(request2, 543);

      var item = response2.Data.FindContentByName("header");
      Assert.IsNotNull(item);
    }

    [TestMethod]
    public void DetermineCacheKeyStressTest()
    {
      var createTimes = new HashSet<string>();
      var cacheKeys = new HashSet<string>();
      var queryItems = GetRequestParams1();

      for (int x = 0; x < 500; x++)
      {
        var request1 = new PCDetermineCacheKeyRequestData();
        request1.AddQueryParameters(queryItems);

        var response = (PCDetermineCacheKeyResponseData)Engine.Engine.ProcessRequest(request1, 542);
        var createDate = response.Data.GetDebugData("CreateDate");
        createTimes.Add(createDate);
        cacheKeys.Add(response.Data.CacheKey);
      }

      Assert.IsTrue(createTimes.Count < 50);
      Assert.AreEqual(1, cacheKeys.Count);
    }

    [TestMethod]
    public void DetermineCacheKeyResponseDataSuccess()
    {
      var pcResponse = new PCResponse(Resources.CacheKeySuccess);
      Assert.AreEqual(0, pcResponse.ResultCode);
      Assert.AreEqual(0, pcResponse.Errors.Count());
      XElement.Parse(pcResponse.ToXML());

      Assert.AreEqual(3, pcResponse.DebugDataKeys.Count());
      Assert.IsFalse(string.IsNullOrEmpty(pcResponse.CacheKey));

      string sourceServer = pcResponse.GetDebugData("SourceServer");
      Assert.IsFalse(string.IsNullOrEmpty(sourceServer));

      string nullResult = pcResponse.GetDebugData("ThisIsNotThere");
      Assert.IsNull(nullResult);

      var response = new PCDetermineCacheKeyResponseData(pcResponse);
      Assert.IsNull(response.GetException());
      Assert.IsTrue(ReferenceEquals(pcResponse, response.Data));
      XElement.Parse(response.ToXML());
    }

    [TestMethod]
    public void DetermineCacheKeyResponseDataErrors()
    {
      var pcResponse = new PCResponse(Resources.ManifestError);
      Assert.AreNotEqual(0, pcResponse.ResultCode);
      Assert.AreEqual(2, pcResponse.Errors.Count());
      XElement.Parse(pcResponse.ToXML());
      Assert.IsTrue(string.IsNullOrEmpty(pcResponse.CacheKey));

      var response = new PCDetermineCacheKeyResponseData(pcResponse);
      Assert.IsNull(response.GetException());
      Assert.IsTrue(ReferenceEquals(pcResponse, response.Data));
    }

    [TestMethod]
    public void GenerateContentResponseData()
    {
      var pcResponse = new PCResponse(Resources.GenerateContentSuccess);
      Assert.AreEqual(0, pcResponse.ResultCode);
      Assert.AreEqual(0, pcResponse.Errors.Count());
      Assert.IsFalse(string.IsNullOrEmpty(pcResponse.CacheKey));

      var headerContent = pcResponse.FindContentByName("header");
      Assert.IsNotNull(headerContent);
      Assert.AreEqual("header", headerContent.Name);
      Assert.AreEqual("predetermined", headerContent.Location);

      var notFoundContent = pcResponse.FindContentByName("ThisContentNotThere");
      Assert.IsNull(notFoundContent);

      var bottomItems = pcResponse.FindContentByLocation("bottom");
      Assert.AreEqual(2, bottomItems.Count());
      var bottomContent = bottomItems.First();
      Assert.IsNotNull(bottomContent);
      Assert.AreEqual("script", bottomContent.Name);
      Assert.AreEqual("bottom", bottomContent.Location);

      var response = new PCGenerateContentResponseData(pcResponse);
      Assert.IsNull(response.GetException());
      Assert.IsTrue(ReferenceEquals(pcResponse, response.Data));
    }

    [TestMethod]
    public void GenerateContentNoCacheResponseData()
    {
      var pcResponse = new PCResponse(Resources.GenerateContentNoCacheSuccess);
      Assert.AreEqual(0, pcResponse.ResultCode);
      Assert.AreEqual(0, pcResponse.Errors.Count());
      Assert.IsTrue(string.IsNullOrEmpty(pcResponse.CacheKey));

      var headerContent = pcResponse.FindContentByName("header");
      Assert.IsNotNull(headerContent);
      Assert.AreEqual("header", headerContent.Name);
      Assert.AreEqual("predetermined", headerContent.Location);

      var bottomItems = pcResponse.FindContentByLocation("bottom");
      Assert.AreEqual(2, bottomItems.Count());
      var bottomContent = bottomItems.First();
      Assert.IsNotNull(bottomContent);
      Assert.AreEqual("script", bottomContent.Name);
      Assert.AreEqual("bottom", bottomContent.Location);

      var response = new PCGenerateContentNoCacheResponseData(pcResponse);
      Assert.IsNull(response.GetException());
      Assert.IsTrue(ReferenceEquals(pcResponse, response.Data));
    }

    [TestMethod]
    public void PCResponseNoResultCodeAndOneBadDebugItem()
    {
      var pcResponse = new PCResponse(Resources.NoResultCode);
      Assert.AreEqual(-1, pcResponse.ResultCode);
      Assert.AreEqual(2, pcResponse.DebugDataKeys.Count());
    }

    [TestMethod]
    public void DetermineCacheKeyProperties()
    {
      var request = new PCDetermineCacheKeyRequestData();
      request.AddQueryParameter("b", "2");
      request.AddQueryParameter("a", "1");
      Assert.AreEqual("a=1&b=2", request.GetQuery());
      Assert.IsNotNull(request.GetCacheMD5());
    }

    [TestMethod]
    public void GenerateContentProperties()
    {
      var request = new PCGenerateContentRequestData("mykey");
      Assert.AreEqual("mykey", request.GetCacheMD5());
      request.AddQueryParameter("b", "2");
      request.AddQueryParameter("b", "1");
      Assert.AreEqual("b=1", request.GetQuery());
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void GenerateContentEmptyCacheKey()
    {
      var request = new PCGenerateContentRequestData(string.Empty);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void GenerateContentNullCacheKey()
    {
      var request = new PCGenerateContentRequestData(null);
    }

    [TestMethod]
    public void GenerateContentNoCacheProperties()
    {
      var request = new PCGenerateContentNoCacheRequestData();
      request.AddQueryParameter("b", "2");
      request.AddQueryParameter("b", "1");
      Assert.AreEqual("b=1", request.GetQuery());
    }

    [TestMethod]
    [ExpectedException(typeof(NotImplementedException))]
    public void GenerateContentNoCachePropertiesCacheKeyNotSupported()
    {
      var request = new PCGenerateContentNoCacheRequestData();
      var test = request.GetCacheMD5();
    }

    [TestMethod]
    public void NullAndEmptyQueryParameters()
    {
      var request = new PCDetermineCacheKeyRequestData();
      request.AddQueryParameter(string.Empty, "1");
      request.AddQueryParameter(null, "2");
      request.AddQueryParameter("b", "1");
      Assert.AreEqual("b=1", request.GetQuery());
    }


  }
}
