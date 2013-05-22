using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Geo.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace Atlantis.Framework.Providers.Geo.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  [DeploymentItem("Atlantis.Framework.Geo.Impl.dll")]
  [DeploymentItem("GeoIP.dat")]
  public class GeoProviderTests
  {
    private IGeoProvider CreateGeoProvider(string requestIP, bool isInternal = false, bool useMockProxy = false)
    {
      MockHttpRequest request = new MockHttpRequest("http://blue.com");

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
      IGeoProvider geoProvider = CreateGeoProvider("127.0.0.1", true);
      geoProvider.SpoofUserIPAddress("148.204.3.3");
      Assert.AreEqual("mx", geoProvider.RequestCountryCode);
    }

    [TestMethod]
    public void SpoofTheIPNotAllowed()
    {
      IGeoProvider geoProvider = CreateGeoProvider("182.50.145.39", false);
      geoProvider.SpoofUserIPAddress("148.204.3.3");
      Assert.AreEqual("sg", geoProvider.RequestCountryCode);
    }

    [TestMethod]
    public void SpoofTooLate()
    {
      IGeoProvider geoProvider = CreateGeoProvider("127.0.0.1", true);
      Assert.AreEqual("us", geoProvider.RequestCountryCode);
      geoProvider.SpoofUserIPAddress("148.204.3.3");
      Assert.AreEqual("us", geoProvider.RequestCountryCode);
    }

    [TestMethod]
    public void RequestCountryProxy()
    {
      IGeoProvider geoProvider = CreateGeoProvider("148.204.3.3", false, true);
      Assert.IsTrue(geoProvider.IsUserInCountry("cn"));
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

  }
}
