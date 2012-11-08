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
      HttpProviderContainer.Instance.RegisterProvider<ISiteContext, MockSiteContextGoDaddy>();
      HttpProviderContainer.Instance.RegisterProvider<IShopperContext, MockShopperContext>();
      HttpProviderContainer.Instance.RegisterProvider<IShopperPreferencesProvider, MockShopperPreference>();
      HttpProviderContainer.Instance.RegisterProvider<ICurrencyProvider, CurrencyProvider>();
      HttpProviderContainer.Instance.RegisterProvider<IProductProvider, ProductProvider>();
    }

    private void TokenSuccess(string xmlTokenData, string shopperCurrency = null)
    {
      SetBasicContextAndProviders();

      if (shopperCurrency != null)
      {
        IShopperPreferencesProvider preferences = HttpProviderContainer.Instance.Resolve<IShopperPreferencesProvider>();
        preferences.UpdatePreference("gdshop_currencyType", shopperCurrency);
      }

      string outputText;

      string token = string.Format(_tokenFormat, xmlTokenData);
      TokenEvaluationResult result = TokenManager.ReplaceTokens(token, HttpProviderContainer.Instance, out outputText);
      Assert.AreEqual(TokenEvaluationResult.Success, result);
      Assert.AreNotEqual(string.Empty, outputText);
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
      TokenSuccess("<current productid=\"58\" period=\"yearly\" dropdecimal=\"true\" dropsymbol=\"true\" />");
    }




  }
}
