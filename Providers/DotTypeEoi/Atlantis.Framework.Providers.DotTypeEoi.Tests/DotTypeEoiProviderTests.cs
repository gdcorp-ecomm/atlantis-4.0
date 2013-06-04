using System.Collections.Generic;
using Atlantis.Framework.DotTypeEoi.Interface;
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
    readonly MockProviderContainer _container = new MockProviderContainer();

    private IShopperContext ShopperContext
    {
      get
      {
        return _container.Resolve<IShopperContext>();
      }
    }

    private IDotTypeEoiProvider NewDotTypeEoiProvider()
    {
      _container.RegisterProvider<ISiteContext, MockSiteContext>();
      _container.RegisterProvider<IManagerContext, MockNoManagerContext>();
      _container.RegisterProvider<IShopperContext, MockShopperContext>();
      _container.RegisterProvider<IDotTypeEoiProvider, DotTypeEoiProvider>();

      return _container.Resolve<IDotTypeEoiProvider>();
    }

    [TestMethod]
    public void DotTypeGetGeneralEoi1()
    {
      IDotTypeEoiProvider provider = NewDotTypeEoiProvider();
      string displayTime;
      IList<IDotTypeEoiGtld> gTlds;
      int totalPages;
      bool isSuccess = provider.GetGeneralEoi(0, 5, 1, out displayTime, out gTlds, out totalPages);
      Assert.AreEqual(true, isSuccess);
      Assert.AreEqual(true, displayTime != string.Empty);
      Assert.AreEqual(true, gTlds.Count == 5);
      Assert.AreEqual(true, gTlds[0].Name != string.Empty);
      Assert.AreEqual(true, totalPages > 0);
    }

    [TestMethod]
    public void DotTypeGetGeneralEoi2()
    {
      IDotTypeEoiProvider provider = NewDotTypeEoiProvider();
      string displayTime;
      IList<IDotTypeEoiGtld> gTlds;
      int totalPages;
      bool isSuccess = provider.GetGeneralEoi(2, 8, 6, out displayTime, out gTlds, out totalPages);
      Assert.AreEqual(true, isSuccess);
      Assert.AreEqual(true, displayTime != string.Empty);
      Assert.AreEqual(true, gTlds.Count == 8);
      Assert.AreEqual(true, gTlds[0].Name != string.Empty);
      Assert.AreEqual(true, totalPages > 0);
    }

    [TestMethod]
    public void DotTypeGetGeneralEoiCategoryList()
    {
      IDotTypeEoiProvider provider = NewDotTypeEoiProvider();
      IList<ICategoryData> categoryList;
      bool isSuccess = provider.GetGeneralEoiCategoryList(out categoryList);
      Assert.AreEqual(true, isSuccess);
      Assert.AreEqual(true, categoryList.Count > 0);
      Assert.AreEqual(true, categoryList[0] != null);
    }

    [TestMethod]
    public void DotTypeShopperWatchListEmpty()
    {
      IDotTypeEoiProvider provider = NewDotTypeEoiProvider();
      IShopperWatchListResponse shopperWatchListResponse;
      ShopperContext.SetLoggedInShopper("878145");
      bool isSuccess = provider.GetShopperWatchList(out shopperWatchListResponse);
      Assert.AreEqual(true, isSuccess);
      Assert.AreEqual(true, shopperWatchListResponse.Gtlds.Count == 0);
    }

    [TestMethod]
    public void DotTypeShopperWatchListSuccess()
    {
      IDotTypeEoiProvider provider = NewDotTypeEoiProvider();
      IShopperWatchListResponse shopperWatchListResponse;
      ShopperContext.SetLoggedInShopper("1");
      bool isSuccess = provider.GetShopperWatchList(out shopperWatchListResponse);
      Assert.AreEqual(true, isSuccess);
      Assert.AreEqual(true, shopperWatchListResponse.Gtlds.Count > 0);
      Assert.AreEqual(true, shopperWatchListResponse.Gtlds[0].Id > -1);
    }

    [TestMethod]
    public void DotTypeAddToShopperWatchListSuccess()
    {
      IDotTypeEoiProvider provider = NewDotTypeEoiProvider();
      string responseMessage;

      var gTlds = new List<IDotTypeEoiGtld>(1);
      var gTld = new DotTypeEoiGtld { Id = 18, GtldSubCategoryId = 234 };
      const string displayTime = "2013-05-23 10:29:55";
      gTlds.Add(gTld);
      bool isSuccess = provider.AddToShopperWatchList(displayTime, gTlds, out responseMessage);
      Assert.AreEqual(true, isSuccess);
    }

    [TestMethod]
    public void DotTypeRemoveFromShopperWatchListSuccess()
    {
      IDotTypeEoiProvider provider = NewDotTypeEoiProvider();
      string responseMessage;

      var gTlds = new List<IDotTypeEoiGtld>(1);
      var gTld = new DotTypeEoiGtld { Id = 18, GtldSubCategoryId = 234 };
      gTlds.Add(gTld);
      bool isSuccess = provider.RemoveFromShopperWatchList(gTlds, out responseMessage);
      Assert.AreEqual(true, isSuccess);
    }
  }
}
