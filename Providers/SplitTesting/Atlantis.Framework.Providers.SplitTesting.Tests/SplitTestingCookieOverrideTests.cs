using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.SplitTesting.Tests
{
  [TestClass]
  public class SplitTestingCookieOverrideTests
  {
    [TestMethod]
    public void ReadsCookieCorrectly()
    {
      var shopperId = "1234abcd";
      var privateLabelId = 1;
      var cookieName = string.Format("SplitTestingOverride{0}", privateLabelId);
      var cookieKey = "123-1";
      var cookieValue = "14";
      var cookies = new NameValueCollection();
      cookies.Add(cookieName, cookieKey + "=" + cookieValue);
      var mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      mockHttpRequest.MockCookies(cookies);

      var container = CreateProviderContainer(shopperId, privateLabelId);
      var sut = new SplitTestingCookieOverride(container);

      var result = sut.CookieValues;
      Assert.IsTrue(result.ContainsKey(cookieKey));
      Assert.AreEqual(cookieValue, result[cookieKey]);
    }

    [TestMethod]
    public void ReadsCookie_NoExceptionWhenCookieMissing()
    {
      var shopperId = "1234abcd";
      var privateLabelId = 1;
   
      var mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
   
      var container = CreateProviderContainer(shopperId, privateLabelId);
      var sut = new SplitTestingCookieOverride(container);

      var result = sut.CookieValues;
      Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void SetsCookieCorrectly()
    {
      var shopperId = "1234abcd";
      var privateLabelId = 1;
      var cookieName = string.Format("SplitTestingOverride{0}", privateLabelId);
      var cookieKey = "123-1";
      var cookieValue = "14";

      var httpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(httpRequest);
      var container = CreateProviderContainer(shopperId, privateLabelId);

      var sut = new SplitTestingCookieOverride(container);
      sut.CookieValues = new Dictionary<string, string>() { { cookieKey, cookieValue } };

      var cookie = HttpContext.Current.Response.Cookies[cookieName];
      Assert.IsNotNull(cookie);
      Assert.AreEqual(cookieValue, cookie.Values[cookieKey]);
    }

    [TestMethod]
    public void SetsCookieCorrectly_2Tests()
    {
      var shopperId = "1234abcd";
      var privateLabelId = 1;
      var cookieName = string.Format("SplitTestingOverride{0}", privateLabelId);
      var cookieKey = "123";
      var cookieValue = "B";
      var cookieKeyB = "1243";
      var cookieValueB = "F";

      var httpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(httpRequest);
      var container = CreateProviderContainer(shopperId, privateLabelId);

      var sut = new SplitTestingCookieOverride(container);
      sut.CookieValues = new Dictionary<string, string>() { { cookieKey, cookieValue } };
      sut.CookieValues = new Dictionary<string, string>() { { cookieKeyB, cookieValueB } };

      var cookie = HttpContext.Current.Response.Cookies[cookieName];
      Assert.IsNotNull(cookie);
      Assert.AreEqual(cookieValue, cookie.Values[cookieKey]);
      Assert.AreEqual(cookieValueB, cookie.Values[cookieKeyB]);
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