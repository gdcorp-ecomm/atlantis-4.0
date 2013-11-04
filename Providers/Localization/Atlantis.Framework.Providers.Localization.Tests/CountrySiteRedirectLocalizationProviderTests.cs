using System.Globalization;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Geo.Interface;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;

namespace Atlantis.Framework.Providers.Localization.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  [DeploymentItem("Atlantis.Framework.Localization.Impl.dll")]
  public class CountrySiteRedirectLocalizationProviderTests
  {
    private IProviderContainer SetContext(string url, string ipCountry)
    {
      HttpContextFactory.ResetHttpContext();

      MockHttpRequest request = new MockHttpRequest(url);
      MockHttpContext.SetFromWorkerRequest(request);
      
      var result = new MockProviderContainer();
      result.RegisterProvider<ISiteContext, MockSiteContext>();
      result.RegisterProvider<ILocalizationProvider, CountrySubdomainLocalizationProvider>();
      result.RegisterProvider<ILocalizationRedirectProvider, LocalizationRedirectProvider>();
      result.RegisterProvider<IProxyContext, ProxyContext.WebProxyContext>();
      result.RegisterProvider<IGeoProvider, MockGeoProvider>();
      result.SetData(MockGeoProvider.REQUEST_COUNTRY_SETTING_NAME, ipCountry);
      
      return result;
    }

    private void CreateCountryCookie(int privateLabelId, string value)
    {
      var countryCookie = new HttpCookie("countrysite" + privateLabelId.ToString(CultureInfo.InvariantCulture), value);
      HttpContext.Current.Request.Cookies.Set(countryCookie);
    }

    private void CreateLanguagePreferenceCookie(int privateLabelId, string value)
    {
      var countryCookie = new HttpCookie("language" + privateLabelId.ToString(CultureInfo.InvariantCulture), value);
      HttpContext.Current.Request.Cookies.Set(countryCookie);
    }
    
    [TestMethod]
    public void TestRedirectNoCountryCookieIpAU()
    {
      var container = SetContext("http://mysite.com", "au");
      
      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsTrue(result.CountrySite.ToUpperInvariant() == "AU");
      Assert.IsTrue(result.ShouldRedirect);
    }

    [TestMethod]
    public void TestRedirectNoCountryCookieSiteAU()
    {
      var container = SetContext("http://au.mysite.com", "us");

      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsTrue(result.CountrySite.ToUpperInvariant() == "AU");
      Assert.IsTrue(!result.ShouldRedirect);
    }

    [TestMethod]
    public void TestRedirectHasCountryCookieAU1()
    {
      var container = SetContext("http://mysite.com", "us");

      CreateCountryCookie(1, "AU");

      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsTrue(result.CountrySite.ToUpperInvariant() == "AU");
      Assert.IsTrue(result.ShouldRedirect);
    }

    [TestMethod]
    public void TestRedirectHasCountryCookieAU2()
    {
      var container = SetContext("http://mysite.com", "us");

      CreateCountryCookie(1, "AU");

      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsTrue(result.CountrySite.ToUpperInvariant() == "AU");
      Assert.IsTrue(result.ShouldRedirect);
    }

    [TestMethod]
    public void TestRedirectNoCountryCookieSiteCA()
    {
      var container = SetContext("http://ca.mysite.com", "us");

      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsTrue(result.CountrySite.ToUpperInvariant() == "CA");
      Assert.IsTrue(!result.ShouldRedirect);
    }

    [TestMethod]
    public void TestRedirectNoCountryCookieIpCA()
    {
      var container = SetContext("http://mysite.com", "ca");

      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsTrue(result.CountrySite.ToUpperInvariant() == "CA");
      Assert.IsTrue(result.ShouldRedirect);
    }

    [TestMethod]
    public void TestRedirectHasCountryCookieCA()
    {
      var container = SetContext("http://mysite.com", "us");

      CreateCountryCookie(1, "CA");

      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsTrue(result.CountrySite.ToUpperInvariant() == "CA");
      Assert.IsTrue(result.ShouldRedirect);
    }

    [TestMethod]
    public void TestRedirectNoCountryCookieES()
    {
      var container = SetContext("http://mysite.com", "es");

      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsTrue(result.CountrySite.ToUpperInvariant() == "ES");
      Assert.IsTrue(result.ShouldRedirect);
    }

    [TestMethod]
    public void TestRedirectHasCountryCookieES()
    {
      var container = SetContext("http://mysite.com", "us");

      CreateCountryCookie(1, "ES");

      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsTrue(result.CountrySite.ToUpperInvariant() == "ES");
      Assert.IsTrue(result.ShouldRedirect);
    }

    [TestMethod]
    public void TestRedirectCountrySiteIN()
    {
      var container = SetContext("http://in.mysite.com", "us");

      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsTrue(result.CountrySite.ToUpperInvariant() == "IN");
      Assert.IsTrue(!result.ShouldRedirect);
    }

    [TestMethod]
    public void TestRedirectNoCountryCookieAndIpIN()
    {
      var container = SetContext("http://mysite.com", "in");

      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsTrue(result.CountrySite.ToUpperInvariant() == "IN");
      Assert.IsTrue(result.ShouldRedirect);
    }

    [TestMethod]
    public void TestRedirectHasCountryCookieIN1()
    {
      var container = SetContext("http://in.mysite.com", "us");

      CreateCountryCookie(1, "IN");

      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsTrue(result.CountrySite.ToUpperInvariant() == "IN");
      Assert.IsTrue(!result.ShouldRedirect);
    }

    [TestMethod]
    public void TestRedirectHasCountryAndLangCookieIN()
    {
      var container = SetContext("http://mysite.com", "us");

      CreateCountryCookie(1, "IN");
      CreateLanguagePreferenceCookie(1, "EN");

      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsTrue(result.CountrySite.ToUpperInvariant() == "IN");
      Assert.IsTrue(result.ShouldRedirect);
    }

    [TestMethod]
    public void TestRedirectNoCountryCookieSiteUK()
    {
      var container = SetContext("http://uk.mysite.com", "us");

      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsTrue(result.CountrySite.ToUpperInvariant() == "UK");
      Assert.IsTrue(!result.ShouldRedirect);
    }

    [TestMethod]
    public void TestRedirectNoCountryCookieIpUK()
    {
      var container = SetContext("http://mysite.com", "uk");

      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsTrue(result.CountrySite.ToUpperInvariant() == "UK");
      Assert.IsTrue(result.ShouldRedirect);
    }

    [TestMethod]
    public void TestRedirectCountrySiteAndLangUK()
    {
      var container = SetContext("http://uk.mysite.com", "us");
      
      CreateLanguagePreferenceCookie(1, "ES");

      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsTrue(result.CountrySite.ToUpperInvariant() == "UK");
      Assert.IsTrue(!result.ShouldRedirect);
    }

    [TestMethod]
    public void TestRedirectCountryLangAndCountryPrefUK()
    {
      var container = SetContext("http://mysite.com", "us");
     
      CreateCountryCookie(1, "UK");
      CreateLanguagePreferenceCookie(1, "EN");

      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsTrue(result.CountrySite.ToUpperInvariant() == "UK");
      Assert.IsTrue(result.ShouldRedirect);
    }

    [TestMethod]
    public void TestRedirectHasCountryCookieESNoRedirectQueryString()
    {
      var container = SetContext("http://mysite.com?countryview=1", "us");

      CreateCountryCookie(1, "ES");

      var localization = container.Resolve<ILocalizationRedirectProvider>();
      var result = localization.RedirectResponse;
      Assert.IsTrue(result.CountrySite.ToUpperInvariant() == "WWW");
      Assert.IsFalse(result.ShouldRedirect);
    }
  }
}
