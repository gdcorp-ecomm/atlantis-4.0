using Atlantis.Framework.Currency.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.Currency;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.Testing.MockLocalization;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.Providers.Currency.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.EcommPricing.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.Currency.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.PLSignupInfo.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.PrivateLabel.Impl.dll")]
  public class CurrencyFormattingTests
  {
    // Fix this with mocks if it becomes fragile later
    private ICurrencyInfo GetCurrencyInfo(string currency)
    {
      var request = new CurrencyTypesRequestData();
      var response = (CurrencyTypesResponseData)DataCache.DataCache.GetProcessRequest(request, CurrencyProviderEngineRequests.CurrencyTypesRequest);
      return response[currency];
    }

    private ICurrencyPrice GetPrice(int price, string currency)
    {
      var info = GetCurrencyInfo(currency);
      return new CurrencyPrice(price, info, CurrencyPriceType.Transactional);
    }

    private IProviderContainer SetContainer(string fullLanguage = null)
    {
      MockProviderContainer container = new MockProviderContainer();

      if (fullLanguage != null)
      {
        container.RegisterProvider<ILocalizationProvider, MockLocalizationProvider>();
        container.SetData(MockLocalizationProviderSettings.FullLanguage, fullLanguage);
      }

      return container;
    }

    [TestMethod]
    public void FormatUSD()
    {
      var container = SetContainer();
      var formatting = new CurrencyFormatting(container);
      var priceText = formatting.FormatPrice(GetPrice(1111, "USD"), PriceFormatOptions.None);

      var container2 = SetContainer("en-US");
      var formattingWithCulture = new CurrencyFormatting(container2);
      var priceText2 = formattingWithCulture.FormatPrice(GetPrice(1111, "USD"), PriceFormatOptions.None);

      Assert.AreEqual(priceText, priceText2);
    }

    [TestMethod]
    public void FormatIndianRupies()
    {
      var container = SetContainer();
      var formatting = new CurrencyFormatting(container);
      var priceText = formatting.FormatPrice(GetPrice(1111, "INR"), PriceFormatOptions.None);

      var container2 = SetContainer("en-IN");
      var formattingWithCulture = new CurrencyFormatting(container2);
      var priceText2 = formattingWithCulture.FormatPrice(GetPrice(1111, "INR"), PriceFormatOptions.None);

      Assert.IsTrue(priceText2.Contains(' '));
      Assert.IsFalse(priceText.Contains(' '));
    }

    [TestMethod]
    public void FormatIndianRupiesLarge()
    {
      var container = SetContainer();
      var formatting = new CurrencyFormatting(container);
      var priceText = formatting.FormatPrice(GetPrice(322221111, "INR"), PriceFormatOptions.None);

      var container2 = SetContainer("en-IN");
      var formattingWithCulture = new CurrencyFormatting(container2);
      var priceText2 = formattingWithCulture.FormatPrice(GetPrice(322221111, "INR"), PriceFormatOptions.None);

      Assert.AreNotEqual(priceText, priceText2);
    }

    [TestMethod]
    public void FormatCanadianFrench()
    {
      var container = SetContainer();
      var formatting = new CurrencyFormatting(container);
      var priceText = formatting.FormatPrice(GetPrice(1111, "CAD"), PriceFormatOptions.None);

      var container2 = SetContainer("fr-CA");
      var formattingWithCulture = new CurrencyFormatting(container2);
      var priceText2 = formattingWithCulture.FormatPrice(GetPrice(1111, "CAD"), PriceFormatOptions.None);

      Assert.AreNotEqual(priceText, priceText2);
    }

    [TestMethod]
    public void FormatSymbolOptions()
    {
      var container2 = SetContainer("en-US");
      var formattingWithCulture = new CurrencyFormatting(container2);
      var priceText = formattingWithCulture.FormatPrice(GetPrice(1111, "EUR"), PriceFormatOptions.AsciiSymbol);
      var priceText2 = formattingWithCulture.FormatPrice(GetPrice(1111, "EUR"), PriceFormatOptions.HtmlSymbol);
      var priceText3 = formattingWithCulture.FormatPrice(GetPrice(1111, "EUR"), PriceFormatOptions.DropSymbol);

      Assert.AreNotEqual(priceText, priceText2);
      Assert.AreNotEqual(priceText3, priceText2);
      Assert.AreNotEqual(priceText, priceText3);
    }

    [TestMethod]
    public void DropDecimalOptions()
    {
      var container2 = SetContainer("en-US");
      var formattingWithCulture = new CurrencyFormatting(container2);
      var priceText = formattingWithCulture.FormatPrice(GetPrice(1111, "USD"), PriceFormatOptions.None);
      var priceText2 = formattingWithCulture.FormatPrice(GetPrice(1111, "USD"), PriceFormatOptions.DropDecimal);
      Assert.AreNotEqual(priceText, priceText2);
    }

    [TestMethod]
    public void NoDecimalCurrency()
    {
      var container2 = SetContainer("en-US");
      var formattingWithCulture = new CurrencyFormatting(container2);
      var priceText = formattingWithCulture.FormatPrice(GetPrice(1111, "JPY"), PriceFormatOptions.None);
      Assert.IsFalse(priceText.Contains("."));
    }

    [TestMethod]
    public void SymbolOnRight()
    {
      var container = SetContainer();
      var formatting = new CurrencyFormatting(container);
      var priceText = formatting.FormatPrice(GetPrice(1111, "KRW"), PriceFormatOptions.None);
      Assert.IsTrue(priceText.StartsWith("1"));
    }

  }
}
