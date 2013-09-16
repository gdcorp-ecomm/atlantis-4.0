using Atlantis.Framework.Currency.Interface;
using Atlantis.Framework.EcommPricing.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Currency.Tests.Mocks;
using Atlantis.Framework.Providers.Interface.Currency;
using Atlantis.Framework.Providers.Interface.Preferences;
using Atlantis.Framework.Providers.Interface.Pricing;
using Atlantis.Framework.Providers.Interface.PromoData;
using Atlantis.Framework.Providers.PromoData;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Xml.Linq;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.Testing.MockPreferencesProvider;

namespace Atlantis.Framework.Providers.Currency.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.EcommPricing.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.Currency.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.PLSignupInfo.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.PrivateLabel.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.PromoData.Impl.dll")]
  public class CurrencyProviderTests
  {
    private IProviderContainer SetContexts(int privateLabelId, string shopperId)
    {
      return SetContexts(privateLabelId, shopperId, true, false);
    }

    private IProviderContainer SetContexts(int privateLabelId, string shopperId, bool includeShopperPreferences, bool includePromoData, bool includeIscPricing = false, bool includeLocalizationProvider = false)
    {
      return SetContexts("http://localhost/default.aspx", privateLabelId, shopperId, includeShopperPreferences, includePromoData, includeIscPricing, includeLocalizationProvider);      
    }

    private IProviderContainer SetContexts(string url, int privateLabelId, string shopperId, bool includeShopperPreferences, bool includePromoData, bool includeIscPricing = false, bool includeLocalizationProvider = false)
    {
      MockProviderContainer result = new MockProviderContainer();

      MockHttpRequest request = new MockHttpRequest(url);
      MockHttpContext.SetFromWorkerRequest(request);

      result.RegisterProvider<ISiteContext, MockSiteContext>();
      result.RegisterProvider<IShopperContext, MockShopperContext>();
      result.RegisterProvider<IManagerContext, MockNoManagerContext>();
      result.RegisterProvider<ICurrencyProvider, CurrencyProvider>();

      if (includeIscPricing)
      {
        result.RegisterProvider<IPricingProvider, IscPricingProvider>();
        IPricingProvider provider = result.Resolve<IPricingProvider>();
        provider.Enabled = true;
      }

      if (includeShopperPreferences)
      {
        result.RegisterProvider<IShopperPreferencesProvider, MockShopperPreference>();
      }

      if (includePromoData)
      {
        result.RegisterProvider<IPromoDataProvider, PromoDataProvider>();
      }

      if (includeLocalizationProvider)
      {
        result.RegisterProvider<ILocalizationProvider, MockLocalizationProvider>();
      }

      result.SetData(MockSiteContextSettings.PrivateLabelId, privateLabelId);
      IShopperContext shopperContext = result.Resolve<IShopperContext>();
      shopperContext.SetLoggedInShopper(shopperId);

      return result;
    }

    [TestMethod]
    public void CurrencyInfoUSD()
    {
      var container = SetContexts(1, "832652");
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();
      ICurrencyInfo usd = currency.GetCurrencyInfo("usd");
      Assert.AreEqual("USD", usd.CurrencyType);
    }

    [TestMethod]
    public void CurrencyInfoDescriptions()
    {
      var container = SetContexts(1, "832652");
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();
      foreach (ICurrencyInfo item in currency.CurrencyInfoList)
      {
        Console.WriteLine(item.Description + " : " + item.DescriptionPlural + " : " + item.Symbol + " : " + item.SymbolHtml);
      }
    }

    [TestMethod]
    public void BasicPricing()
    {
      var container = SetContexts(1, "832652");
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();
      ICurrencyPrice price = currency.GetCurrentPrice(101);

      ICurrencyPrice price2 = currency.GetCurrentPriceByQuantity(101, 1);
      Assert.AreEqual(price, price2);

      ICurrencyPrice price3 = currency.GetListPrice(101);
      Assert.IsTrue(price3.Price > 0);
    }

    [TestMethod]
    public void ConvertingIcannFees()
    {
      var container = SetContexts(1, string.Empty);
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();

      ICurrencyPrice usd18 = new CurrencyPrice(18, currency.GetCurrencyInfo("USD"), CurrencyPriceType.Transactional);
      ICurrencyPrice converted = CurrencyProvider.ConvertPrice(usd18, currency.GetCurrencyInfo("EUR"));
      Console.WriteLine("EUR=" + converted.Price.ToString());

      converted = CurrencyProvider.ConvertPrice(usd18, currency.GetCurrencyInfo("AUD"));
      Console.WriteLine("AUD=" + converted.Price.ToString());

      converted = CurrencyProvider.ConvertPrice(usd18, currency.GetCurrencyInfo("GBP"));
      Console.WriteLine("GBP=" + converted.Price.ToString());
    }

    [TestMethod]
    public void MultiCurrencyCatalogBasic()
    {
      var container = SetContexts(1, string.Empty);
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();

      currency.SelectedDisplayCurrencyType = "EUR";
      ICurrencyPrice euroPrice = currency.GetCurrentPrice(101);
      Assert.AreEqual(CurrencyPriceType.Transactional, euroPrice.Type);
      string text = currency.PriceText(euroPrice);

      currency.SelectedDisplayCurrencyType = "GBP";
      ICurrencyPrice gbpPrice = currency.GetCurrentPrice(101);
      Assert.AreEqual(CurrencyPriceType.Transactional, gbpPrice.Type);
      text = currency.PriceText(gbpPrice);
    }

    [TestMethod]
    public void GetPromoPrice()
    {
      var container = SetContexts(1, "77311", false, true);// regular shopper
      //SetContextsWithoutShopperPreferences(1, "865129");// ddc shopper
      IPromoDataProvider promoData = container.Resolve<IPromoDataProvider>();
      promoData.AddPromoCode("9999testa", "discountCode");
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();

      ICurrencyPrice currentPrice = currency.GetCurrentPrice(101);
      Assert.IsTrue(currentPrice.Price > 0);

      ICurrencyPrice currentPrice2 = currency.GetCurrentPrice(102);
      Assert.IsTrue(currentPrice2.Price > 0);

      bool sale = currency.IsProductOnSale(101);
      Assert.IsTrue(sale); // Promo used for unit test does not seem to be active in DEV any longer. Investigate later

      ICurrencyPrice currentPriceStd = currency.GetCurrentPrice(101, 0);
      Assert.IsTrue(currentPriceStd.Price > 0);

      ICurrencyPrice currentPriceByQuantity = currency.GetCurrentPriceByQuantity(101, 1);
      Assert.IsTrue(currentPriceByQuantity.Price > 0);
    }

    [TestMethod]
    public void MultiCurrencyCatalogReseller()
    {
      var container = SetContexts(1724, string.Empty);
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();

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
    public void JapaneseYen()
    {
      var container = SetContexts(1, string.Empty);
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();

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

      ICurrencyPrice p119 = new CurrencyPrice(119, currency.GetCurrencyInfo("USD"), CurrencyPriceType.Transactional);
      text = currency.PriceText(p119, false);
      Console.WriteLine(text);
    }

    [TestMethod]
    public void ConversionOverflow()
    {
      var container = SetContexts(1, string.Empty);
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();

      currency.SelectedDisplayCurrencyType = "INR";
      ICurrencyPrice pMillion = new CurrencyPrice(100000000, currency.GetCurrencyInfo("USD"), CurrencyPriceType.Transactional);
      string text = currency.PriceText(pMillion);
      Console.WriteLine(text);
    }

    [TestMethod]
    public void ConversionOverflowNegative()
    {
      var container = SetContexts(1, string.Empty);
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();

      currency.SelectedDisplayCurrencyType = "INR";
      ICurrencyPrice pMillion = new CurrencyPrice(-100000000, currency.GetCurrencyInfo("USD"), CurrencyPriceType.Transactional);
      string text = currency.PriceText(pMillion, PriceTextOptions.AllowNegativePrice);
      Console.WriteLine(text);
    }

    [TestMethod]
    public void BasicPricingWithoutShopperPreferenceProviderRegistered()
    {
      var container = SetContexts(1, "832652", false, false);
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();
      ICurrencyPrice price = currency.GetCurrentPrice(101);
    }

    [TestMethod]
    public void TestUSDConversion()
    {
      var container = SetContexts(1, string.Empty);
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();
      currency.SelectedDisplayCurrencyType = "INR";

      ICurrencyPrice icann = new CurrencyPrice(18, currency.GetCurrencyInfo("USD"), CurrencyPriceType.Transactional);
      string text = currency.PriceText(icann, false);
      Console.WriteLine(text);
    }

    [TestMethod]
    public void TestResellerMCP()
    {
      var container = SetContexts(281896, string.Empty);
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();
      ICurrencyInfo euros = currency.GetCurrencyInfo("EUR");
      bool isEurMCP = currency.IsCurrencyTransactionalForContext(euros);
      string defaultCurrencyType = currency.SelectedDisplayCurrencyType;

      Console.WriteLine(isEurMCP);
      Console.WriteLine(defaultCurrencyType);
    }

    [TestMethod]
    public void CurrencyDataList()
    {
      var container = SetContexts(1, string.Empty);

      CurrencyTypesRequestData requestData = new CurrencyTypesRequestData();
      CurrencyTypesResponseData responseData = (CurrencyTypesResponseData)Engine.Engine.ProcessRequest(requestData, CurrencyProviderEngineRequests.CurrencyTypesRequest);      

      int oldCount = responseData.Count;

      Assert.AreNotEqual(0, oldCount);

      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();
      int newCount = 0;
      foreach (ICurrencyInfo info in currency.CurrencyInfoList)
      {
        newCount++;
      }

      Assert.AreEqual(oldCount, newCount);
    }

    [TestMethod]
    public void CurrencyDataUSD()
    {
      var container = SetContexts(1, string.Empty);

      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();
      ICurrencyInfo usdInfoOld = currency.GetCurrencyInfo("USD");
      ICurrencyInfo usdInfoNew = currency.GetCurrencyInfo("USD");

      Assert.IsTrue(usdInfoOld == usdInfoNew);
      Assert.IsTrue(usdInfoOld.Equals(usdInfoNew));
    }

    [TestMethod]
    public void NullCurrencyInfo()
    {
      var container = SetContexts(1, string.Empty);
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();

      ICurrencyInfo usx = currency.GetCurrencyInfo("usx");
      Assert.IsNull(usx);
    }

    [TestMethod]
    public void ValidCurrencyInfo()
    {
      var container = SetContexts(1, string.Empty);
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();

      ICurrencyInfo usx = currency.GetValidCurrencyInfo("usx");
      Assert.IsNotNull(usx);
      Assert.AreEqual("USD", usx.CurrencyType);
    }

    [TestMethod]
    public void CreateICurrencyPrice()
    {
      var container = SetContexts(1, string.Empty);

      CurrencyTypesRequestData requestData = new CurrencyTypesRequestData();
      CurrencyTypesResponseData responseData = (CurrencyTypesResponseData)Engine.Engine.ProcessRequest(requestData, CurrencyProviderEngineRequests.CurrencyTypesRequest);
      ICurrencyPrice oldCreate = new CurrencyPrice(1295, responseData["USD"], CurrencyPriceType.Transactional);

      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();
      ICurrencyPrice newCreate = currency.NewCurrencyPrice(1295, currency.GetCurrencyInfo("USD"), CurrencyPriceType.Transactional);

      Assert.AreEqual(oldCreate.Price, newCreate.Price);

      string oldText = currency.PriceFormat(oldCreate);
      string newText = currency.PriceFormat(newCreate);

      Assert.IsTrue(oldText.Contains("$"));
      Assert.AreEqual(oldText, newText);
    }

    [TestMethod]
    public void CreateICurrencyPriceFromUSD()
    {
      var container = SetContexts(1, string.Empty);
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();

      ICurrencyInfo euro = currency.GetCurrencyInfo("EUR");
      ICurrencyPrice euroTenDollars = currency.NewCurrencyPriceFromUSD(1000, euro);

      ICurrencyPrice euroTenDollarsRounded = currency.NewCurrencyPriceFromUSD(1000, euro, CurrencyConversionRoundingType.Round);
      ICurrencyPrice euroTenDollarsFloor = currency.NewCurrencyPriceFromUSD(1000, euro, CurrencyConversionRoundingType.Floor);
      ICurrencyPrice euroTenDollarsCeiling = currency.NewCurrencyPriceFromUSD(1000, euro, CurrencyConversionRoundingType.Ceiling);

      Assert.AreEqual(euroTenDollars.Price, euroTenDollarsRounded.Price);
      Assert.IsTrue(euroTenDollarsRounded.Price <= euroTenDollarsCeiling.Price);
      Assert.IsTrue(euroTenDollarsRounded.Price >= euroTenDollarsFloor.Price);
    }

    [TestMethod]
    public void PriceFormatDefault()
    {
      var container = SetContexts(1, string.Empty);
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();

      ICurrencyPrice price = currency.NewCurrencyPriceFromUSD(9876);

      string text1 = currency.PriceFormat(price);
      string text2 = currency.PriceFormat(price, false, false);

      Assert.AreEqual(text1, text2);
      Assert.IsTrue(text1.Contains(price.CurrencyInfo.SymbolHtml));
      Assert.IsTrue(text1.Contains(price.CurrencyInfo.DecimalSeparator));
    }

    [TestMethod]
    public void PriceFormatDefaultNegativeMinus()
    {
      var container = SetContexts(1, string.Empty);
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();

      ICurrencyPrice price = currency.NewCurrencyPriceFromUSD(-9876);

      string text1 = currency.PriceFormat(price);
      string text2 = currency.PriceFormat(price, false, false, CurrencyNegativeFormat.Minus);

      Assert.AreEqual(text1, text2);
      Assert.IsTrue(text1.StartsWith("-"));
    }

    [TestMethod]
    public void PriceFormatDefaultNegativeParanthesisIgnored()
    {
      var container = SetContexts(1, string.Empty);
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();

      ICurrencyPrice price = currency.NewCurrencyPriceFromUSD(-9876);

      string text1 = currency.PriceFormat(price, PriceFormatOptions.NegativeParentheses);
      string text2 = currency.PriceFormat(price, false, false, CurrencyNegativeFormat.Parentheses);

      Assert.AreEqual(text1, text2);
      Assert.IsFalse(text1.StartsWith("("));
      Assert.IsFalse(text1.EndsWith(")"));
    }

    [TestMethod]
    public void PriceFormatDropDecimal()
    {
      var container = SetContexts(1, string.Empty);
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();

      ICurrencyPrice price = currency.NewCurrencyPriceFromUSD(9876);

      string text1 = currency.PriceFormat(price, PriceFormatOptions.DropDecimal);
      string text2 = currency.PriceFormat(price, true, false);

      Assert.AreEqual(text1, text2);
      Assert.IsTrue(text1.Contains(price.CurrencyInfo.SymbolHtml));
      Assert.IsFalse(text1.Contains(price.CurrencyInfo.DecimalSeparator));
    }

    [TestMethod]
    public void PriceFormatDropSymbol()
    {
      var container = SetContexts(1, string.Empty);
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();

      ICurrencyPrice price = currency.NewCurrencyPriceFromUSD(9876);

      string text1 = currency.PriceFormat(price, PriceFormatOptions.DropSymbol);
      string text2 = currency.PriceFormat(price, false, true);

      Assert.AreEqual(text1, text2);
      Assert.IsFalse(text1.Contains(price.CurrencyInfo.SymbolHtml));
      Assert.IsTrue(text1.Contains(price.CurrencyInfo.DecimalSeparator));
    }

    [TestMethod]
    public void PriceFormatDropSymbolAndDecimal()
    {
      var container = SetContexts(1, string.Empty);
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();

      ICurrencyPrice price = currency.NewCurrencyPriceFromUSD(9876);

      string text1 = currency.PriceFormat(price, PriceFormatOptions.DropSymbol | PriceFormatOptions.DropDecimal);
      string text2 = currency.PriceFormat(price, true, true);

      Assert.AreEqual(text1, text2);
      Assert.IsFalse(text1.Contains(price.CurrencyInfo.SymbolHtml));
      Assert.IsFalse(text1.Contains(price.CurrencyInfo.DecimalSeparator));
    }

    [TestMethod]
    public void PriceTextDefault()
    {
      var container = SetContexts(1, string.Empty);
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();
      currency.SelectedDisplayCurrencyType = "SGD";

      ICurrencyPrice price = currency.NewCurrencyPriceFromUSD(1000);

      if (currency.SelectedDisplayCurrencyInfo != currency.SelectedTransactionalCurrencyInfo)
      {
        Assert.AreEqual("USD", price.CurrencyInfo.CurrencyType);
      }

      string text1 = currency.PriceText(price);
      string text2 = currency.PriceText(price, false);
      string text3 = currency.PriceText(price, false, false);
      string text4 = currency.PriceText(price, false, false, false);
      string text5 = currency.PriceText(price, false, false, false, CurrencyNegativeFormat.NegativeNotAllowed);

      Assert.IsTrue(text1.Contains(currency.SelectedDisplayCurrencyInfo.SymbolHtml));

      Assert.AreEqual(text1, text2);
      Assert.AreEqual(text1, text3);
      Assert.AreEqual(text1, text4);
      Assert.AreEqual(text1, text5);
    }

    [TestMethod]
    public void PriceTextMask()
    {
      var container = SetContexts(1, string.Empty);
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();
      currency.SelectedDisplayCurrencyType = "USD";

      ICurrencyPrice price = currency.NewCurrencyPriceFromUSD(1000);
      
      string text1 = currency.PriceText(price, PriceTextOptions.MaskPrices);
      string text2 = currency.PriceText(price, true);
      string text3 = currency.PriceText(price, true, false);
      string text4 = currency.PriceText(price, true, false, false);
      string text5 = currency.PriceText(price, true, false, false, CurrencyNegativeFormat.NegativeNotAllowed);

      Assert.IsTrue(text1.Contains(currency.SelectedDisplayCurrencyInfo.SymbolHtml));
      Assert.IsTrue(text1.Contains("X"));

      Assert.AreEqual(text1, text2);
      Assert.AreEqual(text1, text3);
      Assert.AreEqual(text1, text4);
      Assert.AreEqual(text1, text5);
    }

    [TestMethod]
    public void PriceTextNotOffered()
    {
      var container = SetContexts(1, string.Empty);
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();
      currency.SelectedDisplayCurrencyType = "USD";

      ICurrencyPrice price = currency.NewCurrencyPriceFromUSD(-1000);

      string text1 = currency.PriceText(price);
      string text2 = currency.PriceText(price, false);
      string text3 = currency.PriceText(price, false, false);
      string text4 = currency.PriceText(price, false, false, false);
      string text5 = currency.PriceText(price, false, false, false, CurrencyNegativeFormat.NegativeNotAllowed);
      string text6 = currency.PriceText(price, false, false, false, "What!");

      Assert.IsTrue(!text1.Contains(currency.SelectedDisplayCurrencyInfo.SymbolHtml));
      Assert.IsTrue(text1.Contains("not offer"));

      Assert.AreEqual(text1, text2);
      Assert.AreEqual(text1, text3);
      Assert.AreEqual(text1, text4);
      Assert.AreEqual(text1, text5);
    }

    [TestMethod]
    public void PriceTextNegativeAllowed()
    {
      var container = SetContexts(1, string.Empty);
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();
      currency.SelectedDisplayCurrencyType = "USD";

      ICurrencyPrice price = currency.NewCurrencyPriceFromUSD(-1000);

      string text1 = currency.PriceText(price, PriceTextOptions.AllowNegativePrice);
      string text5 = currency.PriceText(price, false, false, false, CurrencyNegativeFormat.Minus);

      Assert.IsTrue(text1.Contains(currency.SelectedDisplayCurrencyInfo.SymbolHtml));
      Assert.IsTrue(text1.Contains("-"));

      Assert.AreEqual(text1, text5);
    }

    [TestMethod]
    public void ConvertPriceFromTransactionalUSD()
    {
      var container = SetContexts(1, string.Empty);
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();

      ICurrencyInfo usdInfo = currency.GetCurrencyInfo("USD");
      ICurrencyInfo euroInfo = currency.GetCurrencyInfo("EUR");

      ICurrencyPrice price = currency.NewCurrencyPrice(1000, usdInfo, CurrencyPriceType.Transactional);

      ICurrencyPrice convertedStaticDefault = CurrencyProvider.ConvertPrice(price, euroInfo);
      ICurrencyPrice convertedDefault = currency.ConvertPrice(price, euroInfo);

      Assert.IsTrue(convertedDefault.CurrencyInfo.Equals(euroInfo));
      Assert.AreEqual(convertedStaticDefault.Price, convertedDefault.Price);
    }

    [TestMethod]
    public void ConvertPriceFromTransactionalUSDwithFloor()
    {
      var container = SetContexts(1, string.Empty);
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();

      ICurrencyInfo usdInfo = currency.GetCurrencyInfo("USD");
      ICurrencyInfo euroInfo = currency.GetCurrencyInfo("EUR");

      ICurrencyPrice price = currency.NewCurrencyPrice(1000, usdInfo, CurrencyPriceType.Transactional);

      ICurrencyPrice convertedStaticDefault = CurrencyProvider.ConvertPrice(price, euroInfo, ConversionRoundingType.Floor);
      ICurrencyPrice convertedDefault = currency.ConvertPrice(price, euroInfo, CurrencyConversionRoundingType.Floor);

      Assert.IsTrue(convertedDefault.CurrencyInfo.Equals(euroInfo));
      Assert.AreEqual(convertedStaticDefault.Price, convertedDefault.Price);
    }

    [TestMethod]
    public void ConvertPriceFromTransactionalUSDwithCeiling()
    {
      var container = SetContexts(1, string.Empty);
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();

      ICurrencyInfo usdInfo = currency.GetCurrencyInfo("USD");
      ICurrencyInfo euroInfo = currency.GetCurrencyInfo("EUR");

      ICurrencyPrice price = currency.NewCurrencyPrice(1000, usdInfo, CurrencyPriceType.Transactional);

      ICurrencyPrice convertedStaticDefault = CurrencyProvider.ConvertPrice(price, euroInfo, ConversionRoundingType.Ceiling);
      ICurrencyPrice convertedDefault = currency.ConvertPrice(price, euroInfo, CurrencyConversionRoundingType.Ceiling);

      Assert.IsTrue(convertedDefault.CurrencyInfo.Equals(euroInfo));
      Assert.AreEqual(convertedStaticDefault.Price, convertedDefault.Price);
    }

    [TestMethod]
    public void ConvertPriceFromTransactionalEUR()
    {
      var container = SetContexts(1, string.Empty);
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();

      ICurrencyInfo usdInfo = currency.GetCurrencyInfo("USD");
      ICurrencyInfo euroInfo = currency.GetCurrencyInfo("EUR");

      ICurrencyPrice price = currency.NewCurrencyPrice(1000, euroInfo, CurrencyPriceType.Transactional);

      ICurrencyPrice convertedStaticDefault = CurrencyProvider.ConvertPrice(price, usdInfo);
      ICurrencyPrice convertedDefault = currency.ConvertPrice(price, usdInfo);

      Assert.IsTrue(convertedDefault.CurrencyInfo.Equals(usdInfo));
      Assert.IsFalse(convertedStaticDefault.CurrencyInfo.Equals(usdInfo));
    }

    [TestMethod]
    public void ConvertPriceFromConvertedUSD()
    {
      var container = SetContexts(1, string.Empty);
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();

      ICurrencyInfo usdInfo = currency.GetCurrencyInfo("USD");
      ICurrencyInfo euroInfo = currency.GetCurrencyInfo("EUR");

      ICurrencyPrice price = currency.NewCurrencyPrice(1000, usdInfo, CurrencyPriceType.Converted);

      ICurrencyPrice convertedStaticDefault = CurrencyProvider.ConvertPrice(price, euroInfo);
      ICurrencyPrice convertedDefault = currency.ConvertPrice(price, euroInfo);

      Assert.IsTrue(convertedDefault.CurrencyInfo.Equals(usdInfo));
      Assert.AreEqual(convertedStaticDefault.Price, convertedDefault.Price);
    }

    [TestMethod]
    public void ConvertPriceFromConvertedEUR()
    {
      var container = SetContexts(1, string.Empty);
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();

      ICurrencyInfo usdInfo = currency.GetCurrencyInfo("USD");
      ICurrencyInfo euroInfo = currency.GetCurrencyInfo("EUR");

      ICurrencyPrice price = currency.NewCurrencyPrice(1000, euroInfo, CurrencyPriceType.Converted);

      ICurrencyPrice convertedStaticDefault = CurrencyProvider.ConvertPrice(price, usdInfo);
      ICurrencyPrice convertedDefault = currency.ConvertPrice(price, usdInfo);

      Assert.IsTrue(convertedDefault.CurrencyInfo.Equals(usdInfo));
      Assert.IsFalse(convertedStaticDefault.CurrencyInfo.Equals(usdInfo));
    }

    [TestMethod]
    public void GetListPrice()
    {
      var container = SetContexts(1, string.Empty);
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();
      currency.SelectedDisplayCurrencyType = "USD";

      ICurrencyInfo usdInfo = currency.GetCurrencyInfo("USD");
      ICurrencyInfo euroInfo = currency.GetCurrencyInfo("EUR");

      ICurrencyPrice euroPrice = currency.GetListPrice(101, transactionCurrency: euroInfo);
      ICurrencyPrice usdPrice = currency.GetListPrice(101);

      ICurrencyPrice euroPriceDDC = currency.GetListPrice(101, 8, euroInfo);
      ICurrencyPrice usdPriceDDC = currency.GetListPrice(101, 8);

      Assert.IsTrue(euroPrice.CurrencyInfo.Equals(euroInfo));
      Assert.IsTrue(usdPrice.CurrencyInfo.Equals(usdInfo));

      var requestData = new ListPriceRequestData(101, 1, 0, "USD");
      var responseData = (ListPriceResponseData)Engine.Engine.ProcessRequest(requestData, CurrencyProviderEngineRequests.ListPriceRequest);
      Assert.AreEqual(responseData.Price, usdPrice.Price);

      requestData = new ListPriceRequestData(101, 1, 0, "EUR");
      responseData = (ListPriceResponseData)Engine.Engine.ProcessRequest(requestData, CurrencyProviderEngineRequests.ListPriceRequest);      
      Assert.AreEqual(responseData.Price, euroPrice.Price);
    }

    [TestMethod]
    public void GetListPriceFakeCurrencyInfoReturnsUSDPrice()
    {
      var container = SetContexts(1, string.Empty);
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();
      currency.SelectedDisplayCurrencyType = "USD";

      var usdPrice = currency.GetListPrice(101, 1);

      string currencyItem = "<currency gdshop_currencyType=\"XXXX\" isTransactional=\"1\" />";
      XElement currencyElement = XElement.Parse(currencyItem);
      MockCurrencyInfo fakeCurrencyInfo = MockCurrencyInfo.FromCacheElement(currencyElement);

      ICurrencyPrice fakePrice = currency.GetListPrice(101, 1, fakeCurrencyInfo);
      Assert.AreEqual(usdPrice.Price, fakePrice.Price);
    }

    [TestMethod]
    public void GetCurrentPrice()
    {
      var container = SetContexts(1, string.Empty);
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();
      currency.SelectedDisplayCurrencyType = "USD";

      ICurrencyInfo usdInfo = currency.GetCurrencyInfo("USD");
      ICurrencyInfo euroInfo = currency.GetCurrencyInfo("EUR");

      ICurrencyPrice euroPrice = currency.GetCurrentPrice(101, transactionCurrency: euroInfo);
      ICurrencyPrice usdPrice = currency.GetCurrentPrice(101);

      ICurrencyPrice euroPriceDDC = currency.GetCurrentPrice(101, 8, euroInfo);
      ICurrencyPrice usdPriceDDC = currency.GetCurrentPrice(101, 8);

      Assert.IsTrue(euroPrice.CurrencyInfo.Equals(euroInfo));
      Assert.IsTrue(usdPrice.CurrencyInfo.Equals(usdInfo));

      var requestData = new PromoPriceRequestData(101, 1, 1, 0, "USD");
      var responseData = (PromoPriceResponseData)Engine.Engine.ProcessRequest(requestData, CurrencyProviderEngineRequests.PromoPriceRequest);
      Assert.AreEqual(responseData.Price, usdPrice.Price);

      requestData = new PromoPriceRequestData(101, 1, 1, 0, "EUR");
      responseData = (PromoPriceResponseData)Engine.Engine.ProcessRequest(requestData, CurrencyProviderEngineRequests.PromoPriceRequest);      
      Assert.AreEqual(responseData.Price, euroPrice.Price);
    }

    [TestMethod]
    public void GetCurrentPriceFakeCurrencyInfoReturnsUSDPrice()
    {
      var container = SetContexts(1, string.Empty);
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();
      currency.SelectedDisplayCurrencyType = "USD";

      var usdPrice = currency.GetCurrentPrice(101, 1);

      string currencyItem = "<currency gdshop_currencyType=\"XXXX\" isTransactional=\"1\" />";
      XElement currencyElement = XElement.Parse(currencyItem);
      MockCurrencyInfo fakeCurrencyInfo = MockCurrencyInfo.FromCacheElement(currencyElement);

      ICurrencyPrice fakePrice = currency.GetCurrentPrice(101, 1, fakeCurrencyInfo);
      Assert.AreEqual(usdPrice.Price, fakePrice.Price);
    }

    [TestMethod]
    public void GetCurrentPriceWithAllReqDataAndNoPricingProviderCodeReturnsNormalPrice()
    {
      var container = SetContexts(1, string.Empty);
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();
      
      ICurrencyInfo usdInfo = currency.GetCurrencyInfo("USD");
      ICurrencyPrice usdPrice = currency.GetCurrentPrice(58, 0, usdInfo, "valid");

      Assert.AreNotEqual(MockPriceProvider.USD_PFID58_PRICE, usdPrice.Price);
    }

    [TestMethod]
    public void GetCurrentPriceWithInvalidIscCodeReturnsNormalPrice()
    {
      var container = SetContexts(1, string.Empty);
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();

      ICurrencyInfo usdInfo = currency.GetCurrencyInfo("USD");
      ICurrencyPrice usdPrice = currency.GetCurrentPrice(58, 0, usdInfo, "invalid");

      Assert.AreNotEqual(MockPriceProvider.USD_PFID58_PRICE, usdPrice.Price);
    }

    [TestMethod]
    public void GetCurrentPriceWithValidIscCodeAndUnmatchedPfidReturnsNormalPrice()
    {
      var container = SetContexts(1, string.Empty);
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();

      ICurrencyInfo usdInfo = currency.GetCurrencyInfo("USD");
      ICurrencyPrice usdPrice = currency.GetCurrentPrice(200, 0, usdInfo, "valid");

      Assert.AreNotEqual(MockPriceProvider.USD_PFID58_PRICE, usdPrice.Price);
    }

    [TestMethod]
    public void GetCurrentPriceWithValidIscCodeMatchedPfidAndMissingCurrencyReturnsNormalPrice()
    {
      var container = SetContexts(1, string.Empty);
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();

      ICurrencyInfo mxnInfo = currency.GetCurrencyInfo("MXN");
      ICurrencyPrice mxnPrice = currency.GetCurrentPrice(58, 0, mxnInfo, "valid");

      Assert.AreNotEqual(MockPriceProvider.USD_PFID58_PRICE, mxnPrice.Price);
    }

    [TestMethod]
    public void GetCurrentPriceWithValidIscCodeMatchedPfid58AndUsdCurrencyReturnsSpecialPrice()
    {
      var container = SetContexts(1, string.Empty, false, false, true);
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();

      ICurrencyInfo usdInfo = currency.GetCurrencyInfo("USD");

      //  This forces the use of mock EcommPricing triplet request
      CurrencyProviderEngineRequests.ValidateNonOrderRequest = (CurrencyProviderEngineRequests.ValidateNonOrderRequest * 1000) + 1;
      CurrencyProviderEngineRequests.PriceEstimateRequest = (CurrencyProviderEngineRequests.PriceEstimateRequest * 1000) + 1;
      try
      {
        ICurrencyPrice usdPrice = currency.GetCurrentPrice(58, 0, usdInfo, "valid");

        Assert.AreEqual(MockPriceProvider.USD_PFID58_PRICE, usdPrice.Price);
      }
      finally
      {
        //  Stop using the mock EcommPricing triplet request
        CurrencyProviderEngineRequests.ValidateNonOrderRequest = (CurrencyProviderEngineRequests.ValidateNonOrderRequest - 1) / 1000;
        CurrencyProviderEngineRequests.PriceEstimateRequest = (CurrencyProviderEngineRequests.PriceEstimateRequest - 1) / 1000;
      }
    }

    [TestMethod]
    public void GetCurrentPriceWithValidIscCodeMatchedPfid58AndGbpCurrencyReturnsSpecialPrice()
    {
      var container = SetContexts(1, string.Empty, false, false, true);
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();

      ICurrencyInfo gbpInfo = currency.GetCurrencyInfo("GBP");

      //  This forces the use of mock EcommPricing triplet request
      CurrencyProviderEngineRequests.ValidateNonOrderRequest = (CurrencyProviderEngineRequests.ValidateNonOrderRequest * 1000) + 1;
      CurrencyProviderEngineRequests.PriceEstimateRequest = (CurrencyProviderEngineRequests.PriceEstimateRequest * 1000) + 1;
      try
      {
        ICurrencyPrice gbpPrice = currency.GetCurrentPrice(58, 0, gbpInfo, "valid");

        Assert.AreEqual(MockPriceProvider.GBP_PFID58_PRICE, gbpPrice.Price);
      }
      finally
      {
        //  Stop using the mock EcommPricing triplet request
        CurrencyProviderEngineRequests.ValidateNonOrderRequest = (CurrencyProviderEngineRequests.ValidateNonOrderRequest - 1) / 1000;
        CurrencyProviderEngineRequests.PriceEstimateRequest = (CurrencyProviderEngineRequests.PriceEstimateRequest - 1) / 1000;
      }
    }

    [TestMethod]
    public void GetCurrentPriceWithValidIscCodeMatchedPfid58AndInrCurrencyReturnsSpecialPrice()
    {
      var container = SetContexts(1, string.Empty, false, false, true);
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();

      ICurrencyInfo inrInfo = currency.GetCurrencyInfo("INR");

      //  This forces the use of mock EcommPricing triplet request
      CurrencyProviderEngineRequests.ValidateNonOrderRequest = (CurrencyProviderEngineRequests.ValidateNonOrderRequest * 1000) + 1;
      CurrencyProviderEngineRequests.PriceEstimateRequest = (CurrencyProviderEngineRequests.PriceEstimateRequest * 1000) + 1;
      try
      {
        ICurrencyPrice inrPrice = currency.GetCurrentPrice(58, 0, inrInfo, "valid");

        Assert.AreEqual(MockPriceProvider.INR_PFID58_PRICE, inrPrice.Price);
      }
      finally
      {
        //  Stop using the mock EcommPricing triplet request
        CurrencyProviderEngineRequests.ValidateNonOrderRequest = (CurrencyProviderEngineRequests.ValidateNonOrderRequest - 1) / 1000;
        CurrencyProviderEngineRequests.PriceEstimateRequest = (CurrencyProviderEngineRequests.PriceEstimateRequest - 1) / 1000;
      }
    }

    [TestMethod]
    public void GetCurrentPriceWithValidIscCodeMatchedPfid101AndUsdCurrencyReturnsSpecialPrice()
    {
      var container = SetContexts(1, string.Empty, false, false, true);
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();

      ICurrencyInfo usdInfo = currency.GetCurrencyInfo("USD");

      //  This forces the use of mock EcommPricing triplet request
      CurrencyProviderEngineRequests.ValidateNonOrderRequest = (CurrencyProviderEngineRequests.ValidateNonOrderRequest * 1000) + 1;
      CurrencyProviderEngineRequests.PriceEstimateRequest = (CurrencyProviderEngineRequests.PriceEstimateRequest * 1000) + 1;
      try
      {
        ICurrencyPrice usdPrice = currency.GetCurrentPrice(101, 0, usdInfo, "valid");
        Assert.AreEqual(MockPriceProvider.USD_PFID101_PRICE, usdPrice.Price);
      }
      finally
      {
        //  Stop using the mock EcommPricing triplet request
        CurrencyProviderEngineRequests.ValidateNonOrderRequest = (CurrencyProviderEngineRequests.ValidateNonOrderRequest - 1) / 1000;
        CurrencyProviderEngineRequests.PriceEstimateRequest = (CurrencyProviderEngineRequests.PriceEstimateRequest - 1) / 1000;
      }
    }

    [TestMethod]
    public void GetCurrentPriceWithValidIscCodeMatchedPfid101AndGbpCurrencyReturnsSpecialPrice()
    {
      var container = SetContexts(1, string.Empty, false, false, true);
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();

      ICurrencyInfo gbpInfo = currency.GetCurrencyInfo("GBP");

      //  This forces the use of mock EcommPricing triplet request
      CurrencyProviderEngineRequests.ValidateNonOrderRequest = (CurrencyProviderEngineRequests.ValidateNonOrderRequest * 1000) + 1;
      CurrencyProviderEngineRequests.PriceEstimateRequest = (CurrencyProviderEngineRequests.PriceEstimateRequest * 1000) + 1;
      try
      {
        ICurrencyPrice gbpPrice = currency.GetCurrentPrice(101, 0, gbpInfo, "valid");

        Assert.AreEqual(MockPriceProvider.GBP_PFID101_PRICE, gbpPrice.Price);
      }
      finally
      {
        //  Stop using the mock EcommPricing triplet request
        CurrencyProviderEngineRequests.ValidateNonOrderRequest = (CurrencyProviderEngineRequests.ValidateNonOrderRequest - 1) / 1000;
        CurrencyProviderEngineRequests.PriceEstimateRequest = (CurrencyProviderEngineRequests.PriceEstimateRequest - 1) / 1000;
      }
    }

    [TestMethod]
    public void GetCurrentPriceWithValidIscCodeMatchedPfid101AndInrCurrencyReturnsSpecialPrice()
    {
      var container = SetContexts(1, string.Empty, false, false, true);
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();

      ICurrencyInfo inrInfo = currency.GetCurrencyInfo("INR");

      //  This forces the use of mock EcommPricing triplet request
      CurrencyProviderEngineRequests.ValidateNonOrderRequest = (CurrencyProviderEngineRequests.ValidateNonOrderRequest * 1000) + 1;
      CurrencyProviderEngineRequests.PriceEstimateRequest = (CurrencyProviderEngineRequests.PriceEstimateRequest * 1000) + 1;
      try
      {
        ICurrencyPrice inrPrice = currency.GetCurrentPrice(101, 0, inrInfo, "valid");

        Assert.AreEqual(MockPriceProvider.INR_PFID101_PRICE, inrPrice.Price);
      }
      finally
      {
        //  Stop using the mock EcommPricing triplet request
        CurrencyProviderEngineRequests.ValidateNonOrderRequest = (CurrencyProviderEngineRequests.ValidateNonOrderRequest - 1) / 1000;
        CurrencyProviderEngineRequests.PriceEstimateRequest = (CurrencyProviderEngineRequests.PriceEstimateRequest - 1) / 1000;
      }
    }

    [TestMethod]
    public void IsProductOnSaleReturnsFalseForPfid101ValidIscAndMxnCurrency()
    {
      var container = SetContexts(1, string.Empty);
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();
      ICurrencyInfo mxnInfo = currency.GetCurrencyInfo("MXN");

      bool result = currency.IsProductOnSale(101, 0, mxnInfo, "valid");
      Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsProductOnSaleReturnsTrueForPfid101AndIscCodeValid()
    {
      var container = SetContexts(1, string.Empty, false, false, true);
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();
      ICurrencyInfo usdInfo = currency.GetCurrencyInfo("USD");

      //  This forces the use of mock EcommPricing triplet request
      CurrencyProviderEngineRequests.ValidateNonOrderRequest = (CurrencyProviderEngineRequests.ValidateNonOrderRequest * 1000) + 1;
      CurrencyProviderEngineRequests.PriceEstimateRequest = (CurrencyProviderEngineRequests.PriceEstimateRequest * 1000) + 1;
      try
      {
        bool result = currency.IsProductOnSale(101, 0, usdInfo, "valid");
        Assert.IsTrue(result);
      }
      finally
      {
        //  Stop using the mock EcommPricing triplet request
        CurrencyProviderEngineRequests.ValidateNonOrderRequest = (CurrencyProviderEngineRequests.ValidateNonOrderRequest-1)/1000;
        CurrencyProviderEngineRequests.PriceEstimateRequest = (CurrencyProviderEngineRequests.PriceEstimateRequest - 1) / 1000; 
      }
    }

    [TestMethod]
    public void GetCurrentPriceByQuantity()
    {
      var container = SetContexts(1, string.Empty);
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();
      currency.SelectedDisplayCurrencyType = "USD";

      ICurrencyInfo usdInfo = currency.GetCurrencyInfo("USD");
      ICurrencyInfo euroInfo = currency.GetCurrencyInfo("EUR");

      ICurrencyPrice euroPrice = currency.GetCurrentPriceByQuantity(58, 12, transactionCurrency: euroInfo);
      ICurrencyPrice usdPrice = currency.GetCurrentPriceByQuantity(58, 12);

      ICurrencyPrice euroPriceCC = currency.GetCurrentPriceByQuantity(58, 12, 16, euroInfo);
      ICurrencyPrice usdPriceCC = currency.GetCurrentPriceByQuantity(58, 12, 16);

      Assert.IsTrue(euroPrice.CurrencyInfo.Equals(euroInfo));
      Assert.IsTrue(usdPrice.CurrencyInfo.Equals(usdInfo));

      var requestData = new PromoPriceRequestData(58, 1, 12, 0, "USD");
      var responseData = (PromoPriceResponseData)Engine.Engine.ProcessRequest(requestData, CurrencyProviderEngineRequests.PromoPriceRequest);
      Assert.AreEqual(responseData.Price, usdPrice.Price);

      requestData = new PromoPriceRequestData(58, 1, 12, 0, "EUR");
      responseData = (PromoPriceResponseData)Engine.Engine.ProcessRequest(requestData, CurrencyProviderEngineRequests.PromoPriceRequest);
      Assert.AreEqual(responseData.Price, euroPrice.Price);
    }

    [TestMethod]
    public void GetCurrentPriceByQuantityFakeCurrencyInfoReturnsUSDPrice()
    {
      var container = SetContexts(1, string.Empty);
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();
      currency.SelectedDisplayCurrencyType = "USD";

      var usdPrice = currency.GetCurrentPriceByQuantity(101, 1111, 1);

      string currencyItem = "<currency gdshop_currencyType=\"XXXX\" isTransactional=\"1\" />";
      XElement currencyElement = XElement.Parse(currencyItem);
      MockCurrencyInfo fakeCurrencyInfo = MockCurrencyInfo.FromCacheElement(currencyElement);

      ICurrencyPrice fakePrice = currency.GetCurrentPriceByQuantity(101, 1111, 1, fakeCurrencyInfo);
      Assert.AreEqual(usdPrice.Price, fakePrice.Price);
    }

    [TestMethod]
    public void PriceGroupsNoLocalizationProviderReturnsZeroPriceGroupIdPricing()
    {
      var container = SetContexts(1, "833437");
      
      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();
      currency.SelectedDisplayCurrencyType = "USD";

      ICurrencyInfo usdInfo = currency.GetCurrencyInfo("USD");
      ICurrencyInfo euroInfo = currency.GetCurrencyInfo("EUR");

      ICurrencyPrice euroPrice = currency.GetListPrice(101, transactionCurrency: euroInfo);
      ICurrencyPrice usdPrice = currency.GetListPrice(101);

      ICurrencyPrice euroPriceDDC = currency.GetListPrice(101, 8, euroInfo);
      ICurrencyPrice usdPriceDDC = currency.GetListPrice(101, 8);

      Assert.IsTrue(euroPrice.CurrencyInfo.Equals(euroInfo));
      Assert.IsTrue(usdPrice.CurrencyInfo.Equals(usdInfo));

      var requestData = new ListPriceRequestData(101, 1, 0, "USD");
      var responseData = (ListPriceResponseData)Engine.Engine.ProcessRequest(requestData, CurrencyProviderEngineRequests.ListPriceRequest);
      Assert.AreEqual(responseData.Price, usdPrice.Price);

      requestData = new ListPriceRequestData(101, 1, 0, "EUR");
      responseData = (ListPriceResponseData)Engine.Engine.ProcessRequest(requestData, CurrencyProviderEngineRequests.ListPriceRequest);
      Assert.AreEqual(responseData.Price, euroPrice.Price);
    }

    [TestMethod]
    public void PriceGroupsInvalidCountrySiteReturnsZeroPriceGroupIdPricing()
    {
      MockProviderContainer container = (MockProviderContainer)SetContexts(1, "833437");

      container.SetData("Localization.CountrySiteInfo", (ICountrySite)null);

      container.RegisterProvider<ILocalizationProvider, MockLocalizationProvider>();

      ICurrencyProvider currency = container.Resolve<ICurrencyProvider>();
      currency.SelectedDisplayCurrencyType = "USD";

      ICurrencyInfo usdInfo = currency.GetCurrencyInfo("USD");
      ICurrencyInfo euroInfo = currency.GetCurrencyInfo("EUR");

      ICurrencyPrice euroPrice = currency.GetListPrice(101, transactionCurrency: euroInfo);
      ICurrencyPrice usdPrice = currency.GetListPrice(101);

      ICurrencyPrice euroPriceDDC = currency.GetListPrice(101, 8, euroInfo);
      ICurrencyPrice usdPriceDDC = currency.GetListPrice(101, 8);

      Assert.IsTrue(euroPrice.CurrencyInfo.Equals(euroInfo));
      Assert.IsTrue(usdPrice.CurrencyInfo.Equals(usdInfo));

      var requestData = new ListPriceRequestData(101, 1, 0, "USD");
      var responseData = (ListPriceResponseData)Engine.Engine.ProcessRequest(requestData, CurrencyProviderEngineRequests.ListPriceRequest);
      Assert.AreEqual(responseData.Price, usdPrice.Price);

      requestData = new ListPriceRequestData(101, 1, 0, "EUR");
      responseData = (ListPriceResponseData)Engine.Engine.ProcessRequest(requestData, CurrencyProviderEngineRequests.ListPriceRequest);
      Assert.AreEqual(responseData.Price, euroPrice.Price);
    }
  }
}
