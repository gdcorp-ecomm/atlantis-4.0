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
      bool isSuccess = provider.GetShopperWatchList("878145", out shopperWatchListResponse);
      Assert.AreEqual(true, isSuccess);
      Assert.AreEqual(true, shopperWatchListResponse.Gtlds.Count == 0);
    }

    [TestMethod]
    public void ShopperWatchListSuccess()
    {
      IDotTypeEoiProvider provider = NewDotTypeEoiProvider();
      IShopperWatchListResponse shopperWatchListResponse;
      bool isSuccess = provider.GetShopperWatchList("1", out shopperWatchListResponse);
      Assert.AreEqual(true, isSuccess);
      Assert.AreEqual(true, shopperWatchListResponse.Gtlds.Count > 0);
      Assert.AreEqual(true, shopperWatchListResponse.Gtlds[0].Id > -1);
    }

    [TestMethod]
    public void DotTypeAddToShopperWatchListSuccess()
    {
      IDotTypeEoiProvider provider = NewDotTypeEoiProvider();
      string responseMessage;

      const string gTlds = @"<gTlds><gTld id='18' gTldSubCategoryId='234' /></gTlds>";
      bool isSuccess = provider.AddToShopperWatchList("2013-05-23 10:29:55", gTlds, out responseMessage);
      Assert.AreEqual(true, isSuccess);
    }

    [TestMethod]
    public void DotTypeRemvoeFromShopperWatchListSuccess()
    {
      IDotTypeEoiProvider provider = NewDotTypeEoiProvider();
      string responseMessage;

      const string gTlds = @"<gTlds><gTld id='18' gTldSubCategoryId='234' /></gTlds>";
      bool isSuccess = provider.RemoveFromShopperWatchList(gTlds, out responseMessage);
      Assert.AreEqual(true, isSuccess);
    }
  }
}
