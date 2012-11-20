using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.Currency;
using Atlantis.Framework.Providers.Interface.Preferences;
using Atlantis.Framework.Providers.Interface.PromoData;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Atlantis.Framework.Providers.PromoData;
using Atlantis.Framework.Testing.MockHttpContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Testing.MockProviders;
using System.Web;

namespace Atlantis.Framework.Providers.Currency.Tests
{
  [TestClass]
  public class CurrencyProviderTests
  {
    private void SetContexts(int privateLabelId, string shopperId)
    {
      SetContexts(privateLabelId, shopperId, true, false);
    }

    private void SetContexts(int privateLabelId, string shopperId, bool includeShopperPreferences, bool includePromoData)
    {
      MockHttpContext.SetMockHttpContext("default.aspx", "http://localhost/default.aspx", string.Empty);
      HttpProviderContainer.Instance.RegisterProvider<ISiteContext, MockSiteContext>();
      HttpProviderContainer.Instance.RegisterProvider<IShopperContext, MockShopperContext>();
      HttpProviderContainer.Instance.RegisterProvider<IManagerContext, MockNoManagerContext>();
      HttpProviderContainer.Instance.RegisterProvider<ICurrencyProvider, CurrencyProvider>();

      if (includeShopperPreferences)
      {
        HttpProviderContainer.Instance.RegisterProvider<IShopperPreferencesProvider, MockShopperPreference>();
      }

      if (includePromoData)
      {
        HttpProviderContainer.Instance.RegisterProvider<IPromoDataProvider, PromoDataProvider>();
      }

      HttpContext.Current.Items[MockSiteContextSettings.PrivateLabelId] = privateLabelId;
      IShopperContext shopperContext = HttpProviderContainer.Instance.Resolve<IShopperContext>();
      shopperContext.SetLoggedInShopper(shopperId);
    }

    [TestMethod]
    public void CurrencyInfoUSD()
    {
      ICurrencyInfo usd = CurrencyData.GetCurrencyInfo("usd");
      Assert.AreEqual("USD", usd.CurrencyType);
    }

    [TestMethod]
    public void CurrencyInfoDescriptions()
    {
      foreach (ICurrencyInfo item in CurrencyData.CurrencyInfoList)
      {
        Console.WriteLine(item.Description + " : " + item.DescriptionPlural + " : " + item.Symbol + " : " + item.SymbolHtml);
      }
    }

    [TestMethod]
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
    public void ConvertingIcannFees()
    {
      SetContexts(1, string.Empty);

      ICurrencyPrice usd18 = new CurrencyPrice(18, CurrencyData.GetCurrencyInfo("USD"), CurrencyPriceType.Transactional);
      ICurrencyPrice converted = CurrencyProvider.ConvertPrice(usd18, CurrencyData.GetCurrencyInfo("EUR"));
      Console.WriteLine("EUR=" + converted.Price.ToString());

      converted = CurrencyProvider.ConvertPrice(usd18, CurrencyData.GetCurrencyInfo("AUD"));
      Console.WriteLine("AUD=" + converted.Price.ToString());

      converted = CurrencyProvider.ConvertPrice(usd18, CurrencyData.GetCurrencyInfo("GBP"));
      Console.WriteLine("GBP=" + converted.Price.ToString());
    }

    [TestMethod]
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
    }

    [TestMethod]
    public void GetPromoPrice()
    {
      SetContexts(1, "77311", false, true);// regular shopper
      //SetContextsWithoutShopperPreferences(1, "865129");// ddc shopper
      IPromoDataProvider promoData = HttpProviderContainer.Instance.Resolve<IPromoDataProvider>();
      promoData.AddPromoCode("9999testa", "discountCode");
      ICurrencyProvider currency = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();

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
      SetContexts(1724, string.Empty);
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
    public void JapaneseYen()
    {
      SetContexts(1, string.Empty);
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
    public void ConversionOverflow()
    {
      SetContexts(1, string.Empty);
      ICurrencyProvider currency = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();

      currency.SelectedDisplayCurrencyType = "INR";
      ICurrencyPrice pMillion = new CurrencyPrice(100000000, CurrencyData.GetCurrencyInfo("USD"), CurrencyPriceType.Transactional);
      string text = currency.PriceText(pMillion);
      Console.WriteLine(text);
    }

    [TestMethod]
    public void ConversionOverflowNegative()
    {
      SetContexts(1, string.Empty);
      ICurrencyProvider currency = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();

      currency.SelectedDisplayCurrencyType = "INR";
      ICurrencyPrice pMillion = new CurrencyPrice(-100000000, CurrencyData.GetCurrencyInfo("USD"), CurrencyPriceType.Transactional);
      string text = currency.PriceText(pMillion, PriceTextOptions.AllowNegativePrice);
      Console.WriteLine(text);
    }


    [TestMethod]
    public void BasicPricingWithoutShopperPreferenceProviderRegistered()
    {
      SetContexts(1, "832652", false, false);
      ICurrencyProvider currency = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();
      ICurrencyPrice price = currency.GetCurrentPrice(101);
    }

    [TestMethod]
    public void TestUSDConversion()
    {
      SetContexts(1, string.Empty);
      ICurrencyProvider currency = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();
      currency.SelectedDisplayCurrencyType = "INR";

      ICurrencyPrice icann = new CurrencyPrice(18, CurrencyData.GetCurrencyInfo("USD"), CurrencyPriceType.Transactional);
      string text = currency.PriceText(icann, false);
      Console.WriteLine(text);
    }

    [TestMethod]
    public void TestResellerMCP()
    {
      SetContexts(281896, string.Empty);
      ICurrencyProvider currency = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();
      ICurrencyInfo euros = CurrencyData.GetCurrencyInfo("EUR");
      bool isEurMCP = currency.IsCurrencyTransactionalForContext(euros);
      string defaultCurrencyType = currency.SelectedDisplayCurrencyType;

      Console.WriteLine(isEurMCP);
      Console.WriteLine(defaultCurrencyType);
    }

    [TestMethod]
    public void CurrencyDataList()
    {
      SetContexts(1, string.Empty);

      int oldCount = 0;
      foreach (ICurrencyInfo info in CurrencyData.CurrencyInfoList)
      {
        oldCount++;
      }

      Assert.AreNotEqual(0, oldCount);

      ICurrencyProvider currency = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();
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
      SetContexts(1, string.Empty);

      ICurrencyInfo usdInfoOld = CurrencyData.GetCurrencyInfo("USD");
      ICurrencyProvider currency = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();
      ICurrencyInfo usdInfoNew = currency.GetCurrencyInfo("USD");

      Assert.IsTrue(usdInfoOld == usdInfoNew);
      Assert.IsTrue(usdInfoOld.Equals(usdInfoNew));
    }

    [TestMethod]
    public void NullCurrencyInfo()
    {
      SetContexts(1, string.Empty);
      ICurrencyProvider currency = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();

      ICurrencyInfo usx = currency.GetCurrencyInfo("usx");
      Assert.IsNull(usx);
    }

    [TestMethod]
    public void ValidCurrencyInfo()
    {
      SetContexts(1, string.Empty);
      ICurrencyProvider currency = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();

      ICurrencyInfo usx = currency.GetValidCurrencyInfo("usx");
      Assert.IsNotNull(usx);
      Assert.AreEqual("USD", usx.CurrencyType);
    }

    [TestMethod]
    public void CreateICurrencyPrice()
    {
      SetContexts(1, string.Empty);

      ICurrencyPrice oldCreate = new CurrencyPrice(1295, CurrencyData.GetCurrencyInfo("USD"), CurrencyPriceType.Transactional);

      ICurrencyProvider currency = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();
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
      SetContexts(1, string.Empty);
      ICurrencyProvider currency = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();

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
      SetContexts(1, string.Empty);
      ICurrencyProvider currency = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();

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
      SetContexts(1, string.Empty);
      ICurrencyProvider currency = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();

      ICurrencyPrice price = currency.NewCurrencyPriceFromUSD(-9876);

      string text1 = currency.PriceFormat(price);
      string text2 = currency.PriceFormat(price, false, false, CurrencyNegativeFormat.Minus);

      Assert.AreEqual(text1, text2);
      Assert.IsTrue(text1.StartsWith("-"));
    }

    [TestMethod]
    public void PriceFormatDefaultNegativeParanthesis()
    {
      SetContexts(1, string.Empty);
      ICurrencyProvider currency = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();

      ICurrencyPrice price = currency.NewCurrencyPriceFromUSD(-9876);

      string text1 = currency.PriceFormat(price, PriceFormatOptions.NegativeParentheses);
      string text2 = currency.PriceFormat(price, false, false, CurrencyNegativeFormat.Parentheses);

      Assert.AreEqual(text1, text2);
      Assert.IsTrue(text1.StartsWith("("));
      Assert.IsTrue(text1.EndsWith(")"));
    }

    [TestMethod]
    public void PriceFormatDropDecimal()
    {
      SetContexts(1, string.Empty);
      ICurrencyProvider currency = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();

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
      SetContexts(1, string.Empty);
      ICurrencyProvider currency = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();

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
      SetContexts(1, string.Empty);
      ICurrencyProvider currency = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();

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
      SetContexts(1, string.Empty);
      ICurrencyProvider currency = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();
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
      SetContexts(1, string.Empty);
      ICurrencyProvider currency = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();
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
      SetContexts(1, string.Empty);
      ICurrencyProvider currency = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();
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
      SetContexts(1, string.Empty);
      ICurrencyProvider currency = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();
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
      SetContexts(1, string.Empty);
      ICurrencyProvider currency = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();

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
      SetContexts(1, string.Empty);
      ICurrencyProvider currency = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();

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
      SetContexts(1, string.Empty);
      ICurrencyProvider currency = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();

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
      SetContexts(1, string.Empty);
      ICurrencyProvider currency = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();

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
      SetContexts(1, string.Empty);
      ICurrencyProvider currency = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();

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
      SetContexts(1, string.Empty);
      ICurrencyProvider currency = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();

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
      SetContexts(1, string.Empty);
      ICurrencyProvider currency = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();
      currency.SelectedDisplayCurrencyType = "USD";

      ICurrencyInfo usdInfo = currency.GetCurrencyInfo("USD");
      ICurrencyInfo euroInfo = currency.GetCurrencyInfo("EUR");

      ICurrencyPrice euroPrice = currency.GetListPrice(101, transactionCurrency: euroInfo);
      ICurrencyPrice usdPrice = currency.GetListPrice(101);

      ICurrencyPrice euroPriceDDC = currency.GetListPrice(101, 8, euroInfo);
      ICurrencyPrice usdPriceDDC = currency.GetListPrice(101, 8);

      Assert.IsTrue(euroPrice.CurrencyInfo.Equals(euroInfo));
      Assert.IsTrue(usdPrice.CurrencyInfo.Equals(usdInfo));

      int usdPriceDirect;
      bool estimate;
      bool success = DataCache.DataCache.GetListPriceEx(1, 101, 0, "USD", out usdPriceDirect, out estimate);

      Assert.AreEqual(usdPriceDirect, usdPrice.Price);

      int euroPriceDirect;
      success = DataCache.DataCache.GetListPriceEx(1, 101, 0, "EUR", out euroPriceDirect, out estimate);

      Assert.AreEqual(euroPriceDirect, euroPrice.Price);
    }

    [TestMethod]
    public void GetCurrentPrice()
    {
      SetContexts(1, string.Empty);
      ICurrencyProvider currency = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();
      currency.SelectedDisplayCurrencyType = "USD";

      ICurrencyInfo usdInfo = currency.GetCurrencyInfo("USD");
      ICurrencyInfo euroInfo = currency.GetCurrencyInfo("EUR");

      ICurrencyPrice euroPrice = currency.GetCurrentPrice(101, transactionCurrency: euroInfo);
      ICurrencyPrice usdPrice = currency.GetCurrentPrice(101);

      ICurrencyPrice euroPriceDDC = currency.GetCurrentPrice(101, 8, euroInfo);
      ICurrencyPrice usdPriceDDC = currency.GetCurrentPrice(101, 8);

      Assert.IsTrue(euroPrice.CurrencyInfo.Equals(euroInfo));
      Assert.IsTrue(usdPrice.CurrencyInfo.Equals(usdInfo));

      int usdPriceDirect;
      bool estimate;
      bool success = DataCache.DataCache.GetPromoPriceEx(1, 101, 0, "USD", out usdPriceDirect, out estimate);

      Assert.AreEqual(usdPriceDirect, usdPrice.Price);

      int euroPriceDirect;
      success = DataCache.DataCache.GetPromoPriceEx(1, 101, 0, "EUR", out euroPriceDirect, out estimate);

      Assert.AreEqual(euroPriceDirect, euroPrice.Price);
    }

    [TestMethod]
    public void GetCurrentPriceByQuantity()
    {
      SetContexts(1, string.Empty);
      ICurrencyProvider currency = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();
      currency.SelectedDisplayCurrencyType = "USD";

      ICurrencyInfo usdInfo = currency.GetCurrencyInfo("USD");
      ICurrencyInfo euroInfo = currency.GetCurrencyInfo("EUR");

      ICurrencyPrice euroPrice = currency.GetCurrentPriceByQuantity(58, 12, transactionCurrency: euroInfo);
      ICurrencyPrice usdPrice = currency.GetCurrentPriceByQuantity(58, 12);

      ICurrencyPrice euroPriceCC = currency.GetCurrentPriceByQuantity(58, 12, 16, euroInfo);
      ICurrencyPrice usdPriceCC = currency.GetCurrentPriceByQuantity(58, 12, 16);

      Assert.IsTrue(euroPrice.CurrencyInfo.Equals(euroInfo));
      Assert.IsTrue(usdPrice.CurrencyInfo.Equals(usdInfo));

      int usdPriceDirect;
      bool estimate;
      bool success = DataCache.DataCache.GetPromoPriceByQtyEx(1, 58, 0, 12, "USD", out usdPriceDirect, out estimate);

      Assert.AreEqual(usdPriceDirect, usdPrice.Price);

      int euroPriceDirect;
      success = DataCache.DataCache.GetPromoPriceByQtyEx(1, 58, 0, 12, "EUR", out euroPriceDirect, out estimate);

      Assert.AreEqual(euroPriceDirect, euroPrice.Price);
    }

  }
}
