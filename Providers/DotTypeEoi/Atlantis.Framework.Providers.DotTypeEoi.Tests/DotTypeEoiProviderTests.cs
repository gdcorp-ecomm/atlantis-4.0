using System.Collections.Generic;
using Atlantis.Framework.DotTypeCache;
using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DotTypeEoi.Interface;
using Atlantis.Framework.Providers.Localization;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.Providers.ProxyContext;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.DotTypeEoi.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("dottypecache.config")]
  [DeploymentItem("Atlantis.Framework.DotTypeEoi.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.Localization.Interface.dll")]
  [DeploymentItem("Atlantis.Framework.Localization.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.Providers.Localization.dll")]
  [DeploymentItem("Atlantis.Framework.Providers.ProxyContext.dll")]
  [DeploymentItem("Atlantis.Framework.RegDotTypeRegistry.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.RegDotTypeProductIds.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DCCDomainsDataCache.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.TLDDataCache.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DotTypeCache.StaticTypes.dll")]
  [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DataCacheGeneric.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.PrivateLabel.Impl.dll")]
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
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.godaddy.com/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      _container.RegisterProvider<ISiteContext, MockSiteContext>();
      _container.RegisterProvider<IManagerContext, MockNoManagerContext>();
      _container.RegisterProvider<IShopperContext, MockShopperContext>();
      _container.RegisterProvider<IDotTypeEoiProvider, DotTypeEoiProvider>();
      _container.RegisterProvider<IDotTypeProvider, DotTypeProvider>();
      _container.RegisterProvider<IProxyContext, WebProxyContext>();
      _container.RegisterProvider<ILocalizationProvider, CountrySubdomainLocalizationProvider>();

      _container.Resolve<IDotTypeProvider>();

      return _container.Resolve<IDotTypeEoiProvider>();
    }

    [TestMethod]
    public void DotTypeGetGeneralEoiLoggedIn()
    {
      IDotTypeEoiProvider provider = NewDotTypeEoiProvider();
      IGeneralEoiData generalEoiData;
      ShopperContext.SetLoggedInShopper("861126");
      
      bool isSuccess = provider.GetGeneralEoi(out generalEoiData);
      Assert.AreEqual(true, isSuccess);
      Assert.AreEqual(true, generalEoiData.DisplayTime != string.Empty);
      Assert.AreEqual(true, generalEoiData.Categories.Count > 0);
      Assert.AreEqual(true, generalEoiData.Categories[0].Name != string.Empty);
      Assert.AreEqual(true, generalEoiData.Categories[0].SubCategories.Count > 0);
      Assert.AreEqual(true, generalEoiData.Categories[0].SubCategories[0].Gtlds.Count > 0);
      Assert.AreEqual(true, generalEoiData.Categories[0].SubCategories[0].Gtlds[0].Id > -1);
    }

    [TestMethod]
    public void DotTypeGetGeneralEoiPaginated1()
    {
      IDotTypeEoiProvider provider = NewDotTypeEoiProvider();
      IGeneralGtldData generalGtldData;
      bool isSuccess = provider.GetGeneralEoi(1, 5, 23, out generalGtldData);
      Assert.AreEqual(true, isSuccess);
      Assert.AreEqual(true, generalGtldData.DisplayTime != string.Empty);
      Assert.AreEqual(true, generalGtldData.Gtlds.Count == 5);
      Assert.AreEqual(true, generalGtldData.Gtlds[0].Name != string.Empty);
      Assert.AreEqual(true, generalGtldData.TotalPages > 0);
    }

    [TestMethod]
    public void DotTypeGetGeneralEoiPaginated2()
    {
      IDotTypeEoiProvider provider = NewDotTypeEoiProvider();
      IGeneralGtldData generalGtldData;
      ShopperContext.SetLoggedInShopper("861126");
      bool isSuccess = provider.GetGeneralEoi(2, 8, 23, out generalGtldData);
      Assert.AreEqual(true, isSuccess);
      Assert.AreEqual(true, generalGtldData.DisplayTime != string.Empty);
      Assert.AreEqual(true, generalGtldData.Gtlds.Count == 8);
      Assert.AreEqual(true, generalGtldData.Gtlds[0].Name != string.Empty);
      Assert.AreEqual(true, generalGtldData.TotalPages > 0);
    }

    [TestMethod]
    public void DotTypeGetGeneralEoiNonPaginated()
    {
      IDotTypeEoiProvider provider = NewDotTypeEoiProvider();
      IGeneralGtldData generalGtldData;
      ShopperContext.SetLoggedInShopper("861126");
      bool isSuccess = provider.GetGeneralEoi(23, out generalGtldData);
      Assert.AreEqual(true, isSuccess);
      Assert.AreEqual(true, generalGtldData.DisplayTime != string.Empty);
      Assert.AreEqual(true, generalGtldData.Gtlds.Count > 0);
      Assert.AreEqual(true, generalGtldData.Gtlds[0].Name != string.Empty);
      Assert.AreEqual(true, generalGtldData.TotalPages ==1);
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
    public void DotTypeSearchEoiStaticTld()
    {
      IDotTypeEoiProvider provider = NewDotTypeEoiProvider();
      IGeneralGtldData generalGtldData;
      bool isSuccess = provider.SearchEoi("com", out generalGtldData);
      Assert.AreEqual(true, isSuccess);
      Assert.AreEqual(true, generalGtldData.Gtlds.Count > 0);
      Assert.AreEqual(true, generalGtldData.Gtlds[0].Id > -1);
    }

    [TestMethod]
    public void DotTypeSearchEoiTldmlTld()
    {
      IDotTypeEoiProvider provider = NewDotTypeEoiProvider();
      IGeneralGtldData generalGtldData;
      bool isSuccess = provider.SearchEoi("k.borg", out generalGtldData);
      Assert.AreEqual(true, isSuccess);
      Assert.AreEqual(true, generalGtldData.Gtlds.Count > 0);
      Assert.AreEqual(true, generalGtldData.Gtlds[0].Id > -1);
    }

    [TestMethod]
    public void DotTypeShopperWatchListEmpty()
    {
      IDotTypeEoiProvider provider = NewDotTypeEoiProvider();
      IShopperWatchListResponse shopperWatchListResponse;
      ShopperContext.SetLoggedInShopper("0");
      bool isSuccess = provider.GetShopperWatchList(out shopperWatchListResponse);
      Assert.AreEqual(true, isSuccess);
      Assert.AreEqual(true, shopperWatchListResponse.GtldIdDictionary.Count == 0);
    }

    [TestMethod]
    public void DotTypeShopperWatchListSuccess()
    {
      IDotTypeEoiProvider provider = NewDotTypeEoiProvider();
      string responseMessage;
      ShopperContext.SetLoggedInShopper("861126");
      provider.AddToShopperWatchList("ads", out responseMessage);

      IShopperWatchListResponse shopperWatchListResponse;
      bool isSuccess = provider.GetShopperWatchList(out shopperWatchListResponse);
      Assert.AreEqual(true, isSuccess);
      Assert.AreEqual(true, shopperWatchListResponse.GtldIdDictionary.Count > 0);
      Assert.AreEqual(true, shopperWatchListResponse.GtldIdDictionary[1609].Id > -1);
    }

    [TestMethod]
    public void DotTypeAddToShopperWatchList2Success()
    {
      IDotTypeEoiProvider provider = NewDotTypeEoiProvider();
      string responseMessage;
      ShopperContext.SetLoggedInShopper("861126");
      bool isSuccess = provider.AddToShopperWatchList("ads", out responseMessage);
      Assert.AreEqual(true, isSuccess);
    }

    [TestMethod]
    public void DotTypeRemoveFromShopperWatchListSuccess()
    {
      IDotTypeEoiProvider provider = NewDotTypeEoiProvider();
      string responseMessage;
      ShopperContext.SetLoggedInShopper("861126");
      bool isSuccess = provider.RemoveFromShopperWatchList("ads", out responseMessage);
      Assert.AreEqual(true, isSuccess);
    }
  }
}
