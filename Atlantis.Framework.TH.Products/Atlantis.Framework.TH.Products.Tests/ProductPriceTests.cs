using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Tokens.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Testing.MockProviders;
using Atlantis.Framework.Providers.Interface.Currency;
using Atlantis.Framework.Providers.Currency;
using Atlantis.Framework.Providers.Interface.Products;
using Atlantis.Framework.Providers.Products;
using Atlantis.Framework.Providers.Interface.Preferences;
using System.Web;

namespace Atlantis.Framework.TH.Products.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.PLSignupInfo.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.ProductOffer.Impl.dll")]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  public class ProductPriceTests
  {
    const string _tokenFormat = "[@T[productprice:{0}]@T]";

    [TestInitialize]
    public void InitializeTests()
    {
      TokenManager.RegisterTokenHandler(new ProductPriceTokenHandler());
    }

    private void SetBasicContextAndProviders()
    {
      MockHttpContext.SetMockHttpContext("default.aspx", "http://www.godaddy.com/default.aspx?ci=1", "ci=1");
      HttpProviderContainer.Instance.RegisterProvider<ISiteContext, MockSiteContext>();
      HttpProviderContainer.Instance.RegisterProvider<IShopperContext, MockShopperContext>();
      HttpProviderContainer.Instance.RegisterProvider<IShopperPreferencesProvider, MockShopperPreference>();
      HttpProviderContainer.Instance.RegisterProvider<ICurrencyProvider, CurrencyProvider>();
      HttpProviderContainer.Instance.RegisterProvider<IProductProvider, ProductProvider>();
    }

    private string TokenSuccess(string xmlTokenData, string shopperCurrency = null, int shopperPriceType = -1)
    {
      SetBasicContextAndProviders();

      if (shopperCurrency != null)
      {
        IShopperPreferencesProvider preferences = HttpProviderContainer.Instance.Resolve<IShopperPreferencesProvider>();
        preferences.UpdatePreference("gdshop_currencyType", shopperCurrency);
      }

      if (shopperPriceType != -1)
      {
        HttpContext.Current.Items[MockShopperContextSettings.PriceType] = shopperPriceType;
      }

      string outputText;

      string token = string.Format(_tokenFormat, xmlTokenData);
      TokenEvaluationResult result = TokenManager.ReplaceTokens(token, HttpProviderContainer.Instance, out outputText);
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

  }
}
