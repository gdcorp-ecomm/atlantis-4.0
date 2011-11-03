﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.CDS.Tokenizer;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.Links;
using System.Web;
using Atlantis.Framework.Providers.Links;
using Atlantis.Framework.Providers.Interface.Products;
using Atlantis.Framework.Providers.Products;
using Atlantis.Framework.Providers.Interface.Currency;
using Atlantis.Framework.Providers.Currency;
using Atlantis.Framework.Testing.MockHttpContext;
using Helpers;
using System.Collections.Specialized;
using SiteConstants;

namespace Atlantis.Framework.CDS.Tests
{
  /// <summary>
  /// Summary description for TokenizerTests
  /// </summary>
  [TestClass]
  public class TokenizerTests
  {

    const bool DROPDECIMAL = true;
    const bool KEEPDECIMAL = false;

    public TokenizerTests()
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

      ISiteContext siteContext = HttpProviderContainer.Instance.Resolve<ISiteContext>();
      TestContexts testContexts = (TestContexts)siteContext;
      testContexts.SetContextInfo(privateLabelId, shopperId);

      IShopperContext shopperContext = HttpProviderContainer.Instance.Resolve<IShopperContext>();
      shopperContext.SetLoggedInShopper(shopperId);
    }

    [TestMethod]
    public void Tokenizer_Can_Replace_Links_Full_Relative()
    {
      //Arrange
      CDSTokenizer t = new CDSTokenizer();

      //Act
      var result = t.Parse("{{links::full::/default.aspx::false::ci|1234556|var2|true}}");
      var expected = Links.GetFullUrl("/default.aspx", QueryParamMode.ExplicitParameters, new NameValueCollection { { "ci", "1234556" }, { "var2", "true" } });

      //Assert
      Assert.IsNotNull(result);
      Assert.AreEqual(expected, result);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void Tokenizer_Can_Replace_LinkType_Secure()
    {
      //Arrange
      CDSTokenizer t = new CDSTokenizer();

      //Act
      var result = t.Parse("{{link::SITEURL::default.aspx::true::ci|1234556|var2|true}}");
      var expected = Links.GetUrl("SITEURL", "default.aspx", QueryParamMode.ExplicitParameters, true, new NameValueCollection { { "ci", "1234556" }, { "var2", "true" } });

      //Assert
      Assert.IsNotNull(result);
      Assert.AreEqual(expected, result);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void Tokenizer_Can_Replace_Links_Help()
    {
      //Arrange
      CDSTokenizer t = new CDSTokenizer();

      //Act
      var result = t.Parse("{{link::HELPURL::::false::ci|1234556|var2|true}}");
      var expected = Links.GetUrl("HELPURL", string.Empty, QueryParamMode.ExplicitParameters, false, new NameValueCollection { { "ci", "1234556" }, { "var2", "true" } });

      //Assert
      Assert.IsNotNull(result);
      Assert.AreEqual(expected, result);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void Tokenizer_Can_Replace_Links_External()
    {
      //Arrange
      CDSTokenizer t = new CDSTokenizer();

      //Act
      var result = t.Parse("{{link::TRAFFICREDIRECTPAGE::::false::ci|51600|target|https://chevrolet.promo.eprize.com/silverado/?affiliate_id=godaddy1}}");
      var expected = Links.GetUrl(LinkTypes.TrafficRedirectPage, string.Empty, QueryParamMode.ExplicitParameters, false, new NameValueCollection { { "ci", "51600" }, { "target", "https://chevrolet.promo.eprize.com/silverado/?affiliate_id=godaddy1" } });

      //Assert
      Assert.IsNotNull(result);
      Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Tokenizer_Can_Replace_Product_Description()
    {
      //Arrange
      CDSTokenizer t = new CDSTokenizer();

      //Act
      var result = t.Parse("{{product::101::description}}");
      var expected = ProductDescription(101);

      //Assert
      Assert.IsNotNull(result);
      Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Tokenizer_Can_Replace_Product_Yearly_Price()
    {
      //Arrange
      CDSTokenizer t = new CDSTokenizer();

      //Act
      var result = t.Parse("{{product::101::price::keepdecimal::yearly}}");
      var expected = GetYearlyPrice(101, false);

      //Assert
      Assert.IsNotNull(result);
      Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Tokenizer_Can_Replace_Product_Monthly_Price()
    {
      //Arrange
      CDSTokenizer t = new CDSTokenizer();

      //Act
      var result = t.Parse("{{product::775::price::keepdecimal::yearly}}");
      var expected = GetYearlyPrice(775, false);

      //Assert
      Assert.IsNotNull(result);
      Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Tokenizer_Can_Replace_Price_KeepDecimal()
    {
      //Arrange
      CDSTokenizer t = new CDSTokenizer();

      //Act
      var result = t.Parse("{{price::usd::1599::keepdecimal}}");
      var expected = GetTransactionalPriceFromUSD(1599, KEEPDECIMAL);

      //Assert
      Assert.IsNotNull(result);
      Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Tokenizer_Can_Replace_Price_DropDecimal()
    {
      //Arrange
      CDSTokenizer t = new CDSTokenizer();

      //Act
      var result = t.Parse("{{price::usd::1599::dropdecimal}}");
      var expected = GetTransactionalPriceFromUSD(1599, DROPDECIMAL);

      //Assert
      Assert.IsNotNull(result);
      Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Tokenizer_Can_Replace_QuickHelp()
    {
      //Arrange
      CDSTokenizer t = new CDSTokenizer();

      //Act
      var result = t.Parse("{{quickhelp::notifier::Desktop Notifier}}");
      var expected = "<span class=\"ndash\" onmouseover=\"atl_ShowQuickHelp(event, 'notifier');\" onmouseout=\"atl_HideQuickHelp();\">desktop notifier</span>";

      //Assert
      Assert.IsNotNull(result);
      Assert.AreEqual(expected, result);
    }

    #region Providers

    private ICurrencyProvider _currencyProvider;
    private ICurrencyProvider Currency
    {
      get
      {
        if (_currencyProvider == null)
          _currencyProvider = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();
        return _currencyProvider;
      }
    }

    private IProductProvider _productProvider;
    private IProductProvider Products
    {
      get
      {
        if (_productProvider == null)
          _productProvider = HttpProviderContainer.Instance.Resolve<IProductProvider>();
        return _productProvider;
      }
    }

    private ILinkProvider _links;
    protected ILinkProvider Links
    {
      get
      {
        if (_links == null)
        {
          _links = HttpProviderContainer.Instance.Resolve<ILinkProvider>();
        }
        return _links;
      }
    }

    //private ContentProvider _content;
    //protected ContentProvider Content
    //{
    //  get
    //  {
    //    if (_content == null)
    //    {
    //      _content = ContentProvider.GetProvider(SiteContext);
    //    }
    //    return _content;
    //  }
    //}



    #endregion

    #region Helper Items

    private string GetTransactionalPriceFromUSD(int price, bool dropDecimal)
    {
      return Currency.PriceText(CurrencyHelper.GetTransactionalPriceFromUSD(price), false, dropDecimal);
    }

    private string GetYearlyPrice(int productId, bool dropDecimal)
    {
      IProduct p = Products.GetProduct(productId);
      return Currency.PriceText(p.CurrentPrice, false, dropDecimal);
    }

    private string GetMonthlyPrice(int productId, bool dropDecimal)
    {
      IProductView p = Products.NewProductView(Products.GetProduct(productId));
      return Currency.PriceText(p.MonthlyCurrentPrice, false, dropDecimal);
    }

    private string ProductDescription(int productId)
    {
      IProduct p = Products.GetProduct(productId);
      return p.Info.Name;
    }

    #endregion


  }

  internal class TestContexts : ProviderBase, ISiteContext, IShopperContext, IManagerContext
  {
    int _privateLabelId = 1;
    string _shopperId = string.Empty;

    public TestContexts(IProviderContainer container)
      : base(container)
    {
    }

    public void SetContextInfo(int privateLabelId, string shopperId)
    {
      _privateLabelId = privateLabelId;
      _shopperId = shopperId;
    }

    #region ISiteContext Members

    public int ContextId
    {
      get
      {
        int result = 6;
        if (_privateLabelId == 2)
        {
          result = 5;
        }
        else if (_privateLabelId == 1)
        {
          result = 1;
        }
        else if (_privateLabelId == 1387)
        {
          result = 2;
        }
        return result;
      }
    }

    public string StyleId
    {
      get { return "0"; }
    }

    public int PrivateLabelId
    {
      get { return _privateLabelId; }
    }

    public string ProgId
    {
      get { return DataCache.DataCache.GetProgID(PrivateLabelId); }
    }

    public System.Web.HttpCookie NewCrossDomainCookie(string cookieName, DateTime expiration)
    {
      throw new NotImplementedException();
    }

    public System.Web.HttpCookie NewCrossDomainMemCookie(string cookieName)
    {
      throw new NotImplementedException();
    }

    public int PageCount
    {
      get { return 0; }
    }

    public string Pathway
    {
      get { return "UnitTest"; }
    }

    public string CI
    {
      get { return string.Empty; }
    }

    public string CommissionJunctionStartDate
    {
      get
      {
        return string.Empty;
      }
      set
      {
        return;
      }
    }

    public string ISC
    {
      get
      {
        string result = string.Empty;
        if (HttpContext.Current != null)
        {
          if (HttpContext.Current.Request.QueryString["ISC"] != null)
          {
            result = HttpContext.Current.Request.QueryString["ISC"];
          }
        }
        return result;
      }
    }

    public string CurrencyType
    {
      get
      {
        return "USD";
      }
    }

    public void SetCurrencyType(string currencyType)
    {
      return;
    }

    public bool IsRequestInternal
    {
      get
      {
        return true;
      }
    }

    public ServerLocationType ServerLocation
    {
      get { return ServerLocationType.Dev; }
    }

    public IManagerContext Manager
    {
      get { return this; }
    }

    #endregion

    #region IShopperContext Members

    public int ShopperPriceType
    {
      get { return 0; }
    }

    public string ShopperId
    {
      get { return _shopperId; }
    }

    public ShopperStatusType ShopperStatus
    {
      get { return ShopperStatusType.Public; }
    }

    public void ClearShopper()
    {
      _shopperId = string.Empty;
    }

    public bool SetLoggedInShopper(string shopperId)
    {
      _shopperId = shopperId;
      return true;
    }

    public bool SetLoggedInShopperWithCookieOverride(string shopperId)
    {
      _shopperId = shopperId;
      return true;
    }

    public void SetNewShopper(string shopperId)
    {
      _shopperId = shopperId;
    }

    #endregion

    #region IManagerContext Members

    public bool IsManager
    {
      get { return false; }
    }

    public string ManagerUserId
    {
      get { return string.Empty; }
    }

    public string ManagerUserName
    {
      get { return string.Empty; }
    }

    public System.Collections.Specialized.NameValueCollection ManagerQuery
    {
      get { return null; }
    }

    public string ManagerShopperId
    {
      get { return string.Empty; }
    }

    public int ManagerPrivateLabelId
    {
      get { return 0; }
    }

    public int ManagerContextId
    {
      get { return 0; }
    }

    #endregion
  }
}
