using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using Atlantis.Framework.CDS.Tokenizer;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Currency;
using Atlantis.Framework.Providers.Interface.Currency;
using Atlantis.Framework.Providers.Interface.Links;
using Atlantis.Framework.Providers.Interface.Products;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Atlantis.Framework.Providers.Links;
using Atlantis.Framework.Providers.Products;
using Atlantis.Framework.Testing.MockHttpContext;
using Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

    const bool MONTHLY = true;
    const bool YEARLY = false;

    const bool CURRENT_PRICE = true;
    const bool LIST_PRICE = false;

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

    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.LinkInfo.Impl.dll")]
    [TestMethod]
    public void Tokenizer_Can_Replace_Links_Full_Relative()
    {
      //Arrange
      CDSTokenizer t = new CDSTokenizer();

      //Act
      var result = t.Parse("{{link::full::/default.aspx::false::ci|1234556|var2|true}}");
      var expected = Links.GetFullUrl("/default.aspx", QueryParamMode.ExplicitParameters, new NameValueCollection { { "ci", "1234556" }, { "var2", "true" } });

      //Assert
      Assert.IsNotNull(result);
      Assert.AreEqual(expected, result);
    }

    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.LinkInfo.Impl.dll")]
    [TestMethod]
    public void Tokenizer_Invalid_Link_Type_Returns_Default()
    {
      //Arrange
      CDSTokenizer t = new CDSTokenizer();

      //Act
      var result = t.Parse("{{link::blah::/default.aspx::false::ci|1234556|var2|true}}");
      var expected = Links.GetFullUrl("/default.aspx", QueryParamMode.ExplicitParameters, new NameValueCollection { { "ci", "1234556" }, { "var2", "true" } });

      //Assert
      Assert.IsNotNull(result);
      Assert.AreEqual(expected, result);
    }

    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.LinkInfo.Impl.dll")]
    [TestMethod]
    public void Tokenizer_Malformed_Link_Token()
    {
      //Arrange
      CDSTokenizer t = new CDSTokenizer();

      //Act
      var result = t.Parse("{{link::full::/default.aspx::false::ci|1234556|var2|true");
      string expected = "{{link::full::/default.aspx::false::ci|1234556|var2|true";

      //Assert
      Assert.IsNotNull(result);
      Assert.AreEqual(expected, result);
    }

    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.LinkInfo.Impl.dll")]
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

    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.LinkInfo.Impl.dll")]
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

    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.LinkInfo.Impl.dll")]
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

    [DeploymentItem("atlantis.config")]
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
    public void Tokenizer_Can_Replace_Product_Package_Monthly_Current_Price()
    {
      //Arrange
      CDSTokenizer t = new CDSTokenizer();

      //Act
      var result = t.Parse("{{productpackage::759|807|3802|42002::monthly::price_current::keepdecimal}}");
      var expected = GetProductPackagePrice(new List<int> { 759, 807, 3802, 42002 }, MONTHLY, CURRENT_PRICE, KEEPDECIMAL);

      //Assert
      Assert.IsNotNull(result);
      Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Tokenizer_Can_Replace_Product_Package_Monthly_List_Price()
    {
      //Arrange
      CDSTokenizer t = new CDSTokenizer();

      //Act
      var result = t.Parse("{{productpackage::759|807|3802|42002::monthly::price_list::keepdecimal}}");
      var expected = GetProductPackagePrice(new List<int> { 759, 807, 3802, 42002 }, MONTHLY, LIST_PRICE, KEEPDECIMAL);

      //Assert
      Assert.IsNotNull(result);
      Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Tokenizer_Can_Replace_Product_Package_Yearly_Current_Price()
    {
      //Arrange
      CDSTokenizer t = new CDSTokenizer();

      //Act
      var result = t.Parse("{{productpackage::759|807|3802|42002::yearly::price_current::keepdecimal}}");
      var expected = GetProductPackagePrice(new List<int> { 759, 807, 3802, 42002 }, YEARLY, CURRENT_PRICE, KEEPDECIMAL);

      //Assert
      Assert.IsNotNull(result);
      Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Tokenizer_Can_Replace_Product_Package_Yearly_List_Price()
    {
      //Arrange
      CDSTokenizer t = new CDSTokenizer();

      //Act
      var result = t.Parse("{{productpackage::759|807|3802|42002::yearly::price_list::keepdecimal}}");
      var expected = GetProductPackagePrice(new List<int> { 759, 807, 3802, 42002 }, YEARLY, LIST_PRICE, KEEPDECIMAL);

      //Assert
      Assert.IsNotNull(result);
      Assert.AreEqual(expected, result);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidProgramException))]
    public void Tokenizer_Invalid_Product_Pricing_Token_Exception()
    {
      //Arrange
      CDSTokenizer t = new CDSTokenizer();

      //Act
      var result = t.Parse("{{product::42002::poop::keepdecimal::yearly}}");

      //Assert
      //System.InvalidProgramException: The operator poop was not in the list of valid operators.
    }

    [TestMethod]
    public void Tokenizer_Can_Replace_Product_Current_Price_Yearly()
    {
      //Arrange
      CDSTokenizer t = new CDSTokenizer();

      //Act
      var result = t.Parse("{{product::42002::price::keepdecimal::yearly}}");
      var expected = GetProductYearlyCurrentPrice(42002, false);

      //Assert
      Assert.IsNotNull(result);
      Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Tokenizer_Can_Replace_Product_Current_Price_Monthly()
    {
      //Arrange
      CDSTokenizer t = new CDSTokenizer();

      //Act
      var result = t.Parse("{{product::42002::price::keepdecimal::monthly}}");
      var expected = GetProductMonthlyCurrentPrice(42002, false);

      //Assert
      Assert.IsNotNull(result);
      Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Tokenizer_Can_Replace_Product_Current_Price_Monthly_New()
    {
      //Arrange
      CDSTokenizer t = new CDSTokenizer();

      //Act
      var result = t.Parse("{{product::42002::price_current::keepdecimal::monthly}}");
      var expected = GetProductMonthlyCurrentPrice(42002, false);

      //Assert
      Assert.IsNotNull(result);
      Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Tokenizer_Can_Replace_Product_List_Price_Monthly()
    {
      //Arrange
      CDSTokenizer t = new CDSTokenizer();

      //Act
      var result = t.Parse("{{product::42002::price_list::keepdecimal::monthly}}");
      var expected = GetProductMonthlyListPrice(42002, false);

      //Assert
      Assert.IsNotNull(result);
      Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Tokenizer_Can_Replace_Product_List_Price_Yearly()
    {
      //Arrange
      CDSTokenizer t = new CDSTokenizer();

      //Act
      var result = t.Parse("{{product::42002::price_list::keepdecimal::yearly}}");
      var expected = GetProductYearlyListPrice(42002, false);

      //Assert
      Assert.IsNotNull(result);
      Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Tokenizer_Can_Replace_Product_SavingsPercentage()
    {
      //Arrange
      CDSTokenizer t = new CDSTokenizer();

      //Act
      var result = t.Parse("{{product::12001::savingspercentage}}");
      var expected = SavingsPercentage(12001);

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
      var result = t.Parse("{{quickhelp::100::POP3}}");
      var expected = "<span data-qh=\\\"100\\\">POP3</span>";

      //Assert
      Assert.IsNotNull(result);
      Assert.AreEqual(expected, result);
    }


    [TestMethod]
    public void Tokenizer_Link_Null_QueryString_Parameter()
    {
      //Arrange
      CDSTokenizer t = new CDSTokenizer();

      //Act
      var result = t.Parse("{{link::full::/default.aspx::false::|1234556|var2|true}}");
      var expected = Links.GetFullUrl("/default.aspx", QueryParamMode.ExplicitParameters, new NameValueCollection { { null, "1234556" }, { "var2", "true" } });

      //Assert
      Assert.IsNotNull(result);
      Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Tokenizer_CompanyName()
    {
      //Arrange
      CDSTokenizer t = new CDSTokenizer();

      //Act
      var result = t.Parse("{{companyname}}", CustomTokens);
      var expected = "Go Daddy";

      //Assert
      Assert.IsNotNull(result);
      Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Tokenizer_CompanyDotComName()
    {
      //Arrange
      CDSTokenizer t = new CDSTokenizer();

      //Act
      var result = t.Parse("{{companydotcomname}}", CustomTokens);
      var expected = "GoDaddy.com";

      //Assert
      Assert.IsNotNull(result);
      Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Tokenizer_StyleId()
    {
      //Arrange
      CDSTokenizer t = new CDSTokenizer();

      //Act
      var result = t.Parse("{{styleid}}", CustomTokens);
      var expected = "1";

      //Assert
      Assert.IsNotNull(result);
      Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Tokenizer_ImageRoot()
    {
      //Arrange
      CDSTokenizer t = new CDSTokenizer();

      //Act
      var result = t.Parse("{{imageroot}}", CustomTokens);
      var expected = Links.ImageRoot;

      //Assert
      Assert.IsNotNull(result);
      Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Tokenizer_ICANNFee()
    {
      //Arrange
      CDSTokenizer t = new CDSTokenizer();

      //Act
      var result = t.Parse("{{icannfee}}", CustomTokens);
      var expected = "18¢/yr";

      //Assert
      Assert.IsNotNull(result);
      Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Tokenizer_Negative_Unknown_WithCustom()
    {
      //Arrange
      CDSTokenizer t = new CDSTokenizer();

      //Act
      var result = t.Parse("{{Bogus}}", CustomTokens);
      var expected = "[ERROR, Unhandled Token: {{Bogus}}]";

      //Assert
      Assert.IsNotNull(result);
      Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Tokenizer_Negative_Unknown()
    {
      //Arrange
      CDSTokenizer t = new CDSTokenizer();

      //Act
      var result = t.Parse("{{Bogus}}");
      var expected = "[ERROR, Unhandled Token: {{Bogus}}]";

      //Assert
      Assert.IsNotNull(result);
      Assert.AreEqual(expected, result);
    }

    [ExpectedException(typeof(InvalidProgramException), "The Link Type  was invalid.")] 
    [TestMethod]
    public void Tokenizer_Negative_Unknown_InvalidArgument()
    {
      //Arrange
      CDSTokenizer t = new CDSTokenizer();

      //Act
      var result = t.Parse("{{link::}}");

      //no need for Assert, will throw:
      //the first part is recognized as a token {{link but not as a multipart token, which is what the link token is
    }

    [ExpectedException(typeof(InvalidProgramException), "The quick help item  was not found.")]
    [TestMethod]
    public void Tokenizer_Negative_Unknown_NotFound()
    {
      //Arrange
      CDSTokenizer t = new CDSTokenizer();

      //Act
      var result = t.Parse("{{quickhelp::}}");

      //no need for Assert, will throw:
    }

    [ExpectedException(typeof(ArgumentOutOfRangeException), "Index was out of range. Must be non-negative and less than the size of the collection.")]
    [TestMethod]
    public void Tokenizer_Negative_Unknown_InvalidArgument2()
    {
      //Arrange
      CDSTokenizer t = new CDSTokenizer();

      //Act
      var result = t.Parse("{{quickhelp::crap}}");

      //no need for Assert, will throw:
    }

    [TestMethod]
    public void Tokenizer_Negative_Unknown_InvalidToken()
    {
      //Arrange
      CDSTokenizer t = new CDSTokenizer();

      //Act
      var result = t.Parse("{{link]]");

      // no replacement will occur, not recognized as a token
      var expected = "{{link]]";

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

    private string GetProductYearlyCurrentPrice(int productId, bool dropDecimal)
    {
      IProduct p = Products.GetProduct(productId);
      return Currency.PriceText(p.CurrentPrice, false, dropDecimal);
    }

    private string GetProductYearlyListPrice(int productId, bool dropDecimal)
    {
      IProduct p = Products.GetProduct(productId);
      return Currency.PriceText(p.ListPrice, false, dropDecimal);
    }

    private string GetProductMonthlyCurrentPrice(int productId, bool dropDecimal)
    {
      IProductView p = Products.NewProductView(Products.GetProduct(productId));
      return Currency.PriceText(p.MonthlyCurrentPrice, false, dropDecimal);
    }

    private string GetProductMonthlyListPrice(int productId, bool dropDecimal)
    {
      IProductView p = Products.NewProductView(Products.GetProduct(productId));
      return Currency.PriceText(p.MonthlyListPrice, false, dropDecimal);
    }
    private string ProductDescription(int productId)
    {
      IProduct p = Products.GetProduct(productId);
      return p.Info.Name;
    }

    private string GetProductPackagePrice(List<int> pfids, bool monthly, bool current, bool dropDecimal)
    {
      IProductView pv;
      ICurrencyPrice price;
      int totalPrice = 0;

      foreach (int pfid in pfids)
      {
        pv = Products.NewProductView(Products.GetProduct(pfid));
        if (current)
        {
          totalPrice = totalPrice + (monthly ? pv.MonthlyCurrentPrice.Price : pv.YearlyCurrentPrice.Price);
        }
        else // list
        {
          totalPrice = totalPrice + (monthly ? pv.MonthlyListPrice.Price : pv.YearlyListPrice.Price);
        }
      }
      price = new CurrencyPrice(totalPrice, Currency.SelectedTransactionalCurrencyInfo, CurrencyPriceType.Transactional);
      return Currency.PriceText(price, false, dropDecimal);
    }

    private string SavingsPercentage(int productId)
    {
      IProduct product = Products.GetProduct(productId);
      IProductView pView = Products.NewProductView(product);
      pView.CalculateSavings(product);
      return pView.SavingsPercentage.ToString();
    }

    private Dictionary<string, string> customTokens = null;
    private Dictionary<string, string> CustomTokens
    {
      get
      {
        if (customTokens == null)
        {
          customTokens = GetCustomTokens();
        }
        return customTokens;
      }
    }

    private Dictionary<string, string> GetCustomTokens()
    {
      Dictionary<string, string> tokenList = new Dictionary<string, string>();

      tokenList.Add("{{companyname}}", "Go Daddy");
      tokenList.Add("{{companydotcomname}}", "GoDaddy.com");
      tokenList.Add("{{styleid}}", "1");
      tokenList.Add("{{imageroot}}", Links.ImageRoot);
      tokenList.Add("{{icannfee}}", "18¢/yr");

      return tokenList;
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
