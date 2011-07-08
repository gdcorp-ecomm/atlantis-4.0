using System.Web;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.Preferences;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Atlantis.Framework.Testing.MockHttpContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.Preferences.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
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

    [TestInitialize]
    public void InitializeTests()
    {
      HttpProviderContainer.Instance.RegisterProvider<ISiteContext, TestContexts>();
      HttpProviderContainer.Instance.RegisterProvider<IShopperContext, TestContexts>();
      HttpProviderContainer.Instance.RegisterProvider<IManagerContext, TestContexts>();
      HttpProviderContainer.Instance.RegisterProvider<IShopperPreferencesProvider, ShopperPreferencesProvider>();
    }

    #region Additional test attributes
    //
    // You can use the following additional attributes as you write your tests:
    //
    // Use ClassInitialize to run code before running the first test in the class
    // [ClassInitialize()]
    // public static void MyClassInitialize(TestContext testContext) { }
    //
    // Use ClassCleanup to run code after all tests in a class have run
    // [ClassCleanup()]
    // public static void MyClassCleanup() { }
    //
    // Use TestInitialize to run code before running each test 
    // [TestInitialize()]
    // public void MyTestInitialize() { }
    //
    // Use TestCleanup to run code after each test has run
    // [TestCleanup()]
    // public void MyTestCleanup() { }
    //
    #endregion

    #region Test Cookie ShopperIds

    // shopper 832652 currency USD
    private const string _S832652 = "rhdfdbphvhxbqbchwgjgmjoeohoevhrh";

    #endregion

    [TestMethod]
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    [DeploymentItem("Interop.gdMiniEncryptLib.dll")]
    [DeploymentItem("atlantis.config")]
    public void GetPreferences()
    {
      MockHttpContext.SetMockHttpContext("default.aspx", "http://siteadmin.debug.intranet.gdg/default.aspx", string.Empty);
      IShopperContext testContexts = HttpProviderContainer.Instance.Resolve<IShopperContext>();
      ((TestContexts)testContexts).SetContextInfo(1, "832652");

      IShopperPreferencesProvider prefs = HttpProviderContainer.Instance.Resolve<IShopperPreferencesProvider>();
      string currency = prefs.GetPreference(_CURRENCYPREFERENCEKEY, "blue");
      Assert.AreNotEqual("blue", currency);

    }

    [TestMethod]
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    [DeploymentItem("Interop.gdMiniEncryptLib.dll")]
    [DeploymentItem("atlantis.config")]
    public void GetPreferencesFromCookie()
    {
      MockHttpContext.SetMockHttpContext("default.aspx", "http://siteadmin.debug.intranet.gdg/default.aspx", string.Empty);
      IShopperContext testContexts = HttpProviderContainer.Instance.Resolve<IShopperContext>();
      ((TestContexts)testContexts).SetContextInfo(1, "832652");

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
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    [DeploymentItem("Interop.gdMiniEncryptLib.dll")]
    [DeploymentItem("atlantis.config")]
    public void GetPreferencesFromCookieNonShopperMatch()
    {
      MockHttpContext.SetMockHttpContext("default.aspx", "http://siteadmin.debug.intranet.gdg/default.aspx", string.Empty);
      IShopperContext testContexts = HttpProviderContainer.Instance.Resolve<IShopperContext>();
      ((TestContexts)testContexts).SetContextInfo(1, "832999");

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
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    [DeploymentItem("Interop.gdMiniEncryptLib.dll")]
    [DeploymentItem("atlantis.config")]
    public void GetPreferencesFromCookieNonShopperMatchFromEmptyShopper()
    {
      MockHttpContext.SetMockHttpContext("default.aspx", "http://siteadmin.debug.intranet.gdg/default.aspx", string.Empty);
      IShopperContext testContexts = HttpProviderContainer.Instance.Resolve<IShopperContext>();
      ((TestContexts)testContexts).SetContextInfo(1, "832653");

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
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    [DeploymentItem("Interop.gdMiniEncryptLib.dll")]
    [DeploymentItem("atlantis.config")]
    public void UpdatePreference()
    {
      MockHttpContext.SetMockHttpContext("default.aspx", "http://siteadmin.debug.intranet.gdg/default.aspx", string.Empty);
      IShopperContext testContexts = HttpProviderContainer.Instance.Resolve<IShopperContext>();
      ((TestContexts)testContexts).SetContextInfo(1, "832652");

      IShopperPreferencesProvider prefs = HttpProviderContainer.Instance.Resolve<IShopperPreferencesProvider>();
      prefs.UpdatePreference(_FLAGPREFERENCEKEY, "it");

    }

  }
}
