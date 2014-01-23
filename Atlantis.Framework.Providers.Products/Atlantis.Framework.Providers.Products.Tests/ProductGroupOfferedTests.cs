using Atlantis.Framework.Engine;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.Products;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.Testing.MockEngine;
using Atlantis.Framework.Testing.MockLocalization;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.Products.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Products.Impl.dll")]
  public class ProductGroupOfferedTests
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

    private IProviderContainer SetContexts(int privateLabelId, string marketId = "")
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

      return container;
    }

    [TestMethod]
    public void ProductGroupOffered()
    {
      var container = SetContexts(1);
      var productProvider = container.Resolve<IProductProvider>();
      Assert.IsTrue(productProvider.IsProductGroupOffered(30));
    }

    [TestMethod]
    public void ProductGroupOfferedForMarket()
    {
      var container = SetContexts(1, "en-us");
      var productProvider = container.Resolve<IProductProvider>();
      Assert.IsTrue(productProvider.IsProductGroupOffered(30));
    }

    [TestMethod]
    public void ProductGroupNotOffered()
    {
      var container = SetContexts(1);
      var productProvider = container.Resolve<IProductProvider>();
      Assert.IsFalse(productProvider.IsProductGroupOffered(0));
    }
  }
}
