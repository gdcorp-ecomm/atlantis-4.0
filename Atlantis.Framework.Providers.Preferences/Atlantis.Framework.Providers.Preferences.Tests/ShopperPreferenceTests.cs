using Atlantis.Framework.Interface;
using Atlantis.Framework.MiniEncrypt;
using Atlantis.Framework.Providers.Interface.Preferences;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Web;

namespace Atlantis.Framework.Providers.Preferences.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.ShopperPrefGet.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.ShopperPrefUpdate.Impl.dll")]
  public class ShopperPreferenceTests
  {
    private const string _CURRENCYPREFERENCEKEY = "gdshop_currencyType";
    private const string _DATACENTERPREFERENCKEY = "dataCenterCode";
    private const string _NEWCURRENCYPREFERENCEKEY = "currency";

    public IProviderContainer SetContext(string shopperId)
    {
      MockHttpRequest request = new MockHttpRequest("http://siteadmin.debug.intranet.gdg/default.aspx");
      MockHttpContext.SetFromWorkerRequest(request);

      var result = new MockProviderContainer();
      result.RegisterProvider<ISiteContext, MockSiteContext>();
      result.RegisterProvider<IShopperContext, MockShopperContext>();
      result.RegisterProvider<IManagerContext, MockNoManagerContext>();
      result.RegisterProvider<IShopperPreferencesProvider, ShopperPreferencesProvider>();

      IShopperContext shopper = result.Resolve<IShopperContext>();
      shopper.SetNewShopper(shopperId);

      return result;
    }

    [TestMethod]
    public void GetPreferences()
    {
      IProviderContainer result = SetContext("832652");

      IShopperPreferencesProvider prefs = result.Resolve<IShopperPreferencesProvider>();
      string currency = prefs.GetPreference(_CURRENCYPREFERENCEKEY, "blue");
      Assert.AreEqual("blue", currency);
    }

    [TestMethod]
    public void GetPreferencesFromCookieNew()
    {
      IProviderContainer result = SetContext("832652");

      ISiteContext siteContext = result.Resolve<ISiteContext>();
      HttpCookie preferencesCookie = siteContext.NewCrossDomainMemCookie("preferences");
      preferencesCookie["garbage"] = "hello";
      preferencesCookie["currency"] = "JPY";
      HttpContext.Current.Request.Cookies.Add(preferencesCookie);

      IShopperPreferencesProvider prefs = result.Resolve<IShopperPreferencesProvider>();
      string currency = prefs.GetPreference(_CURRENCYPREFERENCEKEY, "CHF");
      Assert.AreEqual("JPY", currency);
    }

    [TestMethod]
    public void ShopperIdIgnoredFromCookie()
    {
      IProviderContainer result = SetContext("832652");

      ISiteContext siteContext = result.Resolve<ISiteContext>();
      HttpCookie preferencesCookie = siteContext.NewCrossDomainMemCookie("preferences");
      preferencesCookie["_sid"] = "hello";
      HttpContext.Current.Request.Cookies.Add(preferencesCookie);

      IShopperPreferencesProvider prefs = result.Resolve<IShopperPreferencesProvider>();
      Assert.IsFalse(prefs.HasPreference("_sid"));
    }

    [TestMethod]
    public void BadCookieIgnored()
    {
      IProviderContainer result = SetContext("832652");

      ISiteContext siteContext = result.Resolve<ISiteContext>();
      HttpCookie preferencesCookie = siteContext.NewCrossDomainMemCookie("preferences");
      preferencesCookie.Value = "hello";
      HttpContext.Current.Request.Cookies.Add(preferencesCookie);

      HttpCookie preferencesCookie1 = siteContext.NewCrossDomainMemCookie("preferences1");
      preferencesCookie1["garbage"] = "goodbye";
      HttpContext.Current.Request.Cookies.Add(preferencesCookie1);

      IShopperPreferencesProvider prefs = result.Resolve<IShopperPreferencesProvider>();
      var test = prefs.GetPreference("garbage", "sianora");
      Assert.AreEqual("goodbye", test);
    }

    [TestMethod]
    public void NewCookieOverOldCookie()
    {
      IProviderContainer result = SetContext("832652");

      ISiteContext siteContext = result.Resolve<ISiteContext>();
      HttpCookie preferencesCookie = siteContext.NewCrossDomainMemCookie("preferences");
      preferencesCookie.Values["garbage"] = "hello";
      HttpContext.Current.Request.Cookies.Add(preferencesCookie);

      HttpCookie preferencesCookie1 = siteContext.NewCrossDomainMemCookie("preferences1");
      preferencesCookie1["garbage"] = "goodbye";
      HttpContext.Current.Request.Cookies.Add(preferencesCookie1);

      IShopperPreferencesProvider prefs = result.Resolve<IShopperPreferencesProvider>();
      var test = prefs.GetPreference("garbage", "sianora");
      Assert.AreEqual("hello", test);
    }

    [TestMethod]
    public void GetPreferencesFromCookie()
    {
      IProviderContainer result = SetContext("832652");

      ISiteContext siteContext = result.Resolve<ISiteContext>();
      HttpCookie preferencesCookie = siteContext.NewCrossDomainMemCookie("preferences1");
      preferencesCookie["garbage"] = "hello";
      preferencesCookie["countryFlag"] = "fr";
      preferencesCookie["gdshop_currencyType"] = "JPY";
      HttpContext.Current.Request.Cookies.Add(preferencesCookie);

      IShopperPreferencesProvider prefs = result.Resolve<IShopperPreferencesProvider>();
      string currency = prefs.GetPreference(_CURRENCYPREFERENCEKEY, "CHF");
      Assert.AreEqual("JPY", currency);

    }

    [TestMethod]
    public void UpdatePreference()
    {
      IProviderContainer result = SetContext("832652");

      IShopperPreferencesProvider prefs = result.Resolve<IShopperPreferencesProvider>();
      prefs.UpdatePreference("unittest", "success");
      prefs.UpdatePreference("currency", "CHF");

      HttpCookie newCookie = HttpContext.Current.Response.Cookies["preferences"];
      HttpCookie oldCookie = HttpContext.Current.Response.Cookies["preferences1"];

      Assert.IsNotNull(newCookie);
      Assert.IsNotNull(oldCookie);

      Assert.AreEqual("success", newCookie.Values["unittest"]);
      Assert.AreEqual("success", oldCookie.Values["unittest"]);
      Assert.AreEqual("CHF", newCookie.Values["currency"]);
      Assert.AreEqual("CHF", oldCookie.Values["gdshop_currencyType"]);

      string encryptedShopperId = oldCookie.Values["_sid"];
      using (var cookieEncryption = CookieEncryption.CreateDisposable())
      {
        string shopperId;
        Assert.IsTrue(cookieEncryption.TryDecrypteCookieValue(encryptedShopperId, out shopperId));
        Assert.AreEqual("832652", shopperId);
      }
    }

    [TestMethod]
    public void UpdatePreferenceMultiple()
    {
      IProviderContainer result = SetContext("832652");

      IShopperPreferencesProvider prefs = result.Resolve<IShopperPreferencesProvider>();
      Dictionary<string, string> updates = new Dictionary<string, string>();
      updates["currency"] = "CHF";
      updates["unittest"] = "success";
      prefs.UpdatePreferences(updates);

      HttpCookie newCookie = HttpContext.Current.Response.Cookies["preferences"];
      HttpCookie oldCookie = HttpContext.Current.Response.Cookies["preferences1"];

      Assert.IsNotNull(newCookie);
      Assert.IsNotNull(oldCookie);

      Assert.AreEqual("success", newCookie.Values["unittest"]);
      Assert.AreEqual("success", oldCookie.Values["unittest"]);
      Assert.AreEqual("CHF", newCookie.Values["currency"]);
      Assert.AreEqual("CHF", oldCookie.Values["gdshop_currencyType"]);
    }

    [TestMethod]
    public void UpdateNoChange()
    {
      IProviderContainer result = SetContext("832652");

      ISiteContext siteContext = result.Resolve<ISiteContext>();
      HttpCookie preferencesCookie = siteContext.NewCrossDomainMemCookie("preferences");
      preferencesCookie["currency"] = "JPY";
      HttpContext.Current.Request.Cookies.Add(preferencesCookie);

      IShopperPreferencesProvider prefs = result.Resolve<IShopperPreferencesProvider>();
      prefs.UpdatePreference("currency", "JPY");

      bool found = false;
      foreach (string key in HttpContext.Current.Response.Cookies.AllKeys)
      {
        if (("preferences".Equals(key)) || ("preferences1".Equals(key)))
        {
          found = true;
          break;
        }
      }

      Assert.IsFalse(found);
    }

    [TestMethod]
    public void HasPreference()
    {
      IProviderContainer result = SetContext("832652");

      ISiteContext siteContext = result.Resolve<ISiteContext>();
      HttpCookie preferencesCookie = siteContext.NewCrossDomainMemCookie("preferences");
      preferencesCookie["garbage"] = "hello";
      preferencesCookie["currency"] = "JPY";
      HttpContext.Current.Request.Cookies.Add(preferencesCookie);

      IShopperPreferencesProvider prefs = result.Resolve<IShopperPreferencesProvider>();
      Assert.IsTrue(prefs.HasPreference("garBAGE"));
    }

    [TestMethod]
    public void SaveAllPreferencesDoesNothing()
    {
      IProviderContainer result = SetContext("832652");

      ISiteContext siteContext = result.Resolve<ISiteContext>();
      HttpCookie preferencesCookie = siteContext.NewCrossDomainMemCookie("preferences");
      preferencesCookie["currency"] = "JPY";
      HttpContext.Current.Request.Cookies.Add(preferencesCookie);

      IShopperPreferencesProvider prefs = result.Resolve<IShopperPreferencesProvider>();
      prefs.SaveAllPreferencesToDatabase();

      bool found = false;
      foreach (string key in HttpContext.Current.Response.Cookies.AllKeys)
      {
        if (("preferences".Equals(key)) || ("preferences1".Equals(key)))
        {
          found = true;
          break;
        }
      }

      Assert.IsFalse(found);
    }

    [TestMethod]
    public void NoHttpContext()
    {
      IProviderContainer result = SetContext("832652");
      HttpContext.Current = null;

      IShopperPreferencesProvider prefs = result.Resolve<IShopperPreferencesProvider>();
      prefs.UpdatePreference("unittest", "myvalue");
      Assert.IsTrue(prefs.HasPreference("unittest"));
    }
  }
}
