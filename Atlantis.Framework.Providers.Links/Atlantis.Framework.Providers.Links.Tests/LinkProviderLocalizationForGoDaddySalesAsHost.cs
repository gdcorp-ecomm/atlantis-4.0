using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Links.Interface;
using Atlantis.Framework.Links.MockImpl;
using Atlantis.Framework.Localization.MockImpl;
using Atlantis.Framework.Providers.Interface.Links;
using Atlantis.Framework.Providers.Localization;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using afe = Atlantis.Framework.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.Links.Tests
{
  /// <summary>
  /// This class tests the LinkProvider using a mock'd database in xyzfakeData. 
  /// Add to this class when you have tests that use GD's sales site for the HttpRequest, and wish
  /// to verify Localization specific functionality.
  /// </summary>
  [TestClass]
  [DeploymentItem(afeConfig)]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
//  [DeploymentItem("Atlantis.Framework.Links.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.Links.MockImpl.dll")]
  [DeploymentItem("Atlantis.Framework.Localization.MockImpl.dll")]
  [DeploymentItem("Atlantis.Framework.DataCacheGeneric.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
  public class LinkProviderLocalizationForGoDaddySalesAsHost
  {
    private static Dictionary<string, ILinkInfo> gdfakeData;

    private static string fakeMarketsActiveData = Properties.Resource1.MarketsActive;

    private static string fakeCountrySitesActive = Properties.Resource1.CountrySitesActive;

    private static string fakeCountrySiteMarketMappingData = Properties.Resource1.CountrySiteMarketMappings;

    public const string afeConfig = "atlantis.linkinfotests.config";

    private MockProviderContainer FrameworkContainer;

    [ClassInitialize]
    public static void ClassInit(TestContext c)
    {
      gdfakeData = new Dictionary<string, ILinkInfo>
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
                BaseUrl = "us.dcc.godaddy.com/mypage.aspx",
                CountrySupportType = LinkTypeCountrySupport.ReplaceHostNameSupport,
                CountryParameter = String.Empty,
                LanguageSupportType = LinkTypeLanguageSupport.QueryStringSupport,
                LanguageParameter = "lang"
              }
          },
          { 
            "SUPPORTURL", 
            new LinkInfoImpl
              {
                BaseUrl = "support.godaddy.com",
                CountrySupportType = LinkTypeCountrySupport.QueryStringSupport,
                CountryParameter = "cntry",
                LanguageSupportType = LinkTypeLanguageSupport.QueryStringSupport,
                LanguageParameter = "lang"
              }
          }
        };

      afe.Engine.ReloadConfig(afeConfig);

      ReloadCache();
    }

    [TestCleanup]
    public void TestCleanup()
    {
      FrameworkContainer = null;
    }

    [TestInitialize]
    public void TestInit()
    {
      FrameworkContainer = new MockProviderContainer();

      FrameworkContainer.RegisterProvider<ISiteContext, MockSiteContext>();
      FrameworkContainer.RegisterProvider<IShopperContext, MockShopperContext>();
      FrameworkContainer.RegisterProvider<IManagerContext, MockManagerContext>();
      FrameworkContainer.RegisterProvider<ILocalizationProvider, CountrySubdomainLocalizationProvider>();
      FrameworkContainer.RegisterProvider<ILinkProvider, LinkProvider>();
    }

    private static void ReloadCache()
    {
      foreach (var config in Engine.Engine.GetConfigElements())
      {
        DataCache.DataCache.ClearCachedData(config.RequestType);
      }
    }

    private void SetupMarket(string marketId)
    {
      var lp = FrameworkContainer.Resolve<ILocalizationProvider>();
      lp.SetMarket(marketId);
    }

    private ILinkProvider NewLinkProvider(string url, int privateLabelId, string shopperId, bool isInternal = false, bool isManager = false, IPAddress userHostAddress = null)
    {
      var request = new MockHttpRequest(url);
      if (userHostAddress != null)
      {
        request.MockRemoteAddress(userHostAddress);
      }

      MockHttpContext.SetFromWorkerRequest(request);

      FrameworkContainer.SetMockSetting(MockSiteContextSettings.IsRequestInternal, isInternal);
      FrameworkContainer.SetMockSetting(MockSiteContextSettings.PrivateLabelId, privateLabelId);

      if (isManager)
      {
        FrameworkContainer.SetMockSetting(MockManagerContextSettings.IsManager, isManager);
        FrameworkContainer.SetMockSetting(MockManagerContextSettings.PrivateLabelId, privateLabelId);
        FrameworkContainer.SetMockSetting(MockManagerContextSettings.ShopperId, shopperId);
      }
      else
      {
        var shopperContext = FrameworkContainer.Resolve<IShopperContext>();
        shopperContext.SetNewShopper(shopperId);
      }

      HttpContext.Current.Items[MockLinkInfoRequestContextSettings.LinkInfoTable + ".1"] = gdfakeData;

      HttpContext.Current.Items[MockLocalizationSettings.CountrySiteMarketMappingsTable] = fakeCountrySiteMarketMappingData;
      HttpContext.Current.Items[MockLocalizationSettings.CountrySitesActiveTable] = fakeCountrySitesActive;
      HttpContext.Current.Items[MockLocalizationSettings.MarketsActiveTable] = fakeMarketsActiveData;

      var linkProvider = FrameworkContainer.Resolve<ILinkProvider>();
      return linkProvider;
    }

    [TestMethod]
    public void NonWWWPrefixPreservedAndPageLinkTypeWorks()
    {
      string homePage = "http://www.godaddy.com/default.aspx";
      var links = NewLinkProvider(homePage, 1, "832652");
      SetupMarket("en-us");
      string url = links.GetUrl("DCCURL", "");
      Assert.IsTrue(url.IndexOf("//", 8, StringComparison.OrdinalIgnoreCase) == -1);

      // verify the host entry was not removed 
      Assert.IsTrue(url.Contains("us."));
      // verify the host entry didn't get the WWW code
      Assert.IsFalse(url.Contains("www."));
      // verify the language is present
      Assert.IsFalse(url.Contains("/en"));

      Assert.IsTrue(url.Contains("/mypage.aspx"));

      // ensure country and language didn't showup as query params
      int iQueryStart = url.IndexOf('?');
      Assert.IsTrue(iQueryStart != -1 && url.IndexOf("=en-us", iQueryStart, StringComparison.OrdinalIgnoreCase) != -1);
      Assert.IsFalse(iQueryStart != -1 && url.IndexOf("=us", iQueryStart, StringComparison.OrdinalIgnoreCase) != -1);
      Assert.IsFalse(iQueryStart != -1 && url.IndexOf("=www", iQueryStart, StringComparison.OrdinalIgnoreCase) != -1);
    }

    [TestMethod]
    public void GetSalesFromSales()
    {
      string homePage = "http://www.godaddy.com/default.aspx";
      var links = NewLinkProvider(homePage, 1, "832652");
      SetupMarket("en-us");
      string url = links.GetUrl("SITEURL", "/default.aspx");
      Assert.IsTrue(url.IndexOf("//", 8, StringComparison.OrdinalIgnoreCase) == -1);

      // verify the host entry was not removed 
      Assert.IsTrue(url.Contains("www."));
      // verify the host entry didn't get the US country code
      Assert.IsFalse(url.Contains("us."));
      // verify the language is present
      Assert.IsFalse(url.Contains("/en"));

      // ensure country and language didn't showup as query params
      int iQueryStart = url.IndexOf('?');
      Assert.IsFalse(iQueryStart != -1 && url.IndexOf("=en", iQueryStart, StringComparison.OrdinalIgnoreCase) != -1);
      Assert.IsFalse(iQueryStart != -1 && url.IndexOf("=us", iQueryStart, StringComparison.OrdinalIgnoreCase) != -1);
      Assert.IsFalse(iQueryStart != -1 && url.IndexOf("=www", iQueryStart, StringComparison.OrdinalIgnoreCase) != -1);
    }

    [TestMethod]
    public void GetSalesFromSalesSpanish()
    {
      string homePage = "http://www.godaddy.com/es/default.aspx";
      var links = NewLinkProvider(homePage, 1, "832652");
      SetupMarket("es-us");
      string url = links.GetUrl("SITEURL", "/default.aspx");
      Assert.IsTrue(url.IndexOf("//", 8, StringComparison.OrdinalIgnoreCase) == -1);

      // verify the host entry was not removed 
      Assert.IsTrue(url.Contains("www."));
      // verify the host entry didn't get the US country code
      Assert.IsFalse(url.Contains("us."));
      // verify the language wasn't written into the host entry
      Assert.IsFalse(url.Contains("es."));
      // verify the language is present
      Assert.IsTrue(url.Contains("/es"));

      // ensure country and language didn't showup as query params
      int iQueryStart = url.IndexOf('?');
      Assert.IsFalse(iQueryStart != -1 && url.IndexOf("=en", iQueryStart, StringComparison.OrdinalIgnoreCase) != -1);
      Assert.IsFalse(iQueryStart != -1 && url.IndexOf("=es", iQueryStart, StringComparison.OrdinalIgnoreCase) != -1);
      Assert.IsFalse(iQueryStart != -1 && url.IndexOf("=us", iQueryStart, StringComparison.OrdinalIgnoreCase) != -1);
      Assert.IsFalse(iQueryStart != -1 && url.IndexOf("=www", iQueryStart, StringComparison.OrdinalIgnoreCase) != -1);
    }

    [TestMethod]
    public void GetSalesFromSalesBrazillianSpanish()
    {
      string homePage = "http://www.godaddy.com/es-br/default.aspx";
      var links = NewLinkProvider(homePage, 1, "832652");
      SetupMarket("es-br");
      string url = links.GetUrl("SITEURL", "/default.aspx");
      Assert.IsTrue(url.IndexOf("//", 8, StringComparison.OrdinalIgnoreCase) == -1);

      // verify the host entry was not removed 
      Assert.IsTrue(url.Contains("www."));
      // verify the host entry didn't get the US country code
      Assert.IsFalse(url.Contains("us."));
      // verify the language wasn't written into the host entry
      Assert.IsFalse(url.Contains("es."));
      // verify the language is present
      Assert.IsTrue(url.Contains("/es-br"));

      // ensure country and language didn't showup as query params
      int iQueryStart = url.IndexOf('?');
      Assert.IsFalse(iQueryStart != -1 && url.IndexOf("=en", iQueryStart, StringComparison.OrdinalIgnoreCase) != -1);
      Assert.IsFalse(iQueryStart != -1 && url.IndexOf("=es", iQueryStart, StringComparison.OrdinalIgnoreCase) != -1);
      Assert.IsFalse(iQueryStart != -1 && url.IndexOf("=us", iQueryStart, StringComparison.OrdinalIgnoreCase) != -1);
      Assert.IsFalse(iQueryStart != -1 && url.IndexOf("=www", iQueryStart, StringComparison.OrdinalIgnoreCase) != -1);
    }

    [TestMethod]
    public void GetSalesFromSalesIndia()
    {
      string homePage = "http://in.godaddy.com/default.aspx";
      var links = NewLinkProvider(homePage, 1, "832652");
      SetupMarket("en-in");
      string url = links.GetUrl("SITEURL", "/default.aspx");
      Assert.IsTrue(url.IndexOf("//", 8, StringComparison.OrdinalIgnoreCase) == -1);

      // verify the host entry was removed 
      Assert.IsFalse(url.Contains("www."));
      // verify the host entry did get the IN country code
      Assert.IsTrue(url.Contains("in."));
      // verify the default market/language is NOT present
      Assert.IsFalse(url.Contains("/en"));

      // ensure country and language didn't showup as query params
      int iQueryStart = url.IndexOf('?');
      Assert.IsFalse(iQueryStart != -1 && url.IndexOf("=en", iQueryStart, StringComparison.OrdinalIgnoreCase) != -1);
      Assert.IsFalse(iQueryStart != -1 && url.IndexOf("=es", iQueryStart, StringComparison.OrdinalIgnoreCase) != -1);
      Assert.IsFalse(iQueryStart != -1 && url.IndexOf("=us", iQueryStart, StringComparison.OrdinalIgnoreCase) != -1);
      Assert.IsFalse(iQueryStart != -1 && url.IndexOf("=www", iQueryStart, StringComparison.OrdinalIgnoreCase) != -1);
    }

    [TestMethod]
    public void GetSupportFromSales()
    {
      string homePage = "http://www.godaddy.com/default.aspx";
      var links = NewLinkProvider(homePage, 1, "832652");
      SetupMarket("en-us");
      string url = links.GetUrl("SUPPORTURL", "/default.aspx");
      Assert.IsTrue(url.IndexOf("//", 8, StringComparison.OrdinalIgnoreCase) == -1);

      // verify the host entry was not removed 
      Assert.IsTrue(url.Contains("support."));
      // verify the www. was not added
      Assert.IsFalse(url.Contains("www."));
      // verify the host entry didn't get the US country code
      Assert.IsFalse(url.Contains("us."));

      // ensure country and language did showup as query params
      int iQueryStart = url.IndexOf('?');
      Assert.IsTrue(iQueryStart != -1 && url.IndexOf("=en-us", iQueryStart, StringComparison.OrdinalIgnoreCase) != -1);
      Assert.IsFalse(iQueryStart != -1 && url.IndexOf("=us", iQueryStart, StringComparison.OrdinalIgnoreCase) != -1);
      Assert.IsTrue(iQueryStart != -1 && url.IndexOf("=www", iQueryStart, StringComparison.OrdinalIgnoreCase) != -1);
    }

    [TestMethod]
    public void GetSupportFromSalesSpanish()
    {
      string homePage = "http://www.godaddy.com/es/default.aspx";
      var links = NewLinkProvider(homePage, 1, "832652");
      SetupMarket("es-us");
      string url = links.GetUrl("SUPPORTURL", "/default.aspx");
      Assert.IsTrue(url.IndexOf("//", 8, StringComparison.OrdinalIgnoreCase) == -1);

      // verify the host entry was not removed 
      Assert.IsTrue(url.Contains("support."));
      // verify the www. was not added
      Assert.IsFalse(url.Contains("www."));
      // verify the host entry didn't get the US country code
      Assert.IsFalse(url.Contains("us."));
      // verify the language wasn't written into the host entry
      Assert.IsFalse(url.Contains("es."));
      // verify the language is not present
      Assert.IsFalse(url.Contains("/es"));

      // ensure country and language did showup as query params
      int iQueryStart = url.IndexOf('?');
      Assert.IsFalse(iQueryStart != -1 && url.IndexOf("=en", iQueryStart, StringComparison.OrdinalIgnoreCase) != -1);
      Assert.IsTrue(iQueryStart != -1 && url.IndexOf("=es-us", iQueryStart, StringComparison.OrdinalIgnoreCase) != -1);
      Assert.IsFalse(iQueryStart != -1 && url.IndexOf("=us", iQueryStart, StringComparison.OrdinalIgnoreCase) != -1);
      Assert.IsTrue(iQueryStart != -1 && url.IndexOf("=www", iQueryStart, StringComparison.OrdinalIgnoreCase) != -1);
    }

    [TestMethod]
    public void GetSupportFromSalesBrazillianSpanish()
    {
      string homePage = "http://www.godaddy.com/es-br/default.aspx";
      var links = NewLinkProvider(homePage, 1, "832652");
      SetupMarket("es-br");
      string url = links.GetUrl("SUPPORTURL", "/default.aspx");
      Assert.IsTrue(url.IndexOf("//", 8, StringComparison.OrdinalIgnoreCase) == -1);

      // verify the host entry was not removed 
      Assert.IsTrue(url.Contains("support."));
      // verify the www. was not added
      Assert.IsFalse(url.Contains("www."));
      // verify the host entry didn't get the US country code
      Assert.IsFalse(url.Contains("us."));
      // verify the language wasn't written into the host entry
      Assert.IsFalse(url.Contains("es."));
      // verify the language is not present
      Assert.IsFalse(url.Contains("/es"));

      // ensure country and language did showup as query params
      int iQueryStart = url.IndexOf('?');
      Assert.IsFalse(iQueryStart != -1 && url.IndexOf("=en", iQueryStart, StringComparison.OrdinalIgnoreCase) != -1);
      Assert.IsFalse(iQueryStart != -1 && url.IndexOf("=es-us", iQueryStart, StringComparison.OrdinalIgnoreCase) != -1);
      Assert.IsTrue(iQueryStart != -1 && url.IndexOf("=es-br", iQueryStart, StringComparison.OrdinalIgnoreCase) != -1);
      Assert.IsFalse(iQueryStart != -1 && url.IndexOf("=us", iQueryStart, StringComparison.OrdinalIgnoreCase) != -1);
      Assert.IsTrue(iQueryStart != -1 && url.IndexOf("=www", iQueryStart, StringComparison.OrdinalIgnoreCase) != -1);
    }

    [TestMethod]
    public void GetSupportFromIndiaSales()
    {
      string homePage = "http://in.godaddy.com/default.aspx";
      var links = NewLinkProvider(homePage, 1, "832652");
      SetupMarket("en-in");
      string url = links.GetUrl("SUPPORTURL", "/default.aspx");
      Assert.IsTrue(url.IndexOf("//", 8, StringComparison.OrdinalIgnoreCase) == -1);

      // verify the host entry was not removed 
      Assert.IsTrue(url.Contains("support."));
      // verify the www. was not added
      Assert.IsFalse(url.Contains("www."));
      // verify the host entry didn't get the US country code
      Assert.IsFalse(url.Contains("us."));
      // verify the language wasn't written into the host entry
      Assert.IsFalse(url.Contains("in."));
      // verify the language is not present
      Assert.IsFalse(url.Contains("/en"));

      // ensure country and language did showup as query params
      int iQueryStart = url.IndexOf('?');
      Assert.IsTrue(iQueryStart != -1 && url.IndexOf("=en-in", iQueryStart, StringComparison.OrdinalIgnoreCase) != -1);
      Assert.IsFalse(iQueryStart != -1 && url.IndexOf("=us", iQueryStart, StringComparison.OrdinalIgnoreCase) != -1);
      Assert.IsFalse(iQueryStart != -1 && url.IndexOf("=www", iQueryStart, StringComparison.OrdinalIgnoreCase) != -1);
      Assert.IsTrue(iQueryStart != -1 && url.IndexOf("=in", iQueryStart, StringComparison.OrdinalIgnoreCase) != -1);
    }

    [TestMethod]
    public void GetSupportFromIndiaSalesSpanish()
    {
      string homePage = "http://in.godaddy.com/es/default.aspx";
      var links = NewLinkProvider(homePage, 1, "832652");
      SetupMarket("es-in");
      string url = links.GetUrl("SUPPORTURL", "/default.aspx");
      Assert.IsTrue(url.IndexOf("//", 8, StringComparison.OrdinalIgnoreCase) == -1);

      // verify the host entry was not removed 
      Assert.IsTrue(url.Contains("support."));
      // verify the www. was not added
      Assert.IsFalse(url.Contains("www."));
      // verify the host entry didn't get the US country code
      Assert.IsFalse(url.Contains("us."));
      // verify the language wasn't written into the host entry
      Assert.IsFalse(url.Contains("in."));
      // verify the language is not present
      Assert.IsFalse(url.Contains("/en"));
      Assert.IsFalse(url.Contains("/es"));

      // ensure country and language did showup as query params
      int iQueryStart = url.IndexOf('?');
      Assert.IsTrue(iQueryStart != -1 && url.IndexOf("=es-in", iQueryStart, StringComparison.OrdinalIgnoreCase) != -1);
      Assert.IsFalse(iQueryStart != -1 && url.IndexOf("=us", iQueryStart, StringComparison.OrdinalIgnoreCase) != -1);
      Assert.IsFalse(iQueryStart != -1 && url.IndexOf("=www", iQueryStart, StringComparison.OrdinalIgnoreCase) != -1);
      Assert.IsTrue(iQueryStart != -1 && url.IndexOf("=in", iQueryStart, StringComparison.OrdinalIgnoreCase) != -1);
    }

    [TestMethod]
    public void GetSupportFromIndiaSalesBrazillianSpanish()
    {
      string homePage = "http://in.godaddy.com/es-br/default.aspx";
      var links = NewLinkProvider(homePage, 1, "832652");
      SetupMarket("es-br");
      string url = links.GetUrl("SUPPORTURL", "/default.aspx");
      Assert.IsTrue(url.IndexOf("//", 8, StringComparison.OrdinalIgnoreCase) == -1);

      // verify the host entry was not removed 
      Assert.IsTrue(url.Contains("support."));
      // verify the www. was not added
      Assert.IsFalse(url.Contains("www."));
      // verify the host entry didn't get the US country code
      Assert.IsFalse(url.Contains("us."));
      // verify the language wasn't written into the host entry
      Assert.IsFalse(url.Contains("in."));
      // verify the language is not present
      Assert.IsFalse(url.Contains("/en"));
      Assert.IsFalse(url.Contains("/es"));

      // ensure country and language did showup as query params
      int iQueryStart = url.IndexOf('?');
      Assert.IsFalse(iQueryStart != -1 && url.IndexOf("=es-in", iQueryStart, StringComparison.OrdinalIgnoreCase) != -1);
      Assert.IsTrue(iQueryStart != -1 && url.IndexOf("=es-br", iQueryStart, StringComparison.OrdinalIgnoreCase) != -1);
      Assert.IsFalse(iQueryStart != -1 && url.IndexOf("=us", iQueryStart, StringComparison.OrdinalIgnoreCase) != -1);
      Assert.IsFalse(iQueryStart != -1 && url.IndexOf("=www", iQueryStart, StringComparison.OrdinalIgnoreCase) != -1);
      Assert.IsFalse(iQueryStart != -1 && url.IndexOf("=br", iQueryStart, StringComparison.OrdinalIgnoreCase) != -1);
      Assert.IsTrue(iQueryStart != -1 && url.IndexOf("=in", iQueryStart, StringComparison.OrdinalIgnoreCase) != -1);
    }

    [TestMethod]
    public void GetWithoutLocalization()
    {
      string homePage = "http://in.godaddy.com/es-br/default.aspx";
      var links = NewLinkProvider(homePage, 1, "832652");
      SetupMarket("es-br");
      string url = links.GetUrl("SUPPORTURL", "/default.aspx", QueryParamMode.ExplicitWithLocalizationParameters);

      // verify the host entry was not removed 
      Assert.IsTrue(url.StartsWith("http://support.godaddy.com/default.aspx"));
      Assert.IsFalse(url.IndexOf("=es-br", StringComparison.OrdinalIgnoreCase) == -1);
      Assert.IsFalse(url.IndexOf("=in", StringComparison.OrdinalIgnoreCase) == -1);

      string url2 = links.GetUrl("SUPPORTURL", "/default.aspx", QueryParamMode.CommonParameters);
      Assert.IsTrue(String.Equals(url, url2));

      url = links.GetUrl("SUPPORTURL", "/default.aspx", QueryParamMode.ExplicitParameters);
      Assert.IsTrue(url.Equals("http://support.godaddy.com/default.aspx"));
    }

  }
}