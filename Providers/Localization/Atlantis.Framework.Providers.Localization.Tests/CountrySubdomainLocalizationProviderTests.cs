using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Atlantis.Framework.Providers.Localization.Tests
{
  [TestClass]
  [DeploymentItem(afeConfig)]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  [DeploymentItem("Atlantis.Framework.Localization.Impl.dll")]
  public class CountrySubdomainLocalizationProviderTests
  {
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

    private IProviderContainer SetContext(string url)
    {
      MockHttpRequest request = new MockHttpRequest(url);
      MockHttpContext.SetFromWorkerRequest(request);

      IProviderContainer result = new MockProviderContainer();
      result.RegisterProvider<ISiteContext, MockSiteContext>();
      result.RegisterProvider<ILocalizationProvider, CountrySubdomainLocalizationProvider>();
      return result;
    }

    private void CreateCountryookie(int privateLabelId, string value)
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

    [TestMethod]
    public void GlobalNoCookieOnRequest()
    {
      IProviderContainer container = SetContext("http://www.mysite.com/");

      ILocalizationProvider localization = container.Resolve<ILocalizationProvider>();
      Assert.IsTrue(localization.IsGlobalSite());

      HttpCookie responseCookie = GetResponseCookieIfExists(1);
      Assert.IsNotNull(responseCookie);
    }

    [TestMethod]
    public void GlobalCookieAlreadyExists()
    {
      IProviderContainer container = SetContext("http://www.mysite.com/");
      CreateCountryookie(1, "www");

      ILocalizationProvider localization = container.Resolve<ILocalizationProvider>();
      Assert.IsTrue(localization.IsGlobalSite());

      HttpCookie responseCookie = GetResponseCookieIfExists(1);
      Assert.IsNull(responseCookie);
    }

    [TestMethod]
    public void GlobalCookieNonMatch()
    {
      IProviderContainer container = SetContext("http://www.mysite.com/");
      CreateCountryookie(1, "au");

      ILocalizationProvider localization = container.Resolve<ILocalizationProvider>();
      Assert.IsTrue(localization.IsGlobalSite());

      HttpCookie responseCookie = GetResponseCookieIfExists(1);
      Assert.AreEqual("WWW", responseCookie.Value.ToUpperInvariant());
    }

    [TestMethod]
    public void NoDotInHost()
    {
      IProviderContainer container = SetContext("http://gdc/");

      ILocalizationProvider localization = container.Resolve<ILocalizationProvider>();
      Assert.IsTrue(localization.IsGlobalSite());
    }

    [TestMethod]
    public void ShortHost()
    {
      IProviderContainer container = SetContext("http://g/");
      ILocalizationProvider localization = container.Resolve<ILocalizationProvider>();
      Assert.IsTrue(localization.IsGlobalSite());
    }

    [TestMethod]
    public void InvalidSubdomain()
    {
      IProviderContainer container = SetContext("http://australia.mysite.com/");

      ILocalizationProvider localization = container.Resolve<ILocalizationProvider>();
      Assert.IsTrue(localization.IsGlobalSite());
    }

    [TestMethod]
    public void CountryLinkTypeWWW()
    {
      IProviderContainer container = SetContext("http://www.mysite.com/");

      ILocalizationProvider localization = container.Resolve<ILocalizationProvider>();
      string linkType = localization.GetCountrySiteLinkType("BASEURL");
      Assert.AreEqual("BASEURL", linkType);
    }

    [TestMethod]
    public void CountryLinkTypeForValidCountry()
    {
      IProviderContainer container = SetContext("http://au.mysite.com/");

      ILocalizationProvider localization = container.Resolve<ILocalizationProvider>();
      string linkType = localization.GetCountrySiteLinkType("BASEURL");
      Assert.AreEqual("BASEURL.AU", linkType);
    }

    [TestMethod]
    public void HasValidCountrySitesAvailable()
    {
      IProviderContainer container = SetContext("http://au.mysite.com/");

      ILocalizationProvider localization = container.Resolve<ILocalizationProvider>();
      Assert.AreNotEqual(0, localization.ValidCountrySiteSubdomains.Count());
    }

    [TestMethod]
    public void ValidSubdomain()
    {
      IProviderContainer container = SetContext("http://au.mysite.com");

      ILocalizationProvider localization = container.Resolve<ILocalizationProvider>();
      Assert.AreEqual("AU", localization.CountrySite.ToUpperInvariant());
      Assert.IsTrue(localization.IsCountrySite("au"));
    }

    [TestMethod]
    public void NoHttpContext()
    {
      HttpContext.Current = null;

      IProviderContainer container = new MockProviderContainer();
      container.RegisterProvider<ISiteContext, MockSiteContext>();
      container.RegisterProvider<ILocalizationProvider, CountrySubdomainLocalizationProvider>();
      ILocalizationProvider localization = container.Resolve<ILocalizationProvider>();

      Assert.IsTrue(localization.IsGlobalSite());
    }

    [TestMethod]
    public void ValidSubdomainCheckMultiple()
    {
      IProviderContainer container = SetContext("http://au.mysite.com");

      ILocalizationProvider localization = container.Resolve<ILocalizationProvider>();
      Assert.AreEqual("AU", localization.CountrySite.ToUpperInvariant());

      HashSet<string> caseSensitiveHashset = new HashSet<string>(StringComparer.Ordinal);
      caseSensitiveHashset.Add("Au");
      caseSensitiveHashset.Add("cA");

      Assert.IsTrue(localization.IsAnyCountrySite(caseSensitiveHashset));
    }

    [TestMethod]
    public void ValidSubdomainProxied()
    {
      IProviderContainer container = SetContext("http://au.mysite.com");
      container.RegisterProvider<IProxyContext, LocalizationTestWebProxy>();

      ILocalizationProvider localization = container.Resolve<ILocalizationProvider>();
      Assert.AreEqual("UK", localization.CountrySite.ToUpperInvariant());
      Assert.IsTrue(localization.IsCountrySite("uk"));
    }

    [TestMethod]
    public void TransperfectLanguage()
    {
      IProviderContainer container = SetContext("http://www.mysite.com");
      container.RegisterProvider<IProxyContext, TransperfectTestWebProxy>();

      ILocalizationProvider localization = container.Resolve<ILocalizationProvider>();
      Assert.IsFalse(localization.IsGlobalSite());
      Assert.AreEqual("ES", localization.ShortLanguage.ToUpperInvariant());
      Assert.AreEqual("ES-US", localization.FullLanguage.ToUpperInvariant());
      Assert.IsTrue(localization.IsActiveLanguage("eS"));
      Assert.IsFalse(localization.IsActiveLanguage("eS-mx"));
    }

    [TestMethod]
    public void SetMarketOnTransperfectProxy()
    {
      IProviderContainer container = SetContext("http://ca.mysite.com");
      container.RegisterProvider<IProxyContext, TransperfectTestWebProxy>();

      ILocalizationProvider localization = container.Resolve<ILocalizationProvider>();
      Assert.IsFalse(localization.IsGlobalSite());
      Assert.AreEqual("ES", localization.ShortLanguage.ToUpperInvariant());

      localization.SetMarket("fr-CA");
      Assert.AreEqual("es", localization.ShortLanguage.ToLowerInvariant());
      Assert.AreEqual("es-us", localization.FullLanguage.ToLowerInvariant());
      Assert.AreEqual("es-us", localization.MarketInfo.Id.ToLowerInvariant());
      Assert.AreEqual("es-us", localization.CurrentCultureInfo.Name.ToLowerInvariant());
    }

    [TestMethod]
    public void SetMarketIdOnNormalRequest()
    {
      IProviderContainer container = SetContext("http://ca.mysite.com");

      ILocalizationProvider localization = container.Resolve<ILocalizationProvider>();
      Assert.AreEqual("EN-CA", localization.FullLanguage.ToUpperInvariant());

      localization.SetMarket("en-IN");
      Assert.AreEqual("EN", localization.ShortLanguage.ToUpperInvariant());
      Assert.AreEqual("en-in", localization.FullLanguage.ToLowerInvariant());
      Assert.AreEqual("en-IN", localization.MarketInfo.Id);
      Assert.AreEqual("en-IN", localization.CurrentCultureInfo.Name);
    }

    [TestMethod]
    public void CultureDefault()
    {
      IProviderContainer container = SetContext("http://www.mysite.com");
      ILocalizationProvider localization = container.Resolve<ILocalizationProvider>();
      Assert.IsTrue(localization.IsGlobalSite());

      CultureInfo info = localization.CurrentCultureInfo;
      Assert.AreEqual("en-US", info.Name);
    }

    [TestMethod]
    public void CultureAustralianEnglish()
    {
      IProviderContainer container = SetContext("http://au.mysite.com");
      ILocalizationProvider localization = container.Resolve<ILocalizationProvider>();

      CultureInfo info = localization.CurrentCultureInfo;
      Assert.AreEqual("en-AU", info.Name);
    }

    [TestMethod]
    public void CultureCanadianEnglish()
    {
      IProviderContainer container = SetContext("http://ca.mysite.com");
      ILocalizationProvider localization = container.Resolve<ILocalizationProvider>();

      CultureInfo info = localization.CurrentCultureInfo;
      Assert.AreEqual("en-CA", info.Name);
    }

    [TestMethod]
    public void CultureCanadianFrenchSetToQaPs()
    {
      IProviderContainer container = SetContext("http://ca.mysite.com");
      ILocalizationProvider localization = container.Resolve<ILocalizationProvider>();
      CultureInfo info = localization.CurrentCultureInfo;
      Assert.AreEqual("en-CA", info.Name);

      localization.SetMarket("qa-PS");

      info = localization.CurrentCultureInfo;
      //Assert.AreEqual("en-US", info.Name);  // The active market table data was changed
      Assert.AreEqual("de-DE", info.Name);
    }

    [TestMethod]
    public void CultureIndianEnglish()
    {
      IProviderContainer container = SetContext("http://in.mysite.com");
      ILocalizationProvider localization = container.Resolve<ILocalizationProvider>();
      CultureInfo info = localization.CurrentCultureInfo;
      Assert.AreEqual("en-IN", info.Name);

      localization.SetMarket("en-IN");

      info = localization.CurrentCultureInfo;
      Assert.AreEqual("en-IN", info.Name);
    }

    [TestMethod]
    public void CultureUnitedKingdomEnglish()
    {
      IProviderContainer container = SetContext("http://uk.mysite.com");
      ILocalizationProvider localization = container.Resolve<ILocalizationProvider>();
      CultureInfo info = localization.CurrentCultureInfo;
      Assert.AreEqual("en-GB", info.Name);

      localization.SetMarket("en-GB");

      info = localization.CurrentCultureInfo;
      Assert.AreEqual("en-GB", info.Name);
    }

  }
}
