using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Tokens.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Testing.MockProviders;
using Atlantis.Framework.Providers.Interface.Currency;
using Atlantis.Framework.Providers.Currency;
using Atlantis.Framework.Providers.Interface.Products;
using Atlantis.Framework.Providers.Products;
using Atlantis.Framework.Providers.Interface.Preferences;
using System.Web;
using Atlantis.Framework.Testing.MockPreferencesProvider;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;

namespace Atlantis.Framework.TH.Products.Tests
{
  [TestClass]
  [ExcludeFromCodeCoverage]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.PLSignupInfo.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.ProductOffer.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DataCacheGeneric.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.Currency.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.EcommPricing.Impl.dll")]
  public class ProductPriceTests
  {
    const string _tokenFormat = "[@T[productprice:{0}]@T]";
    private TestContext _testContextInstance;

    public TestContext TestContext
    {
      get
      {
        return _testContextInstance;
      }
      set
      {
        _testContextInstance = value;
      }
    }

    [TestInitialize]
    public void InitializeTests()
    {
      TokenManager.RegisterTokenHandler(new ProductPriceTokenHandler());
    }

    private IProviderContainer SetBasicContextAndProviders()
    {
      MockHttpRequest request = new MockHttpRequest("http://www.godaddy.com/default.aspx?ci=1");
      MockHttpContext.SetFromWorkerRequest(request);

      IProviderContainer result = new MockProviderContainer();
      result.RegisterProvider<ISiteContext, MockSiteContext>();
      result.RegisterProvider<IShopperContext, MockShopperContext>();
      result.RegisterProvider<IShopperPreferencesProvider, MockShopperPreference>();
      result.RegisterProvider<ICurrencyProvider, CurrencyProvider>();
      result.RegisterProvider<IProductProvider, ProductProvider>();

      return result;
    }

    private string TokenSuccess(string xmlTokenData, string shopperCurrency = null, int shopperPriceType = -1)
    {
      var container = SetBasicContextAndProviders();

      if (shopperCurrency != null)
      {
        IShopperPreferencesProvider preferences = container.Resolve<IShopperPreferencesProvider>();
        preferences.UpdatePreference("gdshop_currencyType", shopperCurrency);
      }

      if (shopperPriceType != -1)
      {
        ((MockProviderContainer)container).SetMockSetting(MockShopperContextSettings.PriceType, shopperPriceType);
      }

      string outputText;

      string token = string.Format(_tokenFormat, xmlTokenData);
      TokenEvaluationResult result = TokenManager.ReplaceTokens(token, container, out outputText);
      Assert.AreEqual(TokenEvaluationResult.Success, result);
      Assert.AreNotEqual(string.Empty, outputText);

      return outputText;
    }

    [TestMethod]
    public void CurrentPriceBasic()
    {
      TokenSuccess("<current productid=\"58\" />");
    }

    [TestMethod]
    public void CurrentPriceEuroShopper()
    {
      TokenSuccess("<current productid=\"58\" />", "EUR");
    }

    [TestMethod]
    public void CurrentPriceWithPeriod()
    {
      TokenSuccess("<current productid=\"58\" period=\"yearly\" />");
    }

    [TestMethod]
    public void CurrentPriceWithPeriodAndDropOptions()
    {
      string output = TokenSuccess("<current productid=\"58\" period=\"yearly\" dropdecimal=\"true\" dropsymbol=\"true\" />");
      Assert.IsFalse(output.Contains("$"));
    }

    [TestMethod]
    public void ListPriceCurrencyOverride()
    {
      string output = TokenSuccess("<list productid=\"58\" period=\"monthly\" currencytype=\"INR\" />");
      Assert.IsFalse(output.Contains("$"));
    }

    [TestMethod]
    public void ListPricePriceTypeOverride()
    {
      string outputStandard = TokenSuccess("<list productid=\"101\" period=\"yearly\" />");
      string outputDDC = TokenSuccess("<list productid=\"101\" period=\"yearly\" pricetype=\"8\" />");
      Assert.AreNotEqual(outputStandard, outputDDC);
    }

    [TestMethod]
    public void ListPriceCostcoShopper()
    {
      string outputStandard = TokenSuccess("<list productid=\"101\" period=\"yearly\" />");
      string outputCostco = TokenSuccess("<list productid=\"101\" period=\"yearly\" />", null, 16);
      Assert.AreNotEqual(outputStandard, outputCostco);
    }

    [TestMethod]
    public void NonHtmlSymbol()
    {
      string outputHtml = TokenSuccess("<list productid=\"58\" period=\"monthly\" htmlsymbol=\"true\" currencytype=\"EUR\" />");
      Assert.IsTrue(outputHtml.Contains("&euro;"));

      string output = TokenSuccess("<list productid=\"58\" period=\"monthly\" htmlsymbol=\"false\" currencytype=\"EUR\" />");
      Assert.IsFalse(output.Contains("&euro;"));
    }

    [TestMethod]
    public void TemplateBasicNoStrike()
    {
      string outputHtml = TokenSuccess("<template productid=\"58\" period=\"monthly\"><strike><![CDATA[<strike>{1}</strike> {0}]]></strike><nostrike><![CDATA[]]>{0}</nostrike></template>");
      Assert.IsTrue(outputHtml.Contains("$"));
    }

    [TestMethod]
    public void TemplateBasicStrike()
    {
      string outputHtml = TokenSuccess("<template productid=\"40801\" period=\"yearly\"><strike><![CDATA[<strike>{1}</strike> {0}]]></strike><nostrike><![CDATA[]]>{0}</nostrike></template>");
      Assert.IsTrue(outputHtml.Contains("strike"));
    }

    [TestMethod]
    public void WrapSymbolTest()
    {
      string expectedTagName = "span";
      string expectedCssClass = "myClass";
      string data = string.Format("<current productid=\"58\" htmlsymbol=\"true\" currencytype=\"USD\" symboltagname=\"{0}\" symbolcssclass=\"{1}\" />", expectedTagName, expectedCssClass);
      string key = "productprice";
      string tokenString = string.Format("[@T[{1}:{0}]@T]", data, key);
      var token = new ProductPriceToken(key, data, tokenString);
      Assert.IsNotNull(token);

      Assert.AreEqual(expectedTagName, token.WrapTagName);
      Assert.AreEqual(expectedCssClass, token.WrapCssClass);

      var target = new ProductPriceTokenHandler();
      IProviderContainer providerContainer = SetBasicContextAndProviders();
      TokenEvaluationResult actual = target.EvaluateTokens(new List<IToken>() { token }, providerContainer);
      Assert.AreEqual(TokenEvaluationResult.Success, actual);

      TestContext.WriteLine(token.TokenResult);
      Assert.IsTrue(token.TokenResult.Contains(string.Format("<{0} class=\"{1}\">$</{0}>", expectedTagName, expectedCssClass)));

      data = string.Format("<list productid=\"58\" htmlsymbol=\"true\" currencytype=\"USD\" symboltagname=\"{0}\" symbolcssclass=\"{1}\" />", expectedTagName, expectedCssClass);
      tokenString = string.Format("[@T[{1}:{0}]@T]", data, key);
      token = new ProductPriceToken(key, data, tokenString);
      Assert.IsNotNull(token);
      Assert.AreEqual(expectedTagName, token.WrapTagName);
      Assert.AreEqual(expectedCssClass, token.WrapCssClass);

      target = new ProductPriceTokenHandler();
      providerContainer = SetBasicContextAndProviders();
      actual = target.EvaluateTokens(new List<IToken>() { token }, providerContainer);
      Assert.AreEqual(TokenEvaluationResult.Success, actual);

      TestContext.WriteLine(token.TokenResult);
      Assert.IsTrue(token.TokenResult.Contains(string.Format("<{0} class=\"{1}\">$</{0}>", expectedTagName, expectedCssClass)));

      expectedTagName = "span";
      expectedCssClass = string.Empty;
      data = string.Format("<current productid=\"58\" htmlsymbol=\"true\" currencytype=\"USD\" symboltagname=\"{0}\" symbolcssclass=\"{1}\" />", expectedTagName, expectedCssClass);
      tokenString = string.Format("[@T[{1}:{0}]@T]", data, key);
      token = new ProductPriceToken(key, data, tokenString);
      Assert.IsNotNull(token);
      Assert.AreEqual(expectedTagName, token.WrapTagName);
      Assert.AreEqual(expectedCssClass, token.WrapCssClass);

      target = new ProductPriceTokenHandler();
      providerContainer = SetBasicContextAndProviders();
      actual = target.EvaluateTokens(new List<IToken>() { token }, providerContainer);
      Assert.AreEqual(TokenEvaluationResult.Success, actual);

      TestContext.WriteLine(token.TokenResult);
      Assert.IsTrue(token.TokenResult.Contains(string.Format("<{0}>$</{0}>", expectedTagName, expectedCssClass)));

      data = string.Format("<list productid=\"58\" htmlsymbol=\"true\" currencytype=\"USD\" symboltagname=\"{0}\" symbolcssclass=\"{1}\" />", expectedTagName, expectedCssClass);
      tokenString = string.Format("[@T[{1}:{0}]@T]", data, key);
      token = new ProductPriceToken(key, data, tokenString);
      Assert.IsNotNull(token);
      Assert.AreEqual(expectedTagName, token.WrapTagName);
      Assert.AreEqual(expectedCssClass, token.WrapCssClass);

      target = new ProductPriceTokenHandler();
      providerContainer = SetBasicContextAndProviders();
      actual = target.EvaluateTokens(new List<IToken>() { token }, providerContainer);
      Assert.AreEqual(TokenEvaluationResult.Success, actual);

      TestContext.WriteLine(token.TokenResult);
      Assert.IsTrue(token.TokenResult.Contains(string.Format("<{0}>$</{0}>", expectedTagName, expectedCssClass)));

      expectedTagName = string.Empty;
      expectedCssClass = "myClass";
      data = string.Format("<current productid=\"58\" htmlsymbol=\"true\" currencytype=\"USD\" symboltagname=\"{0}\" symbolcssclass=\"{1}\" />", expectedTagName, expectedCssClass);
      tokenString = string.Format("[@T[{1}:{0}]@T]", data, key);
      token = new ProductPriceToken(key, data, tokenString);
      Assert.IsNotNull(token);
      Assert.AreEqual(expectedTagName, token.WrapTagName);
      Assert.AreEqual(expectedCssClass, token.WrapCssClass);

      target = new ProductPriceTokenHandler();
      providerContainer = SetBasicContextAndProviders();
      actual = target.EvaluateTokens(new List<IToken>() { token }, providerContainer);
      Assert.AreEqual(TokenEvaluationResult.Success, actual);

      TestContext.WriteLine(token.TokenResult);
      Assert.IsTrue(token.TokenResult.Contains("$"));
      Assert.IsFalse(token.TokenResult.Contains(string.Format("<{0} class=\"{1}\">$</{0}>", expectedTagName, expectedCssClass)));
      Assert.IsFalse(token.TokenResult.Contains(string.Format("<{0}>$</{0}>", expectedTagName, expectedCssClass)));

      data = string.Format("<list productid=\"58\" htmlsymbol=\"true\" currencytype=\"USD\" symboltagname=\"{0}\" symbolcssclass=\"{1}\" />", expectedTagName, expectedCssClass);
      tokenString = string.Format("[@T[{1}:{0}]@T]", data, key);
      token = new ProductPriceToken(key, data, tokenString);
      Assert.IsNotNull(token);
      Assert.AreEqual(expectedTagName, token.WrapTagName);
      Assert.AreEqual(expectedCssClass, token.WrapCssClass);

      target = new ProductPriceTokenHandler();
      providerContainer = SetBasicContextAndProviders();
      actual = target.EvaluateTokens(new List<IToken>() { token }, providerContainer);
      Assert.AreEqual(TokenEvaluationResult.Success, actual);

      TestContext.WriteLine(token.TokenResult);
      Assert.IsTrue(token.TokenResult.Contains("$"));
      Assert.IsFalse(token.TokenResult.Contains(string.Format("<{0} class=\"{1}\">$</{0}>", expectedTagName, expectedCssClass)));
      Assert.IsFalse(token.TokenResult.Contains(string.Format("<{0}>$</{0}>", expectedTagName, expectedCssClass)));

      expectedTagName = "span";
      expectedCssClass = "myClass";
      data = string.Format("<current productid=\"58\" htmlsymbol=\"false\" currencytype=\"USD\" symboltagname=\"{0}\" symbolcssclass=\"{1}\" />", expectedTagName, expectedCssClass);
      tokenString = string.Format("[@T[{1}:{0}]@T]", data, key);
      token = new ProductPriceToken(key, data, tokenString);
      Assert.IsNotNull(token);
      Assert.AreEqual(expectedTagName, token.WrapTagName);
      Assert.AreEqual(expectedCssClass, token.WrapCssClass);

      target = new ProductPriceTokenHandler();
      providerContainer = SetBasicContextAndProviders();
      actual = target.EvaluateTokens(new List<IToken>() { token }, providerContainer);
      Assert.AreEqual(TokenEvaluationResult.Success, actual);

      TestContext.WriteLine(token.TokenResult);
      Assert.IsFalse(token.TokenResult.Contains(string.Format("<{0} class=\"{1}\">$</{0}>", expectedTagName, expectedCssClass)));
      Assert.IsFalse(token.TokenResult.Contains(string.Format("<{0}>$</{0}>", expectedTagName, expectedCssClass)));

      data = string.Format("<list productid=\"58\" htmlsymbol=\"false\" currencytype=\"USD\" symboltagname=\"{0}\" symbolcssclass=\"{1}\" />", expectedTagName, expectedCssClass);
      tokenString = string.Format("[@T[{1}:{0}]@T]", data, key);
      token = new ProductPriceToken(key, data, tokenString);
      Assert.IsNotNull(token);
      Assert.AreEqual(expectedTagName, token.WrapTagName);
      Assert.AreEqual(expectedCssClass, token.WrapCssClass);

      target = new ProductPriceTokenHandler();
      providerContainer = SetBasicContextAndProviders();
      actual = target.EvaluateTokens(new List<IToken>() { token }, providerContainer);
      Assert.AreEqual(TokenEvaluationResult.Success, actual);

      TestContext.WriteLine(token.TokenResult);
      Assert.IsFalse(token.TokenResult.Contains(string.Format("<{0} class=\"{1}\">$</{0}>", expectedTagName, expectedCssClass)));
      Assert.IsFalse(token.TokenResult.Contains(string.Format("<{0}>$</{0}>", expectedTagName, expectedCssClass)));
    }
  }
}
