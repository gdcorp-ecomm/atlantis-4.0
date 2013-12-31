using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Reflection;
using System.Web;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.Providers.Localization.Tests.Mocks;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockHttpContext = Atlantis.Framework.Testing.MockHttpContext.MockHttpContext;
using MockHttpRequest = Atlantis.Framework.Testing.MockHttpContext.MockHttpRequest;

namespace Atlantis.Framework.Providers.Localization.Tests
{
  [TestClass]
  [DeploymentItem(afeConfig)]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  [DeploymentItem("Atlantis.Framework.Localization.Impl.dll")]
  public class LanguageUrlRewriteProviderTests
  {
    #region Setup

    public const string afeConfig = "atlantis.config";

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
      string[] parts = url.Split(new char[] {'?'}, 2, StringSplitOptions.RemoveEmptyEntries);
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

    private IProviderContainer SetCountrySubdomainContext(string url, string httpMethod = "GET", string virtualDirectoryName = "", bool autoSetHostHeader = true)
    {
      MockHttpRequest request = new MockCustomHttpRequest(url, httpMethod, virtualDirectoryName);
      MockHttpContext.SetFromWorkerRequest(request);
      if (autoSetHostHeader)
      {
        var hostHeader = new KeyValuePair<string, string>("Host", HttpContext.Current.Request.Url.Host);
        var headers = new [] { hostHeader };
        request.MockHeaderValues(headers);
      }

      string filename;
      string queryString;
      ParseUrl(url, out filename, out queryString);
      var mockRequest = new Mocks.Http.MockHttpRequest(new HttpRequest(filename, url, queryString), httpMethod, virtualDirectoryName, autoSetHostHeader);

      var context = new Mocks.Http.MockHttpContext(mockRequest, new Mocks.Http.MockHttpResponse());
      HttpContextFactory.SetHttpContext(context);

      IProviderContainer result = new MockProviderContainer();
      result.RegisterProvider<ISiteContext, MockSiteContext>();
      result.RegisterProvider<ILocalizationProvider, CountrySubdomainLocalizationProvider>();
      result.RegisterProvider<ILanguageUrlRewriteProvider, LanguageUrlRewriteProvider>();

      return result;
    }

    private IProviderContainer SetCountryCookieContext(string url, string countrySite, string httpMethod = "GET", string virtualDirectoryName = "", bool autoSetHostHeader = true)
    {
      MockHttpRequest request = new MockCustomHttpRequest(url, httpMethod, virtualDirectoryName);
      MockHttpContext.SetFromWorkerRequest(request);
      if (autoSetHostHeader)
      {
        var hostHeader = new KeyValuePair<string, string>("Host", HttpContext.Current.Request.Url.Host);
        var headers = new[] { hostHeader };
        request.MockHeaderValues(headers);
      }

      string filename;
      string queryString;
      ParseUrl(url, out filename, out queryString);
      var mockRequest = new Mocks.Http.MockHttpRequest(new HttpRequest(filename, url, queryString), httpMethod, virtualDirectoryName, autoSetHostHeader);
      var context = new Mocks.Http.MockHttpContext(mockRequest, new Mocks.Http.MockHttpResponse());
      HttpContextFactory.SetHttpContext(context);

      IProviderContainer result = new MockProviderContainer();
      result.RegisterProvider<ISiteContext, MockSiteContext>();
      result.RegisterProvider<ILocalizationProvider, CountryCookieLocalizationProvider>();
      result.RegisterProvider<ILanguageUrlRewriteProvider, LanguageUrlRewriteProvider>();
      CreateCountryCookie(1, countrySite);

      return result;
    }

    private void CreateCountryCookie(int privateLabelId, string value)
    {
      HttpCookie countryCookie = new HttpCookie("countrysite" + privateLabelId.ToString(), value);
      HttpContext.Current.Request.Cookies.Set(countryCookie);
    }

    private HttpCookie GetResponseCookieIfExists(int privateLabelId)
    {
      HttpCookie result = null;
      string cookieName = "countrysite" + privateLabelId.ToString();

      foreach (string key in HttpContext.Current.Response.Cookies.AllKeys)
      {
        if (key.Equals(cookieName, StringComparison.OrdinalIgnoreCase))
        {
          result = HttpContext.Current.Response.Cookies[cookieName];
          break;
        }
      }

      return result;
    }

    private MethodInfo GetPrivateMethod(string methodName)
    {
      Type requestType = typeof(LanguageUrlRewriteProvider);
      MethodInfo method = requestType.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.OptionalParamBinding);
      return method;
    }

    #endregion

    #region ProcessLanguageUrl tests

    [TestMethod]
    public void ProcessLanguageUrl_DefaultUrlLanguageAndHttpGet_ResultsInRedirectWithNoUrlLanguage()
    {
      Uri uri = new Uri("https://ca.godaddy.com/en/path2/file.aspx?key1=value1&key2=value2");
      IProviderContainer container = SetCountrySubdomainContext(uri.ToString());
      LanguageUrlRewriteProvider provider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();

      provider.ProcessLanguageUrl();

      Assert.AreEqual("https://ca.godaddy.com/path2/file.aspx?key1=value1&key2=value2", ((Mocks.Http.MockHttpResponse)HttpContextFactory.GetHttpContext().Response).RedirectedToUrl);    }

    [TestMethod]
    public void ProcessLanguageUrl_DefaultUrlLanguageAndHttpPOST_ResultsInUrlRewriteWithoutUrl_And_UpdatedFullLanguage()
    {
      Uri uri = new Uri("http://www.godaddy.com/en/path2/file.aspx?key1=value1&key2=value2");
      IProviderContainer container = SetCountrySubdomainContext(uri.ToString(), "POST");
      LanguageUrlRewriteProvider provider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();
      ILocalizationProvider localizationProvider = container.Resolve<ILocalizationProvider>();

      provider.ProcessLanguageUrl();

      Assert.IsTrue(string.IsNullOrWhiteSpace(((Mocks.Http.MockHttpResponse)HttpContextFactory.GetHttpContext().Response).RedirectedToUrl));
      Assert.AreEqual("http://www.godaddy.com/path2/file.aspx?key1=value1&key2=value2", HttpContext.Current.Request.Url.ToString());
      Assert.AreEqual("en", localizationProvider.RewrittenUrlLanguage);
      Assert.AreEqual("en-us", localizationProvider.FullLanguage.ToLowerInvariant());

      Assert.IsTrue(TestSavedRequestLanguageUrl(provider, localizationProvider, HttpContext.Current.Request.Url.ToString()));
    }

    [TestMethod]
    public void ProcessLanguageUrl_NonDefaultUrlLanguage_ResultsInUrlRewriteWithoutUrl_And_UpdatedFullLanguage()
    {
      Uri uri = new Uri("http://www.godaddy.com/es/path2/file.aspx?key1=value1&key2=value2");
      IProviderContainer container = SetCountrySubdomainContext(uri.ToString());
      LanguageUrlRewriteProvider urlRewriteProvider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();
      ILocalizationProvider localizationProvider = container.Resolve<ILocalizationProvider>();

      urlRewriteProvider.ProcessLanguageUrl();

      Assert.AreEqual("http://www.godaddy.com/path2/file.aspx?key1=value1&key2=value2", HttpContext.Current.Request.Url.ToString());
      Assert.AreEqual("es", localizationProvider.RewrittenUrlLanguage);
      Assert.AreEqual("es-us", localizationProvider.FullLanguage.ToLowerInvariant());

      Assert.IsTrue(TestSavedRequestLanguageUrl(urlRewriteProvider, localizationProvider, HttpContext.Current.Request.Url.ToString()));
    }

    [TestMethod]
    public void ProcessLanguageUrl_NonValidUrlLanguage_ResultsInUnchangedUrl_And_DefaultFullLanguage()
    {
      Uri uri = new Uri("http://ca.godaddy.com/es/path2/file.aspx?key1=value1&key2=value2");
      IProviderContainer container = SetCountrySubdomainContext(uri.ToString());
      LanguageUrlRewriteProvider urlRewriteProvider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();
      ILocalizationProvider localizationProvider = container.Resolve<ILocalizationProvider>();

      urlRewriteProvider.ProcessLanguageUrl();
      Assert.AreEqual("http://ca.godaddy.com/es/path2/file.aspx?key1=value1&key2=value2", HttpContext.Current.Request.Url.ToString());
      Assert.AreEqual("en-ca", localizationProvider.FullLanguage.ToLowerInvariant());

      Assert.IsFalse(TestSavedRequestLanguageUrl(urlRewriteProvider, localizationProvider, HttpContext.Current.Request.Url.ToString()));

      uri = new Uri("http://ca.godaddy.com/somepath/path2/file.aspx?key1=value1&key2=value2");
      container = SetCountrySubdomainContext(uri.ToString());
      urlRewriteProvider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();
      localizationProvider = container.Resolve<ILocalizationProvider>();

      urlRewriteProvider.ProcessLanguageUrl();
      Assert.AreEqual("http://ca.godaddy.com/somepath/path2/file.aspx?key1=value1&key2=value2", HttpContext.Current.Request.Url.ToString());
      Assert.AreEqual("en-ca", localizationProvider.FullLanguage.ToLowerInvariant());

      Assert.IsFalse(TestSavedRequestLanguageUrl(urlRewriteProvider, localizationProvider, HttpContext.Current.Request.Url.ToString()));
    }

    [TestMethod]
    public void ProcessLanguageUrl_NoUrlLanguage_ResultsInUnchangedUrl_And_DefaultFullLanguage()
    {
      Uri uri = new Uri("http://ca.godaddy.com/somepath/path2/file.aspx?key1=value1&key2=value2");
      IProviderContainer container = SetCountrySubdomainContext(uri.ToString());
      LanguageUrlRewriteProvider urlRewriteProvider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();
      ILocalizationProvider localizationProvider = container.Resolve<ILocalizationProvider>();

      urlRewriteProvider.ProcessLanguageUrl();
      Assert.AreEqual("http://ca.godaddy.com/somepath/path2/file.aspx?key1=value1&key2=value2", HttpContext.Current.Request.Url.ToString());
      Assert.AreEqual("en-ca", localizationProvider.FullLanguage.ToLowerInvariant());

      Assert.IsFalse(TestSavedRequestLanguageUrl(urlRewriteProvider, localizationProvider, HttpContext.Current.Request.Url.ToString()));
    }

    [TestMethod]
    public void ProcessLanguageUrl_GlobalUrlWithNoLanguageUrl_ResultsInUnchangedUrl()
    {
      Uri uri = new Uri("http://idp.debug.m.godaddy-com.ide/login.aspx?spkey=GDMDOTMYANET-G1MYAWEB&target=default.aspx");
      IProviderContainer container = SetCountrySubdomainContext(uri.ToString());
      LanguageUrlRewriteProvider urlRewriteProvider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();
      ILocalizationProvider localizationProvider = container.Resolve<ILocalizationProvider>();

      urlRewriteProvider.ProcessLanguageUrl();
      Assert.AreEqual("http://idp.debug.m.godaddy-com.ide/login.aspx?spkey=GDMDOTMYANET-G1MYAWEB&target=default.aspx", HttpContext.Current.Request.Url.ToString());
      Assert.AreEqual("en-us", localizationProvider.FullLanguage.ToLowerInvariant());

      Assert.IsFalse(TestSavedRequestLanguageUrl(urlRewriteProvider, localizationProvider, HttpContext.Current.Request.Url.ToString()));
    }

    [TestMethod]
    public void ProcessLanguageUrl_GlobalUrlWithDefaultLanguageUrl_ResultsInRedirectToUrlWithNoLanguageUrl()
    {
      Uri uri = new Uri("http://idp.debug.m.godaddy-com.ide/en/login.aspx?spkey=GDMDOTMYANET-G1MYAWEB&target=default.aspx2");
      IProviderContainer container = SetCountrySubdomainContext(uri.ToString());
      LanguageUrlRewriteProvider urlRewriteProvider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();
      ILocalizationProvider localizationProvider = container.Resolve<ILocalizationProvider>();

      urlRewriteProvider.ProcessLanguageUrl();

      Assert.AreEqual("http://idp.debug.m.godaddy-com.ide/login.aspx?spkey=GDMDOTMYANET-G1MYAWEB&target=default.aspx2", ((Mocks.Http.MockHttpResponse)HttpContextFactory.GetHttpContext().Response).RedirectedToUrl);

      Assert.IsFalse(TestSavedRequestLanguageUrl(urlRewriteProvider, localizationProvider, HttpContext.Current.Request.Url.ToString()));
    }

    [TestMethod]
    public void ProcessLanguageUrl_GlobalUrlWithNonDefaultLanguageUrl_ResultsChangedUrlAndUpatedFullLanguage()
    {
      Uri uri = new Uri("http://idp.debug.m.godaddy-com.ide/es/login.aspx?spkey=GDMDOTMYANET-G1MYAWEB&target=default.aspx");
      IProviderContainer container = SetCountrySubdomainContext(uri.ToString());
      LanguageUrlRewriteProvider urlRewriteProvider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();
      ILocalizationProvider localizationProvider = container.Resolve<ILocalizationProvider>();

      Assert.AreEqual("en-us", localizationProvider.FullLanguage.ToLowerInvariant());
      urlRewriteProvider.ProcessLanguageUrl();
      Assert.AreEqual("http://idp.debug.m.godaddy-com.ide/login.aspx?spkey=GDMDOTMYANET-G1MYAWEB&target=default.aspx", HttpContext.Current.Request.Url.ToString());
      Assert.AreEqual("es", localizationProvider.RewrittenUrlLanguage);
      Assert.AreEqual("es-us", localizationProvider.FullLanguage.ToLowerInvariant());

      Assert.IsTrue(TestSavedRequestLanguageUrl(urlRewriteProvider, localizationProvider, HttpContext.Current.Request.Url.ToString()));
    }

    [TestMethod]
    public void ProcessLanguageUrl_GlobalUrlWithNonDefaultLanguageAndNoFilename_ResultsChangedUrlWithDefaultAndUpatedFullLanguage()
    {
      Uri uri = new Uri("http://idp.debug.m.godaddy-com.ide/es/");
      IProviderContainer container = SetCountrySubdomainContext(uri.ToString());
      LanguageUrlRewriteProvider urlRewriteProvider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();
      ILocalizationProvider localizationProvider = container.Resolve<ILocalizationProvider>();

      Assert.AreEqual("en-us", localizationProvider.FullLanguage.ToLowerInvariant());
      urlRewriteProvider.ProcessLanguageUrl();
      Assert.AreEqual("http://idp.debug.m.godaddy-com.ide/default.aspx", HttpContext.Current.Request.Url.ToString());
      Assert.AreEqual("es", localizationProvider.RewrittenUrlLanguage);
      Assert.AreEqual("es-us", localizationProvider.FullLanguage.ToLowerInvariant());

      Assert.IsTrue(TestSavedRequestLanguageUrl(urlRewriteProvider, localizationProvider, HttpContext.Current.Request.Url.ToString()));

      uri = new Uri("http://idp.debug.m.godaddy-com.ide/es");
      container = SetCountrySubdomainContext(uri.ToString());
      urlRewriteProvider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();
      localizationProvider = container.Resolve<ILocalizationProvider>();

      Assert.AreEqual("en-us", localizationProvider.FullLanguage.ToLowerInvariant());
      urlRewriteProvider.ProcessLanguageUrl();
      Assert.AreEqual("http://idp.debug.m.godaddy-com.ide/default.aspx", HttpContext.Current.Request.Url.ToString());
      Assert.AreEqual("es", localizationProvider.RewrittenUrlLanguage);
      Assert.AreEqual("es-us", localizationProvider.FullLanguage.ToLowerInvariant());

      Assert.IsTrue(TestSavedRequestLanguageUrl(urlRewriteProvider, localizationProvider, HttpContext.Current.Request.Url.ToString()));
    }

    [TestMethod]
    public void ProcessLanguageUrl_GlobalUrlWithNonDefaultLanguageAndNoFilenameWithApplicationPath_ResultsChangedUrlWithDefaultAndUpatedFullLanguage()
    {
      Uri uri = new Uri("http://idp.debug.m.godaddy-com.ide/virtual/es/");
      IProviderContainer container = SetCountrySubdomainContext(uri.ToString(), virtualDirectoryName:"virtual");
      LanguageUrlRewriteProvider urlRewriteProvider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();
      ILocalizationProvider localizationProvider = container.Resolve<ILocalizationProvider>();

      Assert.AreEqual("en-us", localizationProvider.FullLanguage.ToLowerInvariant());
      urlRewriteProvider.ProcessLanguageUrl();
      Assert.AreEqual("http://idp.debug.m.godaddy-com.ide/virtual/default.aspx", HttpContext.Current.Request.Url.ToString());
      Assert.AreEqual("es", localizationProvider.RewrittenUrlLanguage);
      Assert.AreEqual("es-us", localizationProvider.FullLanguage.ToLowerInvariant());

      Assert.IsTrue(TestSavedRequestLanguageUrl(urlRewriteProvider, localizationProvider, HttpContext.Current.Request.Url.ToString()));

      uri = new Uri("http://idp.debug.m.godaddy-com.ide/virtual/es");
      container = SetCountrySubdomainContext(uri.ToString(), virtualDirectoryName: "virtual");
      urlRewriteProvider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();
      localizationProvider = container.Resolve<ILocalizationProvider>();

      Assert.AreEqual("en-us", localizationProvider.FullLanguage.ToLowerInvariant());
      urlRewriteProvider.ProcessLanguageUrl();
      Assert.AreEqual("http://idp.debug.m.godaddy-com.ide/virtual/default.aspx", HttpContext.Current.Request.Url.ToString());
      Assert.AreEqual("es", localizationProvider.RewrittenUrlLanguage);
      Assert.AreEqual("es-us", localizationProvider.FullLanguage.ToLowerInvariant());

      Assert.IsTrue(TestSavedRequestLanguageUrl(urlRewriteProvider, localizationProvider, HttpContext.Current.Request.Url.ToString()));
    }

    [TestMethod]
    public void ProcessLanguageUrl_GlobalUrlWithInvalidLanguageUrl_ResultsInUnchangedUrlAnd_DefaultFullLanguage()
    {
      Uri uri = new Uri("http://idp.debug.m.godaddy-com.ide/xx/login.aspx?spkey=GDMDOTMYANET-G1MYAWEB&target=default.aspx");
      IProviderContainer container = SetCountrySubdomainContext(uri.ToString());
      LanguageUrlRewriteProvider urlRewriteProvider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();
      ILocalizationProvider localizationProvider = container.Resolve<ILocalizationProvider>();

      Assert.AreEqual("en-us", localizationProvider.FullLanguage.ToLowerInvariant());
      urlRewriteProvider.ProcessLanguageUrl();
      Assert.AreEqual("http://idp.debug.m.godaddy-com.ide/xx/login.aspx?spkey=GDMDOTMYANET-G1MYAWEB&target=default.aspx", HttpContext.Current.Request.Url.ToString());
      Assert.AreEqual("en-us", localizationProvider.FullLanguage.ToLowerInvariant());

      Assert.IsFalse(TestSavedRequestLanguageUrl(urlRewriteProvider, localizationProvider, HttpContext.Current.Request.Url.ToString()));
    }

    [TestMethod]
    public void ProcessLanguageUrl_GlobalUrlWithCountryCookieAndNoLanguageUrl_ResultsInSameUrlAndDefaultLanguageForCountryFullLanguage()
    {
      Uri uri = new Uri("http://idp.debug.m.godaddy-com.ide/login.aspx?spkey=GDMDOTMYANET-G1MYAWEB&target=default.aspx");
      IProviderContainer container = SetCountryCookieContext(uri.ToString(), "ca");

      LanguageUrlRewriteProvider urlRewriteProvider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();
      ILocalizationProvider localizationProvider = container.Resolve<ILocalizationProvider>();

      Assert.AreEqual("en-ca", localizationProvider.FullLanguage.ToLowerInvariant());
      urlRewriteProvider.ProcessLanguageUrl();
      Assert.AreEqual("http://idp.debug.m.godaddy-com.ide/login.aspx?spkey=GDMDOTMYANET-G1MYAWEB&target=default.aspx", HttpContext.Current.Request.Url.ToString());
      Assert.AreEqual("en-ca", localizationProvider.FullLanguage.ToLowerInvariant());

      Assert.IsFalse(TestSavedRequestLanguageUrl(urlRewriteProvider, localizationProvider, HttpContext.Current.Request.Url.ToString()));
    }

    [TestMethod]
    public void ProcessLanguageUrl_GlobalUrlWithCountryCookieAndDefaultLanguageUrl_ResultsInRedirectToUrlWithNoLanguageUrl()
    {
      Uri uri = new Uri("http://idp.debug.m.godaddy-com.ide/en/login.aspx?spkey=GDMDOTMYANET-G1MYAWEB&target=default.aspx");
      IProviderContainer container = SetCountryCookieContext(uri.ToString(), "ca");

      LanguageUrlRewriteProvider urlRewriteProvider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();
      ILocalizationProvider localizationProvider = container.Resolve<ILocalizationProvider>();

      Assert.AreEqual("en-ca", localizationProvider.FullLanguage.ToLowerInvariant());
      urlRewriteProvider.ProcessLanguageUrl();

      Assert.AreEqual("http://idp.debug.m.godaddy-com.ide/login.aspx?spkey=GDMDOTMYANET-G1MYAWEB&target=default.aspx", ((Mocks.Http.MockHttpResponse)HttpContextFactory.GetHttpContext().Response).RedirectedToUrl);

      Assert.IsFalse(TestSavedRequestLanguageUrl(urlRewriteProvider, localizationProvider, HttpContext.Current.Request.Url.ToString()));
    }

    [TestMethod]
    public void ProcessLanguageUrl_GlobalUrlWithCountryCookieAndNonDefaultLanguageUrl_ResultsInChangedUrlAndDefaultLanguageForCountryFullLanguage()
    {
      Uri uri = new Uri("http://idp.debug.m.godaddy-com.ide/es/login.aspx?spkey=GDMDOTMYANET-G1MYAWEB&target=default.aspx");
      IProviderContainer container = SetCountryCookieContext(uri.ToString(), "www");

      LanguageUrlRewriteProvider urlRewriteProvider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();
      ILocalizationProvider localizationProvider = container.Resolve<ILocalizationProvider>();
      Assert.AreEqual("en-us", localizationProvider.FullLanguage.ToLowerInvariant());

      urlRewriteProvider.ProcessLanguageUrl();
      Assert.AreEqual("http://idp.debug.m.godaddy-com.ide/login.aspx?spkey=GDMDOTMYANET-G1MYAWEB&target=default.aspx", HttpContext.Current.Request.Url.ToString());
      Assert.AreEqual("es", localizationProvider.RewrittenUrlLanguage);
      Assert.AreEqual("es-us", localizationProvider.FullLanguage.ToLowerInvariant());

      Assert.IsTrue(TestSavedRequestLanguageUrl(urlRewriteProvider, localizationProvider, HttpContext.Current.Request.Url.ToString()));
    }

    [TestMethod]
    public void ProcessLanguageUrl_ActiveProxy_ResultsInNoUrlProcessing()
    {
      Uri uri = new Uri("http://es.godaddy-com.ide/es/login.aspx?spkey=GDMDOTMYANET-G1MYAWEB&target=default.aspx");
      IProviderContainer container = SetCountrySubdomainContext(uri.ToString());
      container.RegisterProvider<IProxyContext, TransperfectTestWebProxy>();

      ILocalizationProvider localization = container.Resolve<ILocalizationProvider>();
      Assert.IsFalse(localization.IsGlobalSite());
      Assert.AreEqual("ES", localization.ShortLanguage.ToUpperInvariant());
      Assert.AreEqual("es-us", localization.FullLanguage.ToLowerInvariant());
      Assert.AreEqual("es-US", localization.MarketInfo.Id);
      Assert.AreEqual(new CultureInfo("es-US"), localization.CurrentCultureInfo);

      LanguageUrlRewriteProvider urlRewriteProvider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();
      urlRewriteProvider.ProcessLanguageUrl();
      Assert.AreEqual("http://es.godaddy-com.ide/es/login.aspx?spkey=GDMDOTMYANET-G1MYAWEB&target=default.aspx", HttpContext.Current.Request.Url.ToString());
      Assert.IsFalse(localization.IsGlobalSite());
      Assert.AreEqual("ES", localization.ShortLanguage.ToUpperInvariant());
      Assert.AreEqual("es-us", localization.FullLanguage.ToLowerInvariant());
      Assert.AreEqual("es-US", localization.MarketInfo.Id);
      Assert.AreEqual(new CultureInfo("es-US"), localization.CurrentCultureInfo);

      Assert.IsFalse(TestSavedRequestLanguageUrl(urlRewriteProvider, localization, HttpContext.Current.Request.Url.ToString()));
    }

    [TestMethod]
    public void ProcessLanguageUrl_GlobalUrlWithCountryCookieInvalidLanguageButValidMarketInUrl_ResultsInRedirectToUrlWithNoMarketId()
    {
      Uri uri = new Uri("http://idp.debug.m.godaddy-com.ide/qa-ps/login.aspx?spkey=GDMDOTMYANET-G1MYAWEB&target=default.aspx");
      IProviderContainer container = SetCountryCookieContext(uri.ToString(), "in");

      LanguageUrlRewriteProvider urlRewriteProvider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();
      ILocalizationProvider localizationProvider = container.Resolve<ILocalizationProvider>();

      Assert.AreEqual("en-in", localizationProvider.FullLanguage.ToLowerInvariant());
      urlRewriteProvider.ProcessLanguageUrl();

      Assert.AreEqual("http://idp.debug.m.godaddy-com.ide/login.aspx?spkey=GDMDOTMYANET-G1MYAWEB&target=default.aspx", ((Mocks.Http.MockHttpResponse)HttpContextFactory.GetHttpContext().Response).RedirectedToUrl);

      Assert.IsFalse(TestSavedRequestLanguageUrl(urlRewriteProvider, localizationProvider, HttpContext.Current.Request.Url.ToString()));
    }

    [TestMethod]
    public void ProcessLanguageUrl_GlobalUrlWithCountrySubdomainInvalidLanguageButValidMarketInUrl_ResultsInRedirectToUrlWithNoMarketId()
    {
      Uri uri = new Uri("http://au.debug.godaddy-com.ide/qa-pz?spkey=GDMDOTMYANET-G1MYAWEB&target=default.aspx");
      IProviderContainer container = SetCountrySubdomainContext(uri.ToString());

      LanguageUrlRewriteProvider urlRewriteProvider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();
      ILocalizationProvider localizationProvider = container.Resolve<ILocalizationProvider>();

      Assert.AreEqual("en-au", localizationProvider.FullLanguage.ToLowerInvariant());
      urlRewriteProvider.ProcessLanguageUrl();

      Assert.AreEqual("http://au.debug.godaddy-com.ide/?spkey=GDMDOTMYANET-G1MYAWEB&target=default.aspx", ((Mocks.Http.MockHttpResponse)HttpContextFactory.GetHttpContext().Response).RedirectedToUrl);

      Assert.IsFalse(TestSavedRequestLanguageUrl(urlRewriteProvider, localizationProvider, HttpContext.Current.Request.Url.ToString()));
    }
    #endregion

    #region GetLanguageFreeUrlPath Tests

    [TestMethod]
    public void GetLanguageFreeUrlPath_SpecifiedLanguageCode_RemovedFromUrl()
    {
      Uri uri = new Uri("https://whois.godaddy.com/en/path2/file.aspx?key1=value1&key2=value2");
      IProviderContainer container = SetCountrySubdomainContext(uri.ToString());
      LanguageUrlRewriteProvider provider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();

      MethodInfo method = GetPrivateMethod("GetLanguageFreeUrlPath");
      string result = (string) method.Invoke(provider, new object[] { "en" });
      Assert.AreEqual("/path2/file.aspx", result);

      uri = new Uri("https://whois.godaddy.com/va/en/path2/file.aspx?key1=value1&key2=value2");
      container = SetCountrySubdomainContext(uri.ToString(), virtualDirectoryName:"va");
      provider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();
      result = (string)method.Invoke(provider, new object[] { "en" });
      Assert.AreEqual("/va/path2/file.aspx", result);

      uri = new Uri("https://whois.godaddy.com/va/en/path2/file.aspx?key1=value1&key2=value2");
      container = SetCountrySubdomainContext(uri.ToString());
      provider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();
      result = (string)method.Invoke(provider, new object[] { "en" });
      Assert.AreEqual("/va/path2/file.aspx", result);
    }

    [TestMethod]
    public void GetLanguageFreeUrlPath_NoFilename_RemovedFromUrl()
    {
      Uri uri = new Uri("https://whois.godaddy.com/en/?key1=value1&key2=value2");
      IProviderContainer container = SetCountrySubdomainContext(uri.ToString());
      LanguageUrlRewriteProvider provider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();

      MethodInfo method = GetPrivateMethod("GetLanguageFreeUrlPath");
      string result = (string)method.Invoke(provider, new object[] { "en" });
      Assert.AreEqual("/", result);

      uri = new Uri("https://whois.godaddy.com/en?key1=value1&key2=value2");
      container = SetCountrySubdomainContext(uri.ToString(), virtualDirectoryName: "va");
      provider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();
      result = (string)method.Invoke(provider, new object[] { "en" });
      Assert.AreEqual("", result);

      uri = new Uri("https://whois.godaddy.com/en/");
      container = SetCountrySubdomainContext(uri.ToString());
      provider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();
      result = (string)method.Invoke(provider, new object[] { "en" });
      Assert.AreEqual("/", result);

      uri = new Uri("https://whois.godaddy.com/en");
      container = SetCountrySubdomainContext(uri.ToString());
      provider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();
      result = (string)method.Invoke(provider, new object[] { "en" });
      Assert.AreEqual("", result);

      uri = new Uri("https://whois.godaddy.com/qa-qa?key1=value1&key2=value2");
      container = SetCountrySubdomainContext(uri.ToString(), virtualDirectoryName: "va");
      provider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();
      result = (string)method.Invoke(provider, new object[] { "qa-qa" });
      Assert.AreEqual("", result);

      uri = new Uri("https://whois.godaddy.com/qa-qa/");
      container = SetCountrySubdomainContext(uri.ToString());
      provider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();
      result = (string)method.Invoke(provider, new object[] { "qa-qa" });
      Assert.AreEqual("/", result);

      uri = new Uri("https://whois.godaddy.com/qa-qa");
      container = SetCountrySubdomainContext(uri.ToString());
      provider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();
      result = (string)method.Invoke(provider, new object[] { "qa-qa" });
      Assert.AreEqual("", result);
    }

    [TestMethod]
    public void GetLanguageFreeUrlPath_MultipleSpecifiedLanguageCode_RemovedOnlyOnceFromUrl()
    {
      Uri uri = new Uri("https://whois.godaddy.com/en/path2/en/file.aspx?key1=value1&key2=value2");
      IProviderContainer container = SetCountrySubdomainContext(uri.ToString());
      LanguageUrlRewriteProvider provider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();

      MethodInfo method = GetPrivateMethod("GetLanguageFreeUrlPath");
      string result = (string)method.Invoke(provider, new object[] { "en" });
      Assert.AreEqual("/path2/en/file.aspx", result);

      uri = new Uri("https://whois.godaddy.com/va/en/path2/en/file.aspx?key1=value1&key2=value2");
      container = SetCountrySubdomainContext(uri.ToString(), virtualDirectoryName: "va");
      provider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();
      result = (string)method.Invoke(provider, new object[] { "en" });
      Assert.AreEqual("/va/path2/en/file.aspx", result);

      uri = new Uri("https://whois.godaddy.com/va/en/path2/en/file.aspx?key1=value1&key2=value2");
      container = SetCountrySubdomainContext(uri.ToString());
      provider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();
      result = (string)method.Invoke(provider, new object[] { "en" });
      Assert.AreEqual("/va/path2/en/file.aspx", result);
    }

    [TestMethod]
    public void GetLanguageFreeUrlPath_InvalidLanguageCode_NotRemovedFromUrl()
    {
      Uri uri = new Uri("https://whois.godaddy.com/hi/path2/file.aspx?key1=value1&key2=value2");
      IProviderContainer container = SetCountrySubdomainContext(uri.ToString());
      LanguageUrlRewriteProvider provider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();

      MethodInfo method = GetPrivateMethod("GetLanguageFreeUrlPath");
      string result = (string)method.Invoke(provider, new object[] { "  " });
      Assert.AreEqual("/hi/path2/file.aspx", result);

      uri = new Uri("https://whois.godaddy.com/va/hi/path2/file.aspx?key1=value1&key2=value2");
      container = SetCountrySubdomainContext(uri.ToString(), virtualDirectoryName:"va");
      provider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();

      result = (string)method.Invoke(provider, new object[] { "  " });
      Assert.AreEqual("/va/hi/path2/file.aspx", result);
    }

    #endregion

    #region GetUrlLanguage Tests

    [TestMethod]
    public void GetUrlLanguage_UrlWithPath_ReturnsFirstSegmentIfValidLanguageForCountrySite()
    {
      Uri uri = new Uri("http://idp.debug.m.godaddy-com.ide/en/login.aspx?spkey=GDMDOTMYANET-G1MYAWEB&target=default.aspx");
      IProviderContainer container = SetCountrySubdomainContext(uri.ToString());
      LanguageUrlRewriteProvider provider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();

      MethodInfo method = GetPrivateMethod("GetUrlLanguage");
      string validMarketId = null;
      string result = (string)method.Invoke(provider, new object[] { validMarketId });
      Assert.AreEqual("en", result);

      uri = new Uri("http://idp.debug.m.godaddy-com.ide/va/en/login.aspx?spkey=GDMDOTMYANET-G1MYAWEB&target=default.aspx");
      container = SetCountrySubdomainContext(uri.ToString(), virtualDirectoryName:"va");
      provider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();
      result = (string)method.Invoke(provider, new object[] { validMarketId });
      Assert.AreEqual("en", result);

      uri = new Uri("http://idp.debug.m.godaddy-com.ide/va/en/login.aspx?spkey=GDMDOTMYANET-G1MYAWEB&target=default.aspx");
      container = SetCountrySubdomainContext(uri.ToString());
      provider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();
      result = (string)method.Invoke(provider, new object[] { validMarketId });
      Assert.AreEqual(string.Empty, result);

      uri = new Uri("https://whois.godaddy.com/fr");
      container = SetCountrySubdomainContext(uri.ToString());
      provider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();
      result = (string)method.Invoke(provider, new object[] { validMarketId });
      Assert.AreEqual(string.Empty, result);

      uri = new Uri("https://whois.godaddy.com/es/");
      container = SetCountrySubdomainContext(uri.ToString());
      provider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();
      result = (string)method.Invoke(provider, new object[] { validMarketId });
      Assert.AreEqual("es", result);
    }

    [TestMethod]
    public void GetUrlLanguage_UrlWithNoPath_ReturnsEmptyString()
    {
      Uri uri = new Uri("https://whois.godaddy.com");
      IProviderContainer container = SetCountrySubdomainContext(uri.ToString());
      LanguageUrlRewriteProvider provider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();

      MethodInfo method = GetPrivateMethod("GetUrlLanguage");
      string validMarketId = null;
      string result = (string)method.Invoke(provider, new object[] { validMarketId });
      Assert.AreEqual(string.Empty, result);

      uri = new Uri("https://whois.godaddy.com/va");
      container = SetCountrySubdomainContext(uri.ToString(), virtualDirectoryName:"va");
      provider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();

      result = (string)method.Invoke(provider, new object[] { validMarketId });
      Assert.AreEqual(string.Empty, result);      
    }

    [TestMethod]
    public void GetUrlLanguage_UrlWithNoPage_ReturnsLanguage()
    {

      Uri uri = new Uri("https://whois.godaddy.com/qa-qa");
      IProviderContainer container = SetCountrySubdomainContext(uri.ToString());
      container.SetData(MockSiteContextSettings.IsRequestInternal, true);
      LanguageUrlRewriteProvider provider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();

      MethodInfo method = GetPrivateMethod("GetUrlLanguage");
      string validMarketId = null;
      string result = (string)method.Invoke(provider, new object[] {validMarketId});
      Assert.AreEqual("qa-qa", result);

      uri = new Uri("https://whois.godaddy.com/qa-qa/");
      container = SetCountrySubdomainContext(uri.ToString());
      container.SetData(MockSiteContextSettings.IsRequestInternal, true);
      provider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();

      result = (string)method.Invoke(provider, new object[] { validMarketId });
      Assert.AreEqual("qa-qa", result);

      uri = new Uri("https://whois.godaddy.com/qa-qa/path");
      container = SetCountrySubdomainContext(uri.ToString());
      container.SetData(MockSiteContextSettings.IsRequestInternal, true);
      provider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();

      result = (string)method.Invoke(provider, new object[] { validMarketId });
      Assert.AreEqual("qa-qa", result);

      uri = new Uri("https://whois.godaddy.com/qa-qa/path/");
      container = SetCountrySubdomainContext(uri.ToString());
      container.SetData(MockSiteContextSettings.IsRequestInternal, true);
      provider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();

      result = (string)method.Invoke(provider, new object[] { validMarketId });
      Assert.AreEqual("qa-qa", result);
    }

    #endregion

    #region IsValidLanguageCode tests

    [TestMethod]
    public void IsValidLanguageCode_InvalidCode_ReturnsFalse()
    {
      Uri uri = new Uri("https://whois.godaddy.com");
      IProviderContainer container = SetCountrySubdomainContext(uri.ToString());
      ILanguageUrlRewriteProvider provider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();

      Assert.IsFalse(provider.IsValidLanguageCodeForRequest(""));
      Assert.IsFalse(provider.IsValidLanguageCodeForRequest("  "));

      Assert.IsFalse(provider.IsValidLanguageCodeForRequest("11"));
      Assert.IsFalse(provider.IsValidLanguageCodeForRequest("22-33"));

      Assert.IsFalse(provider.IsValidLanguageCodeForRequest("esx"));
      Assert.IsFalse(provider.IsValidLanguageCodeForRequest("es-usx"));
      Assert.IsFalse(provider.IsValidLanguageCodeForRequest("-us"));
      Assert.IsFalse(provider.IsValidLanguageCodeForRequest("22-usx"));
      Assert.IsFalse(provider.IsValidLanguageCodeForRequest("esx-us"));

      Assert.IsFalse(provider.IsValidLanguageCodeForRequest("12(*&)&)*"));
      Assert.IsFalse(provider.IsValidLanguageCodeForRequest("12(*&)&)*=us"));
    }

    [TestMethod]
    public void IsValidLanguageCode_ValidCode_ReturnsTrue()
    {
      Uri uri = new Uri("https://whois.godaddy.com");
      IProviderContainer container = SetCountrySubdomainContext(uri.ToString());
      ILanguageUrlRewriteProvider provider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();
      
      Assert.IsTrue(provider.IsValidLanguageCodeForRequest("es"));
      Assert.IsTrue(provider.IsValidLanguageCodeForRequest("Es"));
      Assert.IsTrue(provider.IsValidLanguageCodeForRequest("en"));
      Assert.IsFalse(provider.IsValidLanguageCodeForRequest("qa-qa"));
      Assert.IsFalse(provider.IsValidLanguageCodeForRequest("qa-ps"));

      container.SetData(MockSiteContextSettings.IsRequestInternal, true);
      Assert.IsTrue(provider.IsValidLanguageCodeForRequest("qa-qa"));
      Assert.IsTrue(provider.IsValidLanguageCodeForRequest("qa-ps"));
    }

    [TestMethod]
    public void IsValidLanguageCode_InvalidCodeForCountry_ReturnsFalse()
    {
      Uri uri = new Uri("https://whois.godaddy.com");
      IProviderContainer container = SetCountrySubdomainContext(uri.ToString());
      ILanguageUrlRewriteProvider provider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();

      Assert.IsFalse(provider.IsValidLanguageCodeForRequest("fr"));
      Assert.IsFalse(provider.IsValidLanguageCodeForRequest("hi-us"));

      uri = new Uri("https://ca.godaddy.com");
      container = SetCountrySubdomainContext(uri.ToString());
      provider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();

      Assert.IsFalse(provider.IsValidLanguageCodeForRequest("sw"));
      Assert.IsFalse(provider.IsValidLanguageCodeForRequest("sw-ca"));
    }

    #endregion

    #region SavedRequestLanguageUrl tests

    private bool TestSavedRequestLanguageUrl(ILanguageUrlRewriteProvider provider, ILocalizationProvider localizationProvider, string url)
    {
      bool result = false;
      string key = "REWRITTENURLLANGUAGEINFO";
      
      //  Make sure it was saved
      if (string.IsNullOrEmpty(localizationProvider.RewrittenUrlLanguage))
      {
        Assert.IsTrue(string.IsNullOrEmpty(HttpContextFactory.GetHttpContext().Request.Headers[key]));
      }
      else
      {
        result = true;
        string oldLanguage = localizationProvider.RewrittenUrlLanguage.ToLowerInvariant();
        string oldMarketId = localizationProvider.MarketInfo.Id.ToLowerInvariant();
        string oldValue = (localizationProvider.RewrittenUrlLanguage + "|" + localizationProvider.MarketInfo.Id).ToLowerInvariant();

        //  Make sure rewritten value was saved for the request
        Assert.AreEqual(oldValue, HttpContextFactory.GetHttpContext().Request.Headers[key].ToLowerInvariant());

        //  Reprocess to simulate child request going through pipeline again
        provider.ProcessLanguageUrl();

        //  Not redirected
        Assert.IsTrue(string.IsNullOrWhiteSpace(((Mocks.Http.MockHttpResponse)HttpContextFactory.GetHttpContext().Response).RedirectedToUrl));

        //  No further rewriting of the url
        Assert.AreEqual(url, HttpContext.Current.Request.Url.ToString());

        //  Old values are retained
        Assert.AreEqual(oldLanguage, localizationProvider.RewrittenUrlLanguage.ToLowerInvariant());
        Assert.AreEqual(oldMarketId, localizationProvider.FullLanguage.ToLowerInvariant());
        Assert.AreEqual(oldValue, HttpContextFactory.GetHttpContext().Request.Headers[key].ToLowerInvariant());  
      }
      return result;
    }

    [TestMethod]
    public void SavedRequestLanguageUrl_UrlWithPageButNoLanguage_ReturnsSavedValue()
    {
      string key = "REWRITTENURLLANGUAGEINFO";

      Uri uri = new Uri("http://idp.debug.m.godaddy-com.ide/login.aspx?spkey=GDMDOTMYANET-G1MYAWEB&target=default.aspx");
      IProviderContainer container = SetCountrySubdomainContext(uri.ToString());
      LanguageUrlRewriteProvider provider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();

      HttpContextFactory.GetHttpContext().Request.Headers[key] = "en|en-us";
      MethodInfo method = GetPrivateMethod("GetUrlLanguage");
      string validMarketId = null;
      string result = (string)method.Invoke(provider, new object[] { validMarketId });
      Assert.AreEqual("en", result);

      uri = new Uri("http://idp.debug.m.godaddy-com.ide/va/login.aspx?spkey=GDMDOTMYANET-G1MYAWEB&target=default.aspx");
      container = SetCountrySubdomainContext(uri.ToString(), virtualDirectoryName: "va");
      HttpContextFactory.GetHttpContext().Request.Headers[key] = "en|en-us";
      provider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();
      result = (string)method.Invoke(provider, new object[] { validMarketId });
      Assert.AreEqual("en", result);

      uri = new Uri("http://idp.debug.m.godaddy-com.ide/va/login.aspx?spkey=GDMDOTMYANET-G1MYAWEB&target=default.aspx");
      container = SetCountrySubdomainContext(uri.ToString());
      HttpContextFactory.GetHttpContext().Request.Headers.Remove(key);
      provider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();
      result = (string)method.Invoke(provider, new object[] { validMarketId });
      Assert.AreEqual(string.Empty, result);

      uri = new Uri("https://whois.godaddy.com/fr");
      container = SetCountrySubdomainContext(uri.ToString());
      provider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();
      result = (string)method.Invoke(provider, new object[] { validMarketId });
      Assert.AreEqual(string.Empty, result);

      uri = new Uri("https://whois.godaddy.com/");
      container = SetCountrySubdomainContext(uri.ToString());
      HttpContextFactory.GetHttpContext().Request.Headers[key] = "es|es-us";
      provider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();
      result = (string)method.Invoke(provider, new object[] { validMarketId });
      Assert.AreEqual("es", result);
    }

    [TestMethod]
    public void SavedRequestLanguageUrl_UrlWithNoLanguage_ReturnsSavedValue()
    {
      string key = "REWRITTENURLLANGUAGEINFO";

      Uri uri = new Uri("https://whois.godaddy.com");
      IProviderContainer container = SetCountrySubdomainContext(uri.ToString());
      HttpContextFactory.GetHttpContext().Request.Headers[key] = "es|es-us";
      LanguageUrlRewriteProvider provider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();

      MethodInfo method = GetPrivateMethod("GetUrlLanguage");
      string validMarketId = null;
      string result = (string)method.Invoke(provider, new object[] { validMarketId });
      Assert.AreEqual("es", result);

      uri = new Uri("https://whois.godaddy.com/va");
      container = SetCountrySubdomainContext(uri.ToString(), virtualDirectoryName: "va");
      HttpContextFactory.GetHttpContext().Request.Headers[key] = "es|es-us";
      provider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();

      result = (string)method.Invoke(provider, new object[] { validMarketId });
      Assert.AreEqual("es", result);

      uri = new Uri("https://whois.godaddy.com/va/es");
      container = SetCountrySubdomainContext(uri.ToString(), virtualDirectoryName: "va");
      provider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();

      result = (string)method.Invoke(provider, new object[] { validMarketId });
      Assert.AreEqual("es", result);  
    }

    [TestMethod]
    public void SavedRequestLanguageUrl_UrlWithLanguage_ReturnsUrlLanguage()
    {
      string key = "REWRITTENURLLANGUAGEINFO";

      Uri uri = new Uri("https://whois.godaddy.com/qa-qa");
      IProviderContainer container = SetCountrySubdomainContext(uri.ToString());
      HttpContextFactory.GetHttpContext().Request.Headers[key] = "es|es-us";
      container.SetData(MockSiteContextSettings.IsRequestInternal, true);
      LanguageUrlRewriteProvider provider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();

      MethodInfo method = GetPrivateMethod("GetUrlLanguage");
      string validMarketId = null;
      string result = (string)method.Invoke(provider, new object[] { validMarketId });
      Assert.AreEqual("qa-qa", result);

      uri = new Uri("https://whois.godaddy.com/qa-qa/");
      container = SetCountrySubdomainContext(uri.ToString());
      HttpContextFactory.GetHttpContext().Request.Headers[key] = "es|es-us";
      container.SetData(MockSiteContextSettings.IsRequestInternal, true);
      provider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();

      result = (string)method.Invoke(provider, new object[] { validMarketId });
      Assert.AreEqual("qa-qa", result);

      uri = new Uri("https://whois.godaddy.com/qa-qa/path");
      container = SetCountrySubdomainContext(uri.ToString());
      HttpContextFactory.GetHttpContext().Request.Headers[key] = "es|es-us";
      container.SetData(MockSiteContextSettings.IsRequestInternal, true);
      provider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();

      result = (string)method.Invoke(provider, new object[] { validMarketId });
      Assert.AreEqual("qa-qa", result);

      uri = new Uri("https://whois.godaddy.com/qa-qa/path/");
      container = SetCountrySubdomainContext(uri.ToString());
      HttpContextFactory.GetHttpContext().Request.Headers[key] = "es|es-us";
      container.SetData(MockSiteContextSettings.IsRequestInternal, true);
      provider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();

      result = (string)method.Invoke(provider, new object[] { validMarketId });
      Assert.AreEqual("qa-qa", result);
    }
    #endregion
  }
}
