using Atlantis.Framework.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.Shopper.Tests
{
  [TestClass]
  public class ShopperIdAndStatusTests
  {
    [TestMethod]
    public void DefaultShopperIdAndStatusNoDebugContext()
    {
      var container = new MockProviderContainer();
      var shopperIdAndStatus = new ShopperIdAndStatus(container);

      Assert.AreEqual(string.Empty, shopperIdAndStatus.ShopperId);
      Assert.AreEqual(ShopperStatusType.Public, shopperIdAndStatus.Status);
    }

    [TestMethod]
    public void SetShopperIdAndStatusNoDebugContext()
    {
      var container = new MockProviderContainer();
      var shopperIdAndStatus = new ShopperIdAndStatus(container);
      shopperIdAndStatus.SetShopperAndStatus("832652", ShopperStatusType.Authenticated, "UnitTest");

      Assert.AreEqual("832652", shopperIdAndStatus.ShopperId);
      Assert.AreEqual(ShopperStatusType.Authenticated, shopperIdAndStatus.Status);
    }

    [TestMethod]
    public void SetShopperIdAndStatusNoDebugContextNullShopper()
    {
      var container = new MockProviderContainer();
      var shopperIdAndStatus = new ShopperIdAndStatus(container);
      shopperIdAndStatus.SetShopperAndStatus(null, ShopperStatusType.Public, "UnitTest");

      Assert.AreEqual(string.Empty, shopperIdAndStatus.ShopperId);
      Assert.AreEqual(ShopperStatusType.Public, shopperIdAndStatus.Status);
    }

    [TestMethod]
    public void SetShopperIdAndStatusWithDebugContext()
    {
      var container = new MockProviderContainer();
      container.RegisterProvider<IDebugContext, MockDebugContext>();

      var shopperIdAndStatus = new ShopperIdAndStatus(container);
      shopperIdAndStatus.SetShopperAndStatus("832652", ShopperStatusType.Authenticated, "UnitTest");

      var debug = container.Resolve<IDebugContext>();
      Assert.AreEqual(2, debug.GetDebugTrackingData().Count);
    }

    [TestMethod]
    public void SetShopperIdAndStatusWithDebugContextNullSource()
    {
      var container = new MockProviderContainer();
      container.RegisterProvider<IDebugContext, MockDebugContext>();

      var shopperIdAndStatus = new ShopperIdAndStatus(container);
      shopperIdAndStatus.SetShopperAndStatus("832652", ShopperStatusType.Authenticated, null);

      var debug = container.Resolve<IDebugContext>();
      Assert.AreEqual(2, debug.GetDebugTrackingData().Count);
    }

    [TestMethod]
    public void SetShopperIdAndStatusHttpContextSetsExceptionWebState()
    {
      var request = new MockHttpRequest("http://mysite.com/page.aspx");
      MockHttpContext.SetFromWorkerRequest(request);

      var container = new MockProviderContainer();
      var shopperIdAndStatus = new ShopperIdAndStatus(container);
      shopperIdAndStatus.SetShopperAndStatus("832652", ShopperStatusType.Authenticated, "UnitTest");

      Assert.AreEqual("832652", AtlantisExceptionWebState.ShopperId);
    }
  }
}
