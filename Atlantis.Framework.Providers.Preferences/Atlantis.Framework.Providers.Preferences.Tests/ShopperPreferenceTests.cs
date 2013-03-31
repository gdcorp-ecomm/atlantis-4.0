using System.Web;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.Preferences;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Atlantis.Framework.Testing.MockHttpContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Testing.MockProviders;

namespace Atlantis.Framework.Providers.Preferences.Tests
{
  [TestClass]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  [DeploymentItem("Interop.gdMiniEncryptLib.dll")]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.ShopperPrefGet.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.ShopperPrefUpdate.Impl.dll")]
  public class ShopperPreferenceTests
  {
    private const string _CURRENCYPREFERENCEKEY = "gdshop_currencyType";
    private const string _FLAGPREFERENCEKEY = "countryFlag";
    private const string _DATACENTERPREFERENCKEY = "dataCenterCode";

    public ShopperPreferenceTests()
    {
      //
      // TODO: Add constructor logic here
      //
    }

    private TestContext testContextInstance;

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext
    {
      get
      {
        return testContextInstance;
      }
      set
      {
        testContextInstance = value;
      }
    }

    public void SetContext(string shopperId)
    {
      MockHttpRequest request = new MockHttpRequest("http://siteadmin.debug.intranet.gdg/default.aspx");
      MockHttpContext.SetFromWorkerRequest(request);
 
      HttpProviderContainer.Instance.RegisterProvider<ISiteContext, MockSiteContext>();
      HttpProviderContainer.Instance.RegisterProvider<IShopperContext, MockShopperContext>();
      HttpProviderContainer.Instance.RegisterProvider<IManagerContext, MockNoManagerContext>();
      HttpProviderContainer.Instance.RegisterProvider<IShopperPreferencesProvider, ShopperPreferencesProvider>();

      IShopperContext shopper = HttpProviderContainer.Instance.Resolve<IShopperContext>();
      shopper.SetNewShopper(shopperId);

    }

    // shopper 832652 currency USD
    private const string _S832652 = "rhdfdbphvhxbqbchwgjgmjoeohoevhrh";

    [TestMethod]
    public void GetPreferences()
    {
      SetContext("832652");

      IShopperPreferencesProvider prefs = HttpProviderContainer.Instance.Resolve<IShopperPreferencesProvider>();
      string currency = prefs.GetPreference(_CURRENCYPREFERENCEKEY, "blue");
      Assert.AreNotEqual("blue", currency);

    }

    [TestMethod]
    public void GetPreferencesFromCookie()
    {
      SetContext("832652");

      ISiteContext siteContext = HttpProviderContainer.Instance.Resolve<ISiteContext>();
      HttpCookie preferencesCookie = siteContext.NewCrossDomainMemCookie("preferences1");
      preferencesCookie["_sid"] = _S832652;
      preferencesCookie["garbage"] = "hello";
      preferencesCookie["countryFlag"] = "fr";
      preferencesCookie["gdshop_currencyType"] = "JPY";
      HttpContext.Current.Request.Cookies.Add(preferencesCookie);

      IShopperPreferencesProvider prefs = HttpProviderContainer.Instance.Resolve<IShopperPreferencesProvider>();
      string currency = prefs.GetPreference(_CURRENCYPREFERENCEKEY, "CHF");
      Assert.AreEqual("JPY", currency);

    }

    [TestMethod]
    public void GetPreferencesFromCookieNonShopperMatch()
    {
      SetContext("832999");

      ISiteContext siteContext = HttpProviderContainer.Instance.Resolve<ISiteContext>();
      HttpCookie preferencesCookie = siteContext.NewCrossDomainMemCookie("preferences1");
      preferencesCookie["_sid"] = _S832652;
      preferencesCookie["countryFlag"] = "fr";
      preferencesCookie["gdshop_currencyType"] = "JPY";
      HttpContext.Current.Request.Cookies.Add(preferencesCookie);

      IShopperPreferencesProvider prefs = HttpProviderContainer.Instance.Resolve<IShopperPreferencesProvider>();
      string currency = prefs.GetPreference(_CURRENCYPREFERENCEKEY, "NOT");
      Assert.AreEqual("NOT", currency);

    }

    [TestMethod]
    public void GetPreferencesFromCookieNonShopperMatchFromEmptyShopper()
    {
      SetContext("832653");

      ISiteContext siteContext = HttpProviderContainer.Instance.Resolve<ISiteContext>();
      HttpCookie preferencesCookie = siteContext.NewCrossDomainMemCookie("preferences1");
      preferencesCookie["_sid"] = string.Empty;
      preferencesCookie["countryFlag"] = "fr";
      preferencesCookie["gdshop_currencyType"] = "JPY";
      HttpContext.Current.Request.Cookies.Add(preferencesCookie);

      IShopperPreferencesProvider prefs = HttpProviderContainer.Instance.Resolve<IShopperPreferencesProvider>();
      string currency = prefs.GetPreference(_CURRENCYPREFERENCEKEY, "NOT");
      Assert.AreEqual("JPY", currency);

    }


    [TestMethod]
    public void UpdatePreference()
    {
      SetContext("832652");

      IShopperPreferencesProvider prefs = HttpProviderContainer.Instance.Resolve<IShopperPreferencesProvider>();
      prefs.UpdatePreference(_FLAGPREFERENCEKEY, "it");
    }

  }
}
