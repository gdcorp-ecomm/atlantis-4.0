using Atlantis.Framework.Providers.CDS;
using Atlantis.Framework.Providers.Interface.CDS;
using Atlantis.Framework.Providers.Interface.Currency;
using Atlantis.Framework.Providers.Interface.Links;
using Atlantis.Framework.Providers.Interface.Products;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
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

    public class PageData
    {
      public string Stuff { get; set; }
      public string Noise { get; set; }
    }

    private IProviderContainer _providerContainer;
    private IProviderContainer ProviderContainer
    {
      get
      {
        if (_providerContainer == null)
        {
          _providerContainer = new MockProviderContainer();
          _providerContainer.RegisterProvider<ISiteContext, TestContexts>();
          _providerContainer.RegisterProvider<IShopperContext, TestContexts>();
          _providerContainer.RegisterProvider<IManagerContext, TestContexts>();
          _providerContainer.RegisterProvider<ILinkProvider, LinkProvider>();
          _providerContainer.RegisterProvider<IProductProvider, ProductProvider>();
          _providerContainer.RegisterProvider<ICurrencyProvider, CurrencyProvider>();
          _providerContainer.RegisterProvider<ICDSProvider, CDSProvider>();
        }

        return _providerContainer;
      }
    }

    [TestInitialize]
    public void InitializeTests()
    {
      var privateLabelId = 1;
      var shopperId = string.Empty;
      MockHttpContext.SetMockHttpContext("default.aspx", "http://www.debug.godaddy-com.ide/", string.Empty);
      

      

      ISiteContext siteContext = ProviderContainer.Resolve<ISiteContext>();
      TestContexts testContexts = (TestContexts)siteContext;
      testContexts.SetContextInfo(privateLabelId, shopperId);

      IShopperContext shopperContext = ProviderContainer.Resolve<IShopperContext>();
      shopperContext.SetLoggedInShopper(shopperId);
    }

    [TestMethod]
    public void Provider_Calls_The_triplet()
    {

      //Arrange

      //Act
      ICDSProvider provider = ProviderContainer.Resolve<ICDSProvider>();
      PageData model = provider.GetModel<PageData>("en/sales/1/domainaddon/domain-backorders.aspx", null);

      //Assert
      Assert.IsNotNull(model);      
    }

    [TestMethod]
    public void Provider_Returns_Blank_Model_With_404()
    {

        //Arrange

        //Act
        ICDSProvider provider = ProviderContainer.Resolve<ICDSProvider>();
        PageData model = provider.GetModel<PageData>("sales/1/danica", null);

        //Assert
        Assert.IsNull(model);
    }

    [TestMethod]
    public void Provider_Deserializes_Json()
    {

      //Arrange


      //Act
      ICDSProvider provider = ProviderContainer.Resolve<ICDSProvider>();
      var data = provider.GetJson("gdtv/celebs/leeann-dearing/",null);
      PageData model = provider.GetModel<PageData>("gdtv/celebs/leeann-dearing/", null);

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
      ICDSProvider provider = ProviderContainer.Resolve<ICDSProvider>();

      //Act
      var data = provider.GetJson("content/es/sales/1/homepage/new/magic", null);

      //Assert
      Assert.IsNotNull(data);
      Assert.AreNotEqual(data, string.Empty);
    }

    [TestMethod]
    public void Validate_JSON_From_CDS_Valid()
    {
      //Arrange

      //Act
      ICDSProvider provider = ProviderContainer.Resolve<ICDSProvider>();
      var data = provider.GetJson("content/es/sales/1/homepage/new/default", null);

      //Assert
      Assert.IsTrue(JSONValidator.Validate(data));
    }

    [TestMethod]
    public void Validate_JSON_From_CDS_InValid()
    {
      //Arrange

      //Act
      ICDSProvider provider = ProviderContainer.Resolve<ICDSProvider>();
      var data = provider.GetJson("test/invalid/PoorlyFormed", null);

      //Assert
      Assert.IsFalse(JSONValidator.Validate(data));
    }

  }
}
