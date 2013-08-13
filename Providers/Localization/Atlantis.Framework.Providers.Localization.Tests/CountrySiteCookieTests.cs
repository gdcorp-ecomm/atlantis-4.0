using Atlantis.Framework.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;
using System.Web;

namespace Atlantis.Framework.Providers.Localization.Tests
{
  [TestClass]
  [DeploymentItem("Atlantis.Framework.Providers.Localization.dll")]
  public class CountrySiteCookieTests
  {
    public CountrySiteCookieTests()
    {
      
    }

    private PrivateObject NewCountrySiteCookieClass(object[] constructorArgs)
    {
      Assembly assembly = Assembly.Load("Atlantis.Framework.Providers.Localization");
      Type countrySiteCookieType = assembly.GetType("Atlantis.Framework.Providers.Localization.CountrySiteCookie");
      PrivateObject result = new PrivateObject(countrySiteCookieType, constructorArgs); 
      return result;
    }

    private PrivateObject NewCountrySiteCookieContext(int privateLabelId = 1)
    {
      MockHttpRequest request = new MockHttpRequest("http://www.mysite.com");
      MockHttpContext.SetFromWorkerRequest(request);

      MockProviderContainer container = new MockProviderContainer();
      container.RegisterProvider<ISiteContext, MockSiteContext>();
      container.SetData(MockSiteContextSettings.PrivateLabelId, privateLabelId);

      object[] constructorParams = new object[1] { container };
      return NewCountrySiteCookieClass(constructorParams);
    }

    private PrivateObject NewCountrySiteCookieNoHttpContext()
    {
      HttpContext.Current = null;
      IProviderContainer container = new MockProviderContainer();

      object[] constructorParams = new object[1] { container };
      return NewCountrySiteCookieClass(constructorParams);
    }

    [TestMethod]
    public void NoHttpContext()
    {
      PrivateObject countrySiteCookie = NewCountrySiteCookieNoHttpContext();
      object hasValue = countrySiteCookie.GetProperty("HasValue");
      Assert.IsFalse((bool)hasValue);

      object value = countrySiteCookie.GetProperty("Value");
      Assert.AreEqual(string.Empty, (string)value);

      countrySiteCookie.SetProperty("Value", "nothinghappens");
    }

    [TestMethod]
    public void NoCookie()
    {
      PrivateObject countrySiteCookie = NewCountrySiteCookieContext();
      object hasValue = countrySiteCookie.GetProperty("HasValue");
      Assert.IsFalse((bool)hasValue);
    }

    [TestMethod]
    public void SetCookie()
    {
      PrivateObject countrySiteCookie = NewCountrySiteCookieContext();
      countrySiteCookie.SetProperty("Value", "testValue");
      HttpCookie setCookie = HttpContext.Current.Response.Cookies["countrysite1"];
      Assert.AreEqual("testValue", setCookie.Value);
    }

    [TestMethod]
    public void CookieExistsAndProperlyLazyLoaded()
    {
      PrivateObject countrySiteCookie = NewCountrySiteCookieContext();
      HttpCookie testCookie = new HttpCookie("countrysite1", "ca");
      HttpContext.Current.Request.Cookies.Set(testCookie);
      object cookieValue = countrySiteCookie.GetProperty("Value");
      Assert.AreEqual("ca", (string)cookieValue);
    }

    [TestMethod]
    public void CookieNameNotHardcoded()
    {
      PrivateObject countrySiteCookie = NewCountrySiteCookieContext(3);

      Lazy<string> cookieNameMember = (Lazy<string>)countrySiteCookie.GetField("_cookieName", BindingFlags.NonPublic | BindingFlags.Instance);
      Assert.AreEqual("countrysite3", cookieNameMember.Value);
    }

  }
}
