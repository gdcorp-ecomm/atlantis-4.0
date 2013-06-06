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
      IGeneralEoiData generalEoiData;
      bool isSuccess = provider.GetGeneralEoi(0, 5, 1, "en-us", out generalEoiData);
      Assert.AreEqual(true, isSuccess);
      Assert.AreEqual(true, generalEoiData.DisplayTime != string.Empty);
      Assert.AreEqual(true, generalEoiData.Gtlds.Count == 5);
      Assert.AreEqual(true, generalEoiData.Gtlds[0].Name != string.Empty);
      Assert.AreEqual(true, generalEoiData.TotalPages > 0);
    }

    [TestMethod]
    public void DotTypeGetGeneralEoi2()
    {
      IDotTypeEoiProvider provider = NewDotTypeEoiProvider();
      IGeneralEoiData generalEoiData;
      bool isSuccess = provider.GetGeneralEoi(2, 8, 6, "en-us", out generalEoiData);
      Assert.AreEqual(true, isSuccess);
      Assert.AreEqual(true, generalEoiData.DisplayTime != string.Empty);
      Assert.AreEqual(true, generalEoiData.Gtlds.Count == 8);
      Assert.AreEqual(true, generalEoiData.Gtlds[0].Name != string.Empty);
      Assert.AreEqual(true, generalEoiData.TotalPages > 0);
    }

    [TestMethod]
    public void DotTypeGetGeneralEoiCategoryList()
    {
      IDotTypeEoiProvider provider = NewDotTypeEoiProvider();
      IList<ICategoryData> categoryList;
      bool isSuccess = provider.GetGeneralEoiCategoryList("en-us", out categoryList);
      Assert.AreEqual(true, isSuccess);
      Assert.AreEqual(true, categoryList.Count > 0);
      Assert.AreEqual(true, categoryList[0] != null);
    }

    [TestMethod]
    public void DotTypeShopperWatchListEmpty()
    {
      IDotTypeEoiProvider provider = NewDotTypeEoiProvider();
      IShopperWatchListResponse shopperWatchListResponse;
      ShopperContext.SetLoggedInShopper("0");
      bool isSuccess = provider.GetShopperWatchList("en-us", out shopperWatchListResponse);
      Assert.AreEqual(true, isSuccess);
      Assert.AreEqual(true, shopperWatchListResponse.GtldIdDictionary.Count == 0);
    }

    [TestMethod]
    public void DotTypeShopperWatchListSuccess()
    {
      IDotTypeEoiProvider provider = NewDotTypeEoiProvider();
      string responseMessage;
      ShopperContext.SetLoggedInShopper("861126");
      var gTlds = new List<IDotTypeEoiGtld>(1);
      var gTld = new DotTypeEoiGtld { Id = 18, GtldSubCategoryId = 234 };
      const string displayTime = "2013-05-23 10:29:55";
      gTlds.Add(gTld);
      provider.AddToShopperWatchList(displayTime, gTlds, "en-us", out responseMessage);

      IShopperWatchListResponse shopperWatchListResponse;
      bool isSuccess = provider.GetShopperWatchList("en-us", out shopperWatchListResponse);
      Assert.AreEqual(true, isSuccess);
      Assert.AreEqual(true, shopperWatchListResponse.GtldIdDictionary.Count > 0);
      Assert.AreEqual(true, shopperWatchListResponse.GtldIdDictionary[18].Id > -1);
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
      bool isSuccess = provider.AddToShopperWatchList(displayTime, gTlds, "en-us", out responseMessage);
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
