using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Currency;
using Atlantis.Framework.Providers.Interface.Currency;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Atlantis.Framework.Tokens.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.TH.Currency.Tests
{
  [TestClass]
  [ExcludeFromCodeCoverage]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Currency.Impl.dll")]
  public class CurrencyPriceTests
  {
    private const string _TOKEN_FORMAT = "[@T[currencyprice:{0}]@T]";

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
      TokenManager.RegisterTokenHandler(new CurrencyPriceTokenHandler());
    }

    private IProviderContainer SetBasicContextAndProviders()
    {
      MockHttpRequest request = new MockHttpRequest("http://www.godaddy.com/default.aspx?ci=1");
      MockHttpContext.SetFromWorkerRequest(request);

      MockProviderContainer container = new MockProviderContainer();
      container.RegisterProvider<ISiteContext, MockSiteContext>();
      container.RegisterProvider<IShopperContext, MockShopperContext>();
      container.RegisterProvider<ICurrencyProvider, CurrencyProvider>();

      return container;
    }

    private string TokenSuccess(string xmlTokenData)
    {
      var container = SetBasicContextAndProviders();

      string outputText;

      string token = string.Format(_TOKEN_FORMAT, xmlTokenData);
      TokenEvaluationResult result = TokenManager.ReplaceTokens(token, container, out outputText);
      Assert.AreEqual(TokenEvaluationResult.Success, result);
      Assert.AreNotEqual(string.Empty, outputText);

      return outputText;
    }

    // Negative test
    private string TokenFail(string xmlTokenData)
    {
      var container = SetBasicContextAndProviders();

      string outputText;

      string token = string.Format(_TOKEN_FORMAT, xmlTokenData);
      TokenEvaluationResult result = TokenManager.ReplaceTokens(token, container, out outputText);
      Assert.AreEqual(TokenEvaluationResult.Errors, result);

      return outputText;
    }

    [TestMethod]
    public void Basic()
    {
      string output = TokenSuccess("<price usdamount=\"1000\" />");
      Assert.AreEqual("$10.00", output, false);
    }

    [TestMethod]
    public void DropOptions()
    {
      string output = TokenSuccess("<price usdamount=\"1000\" dropdecimal=\"true\" dropsymbol=\"true\" />");
      Assert.AreEqual("10", output, false);
    }

    [TestMethod]
    public void CurrencyOverride()
    {
      string output = TokenSuccess("<price usdamount=\"1000\" currencytype=\"INR\" htmlsymbol=\"false\" />");
      Assert.IsTrue(output.Contains("₨") && !output.Contains("$"));

      output = TokenSuccess("<price usdamount=\"1000\" currencytype=\"INR\" htmlsymbol=\"true\" />");
      Assert.IsTrue(output.Contains("&#8360;") && !output.Contains("$"));
    }

    [TestMethod]
    public void NonHtmlSymbol()
    {
      string outputHtml = TokenSuccess("<price usdamount=\"1000\" htmlsymbol=\"true\" currencytype=\"EUR\" />");
      Assert.IsTrue(outputHtml.Contains("&euro;"));

      string output = TokenSuccess("<price usdamount=\"1000\" htmlsymbol=\"false\" currencytype=\"EUR\" />");
      Assert.IsFalse(output.Contains("&euro;"));
    }

    // Negative tests
    [TestMethod]
    public void InvalidRootElement()
    {
      string output = TokenFail("<foo usdamount=\"1000\" />");
      Assert.AreEqual(string.Empty, output, false);
    }

    [TestMethod]
    public void InvalidXml()
    {
      string output = TokenFail("usdamount=\"1000\"");
      Assert.AreEqual(string.Empty, output, false);
    }

    [TestMethod]
    public void WrapSymbolTest()
    {
      string expectedTagName = "span";
      string expectedCssClass = "myClass";
      string data = string.Format("<price usdamount=\"1000\" htmlsymbol=\"true\" currencytype=\"USD\" symboltagname=\"{0}\" symbolcssclass=\"{1}\" />", expectedTagName, expectedCssClass);
      string key = "currencyprice";
      string tokenString = string.Format("[@T[{1}:{0}]@T]", data, key);
      var token = new CurrencyPriceToken(key, data, tokenString);
      Assert.IsNotNull(token);

      var target = new CurrencyPriceTokenHandler();
      IProviderContainer providerContainer = SetBasicContextAndProviders();
      TokenEvaluationResult actual = target.EvaluateTokens(new List<IToken>() { token }, providerContainer);
      Assert.AreEqual(TokenEvaluationResult.Success, actual);

      TestContext.WriteLine(token.TokenResult);
      Assert.AreEqual(string.Format("<{0} class=\"{1}\">$</{0}>10.00", expectedTagName, expectedCssClass), token.TokenResult);

      expectedTagName = "span";
      expectedCssClass = string.Empty;
      data = string.Format("<price usdamount=\"1000\" htmlsymbol=\"true\" currencytype=\"USD\" symboltagname=\"{0}\" symbolcssclass=\"{1}\" />", expectedTagName, expectedCssClass);
      key = "currencyprice";
      tokenString = string.Format("[@T[{1}:{0}]@T]", data, key);
      token = new CurrencyPriceToken(key, data, tokenString);
      Assert.IsNotNull(token);
      Assert.AreEqual(expectedTagName, token.WrapTagName);
      Assert.AreEqual(expectedCssClass, token.WrapCssClass);

      target = new CurrencyPriceTokenHandler();
      providerContainer = SetBasicContextAndProviders();
      actual = target.EvaluateTokens(new List<IToken>() { token }, providerContainer);
      Assert.AreEqual(TokenEvaluationResult.Success, actual);

      TestContext.WriteLine(token.TokenResult);
      Assert.AreEqual(string.Format("<{0}>$</{0}>10.00", expectedTagName, expectedCssClass), token.TokenResult);

      expectedTagName = string.Empty;
      expectedCssClass = "myClass";
      data = string.Format("<price usdamount=\"1000\" htmlsymbol=\"true\" currencytype=\"USD\" symboltagname=\"{0}\" symbolcssclass=\"{1}\" />", expectedTagName, expectedCssClass);
      key = "currencyprice";
      tokenString = string.Format("[@T[{1}:{0}]@T]", data, key);
      token = new CurrencyPriceToken(key, data, tokenString);
      Assert.IsNotNull(token);
      Assert.AreEqual(expectedTagName, token.WrapTagName);
      Assert.AreEqual(expectedCssClass, token.WrapCssClass);

      target = new CurrencyPriceTokenHandler();
      providerContainer = SetBasicContextAndProviders();
      actual = target.EvaluateTokens(new List<IToken>() { token }, providerContainer);
      Assert.AreEqual(TokenEvaluationResult.Success, actual);

      TestContext.WriteLine(token.TokenResult);
      Assert.AreEqual(string.Format("$10.00", expectedTagName, expectedCssClass), token.TokenResult);

      expectedTagName = "span";
      expectedCssClass = "myClass";
      data = string.Format("<price usdamount=\"1000\" htmlsymbol=\"false\" currencytype=\"USD\" symboltagname=\"{0}\" symbolcssclass=\"{1}\" />", expectedTagName, expectedCssClass);
      key = "currencyprice";
      tokenString = string.Format("[@T[{1}:{0}]@T]", data, key);
      token = new CurrencyPriceToken(key, data, tokenString);
      Assert.IsNotNull(token);
      Assert.AreEqual(expectedTagName, token.WrapTagName);
      Assert.AreEqual(expectedCssClass, token.WrapCssClass);

      target = new CurrencyPriceTokenHandler();
      providerContainer = SetBasicContextAndProviders();
      actual = target.EvaluateTokens(new List<IToken>() { token }, providerContainer);
      Assert.AreEqual(TokenEvaluationResult.Success, actual);

      TestContext.WriteLine(token.TokenResult);
      Assert.AreEqual(string.Format("$10.00", expectedTagName, expectedCssClass), token.TokenResult);
    }
  }
}
