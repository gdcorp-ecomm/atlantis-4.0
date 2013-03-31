using Atlantis.Framework.Interface;
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
      MockHttpRequest request = new MockHttpRequest("http://www.mysite.com/");
      MockHttpContext.SetFromWorkerRequest(request);

      IProviderContainer result = new MockProviderContainer();
      result.RegisterProvider<ISiteContext, MockSiteContext>();
      result.RegisterProvider<ILocalizationProvider, CountryCookieLocalizationProvider>();
      return result;
    }

    private void CreateCountryookie(int privateLabelId, string value)
    {
      HttpCookie countryCookie = new HttpCookie("countrysite" + privateLabelId.ToString(), value);
      HttpContext.Current.Request.Cookies.Set(countryCookie);
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
    public void InvalidCookieES()
    {
      IProviderContainer container = SetContext();
      CreateCountryookie(1, "es");

      ILocalizationProvider localization = container.Resolve<ILocalizationProvider>();
      Assert.AreEqual("WWW", localization.CountrySite.ToUpperInvariant());
      Assert.IsTrue(localization.IsCountrySite("www"));
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
    public void EnsureResponseCookieNotSet()
    {
      IProviderContainer container = SetContext();

      ILocalizationProvider localization = container.Resolve<ILocalizationProvider>();
      Assert.IsTrue(localization.IsGlobalSite());

      string key = "countrysite1";
      foreach(string cookiekey in HttpContext.Current.Response.Cookies.Keys)
      {
        Assert.AreNotEqual(key, cookiekey);
      }
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



  }
}
