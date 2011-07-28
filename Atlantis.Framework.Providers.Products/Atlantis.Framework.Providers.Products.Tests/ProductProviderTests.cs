using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Currency;
using Atlantis.Framework.Providers.Interface.Currency;
using Atlantis.Framework.Providers.Interface.Products;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Testing.MockHttpContext;
using System;

namespace Atlantis.Framework.Providers.Products.Tests
{
  [TestClass]
  public class ProductProviderTests
  {
    public ProductProviderTests()
    {
      MockHttpContext.SetMockHttpContext("default.aspx", "http://localhost/default.aspx", string.Empty);
      HttpProviderContainer.Instance.RegisterProvider<ISiteContext, TestContexts>();
      HttpProviderContainer.Instance.RegisterProvider<IShopperContext, TestContexts>();
      HttpProviderContainer.Instance.RegisterProvider<IManagerContext, TestContexts>();
      HttpProviderContainer.Instance.RegisterProvider<IProductProvider, ProductProvider>();
      HttpProviderContainer.Instance.RegisterProvider<ICurrencyProvider, CurrencyProvider>();
    }

    [TestMethod]
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    public void GetCurrencyData()
    {
      var productProvider = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();
      ICurrencyInfo usdInfo = CurrencyData.GetCurrencyInfo("USD");
    }

    [TestMethod]
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    public void GetProvider()
    {
      var productProvider = HttpProviderContainer.Instance.Resolve<IProductProvider>();

      IProduct prod1 = productProvider.GetProduct(102);

      Assert.IsTrue(prod1.Duration > 0);

      Assert.IsTrue(prod1.CurrentPrice.Price > 0);
    }

    [TestMethod]
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    public void GetProduct()
    {
      var productProvider = HttpProviderContainer.Instance.Resolve<IProductProvider>();
      IProduct prod1 = productProvider.GetProduct(56401);
      bool onSate = prod1.IsOnSale;
      Assert.IsTrue(prod1.Duration > 0);
      Assert.IsTrue(prod1.CurrentPrice.Price > 0);
    }

    [TestMethod]
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    public void FormatTests()
    {
      ICurrencyInfo usdInfo = CurrencyData.GetCurrencyInfo("USD");
      var currency = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();
      ICurrencyPrice postivePrice = new CurrencyPrice(499, usdInfo, CurrencyPriceType.Transactional);
      ICurrencyPrice zeroPrice = new CurrencyPrice(0, usdInfo, CurrencyPriceType.Transactional);
      ICurrencyPrice negativePrice = new CurrencyPrice(-399, usdInfo, CurrencyPriceType.Transactional);

      Console.WriteLine(currency.PriceText(postivePrice, false, CurrencyNegativeFormat.NegativeNotAllowed));
      Console.WriteLine(currency.PriceText(zeroPrice, false, CurrencyNegativeFormat.NegativeNotAllowed));
      Console.WriteLine(currency.PriceText(negativePrice, false, CurrencyNegativeFormat.NegativeNotAllowed));
      Console.WriteLine();
      Console.WriteLine(currency.PriceText(postivePrice, false, CurrencyNegativeFormat.Minus));
      Console.WriteLine(currency.PriceText(zeroPrice, false, CurrencyNegativeFormat.Minus));
      Console.WriteLine(currency.PriceText(negativePrice, false, CurrencyNegativeFormat.Minus));
      Console.WriteLine();
      Console.WriteLine(currency.PriceText(postivePrice, false, CurrencyNegativeFormat.Parentheses));
      Console.WriteLine(currency.PriceText(zeroPrice, false, CurrencyNegativeFormat.Parentheses));
      Console.WriteLine(currency.PriceText(negativePrice, false, CurrencyNegativeFormat.Parentheses));
      Console.WriteLine();
    }

  }
}
