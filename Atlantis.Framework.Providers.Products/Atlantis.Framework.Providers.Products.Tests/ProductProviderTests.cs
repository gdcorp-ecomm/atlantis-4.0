using System.Collections.Generic;
using Atlantis.Framework.Engine;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Currency;
using Atlantis.Framework.Providers.Interface.Currency;
using Atlantis.Framework.Providers.Interface.Preferences;
using Atlantis.Framework.Providers.Interface.Products;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.Testing.MockEngine;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockLocalization;
using Atlantis.Framework.Testing.MockPreferencesProvider;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.Products.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.DataCacheGeneric.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.Currency.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.EcommPricing.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.Products.Impl.dll")]
  public class ProductProviderTests
  {
    private IErrorLogger _defaultLogger;
    private MockErrorLogger _testLogger;

    [TestInitialize]
    public void InitTest()
    {
      _defaultLogger = EngineLogging.EngineLogger;
      _testLogger = new MockErrorLogger();
      EngineLogging.EngineLogger = _testLogger;
    }

    [TestCleanup]
    public void CleanupTest()
    {
      EngineLogging.EngineLogger = _defaultLogger;
    }

    private IProviderContainer SetContexts(int privateLabelId, string shopperId, bool includeShopperPreferences = true)
    {
      var request = new MockHttpRequest("http://localhost/default.aspx");
      MockHttpContext.SetFromWorkerRequest(request);

      var container = new MockProviderContainer();
      container.SetData(MockSiteContextSettings.PrivateLabelId, privateLabelId);

      container.RegisterProvider<ISiteContext, MockSiteContext>();
      container.RegisterProvider<IShopperContext, MockShopperContext>();
      container.RegisterProvider<IManagerContext, MockNoManagerContext>();
      container.RegisterProvider<ICurrencyProvider, CurrencyProvider>();
      container.RegisterProvider<IProductProvider, ProductProvider>();

      if (includeShopperPreferences)
      {
        container.RegisterProvider<IShopperPreferencesProvider, MockShopperPreference>();
      }

      var shopperContext = container.Resolve<IShopperContext>();
      shopperContext.SetLoggedInShopper(shopperId);

      return container;
    }

    [TestMethod]
    public void GetProvider()
    {
      var container = SetContexts(1, string.Empty);
      var productProvider = container.Resolve<IProductProvider>();
      IProduct prod1 = productProvider.GetProduct(102);
      Assert.IsTrue(prod1.Duration > 0);
      Assert.IsTrue(prod1.CurrentPrice.Price > 0);
    }

    [TestMethod]
    public void GetProduct()
    {
      var container = SetContexts(1, string.Empty);
      var productProvider = container.Resolve<IProductProvider>();
      IProduct prod1 = productProvider.GetProduct(56401);
      Assert.IsNotNull(prod1.IsOnSale);
      Assert.IsNotNull(prod1.GetIsOnSale(2));
      Assert.IsTrue(prod1.Duration > 0);
      Assert.IsTrue(prod1.CurrentPrice.Price > 0);
      Assert.AreNotEqual(0, prod1.HalfYears);
      Assert.AreNotEqual(0, prod1.Quarters);
      Assert.AreNotEqual(0, prod1.Months);
      Assert.AreNotEqual(0, prod1.Years);
      Assert.AreNotEqual(string.Empty, prod1.Info.FriendlyDescription);
    }

    [TestMethod]
    public void GetProductList()
    {
      var container = SetContexts(1, string.Empty);
      var productProvider = container.Resolve<IProductProvider>();
      List<IProduct> products = productProvider.NewProductList(new[] {58, 59, 60});
      Assert.AreEqual(3, products.Count);
    }

    [TestMethod]
    public void ListPriceMonthlyProduct()
    {
      var container = SetContexts(1, string.Empty);
      var productProvider = container.Resolve<IProductProvider>();
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
      var container = SetContexts(1, string.Empty);
      var productProvider = container.Resolve<IProductProvider>();
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
      var container = SetContexts(1, string.Empty);
      var productProvider = container.Resolve<IProductProvider>();
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
      var container = SetContexts(1, string.Empty);
      var productProvider = container.Resolve<IProductProvider>();
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
      var container = SetContexts(1, string.Empty);
      var productProvider = container.Resolve<IProductProvider>();
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
    public void CurrentYearlyPriceDdc()
    {
      var container = SetContexts(1, string.Empty);
      var productProvider = container.Resolve<IProductProvider>();
      var dotCom1Year = productProvider.GetProduct(101);

      ICurrencyPrice listPrice = dotCom1Year.GetListPrice(RecurringPaymentUnitType.Annual);
      ICurrencyPrice currentPriceDdc = dotCom1Year.GetCurrentPrice(RecurringPaymentUnitType.Annual, 16);

      Assert.AreNotEqual(currentPriceDdc.Price, listPrice.Price);
    }

    [TestMethod]
    public void CurrentYearlyPriceEuro()
    {
      var container = SetContexts(1, string.Empty);
      var productProvider = container.Resolve<IProductProvider>();
      var currency = container.Resolve<ICurrencyProvider>();
      var euro = currency.GetCurrencyInfo("EUR");

      var dotCom1Year = productProvider.GetProduct(101);

      ICurrencyPrice currentPrice = dotCom1Year.GetCurrentPrice(RecurringPaymentUnitType.Annual);
      ICurrencyPrice currentPriceEuro = dotCom1Year.GetCurrentPrice(RecurringPaymentUnitType.Annual, -1, euro);

      Assert.AreNotEqual(currentPrice.CurrencyInfo.CurrencyType, currentPriceEuro.CurrencyInfo.CurrencyType);
    }

    [TestMethod]
    public void GetInvalidProduct()
    {
      _testLogger.Exceptions.Clear();

      var container = SetContexts(1, string.Empty);
      var productProvider = container.Resolve<IProductProvider>();
      IProduct prod1 = productProvider.GetProduct(0);

      int typeId = prod1.Info.ProductTypeId;

      Assert.AreEqual(0, typeId);
      Assert.AreEqual(string.Empty, prod1.Info.FriendlyDescription);
      Assert.AreEqual(1, _testLogger.Exceptions.Count);
    }

    /* Test no longer valid; method removed
    [TestMethod]
    public void NonUnifiedPfidBasic()
    {
      var container = SetContexts(2, string.Empty);
      var productProvider = container.Resolve<IProductProvider>();
      int pfid = productProvider.GetNonUnifiedPfid(101);
      Assert.AreEqual(2500101, pfid);
    }
    */

    [TestMethod]
    public void UnifiedProductIdBasic()
    {
      var container = SetContexts(2, string.Empty);
      var productProvider = container.Resolve<IProductProvider>();
      int productId = productProvider.GetUnifiedProductIdByPfid(2500101);
      Assert.AreEqual(101, productId);
    }

    [TestMethod]
    public void LocalizedProductDescriptionNoLocalization()
    {
      var container = SetContexts(1, string.Empty);
      var productProvider = container.Resolve<IProductProvider>();
      IProduct dotCom1Year = productProvider.GetProduct(101);
      string description = dotCom1Year.Info.FriendlyDescription;
      Assert.AreEqual(dotCom1Year.Info.FriendlyDescription, description);
    }

    [TestMethod]
    public void LocalizedProductDescriptionPtBr()
    {
      string descriptionEnglish;
      string nameEnglish;

      {
        var usContainer = SetContexts(1, string.Empty);
        var productProvider = usContainer.Resolve<IProductProvider>();
        IProduct dotCom1Year = productProvider.GetProduct(101);
        descriptionEnglish = dotCom1Year.Info.FriendlyDescription;
        nameEnglish = dotCom1Year.Info.Name;
      }

      string description;
      string name;

      {
        var container = SetContexts(1, string.Empty);
        container.RegisterProvider<ILocalizationProvider, MockLocalizationProvider>();
        container.SetData(MockLocalizationProviderSettings.FullLanguage, "pt-br");
        var productProvider = container.Resolve<IProductProvider>();
        var dotCom1Year = productProvider.GetProduct(101);
        description = dotCom1Year.Info.FriendlyDescription;
        name = dotCom1Year.Info.Name;
      }

      Assert.AreNotEqual(descriptionEnglish, description);
      Assert.AreNotEqual(nameEnglish, name);
    }
  }
}