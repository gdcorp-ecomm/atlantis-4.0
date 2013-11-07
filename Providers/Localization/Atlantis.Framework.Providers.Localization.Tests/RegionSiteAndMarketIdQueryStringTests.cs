using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Security.Policy;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Localization.MockImpl;
using Atlantis.Framework.Providers.Geo.Interface;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.Providers.Localization.Tests.Mocks;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;

namespace Atlantis.Framework.Providers.Localization.Tests
{
  [TestClass]
  [DeploymentItem(afeConfig)]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  [DeploymentItem("Atlantis.Framework.Localization.MockImpl.dll")]
  public class RegionSiteAndMarketIdQueryStringTests
  {

    #region Setup

    public const string afeConfig = "atlantis.internationalization.redirect.provider.tests.config";

    [ClassInitialize]
    public static void ClassInit(TestContext c)
    {
      Atlantis.Framework.Engine.Engine.ReloadConfig(afeConfig);

      ReloadCache();
    }

    private static void ReloadCache()
    {
      foreach (var config in Engine.Engine.GetConfigElements())
      {
        DataCache.DataCache.ClearCachedData(config.RequestType);
      }
    }

    private void ParseUrl(string url, out string filename, out string queryString)
    {
      string[] parts = url.Split(new char[] { '?' }, 2, StringSplitOptions.RemoveEmptyEntries);
      if (parts.Length == 2)
        queryString = parts[1];
      else
      {
        queryString = string.Empty;
      }

      filename = parts[0].Substring(parts[0].LastIndexOf('/') + 1);
      if (!filename.Contains("."))
        filename = string.Empty;
    }

    private HttpRequest SetHttpContext(string url, string httpVerbName = "GET", string applicationPath = "", NameValueCollection mockCookies = null,
      string acceptLanguageHeaderValues = null)
    {
      MockHttpRequest request = new MockCustomHttpRequest(url, httpVerbName, applicationPath);

      //  Add any cookies because the cookie code uses HttpContext.Current
      if (mockCookies != null)
      {
        request.MockCookies(mockCookies);
      }

      //  Add any header values
      if (!string.IsNullOrEmpty(acceptLanguageHeaderValues))
      {
        ((MockCustomHttpRequest)request).MockAcceptLanguageHeaderValues(acceptLanguageHeaderValues);
      }

      MockHttpContext.SetFromWorkerRequest(request);

      //  Set testing resources (put in HttpContext.Current.Items because the MockImpl Localization triplet classes look there)
      HttpContext.Current.Items[MockLocalizationSettings.CountrySiteMarketMappingsTable] = Properties.Resources.CountrySiteMarketMappings;
      HttpContext.Current.Items[MockLocalizationSettings.CountrySitesActiveTable] = Properties.Resources.CountrySitesActive;
      HttpContext.Current.Items[MockLocalizationSettings.MarketsActiveTable] = Properties.Resources.MarketsActive;

      return HttpContext.Current.Request;
    }

    //  Set Http classes (Context, Request, Response, etc) in the HttpContextFactory.
    //  This allows mocking of some dependencies like applicationPath
    private void SetHttpContextFactory(string url, HttpRequest baseRequest, string httpVerbName = "GET", string applicationPath = "")
    {
      string filename;
      string queryString;
      ParseUrl(url, out filename, out queryString);
      var mockRequest = new Mocks.Http.MockHttpRequest(baseRequest, httpVerbName ?? "GET", applicationPath ?? string.Empty);
      var context = new Mocks.Http.MockHttpContext(mockRequest, new Mocks.Http.MockHttpResponse());
      HttpContextFactory.SetHttpContext(context);
    }

    private IProviderContainer SetContext(string url, string ipCountry, NameValueCollection mockCookies = null,
      string acceptLanguageHeaderValues = null)
    {
      string httpVerbName = "GET";
      string applicationPath = string.Empty;

      HttpRequest request = SetHttpContext(url, httpVerbName, applicationPath, mockCookies, acceptLanguageHeaderValues);
      SetHttpContextFactory(url, request, httpVerbName, applicationPath);

      var result = new MockProviderContainer();
      result.RegisterProvider<ISiteContext, MockSiteContext>();
      result.RegisterProvider<ILocalizationRedirectProvider, InternationalizationRedirectProvider>();
      result.RegisterProvider<ILanguageUrlRewriteProvider, LanguageUrlRewriteProvider>();
      result.RegisterProvider<IGeoProvider, MockGeoProvider>();
      result.SetData(MockGeoProvider.REQUEST_COUNTRY_SETTING_NAME, ipCountry);

      return result;
    }

    private IProviderContainer SetCookieContext(string url, string ipCountry, NameValueCollection mockCookies = null,
                                                string acceptLanguageHeaderValues = null)
    {
      var result = SetContext(url, ipCountry, mockCookies, acceptLanguageHeaderValues);
      result.RegisterProvider<ILocalizationProvider, CountryCookieLocalizationProvider>();
      return result;
    }

    private IProviderContainer SetSubdomainContext(string url, string ipCountry, NameValueCollection mockCookies = null, string acceptLanguageHeaderValues = null)
    {
      var result = SetContext(url, ipCountry, mockCookies, acceptLanguageHeaderValues);
      result.RegisterProvider<ILocalizationProvider, CountrySubdomainLocalizationProvider>();
      return result;
    }

    private void CreateCountryCookie(int privateLabelId, string value, NameValueCollection nvc = null)
    {
      string cookieName = "countrysite" + privateLabelId.ToString(CultureInfo.InvariantCulture);
      if (nvc != null)
      {
        nvc[cookieName] = value;
      }
      else
      {
        var countryCookie = new HttpCookie(cookieName, value);
        HttpContext.Current.Response.Cookies.Set(countryCookie);
      }
    }

    private void CreateLanguagePreferenceCookie(int privateLabelId, string value, NameValueCollection nvc = null)
    {
      string cookieName = "language" + privateLabelId.ToString(CultureInfo.InvariantCulture);
      if (nvc != null)
      {
        nvc[cookieName] = value;
      }
      else
      {
        var languageCookie = new HttpCookie(cookieName, value);
        HttpContext.Current.Response.Cookies.Set(languageCookie);
      }
    }

    private void TestLanguageCookie(IProviderContainer container, string expectedValue)
    {
      var siteContext = container.Resolve<ISiteContext>();
      string cookieName = "language" + siteContext.PrivateLabelId.ToString(CultureInfo.InvariantCulture);
      Assert.AreEqual(expectedValue.ToLowerInvariant(), HttpContext.Current.Request.Cookies[cookieName].Value.ToLowerInvariant());
    }

    private void TestCountrySiteCookie(IProviderContainer container, string expectedValue)
    {
      var siteContext = container.Resolve<ISiteContext>();
      string cookieName = "countrysite" + siteContext.PrivateLabelId.ToString(CultureInfo.InvariantCulture);
      Assert.AreEqual(expectedValue.ToLowerInvariant(), HttpContext.Current.Request.Cookies[cookieName].Value.ToLowerInvariant());
    }

    #endregion

    #region regionsite querystring tests

    [TestMethod]
    public void Cookie_WWW_RegionSite_INVALID_MarketId_NONE_CountrySiteCookie_VALID_LanguageCookie_NONE_IpCountry_IGNORED_Result_CSCOOKIE_DEFAULTMARKETID()
    {
      var container = SetCookieContext("http://www.mysite.com?regionsite=11", "uk");
      CreateCountryCookie(1, "au");

      var localization = container.Resolve<ILocalizationProvider>();
      
      Assert.AreEqual("au", localization.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-au", localization.MarketInfo.Id.ToLowerInvariant());
      TestCountrySiteCookie(container, "au");
      TestLanguageCookie(container, "en-au");
    }

    [TestMethod]
    public void Cookie_WWW_RegionSite_VALID_MarketId_INVALID_CountrySiteCookie_IGNORED_LanguageCookie_IGNORED_IpCountry_IGNORED_Result_REGIONSITE_DEFAULTMARKETID()
    {
      var container = SetCookieContext("http://www.mysite.com?regionsite=ca&marketid=11", "uk");
      CreateCountryCookie(1, "au");
      CreateLanguagePreferenceCookie(1, "fr-ca");

      var localization = container.Resolve<ILocalizationProvider>();
      Assert.AreEqual("ca", localization.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-ca", localization.MarketInfo.Id.ToLowerInvariant());
      TestCountrySiteCookie(container, "ca");
      TestLanguageCookie(container, "en-ca");
    }

    [TestMethod]
    public void Cookie_WWW_RegionSite_VALID_MarketId_VALID_CountrySiteCookie_IGNORED_LanguageCookie_IGNORED_IpCountry_IGNORED_Result_REGIONSITE_MARKETID()
    {
      var container = SetCookieContext("http://www.mysite.com?regionsite=ca&marketid=fr-ca", "uk");
      CreateCountryCookie(1, "au");
      CreateLanguagePreferenceCookie(1, "en-ca");

      var localization = container.Resolve<ILocalizationProvider>();
      Assert.AreEqual("ca", localization.CountrySite.ToLowerInvariant());
      Assert.AreEqual("fr-ca", localization.MarketInfo.Id.ToLowerInvariant());
      TestCountrySiteCookie(container, "ca");
      TestLanguageCookie(container, "fr-ca");
    }

    [TestMethod]
    public void Cookie_IGNORED_RegionSite_WWW_MarketId_VALID_CountrySiteCookie_IGNORED_LanguageCookie_IGNORED_IpCountry_IGNORED_Result_REGIONSITE_MARKETID()
    {
      var container = SetCookieContext("http://gui.mysite.com?regionsite=www&marketid=en-us", "uk");
      CreateCountryCookie(1, "ca");
      CreateLanguagePreferenceCookie(1, "fr-ca");

      var localization = container.Resolve<ILocalizationProvider>();
      Assert.AreEqual("www", localization.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-us", localization.MarketInfo.Id.ToLowerInvariant());
      TestCountrySiteCookie(container, "www");
      TestLanguageCookie(container, "en-us");
    }

    [TestMethod]
    public void Subdomain_VALID_RegionSite_IGNORED_MarketId_INVALID_CountrySiteCookie_IGNORED_IpCountry_IGNORED_Result_SUBDOMAIN_DEFAULTMARKETID()
    {
      var container = SetSubdomainContext("http://ca.mysite.com?regionsite=11", "uk");
      CreateCountryCookie(1, "au");
      CreateLanguagePreferenceCookie(1, "en-au");

      var localization = container.Resolve<ILocalizationProvider>();
      Assert.AreEqual("ca", localization.CountrySite.ToLowerInvariant());
      Assert.AreEqual("au", localization.PreviousCountrySiteCookieValue.ToLowerInvariant());
      TestCountrySiteCookie(container, "ca");
      Assert.AreEqual("en-ca", localization.MarketInfo.Id.ToLowerInvariant());
      Assert.AreEqual("en-au", localization.PreviousLanguageCookieValue.ToLowerInvariant());
      TestLanguageCookie(container, "en-ca");
    }

    [TestMethod]
    public void Subdomain_VALID_RegionSite_IGNORED_MarketId_VALID_CountrySiteCookie_IGNORED_IpCountry_IGNORED_Result_SUBDOMAIN_MARKETID()
    {
      var container = SetSubdomainContext("http://ca.mysite.com?regionsite=11&marketid=fr-ca", "uk");
      CreateCountryCookie(1, "au");
      CreateLanguagePreferenceCookie(1, "en-au");

      var localization = container.Resolve<ILocalizationProvider>();
      Assert.AreEqual("ca", localization.CountrySite.ToLowerInvariant());
      Assert.AreEqual("au", localization.PreviousCountrySiteCookieValue.ToLowerInvariant());
      TestCountrySiteCookie(container, "ca");
      Assert.AreEqual("fr-ca", localization.MarketInfo.Id.ToLowerInvariant());
      Assert.AreEqual("en-au", localization.PreviousLanguageCookieValue.ToLowerInvariant());
      TestLanguageCookie(container, "fr-ca");
    }

    [TestMethod]
    public void Subdomain_WWW_RegionSite_IGNORED_MarketId_NONE_CountrySiteCookie_VALID_LanguageCookie_IGNORED_IpCountry_IGNORED_Result_WWW_DEFAULTMARKETID()
    {
      var container = SetSubdomainContext("http://www.mysite.com?regionsite=in", "uk");
      CreateCountryCookie(1, "au");

      var localization = container.Resolve<ILocalizationProvider>();
      Assert.AreEqual("www", localization.CountrySite.ToLowerInvariant());
      Assert.AreEqual("au", localization.PreviousCountrySiteCookieValue.ToLowerInvariant());
      TestCountrySiteCookie(container, "www");  //  This is OK because there will be a redirect to the countrysite
      Assert.AreEqual("en-us", localization.MarketInfo.Id.ToLowerInvariant());
      Assert.IsNull(localization.PreviousLanguageCookieValue);
      TestLanguageCookie(container, "en-us");
    }

    [TestMethod]
    public void Subdomain_WWW_RegionSite_IGNORED_MarketId_INVALID_CountrySiteCookie_VALID_IpCountry_IGNORED_Result_CSCOOKIE_MARKETID()
    {
      var container = SetSubdomainContext("http://www.mysite.com?regionsite=11&marketid=es-us", "uk");
      CreateCountryCookie(1, "ca");
      CreateLanguagePreferenceCookie(1, "fr-ca");

      var localization = container.Resolve<ILocalizationProvider>();
      Assert.AreEqual("www", localization.CountrySite.ToLowerInvariant());
      Assert.AreEqual("ca", localization.PreviousCountrySiteCookieValue.ToLowerInvariant());
      TestCountrySiteCookie(container, "www");    //  This is OK because there will be a redirect to the countrysite
      Assert.AreEqual("es-us", localization.MarketInfo.Id.ToLowerInvariant());  //  This is OK because there will be a redirect to countrysite and its default market
      Assert.AreEqual("fr-ca", localization.PreviousLanguageCookieValue.ToLowerInvariant());
      TestLanguageCookie(container, "es-us");
    }

    #endregion
  }
}
