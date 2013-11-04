using System;
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

    private IProviderContainer SetCountrySubdomainContext(string url, string httpMethod = "GET", string virtualDirectoryName = "")
    {
      MockHttpRequest request = new MockCustomHttpRequest(url, httpMethod, virtualDirectoryName);
      MockHttpContext.SetFromWorkerRequest(request);

      string filename;
      string queryString;
      ParseUrl(url, out filename, out queryString);
      var mockRequest = new Mocks.Http.MockHttpRequest(new HttpRequest(filename, url, queryString), httpMethod, virtualDirectoryName);
      var context = new Mocks.Http.MockHttpContext(mockRequest, new Mocks.Http.MockHttpResponse());
      HttpContextFactory.SetHttpContext(context);

      IProviderContainer result = new MockProviderContainer();
      result.RegisterProvider<ISiteContext, MockSiteContext>();
      result.RegisterProvider<ILocalizationProvider, CountrySubdomainLocalizationProvider>();
      result.RegisterProvider<ILanguageUrlRewriteProvider, LanguageUrlRewriteProvider>();

      return result;
    }

    private IProviderContainer SetCountryCookieContext(string url, string countrySite, string httpMethod = "GET", string virtualDirectoryName = "")
    {
      MockHttpRequest request = new MockCustomHttpRequest(url, httpMethod, virtualDirectoryName);
      MockHttpContext.SetFromWorkerRequest(request);

      string filename;
      string queryString;
      ParseUrl(url, out filename, out queryString);
      var mockRequest = new Mocks.Http.MockHttpRequest(new HttpRequest(filename, url, queryString), httpMethod, virtualDirectoryName);
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

      Assert.AreEqual("https://ca.godaddy.com/path2/file.aspx?key1=value1&key2=value2", ((Mocks.Http.MockHttpResponse)HttpContextFactory.GetHttpContext().Response).RedirectedToUrl);
    }

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

      uri = new Uri("http://ca.godaddy.com/somepath/path2/file.aspx?key1=value1&key2=value2");
      container = SetCountrySubdomainContext(uri.ToString());
      urlRewriteProvider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();
      localizationProvider = container.Resolve<ILocalizationProvider>();

      urlRewriteProvider.ProcessLanguageUrl();
      Assert.AreEqual("http://ca.godaddy.com/somepath/path2/file.aspx?key1=value1&key2=value2", HttpContext.Current.Request.Url.ToString());
      Assert.AreEqual("en-ca", localizationProvider.FullLanguage.ToLowerInvariant());
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

      uri = new Uri("http://idp.debug.m.godaddy-com.ide/es");
      container = SetCountrySubdomainContext(uri.ToString());
      urlRewriteProvider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();
      localizationProvider = container.Resolve<ILocalizationProvider>();

      Assert.AreEqual("en-us", localizationProvider.FullLanguage.ToLowerInvariant());
      urlRewriteProvider.ProcessLanguageUrl();
      Assert.AreEqual("http://idp.debug.m.godaddy-com.ide/default.aspx", HttpContext.Current.Request.Url.ToString());
      Assert.AreEqual("es", localizationProvider.RewrittenUrlLanguage);
      Assert.AreEqual("es-us", localizationProvider.FullLanguage.ToLowerInvariant());
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

      uri = new Uri("http://idp.debug.m.godaddy-com.ide/virtual/es");
      container = SetCountrySubdomainContext(uri.ToString(), virtualDirectoryName: "virtual");
      urlRewriteProvider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();
      localizationProvider = container.Resolve<ILocalizationProvider>();

      Assert.AreEqual("en-us", localizationProvider.FullLanguage.ToLowerInvariant());
      urlRewriteProvider.ProcessLanguageUrl();
      Assert.AreEqual("http://idp.debug.m.godaddy-com.ide/virtual/default.aspx", HttpContext.Current.Request.Url.ToString());
      Assert.AreEqual("es", localizationProvider.RewrittenUrlLanguage);
      Assert.AreEqual("es-us", localizationProvider.FullLanguage.ToLowerInvariant());
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
      LanguageUrlRewriteProvider provider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();

      MethodInfo method = GetPrivateMethod("IsValidLanguageCode");

      Assert.IsFalse((bool)method.Invoke(provider, new object[] { "" }));
      Assert.IsFalse((bool)method.Invoke(provider, new object[] { "  " }));

      Assert.IsFalse((bool)method.Invoke(provider, new object[] { "11" }));
      Assert.IsFalse((bool)method.Invoke(provider, new object[] { "22-33" }));

      Assert.IsFalse((bool)method.Invoke(provider, new object[] { "esx" }));
      Assert.IsFalse((bool)method.Invoke(provider, new object[] { "es-usx" }));
      Assert.IsFalse((bool)method.Invoke(provider, new object[] { "-us" }));
      Assert.IsFalse((bool)method.Invoke(provider, new object[] { "22-usx" }));
      Assert.IsFalse((bool)method.Invoke(provider, new object[] { "esx-us" }));

      Assert.IsFalse((bool)method.Invoke(provider, new object[] { "12(*&)&)*" }));
      Assert.IsFalse((bool)method.Invoke(provider, new object[] { "12(*&)&)*=us" }));
    }

    [TestMethod]
    public void IsValidLanguageCode_ValidCode_ReturnsTrue()
    {
      Uri uri = new Uri("https://whois.godaddy.com");
      IProviderContainer container = SetCountrySubdomainContext(uri.ToString());
      LanguageUrlRewriteProvider provider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();

      MethodInfo method = GetPrivateMethod("IsValidLanguageCode");
      Assert.IsTrue((bool)method.Invoke(provider, new object[] { "es" }));
      Assert.IsTrue((bool)method.Invoke(provider, new object[] { "Es" }));
      Assert.IsTrue((bool)method.Invoke(provider, new object[] { "en" }));
      Assert.IsFalse((bool)method.Invoke(provider, new object[] { "qa-qa" }));
      Assert.IsFalse((bool)method.Invoke(provider, new object[] { "qa-ps" }));

      container.SetData(MockSiteContextSettings.IsRequestInternal, true);
      Assert.IsTrue((bool)method.Invoke(provider, new object[] { "qa-qa" }));
      Assert.IsTrue((bool)method.Invoke(provider, new object[] { "qa-ps" }));
    }

    [TestMethod]
    public void IsValidLanguageCode_InvalidCodeForCountry_ReturnsFalse()
    {
      Uri uri = new Uri("https://whois.godaddy.com");
      IProviderContainer container = SetCountrySubdomainContext(uri.ToString());
      LanguageUrlRewriteProvider provider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();

      MethodInfo method = GetPrivateMethod("IsValidLanguageCode");
      Assert.IsFalse((bool)method.Invoke(provider, new object[] { "fr" }));
      Assert.IsFalse((bool)method.Invoke(provider, new object[] { "hi-us" }));

      uri = new Uri("https://ca.godaddy.com");
      container = SetCountrySubdomainContext(uri.ToString());
      provider = (LanguageUrlRewriteProvider)container.Resolve<ILanguageUrlRewriteProvider>();

      method = GetPrivateMethod("IsValidLanguageCode");
      Assert.IsFalse((bool)method.Invoke(provider, new object[] { "sw" }));
      Assert.IsFalse((bool)method.Invoke(provider, new object[] { "sw-ca" }));
    }

    #endregion
  }
}
