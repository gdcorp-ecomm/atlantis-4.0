using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Currency;
using Atlantis.Framework.Providers.Interface.Currency;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Atlantis.Framework.Tokens.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.TH.Currency.Tests
{
  [TestClass]
  public class CurrencyPriceTests
  {
    private const string _TOKEN_FORMAT = "[@T[currencyprice:{0}]@T]";

    [TestInitialize]
    public void InitializeTests()
    {
      TokenManager.RegisterTokenHandler(new CurrencyPriceTokenHandler());
    }

    private void SetBasicContextAndProviders()
    {
      MockHttpContext.SetMockHttpContext("default.aspx", "http://www.godaddy.com/default.aspx?ci=1", "ci=1");
      HttpProviderContainer.Instance.RegisterProvider<ISiteContext, MockSiteContext>();
      HttpProviderContainer.Instance.RegisterProvider<IShopperContext, MockShopperContext>();
      HttpProviderContainer.Instance.RegisterProvider<ICurrencyProvider, CurrencyProvider>();
    }

    private string TokenSuccess(string xmlTokenData)
    {
      SetBasicContextAndProviders();

      string outputText;

      string token = string.Format(_TOKEN_FORMAT, xmlTokenData);
      TokenEvaluationResult result = TokenManager.ReplaceTokens(token, HttpProviderContainer.Instance, out outputText);
      Assert.AreEqual(TokenEvaluationResult.Success, result);
      Assert.AreNotEqual(string.Empty, outputText);

      return outputText;
    }

    // Negative test
    private string TokenFail(string xmlTokenData)
    {
      SetBasicContextAndProviders();

      string outputText;

      string token = string.Format(_TOKEN_FORMAT, xmlTokenData);
      TokenEvaluationResult result = TokenManager.ReplaceTokens(token, HttpProviderContainer.Instance, out outputText);
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
      string output = TokenSuccess("<price usdamount=\"1000\" currencytype=\"INR\" />");
      Assert.IsTrue(output.Contains("Rs.") && !output.Contains("$"));
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
  }
}
