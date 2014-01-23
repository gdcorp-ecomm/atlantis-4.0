using Atlantis.Framework.Engine;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Geo.Interface;
using Atlantis.Framework.Providers.Interface.Products;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.Providers.Shopper.Interface;
using Atlantis.Framework.Testing.MockEngine;
using Atlantis.Framework.Testing.MockLocalization;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.Products.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Products.Impl.dll")]
  public class O365ProductGroupOfferedTests
  {
    private IErrorLogger _defaultLogger;
    private MockErrorLogger _testLogger;

    [TestInitialize]
    public void InitTest()
    {
      _defaultLogger = EngineLogging.EngineLogger;
      _testLogger = new MockErrorLogger();
      EngineLogging.EngineLogger = _testLogger;
    }

    [TestCleanup]
    public void CleanupTest()
    {
      EngineLogging.EngineLogger = _defaultLogger;
    }

    private IProviderContainer SetContexts(int privateLabelId, string marketId = "", string countryCode = "", string shopperId = "")
    {
      var container = new MockProviderContainer();
      container.RegisterProvider<ISiteContext, MockSiteContext>();
      container.RegisterProvider<IProductProvider, ProductProvider>();
      container.SetData(MockSiteContextSettings.PrivateLabelId, privateLabelId);

      if (!string.IsNullOrEmpty(marketId))
      {
        container.RegisterProvider<ILocalizationProvider, MockLocalizationProvider>();

        switch (marketId.ToLowerInvariant())
        {
          case "en-us":
            container.SetData(MockLocalizationProviderSettings.CountrySite, "www");
            container.SetData(MockLocalizationProviderSettings.MarketInfo, new MockMarketInfo(marketId, string.Empty, marketId, false));
            break;
          case "en-zz":
            container.SetData(MockLocalizationProviderSettings.CountrySite, "zz");
            container.SetData(MockLocalizationProviderSettings.MarketInfo, new MockMarketInfo(marketId, string.Empty, marketId, false));
            break;
        }
      }

      if (!string.IsNullOrEmpty(countryCode))
      {
        container.RegisterProvider<IGeoProvider, MockGeoProvider>();
        container.SetData("MockGeoProvider.CountryCode", countryCode);
      }

      if (!string.IsNullOrEmpty(shopperId))
      {
        container.RegisterProvider<IShopperDataProvider, MockShopperDataProvider>();
        container.SetData("MockShopperDataProvider.ShopperId", shopperId);
      }

      return container;
    }

    [TestMethod]
    public void ProductGroupOffered()
    {
      var container = SetContexts(1);
      var productProvider = container.Resolve<IProductProvider>();
      Assert.IsTrue(productProvider.IsProductGroupOffered(99));
    }

    [TestMethod]
    public void ProductGroupOfferedForMarketOnly()
    {
      var container = SetContexts(1, "en-us");
      var productProvider = container.Resolve<IProductProvider>();
      Assert.IsTrue(productProvider.IsProductGroupOffered(99));
    }

    [TestMethod]
    public void ProductGroupOfferedForShopperOnly()
    {
      var container = SetContexts(1, "", "", "usShopper123");
      var productProvider = container.Resolve<IProductProvider>();
      Assert.IsTrue(productProvider.IsProductGroupOffered(99));
    }

    [TestMethod]
    public void ProductGroupOfferedForIPOnly()
    {
      var container = SetContexts(1, "", "us");
      var productProvider = container.Resolve<IProductProvider>();
      Assert.IsTrue(productProvider.IsProductGroupOffered(99));
    }

    [TestMethod]
    public void ProductGroupOfferedForShopper()
    {
      var container = SetContexts(1, "en-us", "", "usShopper123");
      var productProvider = container.Resolve<IProductProvider>();
      Assert.IsTrue(productProvider.IsProductGroupOffered(99));
    }

    [TestMethod]
    public void ProductGroupOfferedForIP()
    {
      var container = SetContexts(1, "en-us", "us");
      var productProvider = container.Resolve<IProductProvider>();
      Assert.IsTrue(productProvider.IsProductGroupOffered(99));
    }

    [TestMethod]
    public void ProductGroupNotOfferedForMarket()
    {
      var container = SetContexts(1, "en-zz");
      var productProvider = container.Resolve<IProductProvider>();
      Assert.IsFalse(productProvider.IsProductGroupOffered(99));
    }

    [TestMethod]
    public void ProductGroupNotOfferedForShopper()
    {
      var container = SetContexts(1, "en-us", "", "zzShopper123");
      var productProvider = container.Resolve<IProductProvider>();
      Assert.IsFalse(productProvider.IsProductGroupOffered(99));
    }

    [TestMethod]
    public void ProductGroupNotOfferedForIP()
    {
      var container = SetContexts(1, "en-us", "zz");
      var productProvider = container.Resolve<IProductProvider>();
      Assert.IsFalse(productProvider.IsProductGroupOffered(99));
    }

    [TestMethod]
    public void ProductGroupNotOfferedForShopperOrIP()
    {
      var container = SetContexts(1, "en-us", "zz", "zzShopper123");
      var productProvider = container.Resolve<IProductProvider>();
      Assert.IsFalse(productProvider.IsProductGroupOffered(99));
    }

    [TestMethod]
    public void ProductGroupNotOfferedForMarketOrShopperOrIP()
    {
      var container = SetContexts(1, "en-zz", "zz", "zzShopper123");
      var productProvider = container.Resolve<IProductProvider>();
      Assert.IsFalse(productProvider.IsProductGroupOffered(99));
    }
  }
}
