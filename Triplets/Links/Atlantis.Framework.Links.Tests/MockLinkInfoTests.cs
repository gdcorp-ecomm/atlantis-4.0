using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Links.MockImpl;
using Atlantis.Framework.Testing.MockHttpContext;
using afe = Atlantis.Framework.Engine;
using Atlantis.Framework.Links.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Atlantis.Framework.Links.Tests
{
  [TestClass]
  [DeploymentItem(afeConfig)]
  [DeploymentItem("Atlantis.Framework.Links.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.Links.MockImpl.dll")]
  public class MockLinkInfoTests
  {
    private static Dictionary<string, ILinkInfo> fakeData;

    public const string afeConfig = "atlantis.mockimpl.config";

    [ClassInitialize]
    public static void InitTests(TestContext c)
    {
      Engine.Engine.ReloadConfig(afeConfig);

      fakeData = new Dictionary<string, ILinkInfo>
        {
          { 
            "MYAURL", 
            new LinkInfoImpl
              {
                BaseUrl = "mya.godaddy.com",
                CountrySupportType = LinkTypeCountrySupport.NoSupport,
                CountryParameter = String.Empty,
                LanguageSupportType = LinkTypeLanguageSupport.NoSupport,
                LanguageParameter = String.Empty
              }
          },
          { 
            "SITEURL", 
            new LinkInfoImpl
              {
                BaseUrl = "www.godaddy.com",
                CountrySupportType = LinkTypeCountrySupport.ReplaceHostNameSupport,
                CountryParameter = String.Empty,
                LanguageSupportType = LinkTypeLanguageSupport.PrefixPathSupport,
                LanguageParameter = String.Empty
              }
          },
          { 
            "CARTURL", 
            new LinkInfoImpl
              {
                BaseUrl = "cart.godaddy.com",
                CountrySupportType = LinkTypeCountrySupport.NoSupport,
                CountryParameter = String.Empty,
                LanguageSupportType = LinkTypeLanguageSupport.PrefixPathSupport,
                LanguageParameter = String.Empty
              }
          },
          { 
            "DCCURL", 
            new LinkInfoImpl
              {
                BaseUrl = "omg.dcc.godaddy.com",
                CountrySupportType = LinkTypeCountrySupport.ReplaceHostNameSupport,
                CountryParameter = String.Empty,
                LanguageSupportType = LinkTypeLanguageSupport.QueryStringSupport,
                LanguageParameter = String.Empty
              }
          },
          { 
            "SUPPORTURL", 
            new LinkInfoImpl
              {
                BaseUrl = "support.godaddy.com",
                CountrySupportType = LinkTypeCountrySupport.QueryStringSupport,
                CountryParameter = String.Empty,
                LanguageSupportType = LinkTypeLanguageSupport.QueryStringSupport,
                LanguageParameter = String.Empty
              }
          }
        };
    }

    private const int kEngineRequest = 731;

    private enum ContextIds
    {
      Invalid = -1,
      NoContext = 0,
      GoDaddy = 1,
      BlueRazor = 2
    }

    internal class TestRequestData : RequestData
    {
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void LinkInfoMockInvalidRequestClass()
    {
      string url = "http://www.godaddy.com/domains/search.aspx";
      var ip = IPAddress.Parse("192.168.0.1");
      var webrequest = new MockHttpRequest(url);
      webrequest.MockRemoteAddress(ip);
      MockHttpContext.SetFromWorkerRequest(webrequest);

      int contextId = (int) ContextIds.GoDaddy;
      HttpContext.Current.Items[MockLinkInfoRequestContextSettings.LinkInfoTable + "." + contextId] = fakeData;

      var request = new TestRequestData();
      var response = (LinkInfoResponseData)afe.Engine.ProcessRequest(request, kEngineRequest);
    }


    [TestMethod]
    public void LinkInfoMockGetXML()
    {
      string url = "http://www.godaddy.com/domains/search.aspx";
      var ip = IPAddress.Parse("192.168.0.1");
      var webrequest = new MockHttpRequest(url);
      webrequest.MockRemoteAddress(ip);
      MockHttpContext.SetFromWorkerRequest(webrequest);

      int contextId = (int) ContextIds.GoDaddy;
      HttpContext.Current.Items[MockLinkInfoRequestContextSettings.LinkInfoTable + "." + contextId] = fakeData;

      var request = new LinkInfoRequestData(contextId);
      var response = (LinkInfoResponseData)afe.Engine.ProcessRequest(request, kEngineRequest);
      string xml = response.ToXML();
      Assert.IsNotNull(xml);
      Assert.IsTrue(xml.Length>0);
      var xmldoc = new XmlDocument();
      xmldoc.LoadXml(xml);
    }

    [TestMethod]
    public void LinkInfoMockReasonableCacheKeys()
    {
      string url = "http://www.godaddy.com/domains/search.aspx";
      var ip = IPAddress.Parse("192.168.0.1");
      var webrequest = new MockHttpRequest(url);
      webrequest.MockRemoteAddress(ip);
      MockHttpContext.SetFromWorkerRequest(webrequest);

      HttpContext.Current.Items[MockLinkInfoRequestContextSettings.LinkInfoTable + "." + (int)ContextIds.GoDaddy] = fakeData;
      HttpContext.Current.Items[MockLinkInfoRequestContextSettings.LinkInfoTable + "." + (int)ContextIds.BlueRazor] = fakeData;

      int contextId1a = (int) ContextIds.GoDaddy;
      var request = new LinkInfoRequestData(contextId1a);
      var key1a = request.GetCacheMD5();

      int contextId1b = (int) ContextIds.GoDaddy;
      request = new LinkInfoRequestData(contextId1b);
      var key1b = request.GetCacheMD5();

      int contextId2a = (int) ContextIds.BlueRazor;
      request = new LinkInfoRequestData(contextId2a);
      var key2a = request.GetCacheMD5();

      int contextId2b = (int) ContextIds.BlueRazor;
      request = new LinkInfoRequestData(contextId2b);
      var key2b = request.GetCacheMD5();

      Assert.AreEqual(key1a, key1b);
      Assert.AreEqual(key2a, key2b);
      Assert.AreNotEqual(key1a, key2a);
    }

    [TestMethod]
    public void LinkInfoMockReturnsResults()
    {
      string url = "http://www.godaddy.com/domains/search.aspx";
      var ip = IPAddress.Parse("192.168.0.1");
      var webrequest = new MockHttpRequest(url);
      webrequest.MockRemoteAddress(ip);
      MockHttpContext.SetFromWorkerRequest(webrequest);

      int contextId = (int) ContextIds.GoDaddy;
      HttpContext.Current.Items[MockLinkInfoRequestContextSettings.LinkInfoTable + "." + contextId] = fakeData;

      var request = new LinkInfoRequestData(contextId);
      var response = (LinkInfoResponseData)afe.Engine.ProcessRequest(request, kEngineRequest);
      Assert.AreEqual(fakeData.Count, response.Links.Count);

      foreach (var item in response.Links)
      {
        Assert.AreEqual(item.Value.CountryParameter, fakeData[item.Key].CountryParameter);
        Assert.AreEqual(item.Value.CountrySupportType, fakeData[item.Key].CountrySupportType);
        Assert.AreEqual(item.Value.LanguageParameter, fakeData[item.Key].LanguageParameter);
        Assert.AreEqual(item.Value.LanguageSupportType, fakeData[item.Key].LanguageSupportType);
      }
    }

    [TestMethod]
    public void LinkInfoCannotSoilTheMockCache()
    {
      var request = new LinkInfoRequestData((int)ContextIds.GoDaddy);
      var response = (LinkInfoResponseData)afe.Engine.ProcessRequest(request, kEngineRequest);

      try
      {
        response.Links.Clear();
      }
      catch (Exception ex)
      {
        Assert.IsTrue(ex is InvalidOperationException);
      }

      try
      {
        response.Links.Add("SITEURL", new LinkInfoImpl
          {
            BaseUrl = "www.readonly.com",
            CountrySupportType = LinkTypeCountrySupport.ReplaceHostNameSupport,
            CountryParameter = String.Empty,
            LanguageSupportType = LinkTypeLanguageSupport.QueryStringSupport,
            LanguageParameter = String.Empty
          });
      }
      catch (Exception ex)
      {
        Assert.IsTrue(ex is InvalidOperationException);
      }

      try
      {
        response.Links["SITEURL"] = new LinkInfoImpl();
      }
      catch (Exception ex)
      {
        Assert.IsTrue(ex is InvalidOperationException);
      }

      try
      {
        response.Links.Remove("SITEURL");
      }
      catch (Exception ex)
      {
        Assert.IsTrue(ex is InvalidOperationException);
      }

    }

  }
}
