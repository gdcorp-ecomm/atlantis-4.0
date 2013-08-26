using System;
using System.Web;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.SplitTesting.Tests
{
  [TestClass]
  public class SplitTestingRequestCacheTests
  {
    [TestMethod]
    public void AddCacheValues_1Val()
    {
      var shopperId = "1234abcd";
      var privateLabelId = 1;
      var httpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(httpRequest);
      var container = CreateProviderContainer(shopperId, privateLabelId);

      var sut = new SplitTestingRequestCache(container);

      var split = "12.34.56.78";
      sut.AddCacheValues(split);

      Assert.IsTrue(HttpContext.Current.Session[sut.LoadCacheKeyName()] != null);
      Assert.IsTrue(sut.GetCacheValues().Count == 1);
      Assert.IsTrue(sut.GetCacheValues().Contains(split));
    }

    [TestMethod]
    public void AddCacheValues_SameValTwice_OnlyOnceInCache()
    {
      var shopperId = "1234abcd";
      var privateLabelId = 1;
      var httpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(httpRequest);
      var container = CreateProviderContainer(shopperId, privateLabelId);

      var sut = new SplitTestingRequestCache(container);

      var split = "12.34.56.78";
      sut.AddCacheValues(split);
      sut.AddCacheValues(split);

      Assert.IsTrue(HttpContext.Current.Session[sut.LoadCacheKeyName()] != null);
      Assert.IsTrue(sut.GetCacheValues().Count == 1);
      Assert.IsTrue(sut.GetCacheValues().Contains(split));
    }

    [TestMethod]
    public void AddCacheValues_2Val()
    {
      var shopperId = "1234abcd";
      var privateLabelId = 1;
      var httpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(httpRequest);
      var container = CreateProviderContainer(shopperId, privateLabelId);

      var sut = new SplitTestingRequestCache(container);

      var split = "12.34.56.78";
      var split2 = "78.65.43.21";
      sut.AddCacheValues(split);
      sut.AddCacheValues(split2);

      Assert.IsTrue(HttpContext.Current.Session[sut.LoadCacheKeyName()] != null);
      Assert.IsTrue(sut.GetCacheValues().Count == 2);
      Assert.IsTrue(sut.GetCacheValues().Contains(split));
      Assert.IsTrue(sut.GetCacheValues().Contains(split2));
    }

    [TestMethod]
    public void AddCacheValues_DifferentObjectInSessionAtStart()
    {
      var shopperId = "1234abcd";
      var privateLabelId = 1;
      var httpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(httpRequest);
      var container = CreateProviderContainer(shopperId, privateLabelId);

      var sut = new SplitTestingRequestCache(container);

      HttpContext.Current.Session[sut.LoadCacheKeyName()] = "different typed object in session";

      var split = "12.34.56.78";
      sut.AddCacheValues(split);

      Assert.IsNotNull(HttpContext.Current.Session[sut.LoadCacheKeyName()]);
      Assert.AreEqual(1, sut.GetCacheValues().Count);
      Assert.IsTrue(sut.GetCacheValues().Contains(split));
    }

    [TestMethod]
    public void GetCacheValue_NoValues_NoException()
    {
      var shopperId = "1234abcd";
      var privateLabelId = 1;
      var httpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(httpRequest);
      var container = CreateProviderContainer(shopperId, privateLabelId);

      var sut = new SplitTestingRequestCache(container);
      Assert.IsNull(HttpContext.Current.Session[sut.LoadCacheKeyName()], "Session cache is not null before test starts");
      var cachedvalues = sut.GetCacheValues();

      Assert.AreEqual(0, cachedvalues.Count);
    }

    [TestMethod]
    public void GetCacheValue_DifferentTypeInSession_NoException()
    {
      var shopperId = "1234abcd";
      var privateLabelId = 1;
      var httpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(httpRequest);
      var container = CreateProviderContainer(shopperId, privateLabelId);

      var sut = new SplitTestingRequestCache(container);

      HttpContext.Current.Session[sut.LoadCacheKeyName()] = "different typed object in session";

      var cachedvalues = sut.GetCacheValues();

      Assert.AreEqual(0, cachedvalues.Count);
    }

    [TestMethod]
    public void ClearCache()
    {
      var shopperId = "1234abcd";
      var privateLabelId = 1;
      var httpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(httpRequest);
      var container = CreateProviderContainer(shopperId, privateLabelId);

      var sut = new SplitTestingRequestCache(container);
      sut.AddCacheValues("1234");
      sut.AddCacheValues("5678");

      Assert.AreEqual(2, sut.GetCacheValues().Count, "Cache is not full before clear call");
      sut.ClearCache();
      Assert.AreEqual(0, sut.GetCacheValues().Count);
    }

    private static MockProviderContainer CreateProviderContainer(string shopperId, int privateLabelId)
    {
      SplitTestingConfiguration.DefaultCategoryName = "Sales";
      var container = new MockProviderContainer();
      container.RegisterProvider<ISiteContext, MockSiteContext>();
      container.RegisterProvider<IShopperContext, MockShopperContext>();
      container.RegisterProvider<IManagerContext, MockNoManagerContext>();
      var shopperContext = container.Resolve<IShopperContext>();
      shopperContext.SetNewShopper(shopperId);
      container.SetMockSetting(MockSiteContextSettings.PrivateLabelId, privateLabelId);
      return container;
    }
  }
}
