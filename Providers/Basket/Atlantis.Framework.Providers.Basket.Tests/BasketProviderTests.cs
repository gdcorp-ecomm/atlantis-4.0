using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Testing.MockProviders;
using Atlantis.Framework.Providers.Basket.Interface;

namespace Atlantis.Framework.Providers.Basket.Tests
{
  [TestClass]
  public class BasketProviderTests
  {
    IProviderContainer SetContext<T>() where T : ProviderBase
    {
      MockProviderContainer result = new MockProviderContainer();
      result.RegisterProvider<ISiteContext, MockSiteContext>();
      result.RegisterProvider<IBasketProvider, T>();

      return result;
    }

    [TestMethod]
    public void CreateBasketAddRequest()
    {
      var container = SetContext<TestBasketProvider>();
      var basket = container.Resolve<IBasketProvider>();
      var basketAddRequest = basket.NewAddRequest();

      Assert.IsNotNull(basketAddRequest.ISC);
      Assert.IsNull(basketAddRequest.OrderDiscountCode);
      Assert.IsNull(basketAddRequest.TransactionCurrency);
      Assert.IsNull(basketAddRequest.YARD);
    }

    [TestMethod]
    public void BasketAddRequestAddNullElement()
    {
      var container = SetContext<TestBasketProvider>();
      var basket = container.Resolve<IBasketProvider>();
      var basketAddRequest = basket.NewAddRequest();
      basketAddRequest.AddElement(null);
      Assert.AreEqual(0, basketAddRequest.Elements.Count());
    }

    [TestMethod]
    public void BasketAddRequestAddElement()
    {
      var element = XElement.Parse("<extraelement />");
      var container = SetContext<TestBasketProvider>();
      var basket = container.Resolve<IBasketProvider>();
      var basketAddRequest = basket.NewAddRequest();
      basketAddRequest.AddElement(element);
      Assert.AreEqual(1, basketAddRequest.Elements.Count());
    }

    [TestMethod]
    public void BasketAddRequestAddNullItem()
    {
      var container = SetContext<TestBasketProvider>();
      var basket = container.Resolve<IBasketProvider>();
      var basketAddRequest = basket.NewAddRequest();
      basketAddRequest.AddItem(null);
      Assert.AreEqual(0, basketAddRequest.Items.Count());
    }

    [TestMethod]
    public void BasketAddRequestAddItem()
    {
      var container = SetContext<TestBasketProvider>();
      var basket = container.Resolve<IBasketProvider>();
      var basketAddRequest = basket.NewAddRequest();
      var item = basket.NewBasketAddItem(101, null);
      basketAddRequest.AddItem(item);
      Assert.AreEqual(1, basketAddRequest.Items.Count());
    }

    [TestMethod]
    public void CreateBasketAddRequestWithOverriddenMethods()
    {
      var container = SetContext<TestBasketProviderOverrides>();
      var basket = container.Resolve<IBasketProvider>();
      var basketAddRequest = basket.NewAddRequest();

      Assert.IsNull(basketAddRequest.ISC);
      Assert.AreEqual(TestBasketProviderOverrides.TESTDISCOUNTCODE, basketAddRequest.OrderDiscountCode);
    }

    [TestMethod]
    public void CreateAddItem()
    {
      var container = SetContext<TestBasketProvider>();
      var basket = container.Resolve<IBasketProvider>();
      var itemAdd = basket.NewBasketAddItem(101, null);

      Assert.AreEqual(101, itemAdd.UnifiedProductId);
      Assert.AreEqual(1, itemAdd.Quantity);
      Assert.IsNull(itemAdd.ItemTrackingCode);
      Assert.IsNull(itemAdd.AffiliateData);
      Assert.IsNull(itemAdd.CiCode);
      Assert.IsNull(itemAdd.DiscountCode);
      Assert.IsNull(itemAdd.Duration);
      Assert.IsNull(itemAdd.FastballDiscount);
      Assert.IsNull(itemAdd.FastballOfferId);
      Assert.IsNull(itemAdd.FastballOfferUid);
      Assert.IsNull(itemAdd.OverridePrice);
      Assert.IsNull(itemAdd.PartnerId);
      Assert.IsNull(itemAdd.Pathway);
      Assert.IsNull(itemAdd.PromoTrackingCode);
      Assert.IsNull(itemAdd.RedemptionCode);
      Assert.IsNull(itemAdd.ResourceId);
      Assert.IsNull(itemAdd.StackId);
      Assert.IsNull(itemAdd.CustomXml);
    }

    [TestMethod]
    public void CreateAddItemWithOverriddenMethods()
    {
      var container = SetContext<TestBasketProviderOverrides>();
      var basket = container.Resolve<IBasketProvider>();
      var itemAdd = basket.NewBasketAddItem(101, null);

      Assert.AreEqual(101, itemAdd.UnifiedProductId);
      Assert.AreEqual(1, itemAdd.Quantity);
      Assert.AreEqual(TestBasketProviderOverrides.TESTCICODE, itemAdd.CiCode);
    }

    [TestMethod]
    public void ChildAddItems()
    {
      var container = SetContext<TestBasketProvider>();
      var basket = container.Resolve<IBasketProvider>();
      var itemAdd = basket.NewBasketAddItem(101, null);
      var childItem1 = basket.NewBasketAddItem(58, null);
      var childItem2 = basket.NewBasketAddItem(59, null);
      itemAdd.AddChildItem(childItem1);
      itemAdd.AddChildItem(childItem2);

      Assert.IsTrue(itemAdd.HasChildItems);
      Assert.AreEqual(2, itemAdd.ChildItems.Count());
    }

    [TestMethod]
    public void OverridePricing()
    {
      var container = SetContext<TestBasketProvider>();
      var basket = container.Resolve<IBasketProvider>();
      var itemAdd = basket.NewBasketAddItem(101, null);
      itemAdd.SetOverridePrice(999, 199);
      Assert.IsTrue(itemAdd.HasOverridePrice);
      Assert.AreEqual(199, itemAdd.OverridePrice.CurrentPrice);
      Assert.AreEqual(999, itemAdd.OverridePrice.ListPrice);
      Assert.IsFalse(string.IsNullOrEmpty(itemAdd.OverridePrice.Hash));

      var overridePriceCOM = new gdOverrideLib.Price();

      try
      {
        var realHash = overridePriceCOM.GetHash("1", "101", "999", "111", DateTime.Now.ToString());
        int result = overridePriceCOM.VerifyHash("1", "101", "999", "111", DateTime.Now.ToString(), itemAdd.OverridePrice.Hash);
        Assert.AreEqual(0, result);
      }
      finally
      {
        Marshal.ReleaseComObject(overridePriceCOM);
      }
    }


  }
}
