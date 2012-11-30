using Atlantis.Framework.Providers.CDS;
using Atlantis.Framework.Providers.Interface.CDS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Atlantis.Framework.Providers.Interface.Links;
using Atlantis.Framework.Providers.Interface.Products;
using Atlantis.Framework.Providers.Interface.Currency;
using Atlantis.Framework.Providers.Links;
using Atlantis.Framework.Providers.Products;
using Atlantis.Framework.Providers.Currency;
using JsonCheckerTool;

namespace Atlantis.Framework.CDS.Tests
{
  /// <summary>
  /// Summary description for ProviderTests
  /// </summary>
  [TestClass]
  public class ProviderTests
  {
    public ProviderTests()
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

    public class PageData
    {
      public string Stuff { get; set; }
      public string Noise { get; set; }
    }

    [TestInitialize]
    public void InitializeTests()
    {
      var privateLabelId = 1;
      var shopperId = string.Empty;
      MockHttpContext.SetMockHttpContext("default.aspx", "http://www.debug.godaddy-com.ide/", string.Empty);
      HttpProviderContainer.Instance.RegisterProvider<ISiteContext, TestContexts>();
      HttpProviderContainer.Instance.RegisterProvider<IShopperContext, TestContexts>();
      HttpProviderContainer.Instance.RegisterProvider<IManagerContext, TestContexts>();
      HttpProviderContainer.Instance.RegisterProvider<ILinkProvider, LinkProvider>();
      HttpProviderContainer.Instance.RegisterProvider<IProductProvider, ProductProvider>();
      HttpProviderContainer.Instance.RegisterProvider<ICurrencyProvider, CurrencyProvider>();
      HttpProviderContainer.Instance.RegisterProvider<ICDSProvider, CDSProvider>();

      ISiteContext siteContext = HttpProviderContainer.Instance.Resolve<ISiteContext>();
      TestContexts testContexts = (TestContexts)siteContext;
      testContexts.SetContextInfo(privateLabelId, shopperId);

      IShopperContext shopperContext = HttpProviderContainer.Instance.Resolve<IShopperContext>();
      shopperContext.SetLoggedInShopper(shopperId);
    }

    [TestMethod]
    public void Provider_Calls_The_triplet()
    {

      //Arrange

      //Act
      ICDSProvider provider = HttpProviderContainer.Instance.Resolve<ICDSProvider>();
      PageData model = provider.GetModel<PageData>("en/sales/1/domainaddon/domain-backorders.aspx");

      //Assert
      Assert.IsNotNull(model);      
    }

    [TestMethod]
    public void Provider_Returns_Blank_Model_With_404()
    {

        //Arrange

        //Act
        ICDSProvider provider = HttpProviderContainer.Instance.Resolve<ICDSProvider>();
        PageData model = provider.GetModel<PageData>("sales/1/danica");

        //Assert
        Assert.IsNull(model);
    }

    [TestMethod]
    public void Provider_Deserializes_Json()
    {

      //Arrange


      //Act
      ICDSProvider provider = HttpProviderContainer.Instance.Resolve<ICDSProvider>();
      var data = provider.GetJSON("gdtv/celebs/leeann-dearing/",null);
      PageData model = provider.GetModel<PageData>("gdtv/celebs/leeann-dearing/");

      //Assert
      Assert.IsNotNull(data);
      Assert.IsNotNull(model);
      Assert.AreEqual("this is stuff", model.Stuff);
      Assert.AreEqual("this is noise", model.Noise);
    }
    [TestMethod]
    public void Provider_Homepage_ES_test()
    {
      //Arrange
      ICDSProvider provider = HttpProviderContainer.Instance.Resolve<ICDSProvider>();

      //Act
      var data = provider.GetJSON("content/es/sales/1/homepage/new/default", null);

      //Assert
      Assert.IsNotNull(data);
      Assert.AreNotEqual(data, string.Empty);
    }

    [TestMethod]
    public void Validate_JSON_From_CDS_Valid()
    {
      //Arrange

      //Act
      ICDSProvider provider = HttpProviderContainer.Instance.Resolve<ICDSProvider>();
      var data = provider.GetJSON("sales/1/lp/email", null);

      //Assert
      Assert.IsTrue(JSONValidator.Validate(data));
    }

    [TestMethod]
    public void Validate_JSON_From_CDS_InValid()
    {
      //Arrange

      //Act
      ICDSProvider provider = HttpProviderContainer.Instance.Resolve<ICDSProvider>();
      var data = provider.GetJSON("test/invalid/PoorlyFormed", null);

      //Assert
      Assert.IsFalse(JSONValidator.Validate(data));
    }
  }
}
