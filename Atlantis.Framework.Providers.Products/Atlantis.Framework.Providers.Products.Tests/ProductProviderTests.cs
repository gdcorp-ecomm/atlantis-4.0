using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Currency;
using Atlantis.Framework.Providers.Interface.Currency;
using Atlantis.Framework.Providers.Interface.Products;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Testing.MockHttpContext;
using System;
using Atlantis.Framework.Testing.MockProviders;
using Atlantis.Framework.Providers.Interface.Preferences;
using Atlantis.Framework.Providers.Interface.PromoData;
using System.Web;
using System.Collections.Generic;

namespace Atlantis.Framework.Providers.Products.Tests
{
  [TestClass]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  public class ProductProviderTests
  {
    private void SetContexts(int privateLabelId, string shopperId)
    {
      SetContexts(privateLabelId, shopperId, true);
    }

    private void SetContexts(int privateLabelId, string shopperId, bool includeShopperPreferences)
    {
      MockHttpContext.SetMockHttpContext("default.aspx", "http://localhost/default.aspx", string.Empty);
      HttpProviderContainer.Instance.RegisterProvider<ISiteContext, MockSiteContext>();
      HttpProviderContainer.Instance.RegisterProvider<IShopperContext, MockShopperContext>();
      HttpProviderContainer.Instance.RegisterProvider<IManagerContext, MockNoManagerContext>();
      HttpProviderContainer.Instance.RegisterProvider<ICurrencyProvider, CurrencyProvider>();
      HttpProviderContainer.Instance.RegisterProvider<IProductProvider, ProductProvider>();

      if (includeShopperPreferences)
      {
        HttpProviderContainer.Instance.RegisterProvider<IShopperPreferencesProvider, MockShopperPreference>();
      }

      HttpContext.Current.Items[MockSiteContextSettings.PrivateLabelId] = privateLabelId;
      IShopperContext shopperContext = HttpProviderContainer.Instance.Resolve<IShopperContext>();
      shopperContext.SetLoggedInShopper(shopperId);
    }

    [TestMethod]
    public void GetProvider()
    {
      SetContexts(1, string.Empty);
      var productProvider = HttpProviderContainer.Instance.Resolve<IProductProvider>();
      IProduct prod1 = productProvider.GetProduct(102);
      Assert.IsTrue(prod1.Duration > 0);
      Assert.IsTrue(prod1.CurrentPrice.Price > 0);
    }

    [TestMethod]
    public void GetProduct()
    {
      SetContexts(1, string.Empty);
      var productProvider = HttpProviderContainer.Instance.Resolve<IProductProvider>();
      IProduct prod1 = productProvider.GetProduct(56401);
      bool onSate = prod1.IsOnSale;
      Assert.IsTrue(prod1.Duration > 0);
      Assert.IsTrue(prod1.CurrentPrice.Price > 0);
    }

    [TestMethod]
    public void GetProductList()
    {
      SetContexts(1, string.Empty);
      var productProvider = HttpProviderContainer.Instance.Resolve<IProductProvider>();
      List<IProduct> products = productProvider.NewProductList(new int[3] { 58, 59, 60 });
      Assert.AreEqual(3, products.Count);
    }

    [TestMethod]
    public void ListPriceMonthlyProduct()
    {
      SetContexts(1, string.Empty);
      var productProvider = HttpProviderContainer.Instance.Resolve<IProductProvider>();
      var monthlyHosting = productProvider.GetProduct(58);

      Assert.AreEqual(RecurringPaymentUnitType.Monthly, monthlyHosting.DurationUnit);
      ICurrencyPrice oldListPrice = monthlyHosting.ListPrice;
      ICurrencyPrice newListPrice = monthlyHosting.GetListPrice(RecurringPaymentUnitType.Monthly);

      Assert.AreEqual(oldListPrice.Price, newListPrice.Price);

      ICurrencyPrice yearlyList = monthlyHosting.GetListPrice(RecurringPaymentUnitType.Annual);
      ICurrencyPrice semiannualList = monthlyHosting.GetListPrice(RecurringPaymentUnitType.SemiAnnual);
      ICurrencyPrice quarterlyList = monthlyHosting.GetListPrice(RecurringPaymentUnitType.Quarterly);

      Assert.IsTrue(yearlyList.Price > semiannualList.Price);
      Assert.IsTrue(semiannualList.Price > quarterlyList.Price);
      Assert.IsTrue(quarterlyList.Price > newListPrice.Price);
    }

    [TestMethod]
    public void CurrentPriceMonthlyProduct()
    {
      SetContexts(1, string.Empty);
      var productProvider = HttpProviderContainer.Instance.Resolve<IProductProvider>();
      var monthlyHosting = productProvider.GetProduct(58);

      Assert.AreEqual(RecurringPaymentUnitType.Monthly, monthlyHosting.DurationUnit);
      ICurrencyPrice oldCurrentPrice = monthlyHosting.CurrentPrice;
      ICurrencyPrice newCurrentPrice = monthlyHosting.GetCurrentPrice(RecurringPaymentUnitType.Monthly);

      Assert.AreEqual(oldCurrentPrice.Price, newCurrentPrice.Price);

      ICurrencyPrice yearlyCurrent = monthlyHosting.GetCurrentPrice(RecurringPaymentUnitType.Annual);
      ICurrencyPrice semiannualCurrent = monthlyHosting.GetCurrentPrice(RecurringPaymentUnitType.SemiAnnual);
      ICurrencyPrice quarterlyCurrent = monthlyHosting.GetCurrentPrice(RecurringPaymentUnitType.Quarterly);

      Assert.IsTrue(yearlyCurrent.Price > semiannualCurrent.Price);
      Assert.IsTrue(semiannualCurrent.Price > quarterlyCurrent.Price);
      Assert.IsTrue(quarterlyCurrent.Price > newCurrentPrice.Price);
    }

    [TestMethod]
    public void CurrentPriceByQuantityMonthlyProduct()
    {
      SetContexts(1, string.Empty);
      var productProvider = HttpProviderContainer.Instance.Resolve<IProductProvider>();
      var monthlyHosting = productProvider.GetProduct(58);

      Assert.AreEqual(RecurringPaymentUnitType.Monthly, monthlyHosting.DurationUnit);
      ICurrencyPrice oldCurrentPrice = monthlyHosting.GetCurrentPriceByQuantity(12);
      ICurrencyPrice newCurrentPrice = monthlyHosting.GetCurrentPriceByQuantity(12, RecurringPaymentUnitType.Monthly);

      Assert.AreEqual(oldCurrentPrice.Price, newCurrentPrice.Price);

      ICurrencyPrice yearlyCurrent = monthlyHosting.GetCurrentPriceByQuantity(12, RecurringPaymentUnitType.Annual);
      ICurrencyPrice semiannualCurrent = monthlyHosting.GetCurrentPriceByQuantity(12, RecurringPaymentUnitType.SemiAnnual);
      ICurrencyPrice quarterlyCurrent = monthlyHosting.GetCurrentPriceByQuantity(12, RecurringPaymentUnitType.Quarterly);

      Assert.IsTrue(yearlyCurrent.Price > semiannualCurrent.Price);
      Assert.IsTrue(semiannualCurrent.Price > quarterlyCurrent.Price);
      Assert.IsTrue(quarterlyCurrent.Price > newCurrentPrice.Price);
    }

    [TestMethod]
    public void ListPriceAnnualProduct()
    {
      SetContexts(1, string.Empty);
      var productProvider = HttpProviderContainer.Instance.Resolve<IProductProvider>();
      var dotCom1Year = productProvider.GetProduct(101);

      Assert.AreEqual(RecurringPaymentUnitType.Annual, dotCom1Year.DurationUnit);
      ICurrencyPrice oldListPrice = dotCom1Year.ListPrice;
      ICurrencyPrice yearlyList = dotCom1Year.GetListPrice(RecurringPaymentUnitType.Annual);

      Assert.AreEqual(oldListPrice.Price, yearlyList.Price);

      ICurrencyPrice monthlyList = dotCom1Year.GetListPrice(RecurringPaymentUnitType.Monthly);
      ICurrencyPrice semiannualList = dotCom1Year.GetListPrice(RecurringPaymentUnitType.SemiAnnual);
      ICurrencyPrice quarterlyList = dotCom1Year.GetListPrice(RecurringPaymentUnitType.Quarterly);

      Assert.IsTrue(yearlyList.Price > semiannualList.Price);
      Assert.IsTrue(semiannualList.Price > quarterlyList.Price);
      Assert.IsTrue(quarterlyList.Price > monthlyList.Price);
    }

    [TestMethod]
    public void CurrentPriceAnnualProduct()
    {
      SetContexts(1, string.Empty);
      var productProvider = HttpProviderContainer.Instance.Resolve<IProductProvider>();
      var dotCom1Year = productProvider.GetProduct(101);

      Assert.AreEqual(RecurringPaymentUnitType.Annual, dotCom1Year.DurationUnit);
      ICurrencyPrice oldCurrentPrice = dotCom1Year.CurrentPrice;
      ICurrencyPrice yearlyCurrent = dotCom1Year.GetCurrentPrice(RecurringPaymentUnitType.Annual);

      Assert.AreEqual(oldCurrentPrice.Price, yearlyCurrent.Price);

      ICurrencyPrice monthlyCurrent = dotCom1Year.GetCurrentPrice(RecurringPaymentUnitType.Monthly);
      ICurrencyPrice semiannualCurrent = dotCom1Year.GetCurrentPrice(RecurringPaymentUnitType.SemiAnnual);
      ICurrencyPrice quarterlyCurrent = dotCom1Year.GetCurrentPrice(RecurringPaymentUnitType.Quarterly);

      Assert.IsTrue(yearlyCurrent.Price > semiannualCurrent.Price);
      Assert.IsTrue(semiannualCurrent.Price > quarterlyCurrent.Price);
      Assert.IsTrue(quarterlyCurrent.Price > monthlyCurrent.Price);
    }

    [TestMethod]
    public void CurrentYearlyPriceDDC()
    {
      SetContexts(1, string.Empty);
      var productProvider = HttpProviderContainer.Instance.Resolve<IProductProvider>();
      var dotCom1Year = productProvider.GetProduct(101);

      ICurrencyPrice listPrice = dotCom1Year.GetListPrice(RecurringPaymentUnitType.Annual);
      ICurrencyPrice currentPriceDDC = dotCom1Year.GetCurrentPrice(RecurringPaymentUnitType.Annual, 16);

      Assert.AreNotEqual(currentPriceDDC.Price, listPrice.Price);
    }

    [TestMethod]
    public void CurrentYearlyPriceEuro()
    {
      SetContexts(1, string.Empty);
      var productProvider = HttpProviderContainer.Instance.Resolve<IProductProvider>();
      var currency = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();
      var euro = currency.GetCurrencyInfo("EUR");

      var dotCom1Year = productProvider.GetProduct(101);

      ICurrencyPrice currentPrice = dotCom1Year.GetCurrentPrice(RecurringPaymentUnitType.Annual);
      ICurrencyPrice currentPriceEuro = dotCom1Year.GetCurrentPrice(RecurringPaymentUnitType.Annual, -1, euro);

      Assert.AreNotEqual(currentPrice.CurrencyInfo.CurrencyType, currentPriceEuro.CurrencyInfo.CurrencyType);
    }

    [TestMethod]
    public void GetInvalidProduct()
    {
      SetContexts(1, string.Empty);
      AtlantisException.SetWebRequestProviderContainer(HttpProviderContainer.Instance);
      var productProvider = HttpProviderContainer.Instance.Resolve<IProductProvider>();
      IProduct prod1 = productProvider.GetProduct(0);
      //success or failure for this test is a log message containing a shopper id
    }
  }
}
