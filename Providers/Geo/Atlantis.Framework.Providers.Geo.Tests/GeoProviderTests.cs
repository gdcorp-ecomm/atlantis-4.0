using Atlantis.Framework.Geo.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Geo.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Reflection;

namespace Atlantis.Framework.Providers.Geo.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  [DeploymentItem("Atlantis.Framework.Geo.Impl.dll")]
  [DeploymentItem("GeoIP.dat")]
  [DeploymentItem("GeoIPLocation.dat")]
  public class GeoProviderTests
  {
    private IGeoProvider CreateGeoProvider(string requestIP, bool isInternal = false, bool useMockProxy = false, string spoofip = null)
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
      container.SetMockSetting(MockSiteContextSettings.IsRequestInternal, isInternal);

      if (useMockProxy)
      {
        container.RegisterProvider<IProxyContext, MockProxy>();
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


  }
}
