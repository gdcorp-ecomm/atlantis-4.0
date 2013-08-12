using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using Atlantis.Framework.DataCache;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.SplitTesting.Tests.Mocks;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.SplitTesting.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  public class SplitTestingCookieTests
  {
    [TestMethod]
    public void ReadsCookieCorrectly()
    {
      var shopperId = "1234abcd";
      var privateLabelId = 1;
      var cookieName = string.Format("SplitTesting{0}_{1}", privateLabelId, shopperId);
      var cookieKey = "123-1";
      var cookieValue = "14";
      var cookies = new NameValueCollection();
      cookies.Add(cookieName, cookieKey + "=" + cookieValue);
      var mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      mockHttpRequest.MockCookies(cookies);

      var container = CreateProviderContainer(shopperId, privateLabelId);
      var sut = new SplitTestingCookie(container);

      var result = sut.CookieValues;
      Assert.IsTrue(result.ContainsKey(cookieKey));
      Assert.AreEqual(cookieValue, result[cookieKey]);
    }

    [TestMethod]
    public void ReadsCookie_WhenCookieMissing()
    {
      var shopperId = "1234abcd";
      var privateLabelId = 1;
   
      var mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
   
      var container = CreateProviderContainer(shopperId, privateLabelId);
      var sut = new SplitTestingCookie(container);

      var result = sut.CookieValues;
      Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void SetsCookieCorrectly()
    {
      var shopperId = "1234abcd";
      var privateLabelId = 1;
      var cookieName = GetCookieName(privateLabelId, shopperId);
      var cookieKey = "123-1";
      var cookieValue = "14";
     
      var httpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(httpRequest);
      var container = CreateProviderContainer(shopperId, privateLabelId);
      DataCacheEngineRequests.GetAppSetting = MockEngineRequests.AppSettingRequest_24hours;

      var sut = new SplitTestingCookie(container);
      sut.CookieValues = new Dictionary<string, string>() {{cookieKey, cookieValue}};

      var cookie = HttpContext.Current.Response.Cookies[cookieName];
      Assert.IsNotNull(cookie);
      Assert.AreEqual(cookieValue, cookie.Values[cookieKey]);

      Assert.IsNotNull(cookie.Expires);
      var expectedExpires = Convert.ToDouble(DateTime.Now.AddHours(24).Ticks);
      var actualExprires = Convert.ToDouble(cookie.Expires.Ticks);
      var oneMinute = TimeSpan.TicksPerSecond * 60;

      Assert.AreEqual(expectedExpires, actualExprires, oneMinute);
    }

    private static string GetCookieName(int privateLabelId, string shopperId)
    {
      var cookieName = string.Format("SplitTesting{0}_{1}", privateLabelId, shopperId);
      return cookieName;
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