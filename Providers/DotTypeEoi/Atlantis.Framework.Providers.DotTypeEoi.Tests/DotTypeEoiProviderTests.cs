using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DotTypeEoi.Interface;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.DotTypeEoi.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.DotTypeEoi.Impl.dll")]
  public class DotTypeEoiProviderTests
  {
    private IDotTypeEoiProvider NewDotTypeEoiProvider()
    {
      var container = new MockProviderContainer();
      container.RegisterProvider<ISiteContext, MockSiteContext>();
      container.RegisterProvider<IManagerContext, MockNoManagerContext>();
      container.RegisterProvider<IShopperContext, MockShopperContext>();
      container.RegisterProvider<IDotTypeEoiProvider, DotTypeEoiProvider>();

      return container.Resolve<IDotTypeEoiProvider>();
    }

    [TestMethod]
    public void DotTypeGetGeneralEoiSuccess()
    {
      IDotTypeEoiProvider provider = NewDotTypeEoiProvider();
      IDotTypeEoiResponse dotTypeEoiResponse;
      bool isSuccess = provider.GetGeneralEoi(out dotTypeEoiResponse);
      Assert.AreEqual(true, isSuccess);
      Assert.AreEqual(true, dotTypeEoiResponse.Categories.Count > 0);
      Assert.AreEqual(true, dotTypeEoiResponse.Categories[0].Gtlds.Count > 0);
    }

    [TestMethod]
    public void ShopperWatchListEmpty()
    {
      IDotTypeEoiProvider provider = NewDotTypeEoiProvider();
      IShopperWatchListResponse shopperWatchListResponse;
      bool isSuccess = provider.GetShopperWatchList(out shopperWatchListResponse);
      Assert.AreEqual(true, isSuccess);
      Assert.AreEqual(true, shopperWatchListResponse.Gtlds.Count == 0);
    }
  }
}
