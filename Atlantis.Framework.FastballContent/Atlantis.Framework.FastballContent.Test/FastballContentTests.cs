using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Atlantis.Framework.FastballContent.Impl;
using Atlantis.Framework.FastballContent.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.Products;
using Atlantis.Framework.Providers.Interface.Currency;
using Atlantis.Framework.Providers.Currency;
using Atlantis.Framework.Providers.Interface.Links;
using Atlantis.Framework.Providers.Links;

namespace Atlantis.Framework.FastballContent.Test
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetFastballContentTests
  {

    private const string _shopperId = "";


    public GetFastballContentTests()
    {
      /// BB46XQSC1
      /// V2NCSB2
      /// sslxas100
      /// gd4915d
      MockHttpContext.SetMockHttpContext("default.aspx", "http://localhost/default.aspx&isc=gd4915d", "isc=gd4915d");
      HttpProviderContainer.Instance.RegisterProvider<ISiteContext, TestContexts>();
      HttpProviderContainer.Instance.RegisterProvider<IShopperContext, TestContexts>();
      HttpProviderContainer.Instance.RegisterProvider<IManagerContext, TestContexts>();
      HttpProviderContainer.Instance.RegisterProvider<ICurrencyProvider, CurrencyProvider>();
      HttpProviderContainer.Instance.RegisterProvider<ILinkProvider, LinkProvider>();
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

    [TestMethod]
    [DeploymentItem("Interop.gdMiniEncryptLib.dll")]
    [DeploymentItem("atlantis.config")]
    public void FastballContentTest()
    {
      string placement = "banner-iscBannerNoFormat";

      FastballContentRequestData request = new FastballContentRequestData(
        "832652", string.Empty, string.Empty, string.Empty, 0,
        2, placement, HttpProviderContainer.Instance);

      FastballContentResponseData response = (FastballContentResponseData)Engine.Engine.ProcessRequest(request, 359);
      Debug.WriteLine(response.ResponseData);
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
