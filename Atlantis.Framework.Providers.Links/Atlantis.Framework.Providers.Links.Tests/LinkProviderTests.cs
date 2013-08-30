using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
  /// Add to this class when you have misc tests that exercise the LinkProvider w/o a need to match
  /// backend database values.
  /// </summary>
  [TestClass]
  [DeploymentItem(afeConfig)]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  [DeploymentItem("Atlantis.Framework.Links.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.Links.MockImpl.dll")]
  [DeploymentItem("Atlantis.Framework.Localization.MockImpl.dll")]
  [DeploymentItem("Atlantis.Framework.DataCacheGeneric.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
  public class LinkProviderTests
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
            "SSLURL", 
            new LinkInfoImpl
              {
                BaseUrl = "certificates.godaddy.com",
                CountrySupportType = LinkTypeCountrySupport.PrefixHostNameSupport,
                CountryParameter = String.Empty,
                LanguageSupportType = LinkTypeLanguageSupport.PrefixPathSupport,
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
      // verify the language is not present
      Assert.IsFalse(url.Contains("/en"));

      Assert.IsTrue(url.Contains("/mypage.aspx"));

      // ensure country didn't showup, but the language does in the query params
      int iQueryStart = url.IndexOf('?');
      Assert.IsTrue(iQueryStart != -1 && url.IndexOf("=en-us", iQueryStart, StringComparison.OrdinalIgnoreCase) != -1);
      Assert.IsFalse(iQueryStart != -1 && url.IndexOf("=us", iQueryStart, StringComparison.OrdinalIgnoreCase) != -1);
      Assert.IsFalse(iQueryStart != -1 && url.IndexOf("=www", iQueryStart, StringComparison.OrdinalIgnoreCase) != -1);
    }

    [TestMethod]
    public void HostPrefixTestForCountries()
    {
      string homePage = "http://in.godaddy.com/default.aspx";
      var links = NewLinkProvider(homePage, 1, "832652");
      SetupMarket("en-in");
      string url = links.GetUrl("SSLURL", "");
      Assert.IsTrue(url.IndexOf("//", 8, StringComparison.OrdinalIgnoreCase) == -1);

      // verify the host entry got the in
      Assert.IsTrue(url.Contains("in."));
      Assert.IsFalse(url.Contains(".in."));
      // verify the host entry didn't get the WWW code
      Assert.IsFalse(url.Contains("www."));
      // verify the language is NOT present
      Assert.IsFalse(url.Contains("/en"));

      // ensure country and language didn't showup as query params
      int iQueryStart = url.IndexOf('?');
      Assert.IsFalse(iQueryStart != -1 && url.IndexOf("=en", iQueryStart, StringComparison.OrdinalIgnoreCase) == -1);
      Assert.IsFalse(iQueryStart != -1 && url.IndexOf("=in", iQueryStart, StringComparison.OrdinalIgnoreCase) == -1);
      Assert.IsFalse(iQueryStart != -1 && url.IndexOf("=www", iQueryStart, StringComparison.OrdinalIgnoreCase) == -1);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ExceptionForQuestionMarkInRelativePathForGetRel()
    {
      var links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, "832652");
      string url = links.GetRelativeUrl("a.aspx?x=0");
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ExceptionForAmpersandMarkInRelativePathGetRel()
    {
      var links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, "832652");
      string url = links.GetRelativeUrl("&x=0");
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ExceptionForQuestionMarkInRelativePathForGetFull()
    {
      var links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, "832652");
      string url = links.GetUrl("SITEURL", "a.aspx?x=0");
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ExceptionForAmpersandMarkInRelativePathGetFull()
    {
      var links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, "832652");
      string url = links.GetUrl("SITEURL", "&x=0");
    }

    [TestMethod]
    public void Misc()
    {
      var links = NewLinkProvider("http://www.godaddy.com/default.aspx", 1, "832652");
      var parms = new NameValueCollection { { "x", "val" }, { "y", "val2" } };
      string url = links.GetRelativeUrl("a.aspx", QueryParamMode.ExplicitParameters, parms);
      Assert.IsTrue(url.Contains("x=val"));
      Assert.IsTrue(url.Contains("y=val2"));
      Assert.IsTrue(url.IndexOf("//", 8, StringComparison.OrdinalIgnoreCase) == -1);
      url = links.GetRelativeUrl("a.aspx", QueryParamMode.ExplicitParameters, "x", "val", "y", "val2");
      Assert.IsTrue(url.Contains("x=val"));
      Assert.IsTrue(url.Contains("y=val2"));
      Assert.IsTrue(url.IndexOf("//", 8, StringComparison.OrdinalIgnoreCase) == -1);
      url = links.GetRelativeUrl("a.aspx", QueryParamMode.ExplicitParameters);
      Assert.IsFalse(url.Contains("?"));
      Assert.IsFalse(url.Contains("&"));
      Assert.IsTrue(url.IndexOf("//", 8, StringComparison.OrdinalIgnoreCase) == -1);

      url = links.GetFullSecureUrl("a.aspx");
      Assert.IsTrue(url.Contains("https://"));
      Assert.IsTrue(url.IndexOf("//",8) == -1);
      url = links.GetFullSecureUrl("/a.aspx", QueryParamMode.ExplicitParameters);
      Assert.IsTrue(url.Contains("https://"));
      Assert.IsTrue(url.IndexOf("//",8) == -1);
      url = links.GetFullSecureUrl("/a.aspx", QueryParamMode.ExplicitParameters, parms);
      Assert.IsTrue(url.Contains("https://"));
      Assert.IsTrue(url.IndexOf("//",8) == -1);
      url = links.GetFullSecureUrl("/a.aspx", QueryParamMode.ExplicitParameters, "x", "val", "y", "val2");
      Assert.IsTrue(url.Contains("https://"));
      Assert.IsTrue(url.Contains("x=val"));
      Assert.IsTrue(url.Contains("y=val2"));
      Assert.IsTrue(url.IndexOf("//",8) == -1);
      url = links.GetFullSecureUrl("/a.aspx", "x", "val", "y", "val2");
      Assert.IsTrue(url.Contains("https://"));
      Assert.IsTrue(url.Contains("x=val"));
      Assert.IsTrue(url.Contains("y=val2"));
      Assert.IsTrue(url.IndexOf("//",8) == -1);

      url = links.GetFullUrl("a.aspx");
      Assert.IsTrue(url.Contains("/a.aspx"));
      Assert.IsTrue(url.IndexOf("//",8) == -1);
      url = links.GetFullUrl("a.aspx", QueryParamMode.ExplicitParameters);
      Assert.IsTrue(url.Contains("/a.aspx"));
      Assert.IsTrue(url.IndexOf("//",8) == -1);
      url = links.GetFullUrl("a.aspx", QueryParamMode.ExplicitParameters, parms);
      Assert.IsTrue(url.Contains("/a.aspx"));
      Assert.IsTrue(url.IndexOf("//",8) == -1);
      url = links.GetFullUrl("a.aspx", QueryParamMode.ExplicitParameters, "x", "val", "y", "val2");
      Assert.IsTrue(url.Contains("/a.aspx"));
      Assert.IsTrue(url.IndexOf("//",8) == -1);
      url = links.GetFullUrl("a.aspx", "x", "val", "y", "val2");
      Assert.IsTrue(url.Contains("/a.aspx"));
      Assert.IsTrue(url.Contains("x=val"));
      Assert.IsTrue(url.Contains("y=val2"));
      Assert.IsTrue(url.IndexOf("//",8) == -1);

      url = links.GetUrl("SITEURL", "a.aspx", QueryParamMode.ExplicitParameters, true);
      Assert.IsTrue(url.Contains("/a.aspx"));
      Assert.IsTrue(url.Contains("https://"));
      Assert.IsTrue(url.IndexOf("//",8) == -1);

      url = links.GetUrl("SITEURL", "a.aspx", QueryParamMode.ExplicitParameters, true, parms);
      Assert.IsTrue(url.Contains("/a.aspx"));
      Assert.IsTrue(url.Contains("https://"));
      Assert.IsTrue(url.Contains("x=val"));
      Assert.IsTrue(url.Contains("y=val2"));
      Assert.IsTrue(url.IndexOf("//",8) == -1);

      url = links.GetUrl("SITEURL", "a.aspx", QueryParamMode.ExplicitParameters, true, "x", "val", "y", "val2");
      Assert.IsTrue(url.Contains("/a.aspx"));
      Assert.IsTrue(url.Contains("https://"));
      Assert.IsTrue(url.Contains("x=val"));
      Assert.IsTrue(url.Contains("y=val2"));
      Assert.IsTrue(url.IndexOf("//",8) == -1);

      url = links.GetUrl("SITEURL", "a.aspx", QueryParamMode.ExplicitParameters, "x", "val", "y", "val2");
      Assert.IsTrue(url.Contains("/a.aspx"));
      Assert.IsTrue(url.Contains("http://"));
      Assert.IsTrue(url.Contains("x=val"));
      Assert.IsTrue(url.Contains("y=val2"));
      Assert.IsTrue(url.IndexOf("//",8) == -1);

      url = links.GetUrl("SITEURL", "a.aspx", parms);
      Assert.IsTrue(url.Contains("/a.aspx"));
      Assert.IsTrue(url.Contains("http://"));
      Assert.IsTrue(url.Contains("x=val"));
      Assert.IsTrue(url.Contains("y=val2"));
      Assert.IsTrue(url.IndexOf("//",8) == -1);

      url = links.GetUrl("SITEURL", "a.aspx", "x", "val", "y", "val2");
      Assert.IsTrue(url.Contains("/a.aspx"));
      Assert.IsTrue(url.Contains("http://"));
      Assert.IsTrue(url.Contains("x=val"));
      Assert.IsTrue(url.Contains("y=val2"));
      Assert.IsTrue(url.IndexOf("//",8) == -1);

      // empty linktype always gets the defaultrootsite
      url = links.GetUrl(String.Empty, String.Empty, QueryParamMode.ExplicitParameters);
      Assert.IsTrue(url.Equals("http://www.godaddy.com/"));
      Assert.IsTrue(url.IndexOf("//",8) == -1);

      url = links.GetUrlArguments();
      Assert.IsTrue(url.Equals(String.Empty));

      url = links.GetUrlArguments(QueryParamMode.ExplicitParameters);
      Assert.IsTrue(url.Equals(String.Empty));

      url = links.GetUrlArguments(parms);
      Assert.IsTrue(url.Equals("?x=val&y=val2"));

      url = links.GetUrlArguments(parms, QueryParamMode.ExplicitParameters);
      Assert.IsTrue(url.Equals("?x=val&y=val2"));

      url = links.GetUrlArguments("x", "val", "y", "val2");
      Assert.IsTrue(url.Equals("?x=val&y=val2"));

      url = links.GetUrlArguments(QueryParamMode.ExplicitParameters, "x", "val", "y", "val2");
      Assert.IsTrue(url.Equals("?x=val&y=val2"));

      LinkProvider.AllowRelativeUrls = true;
      Assert.IsTrue(LinkProvider.AllowRelativeUrls);
      LinkProvider.AllowRelativeUrls = false;
      Assert.IsFalse(LinkProvider.AllowRelativeUrls);

      LinkProvider.AllowRelativeUrls = true;
      url = links.GetRelativeUrl("a.aspx", QueryParamMode.ExplicitParameters, parms);
      Assert.IsTrue(url.Contains("a.aspx?x=val&y=val2"));
      Assert.IsFalse(url.Contains("http"));
      Assert.IsTrue(url.IndexOf("//",8) == -1);

      TestCleanup();
      TestInit();

      LinkProvider.AllowRelativeUrls = false;
      links = NewLinkProvider("https://www.godaddy.com/default.aspx", 1, "832652");
      url = links.GetRelativeUrl("a.aspx", QueryParamMode.ExplicitParameters, parms);
      Assert.IsTrue(url.Contains("a.aspx?x=val&y=val2"));
      Assert.IsTrue(url.IndexOf("//",8) == -1);
// TODO: Mike, the HttpRequest says this isn't a secure connection... why?
//      Assert.IsTrue(url.Contains("https://"));
    }

    [TestMethod]
    public void LowerCaseForSEOFullUrlIgnored()
    {
      var links = NewLinkProvider("http://WWW.GODADDY.COM/default.aspx", 1, "832652");
      LinkProvider.LowerCaseRelativeUrlsForSEO = true;

      string url = links.GetUrl("SITEROOT", "TEST.AsPx");
      Assert.AreNotEqual(url.ToLowerInvariant(), url);
      Assert.IsTrue(url.IndexOf("//",8) == -1);

      LinkProvider.LowerCaseRelativeUrlsForSEO = false;
      var lp = FrameworkContainer.Resolve<ILocalizationProvider>();
      lp.RewrittenUrlLanguage = "es";
      url = links.GetUrl("SITEROOT", "TEST.AsPx");
      Assert.IsFalse(url.ToLowerInvariant().Contains("/es/"));
      Assert.IsTrue(url.IndexOf("//",8) == -1);
    }

    [TestMethod]
    public void LowerCaseForSEO()
    {
      var links = NewLinkProvider("http://WWW.GODADDY.COM/default.aspx", 1, "832652");
      LinkProvider.LowerCaseRelativeUrlsForSEO = false;

      string mixedCaseLink = links.GetRelativeUrl("/tesT.aspx");
      Assert.AreNotEqual(mixedCaseLink.ToLowerInvariant(), mixedCaseLink);
      Assert.IsFalse(mixedCaseLink.ToLowerInvariant().Contains("/es/"));
      Assert.IsTrue(mixedCaseLink.IndexOf("//",8) == -1);

      LinkProvider.LowerCaseRelativeUrlsForSEO = true;
      string lowerCaseLink = links.GetRelativeUrl("/tesT.aspx");
      Assert.AreEqual(lowerCaseLink.ToLowerInvariant(), lowerCaseLink);
      Assert.IsFalse(lowerCaseLink.ToLowerInvariant().Contains("/es/"));
      Assert.IsTrue(lowerCaseLink.IndexOf("//",8) == -1);

      string url = links.GetRelativeUrl("/test.aspx", "ISC", "Blue");
      Assert.IsTrue(url.Contains("ISC=Blue"));
      Assert.IsFalse(url.ToLowerInvariant().Contains("/es/"));
      Assert.IsTrue(url.IndexOf("//",8) == -1);

      LinkProvider.LowerCaseRelativeUrlsForSEO = false;
      var lp = FrameworkContainer.Resolve<ILocalizationProvider>();
      lp.RewrittenUrlLanguage = "es";

      mixedCaseLink = links.GetRelativeUrl("/tesT.aspx");
      Assert.AreNotEqual(mixedCaseLink.ToLowerInvariant(), mixedCaseLink);
      Assert.IsTrue(mixedCaseLink.ToLowerInvariant().Contains("/es/"));
      Assert.IsTrue(mixedCaseLink.IndexOf("//",8) == -1);

      LinkProvider.LowerCaseRelativeUrlsForSEO = true;
      lowerCaseLink = links.GetRelativeUrl("/tesT.aspx");
      Assert.AreEqual(lowerCaseLink.ToLowerInvariant(), lowerCaseLink);
      Assert.IsTrue(lowerCaseLink.ToLowerInvariant().Contains("/es/"));
      Assert.IsTrue(lowerCaseLink.IndexOf("//",8) == -1);

      url = links.GetRelativeUrl("/test.aspx", "ISC", "Blue");
      Assert.IsTrue(url.Contains("ISC=Blue"));
      Assert.IsTrue(url.ToLowerInvariant().Contains("/es/"));
      Assert.IsTrue(url.IndexOf("//",8) == -1);
    }

    [TestMethod]
    public void FullUrlNoTilda()
    {
      var links = NewLinkProvider("http://siteadmin.debug.intranet.gdg/default.aspx", 1724, string.Empty);
      string url = links.GetFullUrl("/test.aspx");
      Assert.IsTrue(url.Contains("prog_id="));
      Assert.IsTrue(url.IndexOf("//",8) == -1);

      LinkProvider.LowerCaseRelativeUrlsForSEO = false;
      var lp = FrameworkContainer.Resolve<ILocalizationProvider>();
      lp.RewrittenUrlLanguage = "es";

      url = links.GetFullUrl("/test.aspx");
      Assert.IsTrue(url.Contains("prog_id="));
      Assert.IsTrue(url.Contains("/es/"));
      Assert.IsTrue(url.IndexOf("//",8) == -1);
    }

    [TestMethod]
    public void ProgIdForHunter()
    {
      var links = NewLinkProvider("http://siteadmin.debug.intranet.gdg/default.aspx?prog_id=hunter", 1724, string.Empty);
      string url = links.GetRelativeUrl("/test.aspx");
      Assert.IsTrue(url.Contains("prog_id="));
      Assert.IsTrue(url.IndexOf("//",8) == -1);

      LinkProviderCommonParameters.HandleProgId = false;

      url = links.GetRelativeUrl("/test.aspx");
      Assert.IsFalse(url.Contains("prog_id="));
      Assert.IsTrue(url.IndexOf("//",8) == -1);

      LinkProvider.LowerCaseRelativeUrlsForSEO = false;
      var lp = FrameworkContainer.Resolve<ILocalizationProvider>();
      lp.RewrittenUrlLanguage = "es";

      url = links.GetRelativeUrl("/test.aspx");
      Assert.IsFalse(url.Contains("prog_id="));
      Assert.IsTrue(url.Contains("/es/"));
      Assert.IsTrue(url.IndexOf("//",8) == -1);

      LinkProviderCommonParameters.HandleProgId = true;

      url = links.GetRelativeUrl("/test.aspx");
      Assert.IsTrue(url.Contains("prog_id="));
      Assert.IsTrue(url.Contains("/es/"));
      Assert.IsTrue(url.IndexOf("//",8) == -1);
    }

    [TestMethod]
    public void ProgIdGoDaddy()
    {
      var links = NewLinkProvider("http://siteadmin.debug.intranet.gdg/default.aspx?prog_id=hunter", 1, string.Empty);
      string url = links.GetRelativeUrl("/test.aspx");
      Assert.IsFalse(url.Contains("prog_id="));
      Assert.IsTrue(url.IndexOf("//",8) == -1);

      LinkProviderCommonParameters.HandleProgId = true;

      LinkProvider.LowerCaseRelativeUrlsForSEO = false;
      var lp = FrameworkContainer.Resolve<ILocalizationProvider>();
      lp.RewrittenUrlLanguage = "es";

      url = links.GetRelativeUrl("/test.aspx");
      Assert.IsFalse(url.Contains("prog_id="));
      Assert.IsTrue(url.Contains("/es/"));
      Assert.IsTrue(url.IndexOf("//",8) == -1);
    }

    [TestMethod]
    public void ProgIdBlueRazor()
    {
      var links = NewLinkProvider("http://siteadmin.debug.intranet.gdg/default.aspx?prog_id=hunter", 2, string.Empty);
      string url = links.GetRelativeUrl("/test.aspx");
      Assert.IsFalse(url.Contains("prog_id="));
      Assert.IsTrue(url.IndexOf("//",8) == -1);

      LinkProviderCommonParameters.HandleProgId = true;

      LinkProvider.LowerCaseRelativeUrlsForSEO = false;
      var lp = FrameworkContainer.Resolve<ILocalizationProvider>();
      lp.RewrittenUrlLanguage = "es";

      url = links.GetRelativeUrl("/test.aspx");
      Assert.IsFalse(url.Contains("prog_id="));
      Assert.IsTrue(url.Contains("/es/"));
      Assert.IsTrue(url.IndexOf("//",8) == -1);
    }

    [TestMethod]
    public void ProgIdWildWestDomains()
    {
      var links = NewLinkProvider("http://siteadmin.debug.intranet.gdg/default.aspx?prog_id=hunter", 1387, string.Empty);
      string url = links.GetRelativeUrl("/test.aspx");
      Assert.IsFalse(url.Contains("prog_id="));
      Assert.IsTrue(url.IndexOf("//",8) == -1);

      LinkProviderCommonParameters.HandleProgId = true;

      LinkProvider.LowerCaseRelativeUrlsForSEO = false;
      var lp = FrameworkContainer.Resolve<ILocalizationProvider>();
      lp.RewrittenUrlLanguage = "es";

      url = links.GetRelativeUrl("/test.aspx");
      Assert.IsFalse(url.Contains("prog_id="));
      Assert.IsTrue(url.Contains("/es/"));
      Assert.IsTrue(url.IndexOf("//",8) == -1);
    }

    [TestMethod]
    public void ISC()
    {
      var links = NewLinkProvider("http://siteadmin.debug.intranet.gdg/default.aspx?isc=blue", 2, string.Empty);

      string url = links.GetRelativeUrl("/test.aspx");
      Assert.IsTrue(url.Contains("isc=blue"));
      Assert.IsTrue(url.IndexOf("//",8) == -1);

      url = links.GetRelativeUrl("/test.aspx", "isc", "green");
      Assert.IsTrue(url.Contains("isc=green"));
      Assert.IsTrue(url.IndexOf("//",8) == -1);

      LinkProviderCommonParameters.HandleISC = false;
      url = links.GetRelativeUrl("/test.aspx");
      Assert.IsFalse(url.Contains("isc="));
      Assert.IsTrue(url.IndexOf("//",8) == -1);

      LinkProviderCommonParameters.HandleISC = true;

      LinkProvider.LowerCaseRelativeUrlsForSEO = false;

      var lp = FrameworkContainer.Resolve<ILocalizationProvider>();
      lp.RewrittenUrlLanguage = "es";

      url = links.GetRelativeUrl("/test.aspx");
      Assert.IsTrue(url.Contains("isc=blue"));
      Assert.IsTrue(url.Contains("/es/"));
      Assert.IsTrue(url.IndexOf("//",8) == -1);

      url = links.GetRelativeUrl("/test.aspx", "isc", "green");
      Assert.IsTrue(url.Contains("isc=green"));
      Assert.IsTrue(url.Contains("/es/"));
      Assert.IsTrue(url.IndexOf("//",8) == -1);

      LinkProviderCommonParameters.HandleISC = false;
      url = links.GetRelativeUrl("/test.aspx");
      Assert.IsFalse(url.Contains("isc="));
      Assert.IsTrue(url.Contains("/es/"));
      Assert.IsTrue(url.IndexOf("//",8) == -1);
    }

    [TestMethod]
    public void IsDebugInternal()
    {
      var links = NewLinkProvider("http://siteadmin.debug.intranet.gdg/default.aspx", 1, "832652", true);
      Assert.IsTrue(links.IsDebugInternal());
    }

    [TestMethod]
    public void IsDevNotDebugInternal()
    {
      var links = NewLinkProvider("http://siteadmin.dev.intranet.gdg/default.aspx", 1, "832652", true);
      Assert.IsFalse(links.IsDebugInternal());
    }

    private static bool _handleActive = false;
    public static void HandleCommonParmeters(IProviderContainer container, NameValueCollection queryMap)
    {
      if (_handleActive)
      {
        queryMap["extra"] = "true";
      }
      _handleActive = false;
    }

    [TestMethod]
    public void CommonParametersDelegate()
    {
      LinkProviderCommonParameters.AddCommonParameters += HandleCommonParmeters;
      try
      {
        var links = NewLinkProvider("http://siteadmin.debug.intranet.gdg/default.aspx", 2, string.Empty);

        _handleActive = true;

        string url = links.GetRelativeUrl("/test.aspx");
        Assert.IsTrue(url.Contains("extra=true"));
        Assert.IsTrue(url.IndexOf("//",8) == -1);

        var lp = FrameworkContainer.Resolve<ILocalizationProvider>();
        lp.RewrittenUrlLanguage = "es";

        url = links.GetRelativeUrl("/test.aspx");
        Assert.IsTrue(url.Contains("/es/"));
        Assert.IsTrue(url.IndexOf("//",8) == -1);

      }
      finally
      {
        LinkProviderCommonParameters.AddCommonParameters -= HandleCommonParmeters;
      }
    }


    [TestMethod]
    public void XSSParameters()
    {
      LinkProviderCommonParameters.AddCommonParameters += HandleCommonParmeters;
      try
      {
        var links = NewLinkProvider("http://www.godaddy.com/ssl-certificates.aspx&ci=8979&%3C/script%3E%3Cscript%3Ealert%28666%29%3C/script%3E=123", 1, string.Empty);

        _handleActive = true;

        string url = links.GetRelativeUrl("/test.aspx", HttpContext.Current.Request.QueryString);
        Assert.IsFalse(url.Contains("<"));
        Assert.IsTrue(url.IndexOf("//",8) == -1);

        var lp = FrameworkContainer.Resolve<ILocalizationProvider>();
        lp.RewrittenUrlLanguage = "es";

        url = links.GetRelativeUrl("/test.aspx", HttpContext.Current.Request.QueryString);
        Assert.IsFalse(url.Contains("<"));
        Assert.IsTrue(url.Contains("/es/"));
        Assert.IsTrue(url.IndexOf("//",8) == -1);

      }
      finally
      {
        LinkProviderCommonParameters.AddCommonParameters -= HandleCommonParmeters;
      }
    }

    [TestMethod]
    public void TestRelativeUrlWithLeadingSlashArguments()
    {
      ILinkProvider links = NewLinkProvider("http://siteadmin.debug.intranet.gdg/default.aspx", 1, string.Empty);
      string url = links.GetRelativeUrl("/hosting/hosting.aspx");
      Assert.AreEqual("http://siteadmin.debug.intranet.gdg/hosting/hosting.aspx", url);
      Assert.IsTrue(url.IndexOf("//",8) == -1);

      url = links.GetRelativeUrl("/");
      Assert.AreEqual("http://siteadmin.debug.intranet.gdg/", url);
      Assert.IsTrue(url.IndexOf("//",8) == -1);

      var lp = FrameworkContainer.Resolve<ILocalizationProvider>();
      lp.RewrittenUrlLanguage = "es";

      url = links.GetRelativeUrl("/hosting/hosting.aspx");
      Assert.AreEqual("http://siteadmin.debug.intranet.gdg/es/hosting/hosting.aspx", url);
      Assert.IsTrue(url.IndexOf("//",8) == -1);

      url = links.GetRelativeUrl("/");
      Assert.AreEqual("http://siteadmin.debug.intranet.gdg/es/", url);
      Assert.IsTrue(url.IndexOf("//",8) == -1);
    }

    /// <summary>
    /// Cannot test this unless we can set HttpRuntime.AppDomainAppVirtualPath
    /// </summary>
    public void TestRelativeUrlWithLeadingTildeArguments()
    {
      ILinkProvider links = NewLinkProvider("http://siteadmin.debug.intranet.gdg/default.aspx", 1, string.Empty);
      string url = links.GetRelativeUrl("~/hosting/hosting.aspx");
      Assert.AreEqual("http://siteadmin.debug.intranet.gdg/hosting/hosting.aspx", url);
      Assert.IsTrue(url.IndexOf("//",8) == -1);

      url = links.GetRelativeUrl("~/");
      Assert.AreEqual("http://siteadmin.debug.intranet.gdg/", url);
      Assert.IsTrue(url.IndexOf("//",8) == -1);

      var lp = FrameworkContainer.Resolve<ILocalizationProvider>();
      lp.RewrittenUrlLanguage = "es";

      url = links.GetRelativeUrl("~/hosting/hosting.aspx");
      Assert.AreEqual("http://siteadmin.debug.intranet.gdg/es/hosting/hosting.aspx", url);
      Assert.IsTrue(url.IndexOf("//",8) == -1);

      url = links.GetRelativeUrl("~/");
      Assert.AreEqual("http://siteadmin.debug.intranet.gdg/es/", url);
      Assert.IsTrue(url.IndexOf("//",8) == -1);
    }

    [TestMethod]
    public void TestRelativeUrlWithNoLeadingCharacterArguments()
    {
      ILinkProvider links = NewLinkProvider("http://siteadmin.debug.intranet.gdg/default.aspx", 1, string.Empty);
      string url = links.GetRelativeUrl("hosting/hosting.aspx");
      Assert.AreEqual("http://siteadmin.debug.intranet.gdg/hosting/hosting.aspx", url);  //  No slash added between host and relative path argument
      Assert.IsTrue(url.IndexOf("//",8) == -1);

      url = links.GetRelativeUrl("");
      Assert.AreEqual("http://siteadmin.debug.intranet.gdg/", url);
      Assert.IsTrue(url.IndexOf("//",8) == -1);

      var lp = FrameworkContainer.Resolve<ILocalizationProvider>();
      lp.RewrittenUrlLanguage = "es";

      url = links.GetRelativeUrl("hosting/hosting.aspx");
      Assert.AreEqual("http://siteadmin.debug.intranet.gdg/es/hosting/hosting.aspx", url);
      Assert.IsTrue(url.IndexOf("//",8) == -1);

      url = links.GetRelativeUrl("");
      Assert.AreEqual("http://siteadmin.debug.intranet.gdg/es/", url);  //  No slash added between host and relative path argument
      Assert.IsTrue(url.IndexOf("//",8) == -1);
    }

  }
}