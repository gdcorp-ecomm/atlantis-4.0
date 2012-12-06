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
  public class ProductCompareTests
  {
    const string _tokenFormat = "[@T[productcompare:{0}]@T]";

    [TestInitialize]
    public void InitializeTests()
    {
      TokenManager.RegisterTokenHandler(new ProductCompareTokenHandler());
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
    
    private void TokenEmpty(string xmlTokenData, string shopperCurrency = null)
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
      Assert.AreEqual(TokenEvaluationResult.Errors, result);
      Assert.AreEqual(string.Empty, outputText);
    }

    [TestMethod]
    public void CompareTwoProductsPercent()
    {
      TokenSuccess("<percent primaryproductid=\"58\" secondaryproductid=\"58\" />");
    }

    [TestMethod]
    public void CompareTwoProductsTimes()
    {
      TokenSuccess("<times primaryproductid=\"58\" secondaryproductid=\"58\" />");
    }

    [TestMethod]
    public void CompareFixedPricePercentMonthly()
    {
      TokenSuccess("<percent primaryproductid=\"58\" secondaryprice=\"10000\" secondaryperiod=\"monthly\" />");
    }

    [TestMethod]
    public void CompareFixedPricePercentAnnually()
    {
      TokenSuccess("<percent primaryproductid=\"58\" secondaryprice=\"10000\" secondaryperiod=\"yearly\" />");
    }

    [TestMethod]
    public void CompareFixedPriceTimesMonthly()
    {
      TokenSuccess("<times primaryproductid=\"58\" secondaryprice=\"10000\" secondaryperiod=\"monthly\" />");
    }

    [TestMethod]
    public void CompareFixedPriceTimesAnnually()
    {
      TokenSuccess("<times primaryproductid=\"58\" secondaryprice=\"10000\" secondaryperiod=\"yearly\" />");
    }

    [TestMethod]
    public void CompareAddition()
    {
      TokenSuccess("<addition primaryproductid=\"58\" secondaryprice=\"100\" secondaryperiod=\"monthly\" />");
    }

    [TestMethod]
    public void CompareTwoProductsAddition()
    {
      TokenSuccess("<addition primaryproductid=\"58\"  secondaryproductid=\"58\" secondaryperiod=\"monthly\" />");
    }

    [TestMethod]
    public void CompareSubtraction()
    {
      TokenSuccess("<subtraction primaryproductid=\"58\" secondaryprice=\"100\" secondaryperiod=\"monthly\" />");
    }

    [TestMethod]
    public void CompareTwoProductsSubtraction()
    {
      TokenSuccess("<subtraction primaryproductid=\"58\"  secondaryproductid=\"58\" secondaryperiod=\"monthly\" />");
    }

    [TestMethod]
    public void CompareMultiplication()
    {
      TokenSuccess("<multiplication primaryproductid=\"58\" secondaryprice=\"3\" secondaryperiod=\"monthly\" />");
    }

    [TestMethod]
    public void CompareDivision()
    {
      TokenSuccess("<division primaryproductid=\"58\" secondaryprice=\"3\" secondaryperiod=\"monthly\" />");
    }

    [TestMethod]
    public void CompareHideBelow()
    {
      TokenEmpty("<percent primaryproductid=\"58\" secondaryproductid=\"58\" hidebelow=\"1\" />");
    }
  }
}
