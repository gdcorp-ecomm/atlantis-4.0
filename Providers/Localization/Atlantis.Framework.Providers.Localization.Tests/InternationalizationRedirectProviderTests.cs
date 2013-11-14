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
  public class InternationalizationRedirectProviderTests
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

    #endregion

    #region Base tests

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

    /// <summary>
    /// Default scenario
    /// </summary>
    [TestMethod]
    public void Subdomain_WWW_CountrySiteCookie_NONE_LanguageCookie_NONE_IpCountry_NONE_BrowserLanguages_NONE_Result_Redirect_NO()
    {
      var container = SetSubdomainContext("http://www.mysite.com", string.Empty);

      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("www", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-us", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);
    }

    /// <summary>
    /// Previous cookies are for different country/market but because countryview=1 is in querystring
    /// Use the subdomain countrysite and calculate market id.
    /// </summary>
    [TestMethod]
    public void CountryView_Subdomain_WWW_CountrySiteCookie_AU_LanguageCookie_ENAU_IpCountry_NONE_BrowserLanguages_NONE_Result_Redirect_NO_COUNTRYVIEW()
    {
      var container = SetSubdomainContext("http://www.mysite.com?countryview=1", string.Empty);
      CreateCountryCookie(1, "au");
      CreateLanguagePreferenceCookie(1, "en-au");
      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("www", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-us", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);
    }

    /// <summary>
    /// Transperfect proxy request
    /// Always use "www" and "es-us" for redirection
    /// </summary>
    [TestMethod]
    public void Transperfect_Subdomain_ES_CountrySiteCookie_VALID_LanguageCookie_VALID_IpCountry_NONE_BrowserLanguages_NONE_Result_Redirect_NO_TRANSPERFECT()
    {
      var container = SetSubdomainContext("http://es.mysite.com", string.Empty);
      container.RegisterProvider<IProxyContext, TransperfectTestWebProxy>();
      CreateCountryCookie(1, "au");
      CreateLanguagePreferenceCookie(1, "en-au");

      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("es", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("es-us", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("es", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.ShortLanguage);
    }

    /// <summary>
    /// If a countrysite cannot be obtained from subdomain, countrysite cookie, or ip address
    /// DO NOT "back-in" to a country site from the language cookie's market ID
    /// </summary>
    [TestMethod]
    public void Subdomain_WWW_CountrySiteCookie_NONE_LanguageCookie_ENCA_IpCountry_NONE_BrowserLanguages_NONE_Result_Redirect_NO_NOCOUNTRYSITE()
    {
      var container = SetSubdomainContext("http://www.mysite.com", string.Empty);
      CreateLanguagePreferenceCookie(1, "en-ca");

      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("www", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-us", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);
    }

    #endregion

    #region Fallback tests

    [TestMethod]
    public void Cookie_CountrySiteCookie_WITHPROBLEMS_LanguageCookie_NONE_IpCountry_NONE_BrowserLanguages_NONE_Result_Redirect_WWW_ENUS()
    {
      var container = SetCookieContext("http://who.mysite.com", string.Empty);
      CreateCountryCookie(1, "yy"); //  No valid default market
      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("www", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-us", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      container = SetCookieContext("http://who.mysite.com", string.Empty);
      CreateCountryCookie(1, "zz"); //  No mapping
      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("www", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-us", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);
    }

    [TestMethod]
    public void Subdomain_WITHPROBLEMS_CountrySiteCookie_NONE_LanguageCookie_NONE_IpCountry_NONE_BrowserLanguages_NONE_Result_Redirect_WWW_ENUS()
    {
      //  Invalid default market
      var container = SetSubdomainContext("http://yy.mysite.com", string.Empty);
      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("www", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-us", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      //  No mapping
      container = SetSubdomainContext("http://zz.mysite.com", string.Empty);
      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("www", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-us", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);
    }

    #endregion

    #region Non Global Site tests

    /// <summary>
    /// Previous cookies are for different country/market but because countryview=1 is in querystring
    /// Use the subdomain countrysite and calculate market id.
    /// </summary>
    [TestMethod]
    public void CountryView_Subdomain_VALID_CountrySiteCookie_AU_LanguageCookie_ENAU_IpCountry_NONE_BrowserLanguages_NONE_Result_Redirect_SUBDOMAIN_DEFAULTMARKET()
    {
      var container = SetSubdomainContext("http://br.mysite.com?countryview=1", string.Empty);
      CreateCountryCookie(1, "au");
      CreateLanguagePreferenceCookie(1, "en-au");
      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("br", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("pt-br", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("pt", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      container = SetSubdomainContext("http://ca.mysite.com?countryview=1", string.Empty);
      CreateCountryCookie(1, "au");
      CreateLanguagePreferenceCookie(1, "en-au");
      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("ca", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-ca", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);
    }

    /// <summary>
    /// Subdomain and its default market id match the LocalizationProvider values
    /// </summary>
    [TestMethod]
    public void Subdomain_CA_CountrySiteCookie_NONE_LanguageCookie_NONE_IpCountry_NONE_BrowserLanguages_NONE_Result_Redirect_NO_DEFAULTSMATCHCURRENT()
    {
      var container = SetSubdomainContext("http://ca.mysite.com", string.Empty);

      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("ca", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-ca", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);
    }

    /// <summary>
    /// Subdomain and language cookie market ID match the LocalizationProvider values
    /// </summary>
    [TestMethod]
    public void Subdomain_CA_CountrySiteCookie_NONE_LanguageCookie_DEFAULT_IpCountry_NONE_BrowserLanguages_NONE_Result_Redirect_NO_DEFAULTSMATCHCURRENT()
    {
      var container = SetSubdomainContext("http://ca.mysite.com", string.Empty);
      CreateLanguagePreferenceCookie(1, "en-CA");

      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("ca", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-ca", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);
    }

    /// <summary>
    /// Language cookie has a market ID that is not the default for the countrysite
    /// </summary>
    [TestMethod]
    public void Subdomain_CA_CountrySiteCookie_NONE_LanguageCookie_NONDEFAULT_IpCountry_NONE_BrowserLanguages_NONE_Result_Redirect_CHANGELANGUAGE()
    {
      var container = SetSubdomainContext("http://ca.mysite.com", string.Empty);
      CreateLanguagePreferenceCookie(1, "fr-CA");

      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("ca", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("fr-ca", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("fr", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);
    }

    /// <summary>
    /// Browser language match for a non-default language of the subdomain countrysite
    /// </summary>
    [TestMethod]
    public void Subdomain_CA_CountrySiteCookie_NONE_LanguageCookie_NONE_IpCountry_NONE_BrowserLanguages_LIST_Result_Redirect_CHANGELANGUAGE()
    {
      string acceptLanguageHeaderValues = "fil,fr-CA;q=0.8,fr;q=0.6,en-US;q=0.4,en;q=0.2,en-CA;q=0.2,";
      var container = SetSubdomainContext("http://ca.mysite.com", string.Empty, null, acceptLanguageHeaderValues);

      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("ca", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("fr-ca", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("fr", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);
    }

    /// <summary>
    /// CountrySite cookie and default market ID match Localization Provider values
    /// </summary>
    [TestMethod]
    public void Cookie_WWW_CountrySiteCookie_VALID_LanguageCookie_NONE_IpCountry_NONE_BrowserLanguages_NONE_Result_Redirect_NO_DEFAULTSMATCHCURRENT()
    {
      var container = SetCookieContext("http://www.mysite.com", string.Empty);
      CreateCountryCookie(1, "au");

      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("au", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-au", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      container = SetCookieContext("http://www.mysite.com", string.Empty);
      CreateCountryCookie(1, "fr");

      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("fr", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("fr-fr", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("fr", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      container = SetCookieContext("http://www.mysite.com", string.Empty);
      CreateCountryCookie(1, "br");

      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("br", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("pt-br", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("pt", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);
    }

    /// <summary>
    /// Language cookie is default for the countrysite from cookie
    /// </summary>
    [TestMethod]
    public void Cookie_WWW_CountrySiteCookie_VALID_LanguageCookie_DEFAULT_IpCountry_NONE_BrowserLanguages_NONE_Result_Redirect_NO_DEFAULTSMATCHCURRENT()
    {
      var container = SetCookieContext("http://www.mysite.com", string.Empty);
      CreateCountryCookie(1, "au");
      CreateLanguagePreferenceCookie(1, "en-AU");

      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("au", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-au", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      container = SetCookieContext("http://www.mysite.com", string.Empty);
      CreateCountryCookie(1, "fr");
      CreateLanguagePreferenceCookie(1, "fr-FR");

      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("fr", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("fr-fr", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("fr", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      container = SetCookieContext("http://www.mysite.com", string.Empty);
      CreateCountryCookie(1, "br");
      CreateLanguagePreferenceCookie(1, "pt-BR");

      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("br", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("pt-br", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("pt", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);
    }

    /// <summary>
    /// Language cookie is valid but not default for the countrysite from cookie
    /// </summary>
    [TestMethod]
    public void Cookie_WWW_CountrySiteCookie_VALID_LanguageCookie_NONDEFAULT_IpCountry_NONE_BrowserLanguages_NONE_Result_Redirect_CHANGELANGUAGE()
    {
      var container = SetCookieContext("http://www.mysite.com", string.Empty);
      CreateCountryCookie(1, "ca");
      CreateLanguagePreferenceCookie(1, "fr-ca");

      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("ca", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("fr-ca", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("fr", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      container = SetCookieContext("http://www.mysite.com", string.Empty);
      CreateCountryCookie(1, "www");
      CreateLanguagePreferenceCookie(1, "es-US");

      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("www", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("es-us", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("es", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);
    }

    #endregion

    #region Global Site with language cookie tests

    [TestMethod]
    public void Cookie_CountrySiteCookie_WWW_LanguageCookie_VALID_IpCountry_NONE_BrowserLanguages_NONE_Result_Redirect_WWW_MARKETIDFROMCOOKIE()
    {
      //  Internal only market - not recognized so default market will be used
      var container = SetCookieContext("http://whois.mysite.com", string.Empty);
      CreateCountryCookie(1, "www");
      CreateLanguagePreferenceCookie(1, "qa-QA");
      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("www", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-us", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      //  Internal only market - recognized
      container = SetCookieContext("http://whois.mysite.com", string.Empty);
      container.SetData(MockSiteContextSettings.IsRequestInternal, true);
      CreateCountryCookie(1, "www");
      CreateLanguagePreferenceCookie(1, "qa-PS");
      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("www", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("qa-ps", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("qa-ps", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);
    }

    #endregion

    #region CountrySite cookie tests

    /// <summary>
    /// Redirect to countrysite cookie's default market
    /// </summary>
    [TestMethod]
    public void Subdomain_WWW_CountrySiteCookie_VALID_LanguageCookie_NONE_IpCountry_NONE_BrowserLanguages_NONE_Result_Redirect_VALID_DEFAULTMARKETID()
    {
      var container = SetSubdomainContext("http://www.mysite.com", string.Empty);
      CreateCountryCookie(1, "au");

      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("au", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-au", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      container = SetSubdomainContext("http://www.mysite.com", string.Empty);
      CreateCountryCookie(1, "ca");

      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("ca", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-ca", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);
    }

    #endregion

    #region CountrySite cookie and Language cookie tests

    /// <summary>
    /// Redirect to non-default market language cookie for countrysite
    /// </summary>
    [TestMethod]
    public void Subdomain_WWW_CountrySiteCookie_CA_LanguageCookie_FRCA_IpCountry_NONE_BrowserLanguages_NONE_Result_Redirect_CA_FRCA()
    {
      var container = SetSubdomainContext("http://www.mysite.com", string.Empty);
      CreateCountryCookie(1, "ca");
      CreateLanguagePreferenceCookie(1, "fr-ca");

      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("ca", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("fr-ca", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("fr", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);
    }

    /// <summary>
    /// Allow "US" as countrysite cookie.  Redirect to non-default but valid two-letter language in cookie
    /// </summary>
    [TestMethod]
    public void Subdomain_WWW_CountrySiteCookie_US_LanguageCookie_ES_IpCountry_NONE_BrowserLanguages_NONE_Result_Redirect_WWW_ESUS_SWITCHLANGUAGE()
    {
      var container = SetSubdomainContext("http://www.mysite.com", string.Empty);
      CreateCountryCookie(1, "www");
      CreateLanguagePreferenceCookie(1, "es");

      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("www", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("es-us", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("es", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      container = SetSubdomainContext("http://www.mysite.com", string.Empty);
      CreateLanguagePreferenceCookie(1, "es");
      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("www", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("es-us", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("es", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      //  Invalid test.  Sites using CookieLocalizationProvider don't have auto redirect
      /*
      container = SetCookieContext("http://who.mysite.com", string.Empty);
      CreateCountryCookie(1, "www");
      CreateLanguagePreferenceCookie(1, "es");
      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("www", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("es-us", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("es", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);
       */
    }

    /// <summary>
    /// Get a valid market id from language cookie and adjust two-letter language values
    /// </summary>
    [TestMethod]
    public void Subdomain_WWW_CountrySiteCookie_CA_LanguageCookie_VALID_IpCountry_NONE_BrowserLanguages_NONE_Result_Redirect_CA_MARKETIDFROMCOOKIE()
    {
      var container = SetSubdomainContext("http://www.mysite.com", string.Empty);
      CreateCountryCookie(1, "ca");
      CreateLanguagePreferenceCookie(1, "en");

      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("ca", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-ca", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      container = SetSubdomainContext("http://www.mysite.com", string.Empty);
      CreateCountryCookie(1, "ca");
      CreateLanguagePreferenceCookie(1, "en-ca");

      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("ca", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-ca", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      container = SetSubdomainContext("http://www.mysite.com", string.Empty);
      CreateCountryCookie(1, "ca");
      CreateLanguagePreferenceCookie(1, "fr");

      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("ca", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("fr-ca", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("fr", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      container = SetSubdomainContext("http://www.mysite.com", string.Empty);
      CreateCountryCookie(1, "ca");
      CreateLanguagePreferenceCookie(1, "fr-ca");

      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("ca", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("fr-ca", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("fr", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);
    }

    /// <summary>
    /// No valid countrysite nor language/market ID found so do not redirect because defaults match Localization Provider values
    /// </summary>
    [TestMethod]
    public void Subdomain_WWW_CountrySiteCookie_INVALID_LanguageCookie_INVALID_IpCountry_NONE_BrowserLanguages_NONE_Result_Redirect_NO_DEFAULTSMATCHCURRENT()
    {
      var container = SetSubdomainContext("http://www.mysite.com", string.Empty);
      CreateCountryCookie(1, "invalid");
      CreateLanguagePreferenceCookie(1, "invalid");

      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("www", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-us", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);
    }

    #endregion

    #region Language cookie and IP Country tests

    /// <summary>
    /// Get countrysite from IP address and redirect to default market for the countrysite; Disregard prior language cookie
    /// </summary>
    [TestMethod]
    public void Subdomain_WWW_CountrySiteCookie_NONE_LanguageCookie_IGNORED_IpCountry_VALID_BrowserLanguages_NONE_Result_Redirect_IPCOUNTRY_AND_DEFAULTMARKETID()
    {
      var container = SetSubdomainContext("http://www.mysite.com", "au");
      CreateLanguagePreferenceCookie(1, "pt-br");

      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("au", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-au", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      container = SetSubdomainContext("http://www.mysite.com", "ca");
      CreateLanguagePreferenceCookie(1, "xx-xx");
      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("ca", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-ca", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);
    }

    #endregion

    #region IP Country tests

    /// <summary>
    /// Get countrysite from IP but do not redirect because countrysite and default market id match LocalizationProvider values
    /// </summary>
    [TestMethod]
    public void Subdomain_WWW_CountrySiteCookie_NONE_LanguageCookie_NONE_IpCountry_US_BrowserLanguages_NONE_Result_Redirect_NO()
    {
      var container = SetSubdomainContext("http://www.mysite.com", "US");

      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("www", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-us", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

    }

    /// <summary>
    /// Invalid countrysite from IP but do not redirect because default countrysite and default market id match LocalizationProvider values
    /// </summary>
    [TestMethod]
    public void Subdomain_WWW_CountrySiteCookie_NONE_LanguageCookie_NONE_IpCountry_INVALID_BrowserLanguages_NONE_Result_Redirect_NO()
    {
      var container = SetSubdomainContext("http://www.mysite.com", "XX");

      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("www", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-us", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);
    }

    /// <summary>
    /// Get countrysite from IP address and redirect to default market id
    /// </summary>
    [TestMethod]
    public void Subdomain_WWW_CountrySiteCookie_NONE_LanguageCookie_NONE_IpCountry_VALID_BrowserLanguages_NONE_Result_Redirect_IPCOUNTRY_SINGLEMARKET()
    {
      var container = SetSubdomainContext("http://www.mysite.com", "au");
      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("au", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-au", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      container = SetSubdomainContext("http://www.mysite.com", "es");
      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("es", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("es-es", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("es", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      container = SetSubdomainContext("http://www.mysite.com", "pt");
      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("pt", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("pt-pt", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("pt", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);
    }

    /// <summary>
    /// Get countrysite from IP address and redirect to default market id
    /// </summary>
    [TestMethod]
    public void Subdomain_WWW_CountrySiteCookie_NONE_LanguageCookie_NONE_IpCountry_VALID_BrowserLanguages_NONE_Result_Redirect_IPCOUNTRY_DEFAULTMARKET()
    {
      var container = SetSubdomainContext("http://www.mysite.com", "www");
      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("www", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-us", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      container = SetSubdomainContext("http://www.mysite.com", "ca");
      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("ca", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-ca", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);
    }
    #endregion

    #region CountrySite cookie and Browser Languages test

    /// <summary>
    /// Get single value market id from browser accept-languages value; Adjust two-letter languages
    /// </summary>
    [TestMethod]
    public void Subdomain_WWW_CountrySiteCookie_CA_LanguageCookie_NONE_IpCountry_NONE_BrowserLanguages_VALID_Result_Redirect_CA_SINGLEBROWSERLANGUAGE()
    {
      string acceptLanguageHeaderValues = "en-CA";
      var container = SetSubdomainContext("http://www.mysite.com", string.Empty, null, acceptLanguageHeaderValues);
      CreateCountryCookie(1, "ca");

      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("ca", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-ca", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      acceptLanguageHeaderValues = "fr-CA";
      container = SetSubdomainContext("http://www.mysite.com", string.Empty, null, acceptLanguageHeaderValues);
      CreateCountryCookie(1, "ca");

      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("ca", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("fr-ca", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("fr", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      acceptLanguageHeaderValues = "fr";
      container = SetSubdomainContext("http://www.mysite.com", string.Empty, null, acceptLanguageHeaderValues);
      CreateCountryCookie(1, "ca");

      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("ca", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("fr-ca", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("fr", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);
    }

    /// <summary>
    /// Browser accept-languages value is not a valid market id so use the countrysite's default market
    /// </summary>
    [TestMethod]
    public void Subdomain_WWW_CountrySiteCookie_CA_LanguageCookie_NONE_IpCountry_NONE_BrowserLanguages_INVALID_Result_Redirect_CA_ENCA_DEFAULTMARKETID()
    {
      string acceptLanguageHeaderValues = "INVALID";
      var container = SetSubdomainContext("http://www.mysite.com", string.Empty, null, acceptLanguageHeaderValues);
      CreateCountryCookie(1, "ca");

      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("ca", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-ca", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);
    }

    /// <summary>
    /// Find valid market ID by browser accept-langauge priority
    /// </summary>
    [TestMethod]
    public void Subdomain_WWW_CountrySiteCookie_CA_LanguageCookie_NONE_IpCountry_NONE_BrowserLanguages_LIST_Result_Redirect_CA_BROWSERLANGUAGEPRIORITY()
    {
      string acceptLanguageHeaderValues = "fil,fr-CA;q=0.8,fr;q=0.6,en-US;q=0.4,en;q=0.2,en-CA;q=0.2,";
      var container = SetSubdomainContext("http://www.mysite.com", string.Empty, null, acceptLanguageHeaderValues);
      CreateCountryCookie(1, "ca");

      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("ca", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("fr-ca", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("fr", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      acceptLanguageHeaderValues = "fil,en;q=0.8,fr-CA;q=0.8,fr;q=0.6,en-US;q=0.4,en-CA;q=0.2,";
      container = SetSubdomainContext("http://www.mysite.com", string.Empty, null, acceptLanguageHeaderValues);
      CreateCountryCookie(1, "ca");

      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("ca", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-ca", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);
    }

    #endregion

    #region IP Country and Browser Languages test

    /// <summary>
    /// Get countrysite from IP address but no browser language match
    /// </summary>
    [TestMethod]
    public void Subdomain_WWW_CountrySiteCookie_NONE_LanguageCookie_NONE_IpCountry_US_BrowserLanguages_ENCA_Result_Redirect_NO_BROWSERLANGUAGEMISMATCH()
    {
      string acceptLanguageHeaderValues = "en-US";
      var container = SetSubdomainContext("http://www.mysite.com", "US", null, acceptLanguageHeaderValues);

      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("www", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-us", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);
    }

    /// <summary>
    /// Get countrysite from IP address but no valid browser accept-languages so use the default market id for the countrysite
    /// </summary>
    [TestMethod]
    public void Subdomain_WWW_CountrySiteCookie_NONE_LanguageCookie_NONE_IpCountry_US_BrowserLanguages_INVALID_Result_Redirect_NO_MATCHCURRENT()
    {
      string acceptLanguageHeaderValues = "INVALID";
      var container = SetSubdomainContext("http://www.mysite.com", "US", null, acceptLanguageHeaderValues);
    
      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("www", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-us", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);
    }

    /// <summary>
    /// Get countrysite from IP address and use single valid browser accept-language for market id; Adjust two-letter language
    /// </summary>
    [TestMethod]
    public void Subdomain_WWW_CountrySiteCookie_NONE_LanguageCookie_NONE_IpCountry_VALID_BrowserLanguages_VALID_Result_Redirect_IPCOUNTRY_SINGLEBROWSERLANGUAGE()
    {
      string acceptLanguageHeaderValues = "es-US";
      var container = SetSubdomainContext("http://www.mysite.com", "US", null, acceptLanguageHeaderValues);

      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("www", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("es-us", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("es", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      acceptLanguageHeaderValues = "es";
      container = SetSubdomainContext("http://www.mysite.com", "US", null, acceptLanguageHeaderValues);

      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("www", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("es-us", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("es", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      acceptLanguageHeaderValues = "fr-ca";
      container = SetSubdomainContext("http://www.mysite.com", "ca", null, acceptLanguageHeaderValues);

      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("ca", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("fr-ca", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("fr", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      acceptLanguageHeaderValues = "en";
      container = SetSubdomainContext("http://www.mysite.com", "ca", null, acceptLanguageHeaderValues);

      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("ca", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-ca", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);
    }

    /// <summary>
    /// Get countrysite from IP address and use browser accept-language priority to get market ID
    /// </summary>
    [TestMethod]
    public void Subdomain_WWW_CountrySiteCookie_NONE_LanguageCookie_NONE_IpCountry_VALID_BrowserLanguages_LIST_Result_Redirect_IPCOUNTRY_BROWSERLANGUAGEPRIORITY()
    {
      string acceptLanguageHeaderValues = "fil,fr-FR;q=0.9,es;q=0.7,fr-CA;q=0.6,es-ES;q=0.4,es-US;q=0.3,en-US;q=0.1,en-CA;q=0.1";

      var container = SetSubdomainContext("http://www.mysite.com", "US", null, acceptLanguageHeaderValues);
      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("www", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("es-us", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("es", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      container = SetSubdomainContext("http://www.mysite.com", "ca", null, acceptLanguageHeaderValues);
      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("ca", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("fr-ca", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("fr", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);
    }

    #endregion

    #region URL language tests

    /// <summary>
    /// Get countrysite from IP address and redirect to default market for the countrysite; Disregard prior language cookie
    /// </summary>
    [TestMethod]
    public void Cookie_UrlLanguage_INVALID_CountrySiteCookie_VALID_LanguageCookie_NONE_IpCountry_NONE_BrowserLanguages_NONE_Result_Redirect_NO_DEFAULTSMATCHCURRENT()
    {
      var container = SetCookieContext("http://whois.mysite.com/xx/default.aspx", string.Empty);
      CreateCountryCookie(1, "ca");
      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("ca", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-ca", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      container = SetCookieContext("http://whois.mysite.com/xx/default.aspx", string.Empty);
      CreateCountryCookie(1, "www");
      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("www", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-us", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      container = SetCookieContext("http://whois.mysite.com/xx/default.aspx", string.Empty);
      CreateCountryCookie(1, "br");
      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("br", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("pt-br", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("pt", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);
    }

    /// <summary>
    /// If there is a valid (for ILocalizationProvider.CountrySite) language in the URL, do not redirect because LanguageUrlRewriteProvider should handle it
    /// BUT RedirectResponse object should have the correct values
    /// </summary>
    [TestMethod]
    public void Subdomain_VALID_UrlLanguage_VALID_CountrySiteCookie_IGNORED_LanguageCookie_IGNORED_IpCountry_IGNORED_BrowserLanguages_NONE_Result_Redirect_NO()
    {
      var container = SetSubdomainContext("http://ca.mysite.com/en/default.aspx", "br");
      CreateCountryCookie(1, "br");
      CreateLanguagePreferenceCookie(1, "pt-br");
      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("ca", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-ca", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      container = SetSubdomainContext("http://ca.mysite.com/fr/default.aspx", "br");
      CreateCountryCookie(1, "br");
      CreateLanguagePreferenceCookie(1, "pt-br");
      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("ca", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("fr-ca", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("fr", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      container = SetSubdomainContext("http://www.mysite.com/en/default.aspx", "br");
      CreateCountryCookie(1, "br");
      CreateLanguagePreferenceCookie(1, "pt-br");
      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("www", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-us", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      container = SetSubdomainContext("http://www.mysite.com/es/default.aspx", "br");
      CreateCountryCookie(1, "br");
      CreateLanguagePreferenceCookie(1, "pt-br");
      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("www", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("es-us", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("es", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      container = SetSubdomainContext("http://fr.mysite.com/fr/default.aspx", "br");
      CreateCountryCookie(1, "br");
      CreateLanguagePreferenceCookie(1, "pt-br");
      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("fr", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("fr-fr", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("fr", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);
    }

    /// <summary>
    /// If there is a valid (for ILocalizationProvider.CountrySite) language in the URL, do not redirect because LanguageUrlRewriteProvider should handle it
    /// BUT RedirectResponse object should have the correct values
    /// </summary>
    [TestMethod]
    public void Subdomain_VALID_UrlLanguage_MARKETID_CountrySiteCookie_IGNORED_LanguageCookie_IGNORED_IpCountry_IGNORED_BrowserLanguages_NONE_Result_Redirect_NO_UNLESS_GLOBAL_AND_VALID_COUNTRYSITE_MARKETID()
    {
      var container = SetSubdomainContext("http://ca.mysite.com/qa-qa/default.aspx", "br");
      CreateCountryCookie(1, "br");
      CreateLanguagePreferenceCookie(1, "pt-br");
      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("ca", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-ca", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      container = SetSubdomainContext("http://ca.mysite.com/es-us/default.aspx", "br");
      CreateCountryCookie(1, "br");
      CreateLanguagePreferenceCookie(1, "pt-br");
      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("ca", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-ca", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      container = SetSubdomainContext("http://www.mysite.com/fr-fr/default.aspx", "br");
      CreateCountryCookie(1, "br");
      CreateLanguagePreferenceCookie(1, "pt-br");
      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("br", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("pt-br", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("pt", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      //  Internal market used but not an internal request
      container = SetSubdomainContext("http://www.mysite.com/qa-ps/default.aspx", "br");
      CreateCountryCookie(1, "br");
      CreateLanguagePreferenceCookie(1, "pt-br");
      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("br", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("pt-br", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("pt", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      //  Internal market used for an internal request
      container = SetSubdomainContext("http://www.mysite.com/qa-ps/default.aspx", "br");
      container.SetData(MockSiteContextSettings.IsRequestInternal, true);
      CreateCountryCookie(1, "br");
      CreateLanguagePreferenceCookie(1, "pt-br");
      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("www", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("qa-ps", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("qa-ps", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);      
    }

    /// <summary>
    /// If there is a valid (for ILocalizationProvider.CountrySite) language in the URL, do not redirect because LanguageUrlRewriteProvider should handle it
    /// BUT RedirectResponse object should have the correct values
    /// </summary>
    [TestMethod]
    public void Cookie_UrlLanguage_MARKETID_CountrySiteCookie_VALID_LanguageCookie_IGNORED_IpCountry_IGNORED_BrowserLanguages_NONE_Result_Redirect_NO_COUNTRYDEFAULTMARKETID()
    {
      var container = SetCookieContext("http://mya.mysite.com/qa-qa/default.aspx", "br");
      CreateCountryCookie(1, "ca");
      CreateLanguagePreferenceCookie(1, "pt-br");
      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("ca", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-ca", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      container = SetCookieContext("http://mya.mysite.com/es-us/default.aspx", "br");
      CreateCountryCookie(1, "ca");
      CreateLanguagePreferenceCookie(1, "pt-br");
      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("ca", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-ca", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      container = SetCookieContext("http://mya.mysite.com/fr-fr/default.aspx", "br");
      CreateCountryCookie(1, "xx"); //  Invalid country cookie
      CreateLanguagePreferenceCookie(1, "pt-br");
      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("br", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("pt-br", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("pt", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      //  Internal market used but not an internal request
      container = SetCookieContext("http://mya.mysite.com/qa-ps/default.aspx", "br");
      CreateCountryCookie(1, "www");  //  Valid country cookie
      CreateLanguagePreferenceCookie(1, "pt-br"); //  Non-matching language cookie
      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("www", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-us", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      //  Internal market used for an internal request - this will redirect because the url language == Market ID
      container = SetCookieContext("http://mya.mysite.com/qa-ps/default.aspx", "br");
      container.SetData(MockSiteContextSettings.IsRequestInternal, true);
      CreateCountryCookie(1, "www");
      CreateLanguagePreferenceCookie(1, "pt-br");
      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("www", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("qa-ps", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("qa-ps", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);
    }

    #endregion

    #region Internal-only tests

    /// <summary>
    /// Check if countrysite is internaly only
    /// </summary>
    [TestMethod]
    public void InternalOnlyCountrySiteTests()
    {
      //  Subdomain
      var container = SetSubdomainContext("http://rc.mysite.com", string.Empty);
      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);  //  This will actually redirect because HostValidation will adjust to www.
      Assert.AreEqual("www", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-us", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      container = SetSubdomainContext("http://rc.mysite.com", string.Empty);
      container.SetData(MockSiteContextSettings.IsRequestInternal, true);
      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("rc", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("rc-rc", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("rclang", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      container = SetSubdomainContext("http://www.mysite.com", string.Empty);
      CreateCountryCookie(1, "rc");
      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("www", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-us", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      container = SetSubdomainContext("http://www.mysite.com", string.Empty);
      container.SetData(MockSiteContextSettings.IsRequestInternal, true);
      CreateCountryCookie(1, "rc");
      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("rc", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("rc-rc", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("rclang", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      //  Cookie
      container = SetCookieContext("http://whois.mysite.com", string.Empty);
      CreateCountryCookie(1, "rc");
      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("www", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-us", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      container = SetCookieContext("http://whois.mysite.com", string.Empty);
      container.SetData(MockSiteContextSettings.IsRequestInternal, true);
      CreateCountryCookie(1, "rc");
      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("rc", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("rc-rc", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("rclang", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      //  IP Address
      container = SetSubdomainContext("http://www.mysite.com", "rc");
      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("www", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-us", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      container = SetSubdomainContext("http://www.mysite.com", "rc");
      container.SetData(MockSiteContextSettings.IsRequestInternal, true);
      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("rc", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("rc-rc", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("rclang", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);
    }

    /// <summary>
    /// Check if countrysite is public but market id is internal only
    /// </summary>
    [TestMethod]
    public void PublicCountrySiteInternalOnlyMarketTests()
    {
      //  Subdomain
      var container = SetSubdomainContext("http://00.mysite.com", string.Empty);
      CreateLanguagePreferenceCookie(1, "00-00");
      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("www", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-us", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      container = SetSubdomainContext("http://00.mysite.com", string.Empty);
      CreateLanguagePreferenceCookie(1, "00-00");
      container.SetData(MockSiteContextSettings.IsRequestInternal, true);
      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("00", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("00-00", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("00lang", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      container = SetSubdomainContext("http://www.mysite.com", string.Empty);
      CreateCountryCookie(1, "00");
      CreateLanguagePreferenceCookie(1, "00-00");
      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("www", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-us", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      container = SetSubdomainContext("http://www.mysite.com", string.Empty);
      container.SetData(MockSiteContextSettings.IsRequestInternal, true);
      CreateCountryCookie(1, "00");
      CreateLanguagePreferenceCookie(1, "00-00");
      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("00", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("00-00", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("00lang", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      //  Cookie
      container = SetCookieContext("http://whois.mysite.com", string.Empty);
      CreateCountryCookie(1, "00");
      CreateLanguagePreferenceCookie(1, "00-00");
      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("www", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-us", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      container = SetCookieContext("http://whois.mysite.com", string.Empty);
      container.SetData(MockSiteContextSettings.IsRequestInternal, true);
      CreateCountryCookie(1, "00");
      CreateLanguagePreferenceCookie(1, "00-00");
      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("00", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("00-00", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("00lang", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      //  IP Address
      container = SetSubdomainContext("http://www.mysite.com", "00");
      CreateLanguagePreferenceCookie(1, "00-00");
      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("www", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-us", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      container = SetSubdomainContext("http://www.mysite.com", "00");
      container.SetData(MockSiteContextSettings.IsRequestInternal, true);
      CreateLanguagePreferenceCookie(1, "00-00");
      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("00", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("00-00", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("00lang", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);
    }

    /// <summary>
    /// Check if countrysite and market are public but mapping is internal only
    /// </summary>
    [TestMethod]
    public void PublicCountrySitePublicMarketInternalOnlyMappingTests()
    {
      //  Subdomain
      var container = SetSubdomainContext("http://01.mysite.com", string.Empty);
      CreateLanguagePreferenceCookie(1, "01-01");
      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("www", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-us", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      container = SetSubdomainContext("http://01.mysite.com", string.Empty);
      CreateLanguagePreferenceCookie(1, "01-01");
      container.SetData(MockSiteContextSettings.IsRequestInternal, true);
      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("01", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("01-01", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("01lang", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      container = SetSubdomainContext("http://www.mysite.com", string.Empty);
      CreateCountryCookie(1, "01");
      CreateLanguagePreferenceCookie(1, "01-01");
      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("www", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-us", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      container = SetSubdomainContext("http://www.mysite.com", string.Empty);
      container.SetData(MockSiteContextSettings.IsRequestInternal, true);
      CreateCountryCookie(1, "01");
      CreateLanguagePreferenceCookie(1, "01-01");
      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("01", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("01-01", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("01lang", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      //  Cookie
      container = SetCookieContext("http://whois.mysite.com", string.Empty);
      CreateCountryCookie(1, "01");
      CreateLanguagePreferenceCookie(1, "01-01");
      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("www", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-us", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      container = SetCookieContext("http://whois.mysite.com", string.Empty);
      container.SetData(MockSiteContextSettings.IsRequestInternal, true);
      CreateCountryCookie(1, "01");
      CreateLanguagePreferenceCookie(1, "01-01");
      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("01", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("01-01", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("01lang", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      //  IP Address
      container = SetSubdomainContext("http://www.mysite.com", "01");
      CreateLanguagePreferenceCookie(1, "01-01");
      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("www", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-us", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);

      container = SetSubdomainContext("http://www.mysite.com", "01");
      container.SetData(MockSiteContextSettings.IsRequestInternal, true);
      CreateLanguagePreferenceCookie(1, "01-01");
      localization = container.Resolve<ILocalizationRedirectProvider>();
      result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("01", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("01-01", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("01lang", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);
    }

    #endregion    

    #region regionsite querystring tests

    [TestMethod]
    public void Cookie_WWW_RegionSite_INVALID_CountrySiteCookie_VALID_IpCountry_IGNORED_Result_Redirect_CSCOOKIE()
    {
      var container = SetCookieContext("http://www.mysite.com?regionsite=11", "uk");
      CreateCountryCookie(1, "au");

      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("au", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-au", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);
    }

    [TestMethod]
    public void Cookie_WWW_RegionSite_VALID_CountrySiteCookie_IGNORED_IpCountry_IGNORED_Result_Redirect_REGIONSITE()
    {
      var container = SetCookieContext("http://www.mysite.com?regionsite=ca", "uk");
      CreateCountryCookie(1, "au");

      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("ca", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-ca", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);
    }

    [TestMethod]
    public void Subdomain_WWW_RegionSite_INVALID_CountrySiteCookie_VALID_IpCountry_IGNORED_Result_Redirect_CSCOOKIE()
    {
      var container = SetSubdomainContext("http://www.mysite.com?regionsite=11", "uk");
      CreateCountryCookie(1, "au");

      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("au", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-au", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);
    }

    [TestMethod]
    public void Subdomain_VALID_RegionSite_INVALID_CountrySiteCookie_VALID_IpCountry_IGNORED_Result_Redirect_NO()
    {
      var container = SetSubdomainContext("http://ca.mysite.com?regionsite=11", "uk");
      CreateCountryCookie(1, "au");

      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsFalse(result.ShouldRedirect);
      Assert.AreEqual("ca", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-ca", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);
    }

    [TestMethod]
    public void Subdomain_WWW_RegionSite_IGNORED_CountrySiteCookie_VALID_IpCountry_IGNORED_Result_Redirect_COOKIE_DEFAULTMARKETID()
    {
      var container = SetSubdomainContext("http://www.mysite.com?regionsite=in", "uk");
      CreateCountryCookie(1, "au");

      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsTrue(result.ShouldRedirect);
      Assert.AreEqual("au", result.CountrySite.ToLowerInvariant());
      Assert.AreEqual("en-au", result.MarketId.ToLowerInvariant());
      Assert.AreEqual("en", result.ShortLanguage.ToLowerInvariant());
      TestLanguageCookie(container, result.MarketId);
    }

    #endregion
  }
}
