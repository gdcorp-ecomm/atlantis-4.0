using Atlantis.Framework.Interface;
using Atlantis.Framework.Localization.Interface;
using Atlantis.Framework.Providers.Geo.Interface;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Atlantis.Framework.Providers.Localization.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  [DeploymentItem("Atlantis.Framework.Localization.Impl.dll")]
  public class CountrySiteCookieLocalizationProviderTests
  {
    private IProviderContainer SetContext()
    {
      return SetContext("http://www.mysite.com/");
    }

    private IProviderContainer SetContext(string url)
    {
      //  Clear HttpContextFactory
      HttpContextFactory.ResetHttpContext();

      MockHttpRequest request = new MockHttpRequest(url);
      MockHttpContext.SetFromWorkerRequest(request);

      IProviderContainer result = new MockProviderContainer();
      result.RegisterProvider<ISiteContext, MockSiteContext>();
      result.RegisterProvider<IGeoProvider, MockGeoProvider>();
      result.RegisterProvider<ILocalizationProvider, CountryCookieLocalizationProvider>();
      return result;
    }

    private void CreateCountryookie(int privateLabelId, string value)
    {
      HttpCookie countryCookie = new HttpCookie("countrysite" + privateLabelId.ToString(), value);
      HttpContext.Current.Response.Cookies.Set(countryCookie);
    }

    private void CreateLanguageCookie(int privateLabelId, string value)
    {
      HttpCookie languageCookie = new HttpCookie("language" + privateLabelId.ToString(), value);
      HttpContext.Current.Response.Cookies.Set(languageCookie);
    }

    [TestMethod]
    public void NoCookie()
    {
      IProviderContainer container = SetContext();

      ILocalizationProvider localization = container.Resolve<ILocalizationProvider>();
      Assert.AreEqual("WWW", localization.CountrySite.ToUpperInvariant());
      Assert.IsTrue(localization.IsGlobalSite());
    }

    [TestMethod]
    public void InvalidCookie()
    {
      IProviderContainer container = SetContext();
      CreateCountryookie(1, "garbage");

      ILocalizationProvider localization = container.Resolve<ILocalizationProvider>();
      Assert.AreEqual("WWW", localization.CountrySite.ToUpperInvariant());
      Assert.IsFalse(localization.IsCountrySite("garbage"));
    }

    [TestMethod]
    public void ValidCookie()
    {
      IProviderContainer container = SetContext();
      CreateCountryookie(1, "au");

      ILocalizationProvider localization = container.Resolve<ILocalizationProvider>();
      Assert.AreEqual("AU", localization.CountrySite.ToUpperInvariant());
      Assert.IsTrue(localization.IsCountrySite("au"));
    }

    [TestMethod]
    public void ValidCookieCheckMultiple()
    {
      IProviderContainer container = SetContext();
      CreateCountryookie(1, "au");

      ILocalizationProvider localization = container.Resolve<ILocalizationProvider>();
      Assert.AreEqual("AU", localization.CountrySite.ToUpperInvariant());

      HashSet<string> caseSensitiveHashset = new HashSet<string>(StringComparer.Ordinal);
      caseSensitiveHashset.Add("Au");
      caseSensitiveHashset.Add("cA");

      Assert.IsTrue(localization.IsAnyCountrySite(caseSensitiveHashset));
    }

    [TestMethod]
    public void ValidCookieES()
    {
      IProviderContainer container = SetContext();
      CreateCountryookie(1, "es");

      ILocalizationProvider localization = container.Resolve<ILocalizationProvider>();
      Assert.AreEqual("ES", localization.CountrySite.ToUpperInvariant());
      Assert.IsTrue(localization.IsCountrySite("es"));
    }

    [TestMethod]
    public void HasValidCountrySitesAvailable()
    {
      IProviderContainer container = SetContext();

      ILocalizationProvider localization = container.Resolve<ILocalizationProvider>();
      Assert.AreNotEqual(0, localization.ValidCountrySiteSubdomains.Count());
    }

    [TestMethod]
    public void CountryLinkTypeWWW()
    {
      IProviderContainer container = SetContext();

      ILocalizationProvider localization = container.Resolve<ILocalizationProvider>();
      string linkType = localization.GetCountrySiteLinkType("BASEURL");
      Assert.AreEqual("BASEURL", linkType);
    }

    [TestMethod]
    public void CountryLinkTypeForValidCountry()
    {
      IProviderContainer container = SetContext();
      CreateCountryookie(1, "au");

      ILocalizationProvider localization = container.Resolve<ILocalizationProvider>();
      string linkType = localization.GetCountrySiteLinkType("BASEURL");
      Assert.AreEqual("BASEURL.AU", linkType);
    }

    [TestMethod]
    public void EnsureResponseCookieSet()
    {
      IProviderContainer container = SetContext();

      ILocalizationProvider localization = container.Resolve<ILocalizationProvider>();
      Assert.IsTrue(localization.IsGlobalSite());

      string key = "countrysite1";
      Assert.AreEqual("www", HttpContext.Current.Response.Cookies[key].Value.ToLowerInvariant());
    }    

    [TestMethod]
    public void CountrySiteContext1Only()
    {
      IProviderContainer container = SetContext();
      HttpContext.Current.Items[MockSiteContextSettings.PrivateLabelId] = 3;
      CreateCountryookie(3, "au");

      ILocalizationProvider localization = container.Resolve<ILocalizationProvider>();
      Assert.IsTrue(localization.IsGlobalSite());
    }

    [TestMethod]
    public void GetMarketsForCountry()
    {
      IProviderContainer container = SetContext();

      ILocalizationProvider localization = container.Resolve<ILocalizationProvider>();
      IEnumerable<IMarket> markets = localization.GetMarketsForCountryCode("es");
      Assert.IsTrue(markets.Any());

    }

    [TestMethod]
    public void QueryString_INVALID_Cookie_INVALID_IpCountry_INVALID_Result_WWW()
    {
      var container = SetContext();
      container.SetData(MockGeoProvider.REQUEST_COUNTRY_SETTING_NAME, string.Empty);
      ILocalizationProvider localization = container.Resolve<ILocalizationProvider>();
      Assert.AreEqual("www", localization.CountrySite.ToLowerInvariant());
      Assert.AreEqual("www", HttpContext.Current.Response.Cookies["countrysite1"].Value.ToLowerInvariant());

      container = SetContext();
      container.SetData(MockGeoProvider.REQUEST_COUNTRY_SETTING_NAME, "XX");
      localization = container.Resolve<ILocalizationProvider>();
      Assert.AreEqual("www", localization.CountrySite.ToLowerInvariant());
      Assert.AreEqual("www", HttpContext.Current.Response.Cookies["countrysite1"].Value.ToLowerInvariant());

      container = SetContext("http://www.mysite.com?regionsite=XA");
      container.SetData(MockGeoProvider.REQUEST_COUNTRY_SETTING_NAME, "XB");
      CreateCountryookie(1, "XC");
      localization = container.Resolve<ILocalizationProvider>();
      Assert.AreEqual("www", localization.CountrySite.ToLowerInvariant());
      Assert.AreEqual("www", HttpContext.Current.Response.Cookies["countrysite1"].Value.ToLowerInvariant());
    }

    [TestMethod]
    public void QueryString_NONE_Cookie_NONE_IpCountry_VALID_Result_IPCOUNTRY()
    {
      var container = SetContext();
      container.SetData(MockGeoProvider.REQUEST_COUNTRY_SETTING_NAME, "CA");
      ILocalizationProvider localization = container.Resolve<ILocalizationProvider>();
      Assert.AreEqual("ca", localization.CountrySite.ToLowerInvariant());
      Assert.AreEqual("ca", HttpContext.Current.Response.Cookies["countrysite1"].Value.ToLowerInvariant());

      container = SetContext();
      container.SetData(MockGeoProvider.REQUEST_COUNTRY_SETTING_NAME, "ca");
      localization = container.Resolve<ILocalizationProvider>();
      Assert.AreEqual("ca", localization.CountrySite.ToLowerInvariant());
      Assert.AreEqual("ca", HttpContext.Current.Response.Cookies["countrysite1"].Value.ToLowerInvariant());

      container = SetContext();
      container.SetData(MockGeoProvider.REQUEST_COUNTRY_SETTING_NAME, "WWW");
      localization = container.Resolve<ILocalizationProvider>();
      Assert.AreEqual("www", localization.CountrySite.ToLowerInvariant());
      Assert.AreEqual("www", HttpContext.Current.Response.Cookies["countrysite1"].Value.ToLowerInvariant());

      container = SetContext();
      container.SetData(MockGeoProvider.REQUEST_COUNTRY_SETTING_NAME, "IN");
      localization = container.Resolve<ILocalizationProvider>();
      Assert.AreEqual("in", localization.CountrySite.ToLowerInvariant());
      Assert.AreEqual("in", HttpContext.Current.Response.Cookies["countrysite1"].Value.ToLowerInvariant());
    }

    [TestMethod]
    public void QueryString_NONE_Cookie_VALID_IpCountry_IGNORED_Result_COOKIE()
    {
      var container = SetContext();
      container.SetData(MockGeoProvider.REQUEST_COUNTRY_SETTING_NAME, "CA");
      CreateCountryookie(1, "UK");
      ILocalizationProvider localization = container.Resolve<ILocalizationProvider>();
      Assert.AreEqual("uk", localization.CountrySite.ToLowerInvariant());
      Assert.AreEqual("uk", HttpContext.Current.Response.Cookies["countrysite1"].Value.ToLowerInvariant());

      container = SetContext();
      container.SetData(MockGeoProvider.REQUEST_COUNTRY_SETTING_NAME, "ca");
      CreateCountryookie(1, "uk");
      localization = container.Resolve<ILocalizationProvider>();
      Assert.AreEqual("uk", localization.CountrySite.ToLowerInvariant());
      Assert.AreEqual("uk", HttpContext.Current.Response.Cookies["countrysite1"].Value.ToLowerInvariant());

      container = SetContext();
      container.SetData(MockGeoProvider.REQUEST_COUNTRY_SETTING_NAME, "IN");
      CreateCountryookie(1, "WWW");
      localization = container.Resolve<ILocalizationProvider>();
      Assert.AreEqual("www", localization.CountrySite.ToLowerInvariant());
      Assert.AreEqual("www", HttpContext.Current.Response.Cookies["countrysite1"].Value.ToLowerInvariant());

      container = SetContext();
      container.SetData(MockGeoProvider.REQUEST_COUNTRY_SETTING_NAME, "IN");
      CreateCountryookie(1, "www");
      localization = container.Resolve<ILocalizationProvider>();
      Assert.AreEqual("www", localization.CountrySite.ToLowerInvariant());
      Assert.AreEqual("www", HttpContext.Current.Response.Cookies["countrysite1"].Value.ToLowerInvariant());
    }

    [TestMethod]
    public void QueryString_VALID_Cookie_IGNORED_IpCountry_IGNORED_Result_QUERYSTRING()
    {
      var container = SetContext("http://www.mysite.com?regionsite=IN");
      container.SetData(MockGeoProvider.REQUEST_COUNTRY_SETTING_NAME, "CA");
      CreateCountryookie(1, "UK");
      ILocalizationProvider localization = container.Resolve<ILocalizationProvider>();
      Assert.AreEqual("in", localization.CountrySite.ToLowerInvariant());
      Assert.AreEqual("in", HttpContext.Current.Response.Cookies["countrysite1"].Value.ToLowerInvariant());

      container = SetContext("http://www.mysite.com?regionsite=in");
      container.SetData(MockGeoProvider.REQUEST_COUNTRY_SETTING_NAME, "ca");
      CreateCountryookie(1, "uk");      
      localization = container.Resolve<ILocalizationProvider>();
      Assert.AreEqual("in", localization.CountrySite.ToLowerInvariant());
      Assert.AreEqual("in", HttpContext.Current.Response.Cookies["countrysite1"].Value.ToLowerInvariant());

      container = SetContext("http://www.mysite.com?regionsite=www");
      container.SetData(MockGeoProvider.REQUEST_COUNTRY_SETTING_NAME, "IN");
      CreateCountryookie(1, "UK");
      localization = container.Resolve<ILocalizationProvider>();
      Assert.AreEqual("www", localization.CountrySite.ToLowerInvariant());
      Assert.AreEqual("www", HttpContext.Current.Response.Cookies["countrysite1"].Value.ToLowerInvariant());

      container = SetContext("http://www.mysite.com?regionsite=ca");
      container.SetData(MockGeoProvider.REQUEST_COUNTRY_SETTING_NAME, "IN");
      CreateCountryookie(1, "www");
      localization = container.Resolve<ILocalizationProvider>();
      Assert.AreEqual("ca", localization.CountrySite.ToLowerInvariant());
      Assert.AreEqual("ca", HttpContext.Current.Response.Cookies["countrysite1"].Value.ToLowerInvariant());
    }
  }
}
