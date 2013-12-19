using System.Runtime.Remoting;
using Atlantis.Framework.Geo.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Geo.Interface;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Net;
using System.Reflection;
using Atlantis.Framework.Testing.MockLocalization;

namespace Atlantis.Framework.Providers.Geo.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  [DeploymentItem("Atlantis.Framework.Geo.Impl.dll")]
  [DeploymentItem("GeoIP.dat")]
  [DeploymentItem("GeoIPCity.dat")]
  public class GeoProviderTests
  {
    private IGeoProvider CreateGeoProvider(string requestIP, bool isInternal = false, bool useMockProxy = false, string spoofip = null, string language = null)
    {
      MockHttpRequest request = new MockHttpRequest("http://blue.com?qaspoofip=" + spoofip ?? string.Empty);

      IPAddress address;
      if (IPAddress.TryParse(requestIP, out address))
      {
        request.MockRemoteAddress(address);
      }

      MockHttpContext.SetFromWorkerRequest(request);

      MockProviderContainer container = new MockProviderContainer();
      container.RegisterProvider<ISiteContext, MockSiteContext>();
      container.RegisterProvider<IShopperContext, MockShopperContext>();
      container.RegisterProvider<IManagerContext, MockNoManagerContext>();
      container.RegisterProvider<IGeoProvider, GeoProvider>();
      container.SetData(MockSiteContextSettings.IsRequestInternal, isInternal);

      if (useMockProxy)
      {
        container.RegisterProvider<IProxyContext, MockProxy>();
      }

      if (language != null)
      {
        container.RegisterProvider<ILocalizationProvider, MockLocalizationProvider>();
        container.SetData(MockLocalizationProviderSettings.FullLanguage, language);
      }

      return container.Resolve<IGeoProvider>();
    }

    [TestMethod]
    public void RequestCountryLocalHost()
    {
      IGeoProvider geoProvider = CreateGeoProvider("127.0.0.1");
      Assert.AreEqual("us", geoProvider.RequestCountryCode);
      Assert.IsTrue(geoProvider.IsUserInCountry("Us"));
      Assert.IsFalse(geoProvider.IsUserInCountry("xx"));
    }

    [TestMethod]
    public void RequestCountrySingapore()
    {
      IGeoProvider geoProvider = CreateGeoProvider("182.50.145.39");
      Assert.AreEqual("sg", geoProvider.RequestCountryCode);
      Assert.IsTrue(geoProvider.IsUserInCountry("sG"));
    }

    [TestMethod]
    public void SpoofTheIP()
    {
      IGeoProvider geoProvider = CreateGeoProvider("127.0.0.1", true, spoofip: "148.204.3.3");
      Assert.AreEqual("mx", geoProvider.RequestCountryCode);
    }

    [TestMethod]
    public void SpoofTheIPNotAllowed()
    {
      IGeoProvider geoProvider = CreateGeoProvider("182.50.145.39", false, spoofip: "148.204.3.3");
      Assert.AreEqual("sg", geoProvider.RequestCountryCode);
    }

    [TestMethod]
    public void RequestCountryProxy()
    {
      IGeoProvider geoProvider = CreateGeoProvider("148.204.3.3", false, true);
      Assert.IsTrue(geoProvider.IsUserInCountry("cn"));
    }

    [TestMethod]
    public void UserInNullCountry()
    {
      IGeoProvider geoProvider = CreateGeoProvider("148.204.3.3", false, true);
      Assert.IsFalse(geoProvider.IsUserInCountry(null));
    }

    [TestMethod]
    public void UserInRegion()
    {
      IGeoProvider geoProvider = CreateGeoProvider("5.158.255.220", false);
      Assert.AreEqual("fr", geoProvider.RequestCountryCode);
      Assert.IsTrue(geoProvider.IsUserInRegion(2, "EU"));
    }

    [TestMethod]
    public void UserInRegionDataCacheError()
    {
      int regions = GeoProviderEngineRequests.Regions;
      try
      {
        GeoProviderEngineRequests.Regions = 999;
        IGeoProvider geoProvider = CreateGeoProvider("5.158.255.220", false);
        Assert.AreEqual("fr", geoProvider.RequestCountryCode);
        Assert.IsFalse(geoProvider.IsUserInRegion(2, "EU"));
      }
      finally
      {
        GeoProviderEngineRequests.Regions = regions;
      }
    }
    [TestMethod]
    public void UserNotInRegion()
    {
      IGeoProvider geoProvider = CreateGeoProvider("1.179.3.3", false);
      Assert.IsFalse(geoProvider.IsUserInRegion(2, "EU"));
    }

    [TestMethod]
    public void UserLocation()
    {
      IGeoProvider geoProvider = CreateGeoProvider("97.74.104.201", false);
      Assert.AreEqual("us", geoProvider.RequestGeoLocation.CountryCode);
      Assert.AreEqual("Scottsdale", geoProvider.RequestGeoLocation.City);
      Assert.AreNotEqual(0, geoProvider.RequestGeoLocation.Latitude);
      Assert.AreNotEqual(0, geoProvider.RequestGeoLocation.Longitude);
      Assert.AreNotEqual(0, geoProvider.RequestGeoLocation.MetroCode);

      Assert.IsFalse(string.IsNullOrEmpty(geoProvider.RequestGeoLocation.GeoRegion));
      Assert.IsFalse(string.IsNullOrEmpty(geoProvider.RequestGeoLocation.GeoRegionName));
      Assert.IsFalse(string.IsNullOrEmpty(geoProvider.RequestGeoLocation.PostalCode));
    }

    [TestMethod]
    public void UserIPCountryFailure()
    {
      int ipcountry = GeoProviderEngineRequests.IPCountryLookup;
      int iplocation = GeoProviderEngineRequests.IPLocationLookup;

      try
      {
        GeoProviderEngineRequests.IPCountryLookup = 999;
        GeoProviderEngineRequests.IPLocationLookup = 999;

        IGeoProvider geoProvider = CreateGeoProvider("182.50.145.39");
        Assert.AreNotEqual("sg", geoProvider.RequestCountryCode);
        Assert.AreEqual(string.Empty, geoProvider.RequestGeoLocation.City);
      }
      finally
      {
        GeoProviderEngineRequests.IPCountryLookup = ipcountry;
        GeoProviderEngineRequests.IPLocationLookup = iplocation;
      }
    }

    [TestMethod]
    public void UserIPLocationFailure()
    {
      int ipcountry = GeoProviderEngineRequests.IPCountryLookup;
      int iplocation = GeoProviderEngineRequests.IPLocationLookup;

      try
      {
        GeoProviderEngineRequests.IPCountryLookup = 999;
        GeoProviderEngineRequests.IPLocationLookup = 999;

        IGeoProvider geoProvider = CreateGeoProvider("97.74.104.201");
        Assert.AreEqual(string.Empty, geoProvider.RequestGeoLocation.City);
      }
      finally
      {
        GeoProviderEngineRequests.IPCountryLookup = ipcountry;
        GeoProviderEngineRequests.IPLocationLookup = iplocation;
      }
    }


    [TestMethod]
    public void UserLocationFirstSavesCountryCall()
    {
      ConfigElement countryLookupConfig;
      Engine.Engine.TryGetConfigElement(GeoProviderEngineRequests.IPCountryLookup, out countryLookupConfig);
      countryLookupConfig.ResetStats();

      IGeoProvider geoProvider = CreateGeoProvider("97.74.104.201", false);
      Assert.AreEqual("us", geoProvider.RequestGeoLocation.CountryCode);
      Assert.AreEqual("us", geoProvider.RequestCountryCode);

      ConfigElementStats stats = countryLookupConfig.ResetStats();
      Assert.AreEqual(0, stats.Succeeded);
      Assert.AreEqual(0, stats.Failed);
    }

    [TestMethod]
    public void GeoLocationConstructorsFromNotFound()
    {
      Type geoLocationType = typeof(Atlantis.Framework.Providers.Geo.GeoLocation);
      MethodInfo fromNotFound = geoLocationType.GetMethod("FromNotFound", BindingFlags.Static | BindingFlags.NonPublic);
      IGeoLocation newNotFound = fromNotFound.Invoke(null, null) as IGeoLocation;
      Assert.AreEqual(string.Empty, newNotFound.City);
    }

    [TestMethod]
    public void GeoLocationConstructorsNullIPLocation()
    {
      Type geoLocationType = typeof(Atlantis.Framework.Providers.Geo.GeoLocation);
      MethodInfo fromNull = geoLocationType.GetMethod("FromIPLocation", BindingFlags.Static | BindingFlags.NonPublic);
      IPLocation nullLocation = null;
      object[] parameters = new object[1] { nullLocation };
      IGeoLocation newNotFound = fromNull.Invoke(null, parameters) as IGeoLocation;
      Assert.AreEqual(string.Empty, newNotFound.City);
    }

    [TestMethod]
    public void Countries()
    {
      IGeoProvider geoProvider = CreateGeoProvider("1.179.3.3", false);
      int count = geoProvider.Countries.Count();
      Assert.AreNotEqual(0, count);
    }

    [TestMethod]
    public void CountriesDataCacheError()
    {
      int countries = GeoProviderEngineRequests.Countries;
      try
      {
        GeoProviderEngineRequests.Countries = 999;
        IGeoProvider geoProvider = CreateGeoProvider("1.179.3.3", false);
        int count = geoProvider.Countries.Count();
        Assert.AreEqual(0, count);
      }
      finally
      {
        GeoProviderEngineRequests.Countries = countries;
      }
    }

    [TestMethod]
    public void TryGetMissingCountry()
    {
      IGeoProvider geoProvider = CreateGeoProvider("1.179.3.3", false);
      IGeoCountry country;
      bool result = geoProvider.TryGetCountryByCode("zz", out country);
      Assert.IsFalse(result);
    }

    [TestMethod]
    public void TryGetCountry()
    {
      IGeoProvider geoProvider = CreateGeoProvider("1.179.3.3", false);
      IGeoCountry country;
      bool result = geoProvider.TryGetCountryByCode("fr", out country);
      Assert.IsTrue(result);

      Assert.AreEqual("fr", country.Code);
      Assert.IsNotNull(country.CallingCode);
      Assert.AreNotEqual(0, country.Id);
      Assert.IsNotNull(country.Name);
    }

    [TestMethod]
    public void TryGetCountryKy()
    {
      IGeoProvider geoProvider = CreateGeoProvider("1.179.3.3", false);
      IGeoCountry country;
      bool result = geoProvider.TryGetCountryByCode("ky", out country);
      Assert.IsTrue(result);

      Assert.AreEqual("ky", country.Code);
      Assert.AreEqual(country.CallingCode, "1");
      Assert.AreNotEqual(0, country.Id);
      Assert.IsNotNull(country.Name);
    }

    [TestMethod]
    public void TryGetCountryZw()
    {
      IGeoProvider geoProvider = CreateGeoProvider("1.179.3.3", false);
      IGeoCountry country;
      bool result = geoProvider.TryGetCountryByCode("zw", out country);
      Assert.IsTrue(result);

      Assert.AreEqual("zw", country.Code);
      Assert.AreEqual(country.CallingCode, "1");
      Assert.AreNotEqual(0, country.Id);
      Assert.IsNotNull(country.Name);
    }

    [TestMethod]
    public void TryGetCallingCodeDefault()
    {
      IGeoProvider geoProvider = CreateGeoProvider("1.179.3.3", false);
      IGeoCountry country;
      bool result = geoProvider.TryGetCountryByCode("hm", out country);
      Assert.IsTrue(result);

      Assert.AreEqual("hm", country.Code);
      Assert.AreEqual(country.CallingCode, "0");
      Assert.AreNotEqual(0, country.Id);
      Assert.IsNotNull(country.Name);
    }

    [TestMethod]
    public void TryGetCountryPtBr()
    {
      IGeoProvider geoProvider = CreateGeoProvider("1.179.3.3", false, language: "pt-br");
      IGeoCountry country;
      bool result = geoProvider.TryGetCountryByCode("us", out country);
      Assert.AreNotEqual("United States", country.Name);
    }

    [TestMethod]
    public void TryGetCountryPtBrRestServiceError()
    {
      int countryNames = GeoProviderEngineRequests.CountryNames;
      try
      {
        GeoProviderEngineRequests.CountryNames = 999;
        IGeoProvider geoProvider = CreateGeoProvider("1.179.3.3", false, language: "pt-br");
        IGeoCountry country;
        bool result = geoProvider.TryGetCountryByCode("us", out country);
        Assert.AreEqual("United States", country.Name);
      }
      finally
      {
        GeoProviderEngineRequests.CountryNames = countryNames;
      }
    }

    [TestMethod]
    public void TryGetCountryUsEn()
    {
      IGeoProvider geoProvider = CreateGeoProvider("1.179.3.3", false, language: "en-us");
      IGeoCountry country;
      bool result = geoProvider.TryGetCountryByCode("us", out country);
      Assert.AreEqual("United States", country.Name);
    }

    [TestMethod]
    public void States()
    {
      IGeoProvider geoProvider = CreateGeoProvider("1.179.3.3", false);
      IGeoCountry country;
      bool result = geoProvider.TryGetCountryByCode("us", out country);

      Assert.IsTrue(country.HasStates);

      int stateCount = country.States.Count();
      Assert.AreNotEqual(0, stateCount);
    }

    [TestMethod]
    public void StatesDataCacheError()
    {
      int states = GeoProviderEngineRequests.States;
      try
      {
        GeoProviderEngineRequests.States = 999;
        IGeoProvider geoProvider = CreateGeoProvider("1.179.3.3", false);
        IGeoCountry country;
        bool result = geoProvider.TryGetCountryByCode("us", out country);

        Assert.IsFalse(country.HasStates);

        int stateCount = country.States.Count();
        Assert.AreEqual(0, stateCount);
      }
      finally
      {
        GeoProviderEngineRequests.States = states;
      }
    }

    [TestMethod]
    public void TryGetMissingState()
    {
      IGeoProvider geoProvider = CreateGeoProvider("1.179.3.3", false);
      IGeoCountry country;
      bool result = geoProvider.TryGetCountryByCode("us", out country);

      IGeoState state;
      result = country.TryGetStateByCode("zz", out state);
      Assert.IsFalse(result);
    }

    [TestMethod]
    public void TryGetKansas()
    {
      IGeoProvider geoProvider = CreateGeoProvider("1.179.3.3", false);
      IGeoCountry country;
      bool result = geoProvider.TryGetCountryByCode("us", out country);

      IGeoState state;
      result = country.TryGetStateByCode("ks", out state);
      Assert.IsTrue(result);
      Assert.IsNotNull(state.Code);
      Assert.IsNotNull(state.Name);
      Assert.AreNotEqual(0, state.Id);
    }

    [TestMethod]
    public void TryGetAlaskaPtBr()
    {
      IGeoProvider geoProvider = CreateGeoProvider("1.179.3.3", false, language: "pt-br");
      IGeoCountry country;
      bool result = geoProvider.TryGetCountryByCode("us", out country);

      IGeoState state;
      result = country.TryGetStateByCode("ak", out state);
      Assert.AreNotEqual("Alaska", state.Name);
    }

    [TestMethod]
    public void TryGetAlaskaPtBrRestServiceError()
    {
      int stateNames = GeoProviderEngineRequests.StateNames;
      try
      {
        GeoProviderEngineRequests.StateNames = 999;

        IGeoProvider geoProvider = CreateGeoProvider("1.179.3.3", false, language: "pt-br");
        IGeoCountry country;
        bool result = geoProvider.TryGetCountryByCode("us", out country);

        IGeoState state;
        result = country.TryGetStateByCode("ak", out state);
        Assert.AreEqual("Alaska", state.Name);
      }
      finally
      {
        GeoProviderEngineRequests.StateNames = stateNames;
      }
    }

    [TestMethod]
    public void TryGetAlaskaUsEn()
    {
      IGeoProvider geoProvider = CreateGeoProvider("1.179.3.3", false, language: "en-us");
      IGeoCountry country;
      bool result = geoProvider.TryGetCountryByCode("us", out country);

      IGeoState state;
      result = country.TryGetStateByCode("ak", out state);
      Assert.AreEqual("Alaska", state.Name);
    }



  }
}
