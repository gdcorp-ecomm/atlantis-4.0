using System;
using Atlantis.Framework.Providers.Interface.Preferences;
using Atlantis.Framework.Providers.Preferences;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Providers.Interface.Currency;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Providers.Currency.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class CurrencyProviderTests
  {
    private void SetContexts(int privateLabelId, string shopperId)
    {
      MockHttpContext.SetMockHttpContext("default.aspx", "http://localhost/default.aspx", string.Empty);
      HttpProviderContainer.Instance.RegisterProvider<ISiteContext, TestContexts>();
      HttpProviderContainer.Instance.RegisterProvider<IShopperContext, TestContexts>();
      HttpProviderContainer.Instance.RegisterProvider<IManagerContext, TestContexts>();
      HttpProviderContainer.Instance.RegisterProvider<ICurrencyProvider, CurrencyProvider>();
      HttpProviderContainer.Instance.RegisterProvider<IShopperPreferencesProvider, ShopperPreferencesProvider>();

      ISiteContext siteContext = HttpProviderContainer.Instance.Resolve<ISiteContext>();
      TestContexts testContexts = (TestContexts)siteContext;
      testContexts.SetContext(privateLabelId, shopperId);
    }

    private void SetContextsWithoutShopperPreferences(int privateLabelId, string shopperId)
    {
      MockHttpContext.SetMockHttpContext("default.aspx", "http://localhost/default.aspx", string.Empty);
      HttpProviderContainer.Instance.RegisterProvider<ISiteContext, TestContexts>();
      HttpProviderContainer.Instance.RegisterProvider<IShopperContext, TestContexts>();
      HttpProviderContainer.Instance.RegisterProvider<IManagerContext, TestContexts>();
      HttpProviderContainer.Instance.RegisterProvider<ICurrencyProvider, CurrencyProvider>();

      ISiteContext siteContext = HttpProviderContainer.Instance.Resolve<ISiteContext>();
      TestContexts testContexts = (TestContexts)siteContext;
      testContexts.SetContext(privateLabelId, shopperId);
    }

    [TestMethod]
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    [DeploymentItem("Interop.gdMiniEncryptLib.dll")]
    [DeploymentItem("atlantis.config")]
    public void CurrencyInfoUSD()
    {
      ICurrencyInfo usd = CurrencyData.GetCurrencyInfo("usd");
      Assert.AreEqual("USD", usd.CurrencyType);
    }

    [TestMethod]
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    [DeploymentItem("Interop.gdMiniEncryptLib.dll")]
    [DeploymentItem("atlantis.config")]
    public void CurrencyInfoDescriptions()
    {
      foreach (ICurrencyInfo item in CurrencyData.CurrencyInfoList)
      {
        Console.WriteLine(item.Description + " : " + item.DescriptionPlural + " : " + item.Symbol + " : " + item.SymbolHtml);
      }
    }

    [TestMethod]
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    [DeploymentItem("Interop.gdMiniEncryptLib.dll")]
    [DeploymentItem("atlantis.config")]
    public void BasicPricing()
    {
      SetContexts(1, "832652");
      ICurrencyProvider currency = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();
      ICurrencyPrice price = currency.GetCurrentPrice(101);

      ICurrencyPrice price2 = currency.GetCurrentPriceByQuantity(101, 1);
      Assert.AreEqual(price, price2);

      ICurrencyPrice price3 = currency.GetListPrice(101);
      Assert.IsTrue(price3.Price > 0);
    }

    [TestMethod]
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    [DeploymentItem("Interop.gdMiniEncryptLib.dll")]
    [DeploymentItem("atlantis.config")]
    public void MultiCurrencyCatalogBasic()
    {
      SetContexts(1, "832652");
      ICurrencyProvider currency = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();

      
      currency.SelectedDisplayCurrencyType = "EUR";
      ICurrencyPrice euroPrice = currency.GetCurrentPrice(101);
      Assert.AreEqual(CurrencyPriceType.Transactional, euroPrice.Type);
      string text = currency.PriceText(euroPrice, false);

      currency.SelectedDisplayCurrencyType = "GBP";
      ICurrencyPrice gbpPrice = currency.GetCurrentPrice(101);
      Assert.AreEqual(CurrencyPriceType.Transactional, gbpPrice.Type);
      text = currency.PriceText(gbpPrice, false);

      currency.SelectedDisplayCurrencyType = "CHF";
      ICurrencyPrice chfPrice = currency.GetCurrentPrice(101);
      Assert.AreEqual("USD", chfPrice.CurrencyInfo.CurrencyType);
      text = currency.PriceText(chfPrice, false);

    }

    [TestMethod]
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    [DeploymentItem("Interop.gdMiniEncryptLib.dll")]
    [DeploymentItem("atlantis.config")]
    public void ConvertingIcannFees()
    {
      SetContexts(1, "");

      ICurrencyPrice usd18 = new CurrencyPrice(18, CurrencyData.GetCurrencyInfo("USD"), CurrencyPriceType.Transactional);
      ICurrencyPrice converted = CurrencyProvider.ConvertPrice(usd18, CurrencyData.GetCurrencyInfo("EUR"));
      Console.WriteLine("EUR=" + converted.Price.ToString());

      converted = CurrencyProvider.ConvertPrice(usd18, CurrencyData.GetCurrencyInfo("AUD"));
      Console.WriteLine("AUD=" + converted.Price.ToString());

      converted = CurrencyProvider.ConvertPrice(usd18, CurrencyData.GetCurrencyInfo("GBP"));
      Console.WriteLine("GBP=" + converted.Price.ToString());
    }

    [TestMethod]
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    [DeploymentItem("Interop.gdMiniEncryptLib.dll")]
    [DeploymentItem("atlantis.config")]
    public void MultiCurrencyCatalogReseller()
    {
      SetContexts(1724, "");
      ICurrencyProvider currency = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();

      currency.SelectedDisplayCurrencyType = "EUR";
      ICurrencyPrice euroPrice = currency.GetCurrentPrice(101);
      Assert.AreEqual(CurrencyPriceType.Transactional, euroPrice.Type);
      Assert.AreEqual("USD", euroPrice.CurrencyInfo.CurrencyType);
      string text = currency.PriceText(euroPrice, false);

      currency.SelectedDisplayCurrencyType = "GBP";
      ICurrencyPrice gbpPrice = currency.GetCurrentPrice(101);
      Assert.AreEqual(CurrencyPriceType.Transactional, gbpPrice.Type);
      Assert.AreEqual("USD", gbpPrice.CurrencyInfo.CurrencyType);
      text = currency.PriceText(gbpPrice, false);

      currency.SelectedDisplayCurrencyType = "CHF";
      ICurrencyPrice chfPrice = currency.GetCurrentPrice(101);
      Assert.AreEqual("USD", chfPrice.CurrencyInfo.CurrencyType);
      text = currency.PriceText(chfPrice, false);

    }

    [TestMethod]
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    [DeploymentItem("Interop.gdMiniEncryptLib.dll")]
    [DeploymentItem("atlantis.config")]
    public void JapaneseYen()
    {
      SetContexts(1, "");
      ICurrencyProvider currency = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();

      currency.SelectedDisplayCurrencyType = "JPY";
      ICurrencyPrice jpyPrice1 = currency.GetCurrentPrice(77);
      string text = currency.PriceText(jpyPrice1, false);
      Console.WriteLine(text);

      jpyPrice1 = currency.GetCurrentPrice(78);
      text = currency.PriceText(jpyPrice1, false);
      Console.WriteLine(text);

      jpyPrice1 = currency.GetCurrentPrice(79);
      text = currency.PriceText(jpyPrice1, false);
      Console.WriteLine(text);

      jpyPrice1 = currency.GetCurrentPrice(1865);
      text = currency.PriceText(jpyPrice1, false);
      Console.WriteLine(text);

      jpyPrice1 = currency.GetCurrentPrice(7413);
      text = currency.PriceText(jpyPrice1, false);
      Console.WriteLine(text);

      ICurrencyPrice p119 = new CurrencyPrice(119, CurrencyData.GetCurrencyInfo("USD"), CurrencyPriceType.Transactional);
      text = currency.PriceText(p119, false);
      Console.WriteLine(text);
    }

    [TestMethod]
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    [DeploymentItem("Interop.gdMiniEncryptLib.dll")]
    [DeploymentItem("atlantis.config")]
    public void ConversionOverflow()
    {
      SetContexts(1, "");
      ICurrencyProvider currency = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();

      currency.SelectedDisplayCurrencyType = "INR";
      ICurrencyPrice pMillion = new CurrencyPrice(100000000, CurrencyData.GetCurrencyInfo("USD"), CurrencyPriceType.Transactional);
      string text = currency.PriceText(pMillion, false);
      Console.WriteLine(text);
    }

    [TestMethod]
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    [DeploymentItem("Interop.gdMiniEncryptLib.dll")]
    [DeploymentItem("atlantis.config")]
    public void BasicPricingWithoutShopperPreferenceProviderRegistered()
    {
      SetContextsWithoutShopperPreferences(1, "832652");
      ICurrencyProvider currency = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();
      ICurrencyPrice price = currency.GetCurrentPrice(101);
    }

    [TestMethod]
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    [DeploymentItem("Interop.gdMiniEncryptLib.dll")]
    [DeploymentItem("atlantis.config")]
    public void TestUSDConversion()
    {
      SetContexts(1, "");
      ICurrencyProvider currency = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();
      currency.SelectedDisplayCurrencyType = "INR";

      ICurrencyPrice icann = new CurrencyPrice(18, CurrencyData.GetCurrencyInfo("USD"), CurrencyPriceType.Transactional);
      string text = currency.PriceText(icann, false);
      Console.WriteLine(text);
    }
  }
}
