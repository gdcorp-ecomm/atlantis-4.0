using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Atlantis.Framework.PresCentral.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.PresCentral.Tests
{
  [TestClass]
  public class PresCentralTests
  {
    private NameValueCollection GetRequestParams1()
    {
      NameValueCollection result = new NameValueCollection();
      result[PCQueryKeyNames.Manifest] = "standardheaderfooter";
      result[PCQueryKeyNames.PrivateLabelId] = "1";
      result[PCQueryKeyNames.Application] = "sales";
      result[PCQueryKeyNames.Bot] = "false";
      result[PCQueryKeyNames.Country] = "us";
      result[PCQueryKeyNames.CountrySite] = "www";
      result[PCQueryKeyNames.CurrencyType] = "USD";
      result[PCQueryKeyNames.DocType] = "XHTML";
      result[PCQueryKeyNames.Https] = "false";
      result[PCQueryKeyNames.Split] = "1";

      return result;
    }

    private NameValueCollection GetRequestParams2()
    {
      NameValueCollection result = new NameValueCollection();
      result[PCQueryKeyNames.Country] = "us";
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
      PCDetermineCacheKeyRequestData request = new PCDetermineCacheKeyRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      request.AddQueryParameters(GetRequestParams1());

      PCDetermineCacheKeyResponseData response = (PCDetermineCacheKeyResponseData)Engine.Engine.ProcessRequest(request, 542);
      Assert.AreEqual(0, response.Data.ResultCode);
    }

    [TestMethod]
    public void DuplicateQueryItems()
    {
      PCDetermineCacheKeyRequestData request1 = new PCDetermineCacheKeyRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      request1.AddQueryParameters(GetRequestParams1());

      PCDetermineCacheKeyRequestData request2 = new PCDetermineCacheKeyRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      request2.AddQueryParameters(GetRequestParams2());

      Assert.AreEqual(request1.GetQuery(), request2.GetQuery());
    }

    [TestMethod]
    public void DetermineCacheKeyDataCache()
    {
      PCDetermineCacheKeyRequestData request1 = new PCDetermineCacheKeyRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      request1.AddQueryParameters(GetRequestParams1());

      PCDetermineCacheKeyResponseData response1 = (PCDetermineCacheKeyResponseData)DataCache.DataCache.GetProcessRequest(request1, 542);

      PCDetermineCacheKeyRequestData request2 = new PCDetermineCacheKeyRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      request2.AddQueryParameters(GetRequestParams2());

      PCDetermineCacheKeyResponseData response2 = (PCDetermineCacheKeyResponseData)DataCache.DataCache.GetProcessRequest(request2, 542);

      string createDate1 = response1.Data.GetDebugData("CreateDate");
      string createDate2 = response2.Data.GetDebugData("CreateDate");
      Assert.AreEqual(createDate1, createDate2);
    }

    [TestMethod]
    public void GenerateContentNoCacheBasic()
    {
      PCGenerateContentNoCacheRequestData request2 = new PCGenerateContentNoCacheRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      request2.AddQueryParameters(GetRequestParams2());

      PCGenerateContentNoCacheResponseData response2 = (PCGenerateContentNoCacheResponseData)Engine.Engine.ProcessRequest(request2, 544);

      PCContentItem item = response2.Data.FindContentByName("header");
      Assert.IsNotNull(item);
    }

    [TestMethod]
    public void GenerateContentBasic()
    {
      PCDetermineCacheKeyRequestData request1 = new PCDetermineCacheKeyRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      request1.AddQueryParameters(GetRequestParams1());

      PCDetermineCacheKeyResponseData response1 = (PCDetermineCacheKeyResponseData)DataCache.DataCache.GetProcessRequest(request1, 542);

      PCGenerateContentRequestData request2 = new PCGenerateContentRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, response1.Data.CacheKey);
      request2.AddQueryParameters(GetRequestParams2());

      PCGenerateContentResponseData response2 = (PCGenerateContentResponseData)Engine.Engine.ProcessRequest(request2, 543);

      PCContentItem item = response2.Data.FindContentByName("header");
      Assert.IsNotNull(item);
    }

    [TestMethod]
    public void DetermineCacheKeyStressTest()
    {
      HashSet<string> createTimes = new HashSet<string>();
      HashSet<string> cacheKeys = new HashSet<string>();
      NameValueCollection queryItems = GetRequestParams1();

      for (int x = 0; x < 5000; x++)
      {
        PCDetermineCacheKeyRequestData request1 = new PCDetermineCacheKeyRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
        request1.AddQueryParameters(queryItems);

        PCDetermineCacheKeyResponseData response = (PCDetermineCacheKeyResponseData)Engine.Engine.ProcessRequest(request1, 542);
        string createDate = response.Data.GetDebugData("CreateDate");
        createTimes.Add(createDate);
        cacheKeys.Add(response.Data.CacheKey);
      }

      Assert.IsTrue(createTimes.Count < 100);
      Assert.AreEqual(1, cacheKeys.Count);

    }
  }
}
