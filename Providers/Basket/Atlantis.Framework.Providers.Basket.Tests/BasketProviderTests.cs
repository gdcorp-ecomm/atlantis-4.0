using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using Atlantis.Framework.Basket.Interface;
using Atlantis.Framework.Engine;
using Atlantis.Framework.Providers.Basket.Constants;
using Atlantis.Framework.Providers.Basket.Tests.Properties;
using Atlantis.Framework.Providers.Shopper.Interface;
using Atlantis.Framework.Testing.MockEngine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Testing.MockProviders;
using Atlantis.Framework.Providers.Basket.Interface;
using Moq;
using Moq.Protected;

namespace Atlantis.Framework.Providers.Basket.Tests
{
  [ExcludeFromCodeCoverage]
  [TestClass]
  [DeploymentItem("atlantis.config")]
  public class BasketProviderTests
  {
    IProviderContainer SetContext<T>(string shopperId = null) where T : ProviderBase
    {
      var result = new MockProviderContainer();
      result.RegisterProvider<ISiteContext, MockSiteContext>();
      result.RegisterProvider<IShopperContext, MockShopperContext>();
      result.RegisterProvider<IShopperDataProvider, MockShopperDataProvider>();
      result.RegisterProvider<IBasketProvider, T>();

      if (shopperId != null)
      {
        var shopperContext = result.Resolve<IShopperContext>();
        shopperContext.SetNewShopper(shopperId);
      }

      return result;
    }

    [TestCleanup]
    public void CleanUp()
    {
      EngineRequestMocking.ClearOverrides();
    }

    [TestMethod]
    public void WillDependencyInjectionConstructorSetSiteContext()
    {
      var siteContext = new Mock<ISiteContext>();
      var testBasketProvider = new TestBasketProvider(siteContext.Object, new Mock<IShopperContext>().Object,
        new Mock<IShopperDataProvider>().Object);

      Assert.AreEqual(siteContext.Object, testBasketProvider.SiteContext);
    }

    [TestMethod]
    public void WillDependencyInjectionConstructorSetShopperContext()
    {
      var shopperContext = new Mock<IShopperContext>();
      var testBasketProvider = new TestBasketProvider(new Mock<ISiteContext>().Object, shopperContext.Object,
        new Mock<IShopperDataProvider>().Object);

      Assert.AreEqual(shopperContext.Object, testBasketProvider.ShopperContext);
    }

    [TestMethod]
    public void WillDependencyInjectionConstructorSetShopperDataProvider()
    {
      var shopperDataProvider = new Mock<IShopperDataProvider>();
      var testBasketProvider = new TestBasketProvider(new Mock<ISiteContext>().Object,
        new Mock<IShopperContext>().Object,
        shopperDataProvider.Object);

      Assert.AreEqual(shopperDataProvider.Object, testBasketProvider.ShopperDataProvider);
    }

    [TestMethod]
    public void WillNewDeleteRequestCreateCorrectRequest()
    {
      var basketProvider = new Mock<BasketProvider>(new Mock<ISiteContext>().Object, new Mock<IShopperContext>().Object,
        new Mock<IShopperDataProvider>().Object) {CallBase = true};
      var deleteRequest = basketProvider.Object.NewDeleteRequest();

      Assert.IsNotNull(deleteRequest);
      Assert.IsInstanceOfType(deleteRequest, typeof(IBasketDeleteRequest));
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void WillDeleteFromBasketThrowIfDeleteRequestIsNull()
    {
      var basketProvider = new Mock<BasketProvider>(new Mock<ISiteContext>().Object, new Mock<IShopperContext>().Object,
        new Mock<IShopperDataProvider>().Object) { CallBase = true };
      basketProvider.Object.DeleteFromBasket(null);
    }

    [TestMethod]
    public void WillDeleteFromBasketThrowIfItemsToDeleteIsEmpty()
    {
      var basketProvider = new Mock<BasketProvider>(new Mock<ISiteContext>().Object, new Mock<IShopperContext>().Object,
        new Mock<IShopperDataProvider>().Object) { CallBase = true };
      var response = basketProvider.Object.DeleteFromBasket(new BasketDeleteRequest());

      Assert.IsTrue(string.Equals("Items to delete in delete request is empty", response.Message));
      Assert.AreEqual(1, response.ErrorCount);
    }

    [TestMethod]
    public void WillDeleteFromBasketReturnErrorIfShopperIdIsNull()
    {
      var shopperContext = new Mock<IShopperContext>();
      var basketProvider = new Mock<BasketProvider>(new Mock<ISiteContext>().Object, shopperContext.Object,
        new Mock<IShopperDataProvider>().Object) { CallBase = true };
      shopperContext.Setup(s => s.ShopperId).Returns((string)null);
      var deleteRequest = basketProvider.Object.NewDeleteRequest();
      deleteRequest.AddItemToDelete(0, 0);
      var response = basketProvider.Object.DeleteFromBasket(deleteRequest);

      Assert.IsTrue(string.Equals("Shopper id is null or empty", response.Message));
      Assert.AreEqual(1, response.ErrorCount);
    }

    [TestMethod]
    public void WillDeleteFromBasketReturnErrorIfShopperIdIsEmpty()
    {
      var shopperContext = new Mock<IShopperContext>();
      var basketProvider = new Mock<BasketProvider>(new Mock<ISiteContext>().Object, shopperContext.Object,
        new Mock<IShopperDataProvider>().Object) { CallBase = true };
      shopperContext.Setup(s => s.ShopperId).Returns(string.Empty);
      var deleteRequest = basketProvider.Object.NewDeleteRequest();
      deleteRequest.AddItemToDelete(0,0);
      var response = basketProvider.Object.DeleteFromBasket(deleteRequest);

      Assert.IsTrue(string.Equals("Shopper id is null or empty", response.Message));
      Assert.AreEqual(1, response.ErrorCount);
    }

    [TestMethod]
    public void WillDeleteFromBasketReturnIfNoExceptionIsThrown()
    {
      var basketDeleteResponse = BasketDeleteResponseData.FromResponseXml(
        new XElement("Response", new XElement("MESSAGE", "Success")).ToString(SaveOptions.DisableFormatting));
      EngineRequestMocking.RegisterOverride(BasketEngineRequests.DeleteItem, basketDeleteResponse);

      var shopperContext = new Mock<IShopperContext>();
      var requestBuilder = new Mock<IBasketDeleteRequestBuilder>();
      var basketProvider = new Mock<BasketProvider>(new Mock<ISiteContext>().Object, shopperContext.Object,
        new Mock<IShopperDataProvider>().Object, requestBuilder.Object) { CallBase = true };
     
      shopperContext.Setup(s => s.ShopperId).Returns("1234");
      requestBuilder.Setup(b => b.BuildRequestData(It.IsAny<string>(), It.IsAny<IBasketDeleteRequest>()))
        .Returns((BasketDeleteRequestData)null);
      basketProvider.Protected()
        .Setup<IBasketResponse>("CreateBasketResponse", ItExpr.IsAny<BasketResponseStatus>())
        .Returns(new Mock<IBasketResponse>().Object);
      basketProvider.Protected()
        .Setup<IBasketResponse>("CreateBasketResponse", ItExpr.IsAny<Exception>())
        .Returns(new Mock<IBasketResponse>().Object);
      
      var deleteRequest = basketProvider.Object.NewDeleteRequest();
      deleteRequest.AddItemToDelete(0, 0);
      var response = basketProvider.Object.DeleteFromBasket(deleteRequest);

      Assert.IsNotNull(response);
      Assert.IsInstanceOfType(response, typeof(IBasketResponse));
    }

    [TestMethod]
    public void WillDeleteFromBasketReturnIfExceptionsAreThrown()
    {
      var shopperContext = new Mock<IShopperContext>();
      var requestBuilder = new Mock<IBasketDeleteRequestBuilder>();
      var basketProvider = new Mock<BasketProvider>(new Mock<ISiteContext>().Object, shopperContext.Object,
        new Mock<IShopperDataProvider>().Object, requestBuilder.Object) { CallBase = true };

      shopperContext.Setup(s => s.ShopperId).Returns("1234");
      requestBuilder.Setup(b => b.BuildRequestData(It.IsAny<string>(), It.IsAny<IBasketDeleteRequest>()))
        .Throws(new Exception());
      basketProvider.Protected()
        .Setup<IBasketResponse>("CreateBasketResponse", ItExpr.IsAny<BasketResponseStatus>())
        .Returns(new Mock<IBasketResponse>().Object);
      basketProvider.Protected()
        .Setup<IBasketResponse>("CreateBasketResponse", ItExpr.IsAny<Exception>())
        .Returns(new Mock<IBasketResponse>().Object);

      var deleteRequest = basketProvider.Object.NewDeleteRequest();
      deleteRequest.AddItemToDelete(0, 0);
      var response = basketProvider.Object.DeleteFromBasket(deleteRequest);

      Assert.IsNotNull(response);
      Assert.IsInstanceOfType(response, typeof(IBasketResponse));
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
        int result = overridePriceCOM.VerifyHash(
          "1", "101", "999", "199", DateTime.Now.ToString(CultureInfo.InvariantCulture), itemAdd.OverridePrice.Hash);
        Assert.AreEqual(0, result);
      }
      finally
      {
        Marshal.ReleaseComObject(overridePriceCOM);
      }
    }

    [TestMethod]
    public void PostToCartWithExistingValidShopper()
    {
      var container = SetContext<TestBasketProvider>(MockShopperDataProvider.MockCreatedShopperId);
      var basket = container.Resolve<IBasketProvider>();
      var basketAddRequest = basket.NewAddRequest();
      var basketItem = basket.NewBasketAddItem(58, string.Empty);
      basketAddRequest.AddItem(basketItem);

      var response = basket.PostToBasket(basketAddRequest);
      Assert.AreEqual(0, response.ErrorCount);
    }

    [TestMethod]
    public void PostToCartWithExistingNoShopper()
    {
      var container = SetContext<TestBasketProvider>();
      var basket = container.Resolve<IBasketProvider>();
      var basketAddRequest = basket.NewAddRequest();
      var basketItem = basket.NewBasketAddItem(58, string.Empty);
      basketAddRequest.AddItem(basketItem);

      var response = basket.PostToBasket(basketAddRequest);
      Assert.AreEqual(0, response.ErrorCount);
    }

    [TestMethod]
    public void PostToCartWithExistingInvalidShopper()
    {
      var container = SetContext<TestBasketProvider>(MockShopperDataProvider.MockBadShopperId);
      var basket = container.Resolve<IBasketProvider>();
      var basketAddRequest = basket.NewAddRequest();
      var basketItem = basket.NewBasketAddItem(58, string.Empty);
      basketAddRequest.AddItem(basketItem);

      var response = basket.PostToBasket(basketAddRequest);
      Assert.AreEqual(0, response.ErrorCount);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void PostToCartNullException()
    {
      var container = SetContext<TestBasketProvider>();
      var basket = container.Resolve<IBasketProvider>();
      basket.PostToBasket(null);
    }

    [TestMethod]
    public void PostToCartAddRequestThrowsException()
    {
      var container = SetContext<TestBasketProvider>("ERR");
      var basket = container.Resolve<IBasketProvider>();
      var basketAddRequest = basket.NewAddRequest();
      var basketItem = basket.NewBasketAddItem(58, string.Empty);
      basketAddRequest.AddItem(basketItem);

      var response = basket.PostToBasket(basketAddRequest);
      Assert.AreNotEqual(0, response.ErrorCount);
    }

    [TestMethod]
    public void ShopperDataProviderErrors()
    {
      IErrorLogger oldLogger = EngineLogging.EngineLogger;
      var mockLogger = new MockErrorLogger();
      EngineLogging.EngineLogger = mockLogger;

      try
      {
        var container = SetContext<TestBasketProvider>(MockShopperDataProvider.MockCreatedShopperId);
        container.SetData("MockShopperDataProvider.CauseErrors", true);

        var basket = container.Resolve<IBasketProvider>();
        var basketAddRequest = basket.NewAddRequest();
        var basketItem = basket.NewBasketAddItem(58, string.Empty);
        basketAddRequest.AddItem(basketItem);

        var response = basket.PostToBasket(basketAddRequest);
        Assert.AreEqual(0, response.ErrorCount);
        Assert.AreEqual(2, mockLogger.Exceptions.Count);
      }
      finally
      {
        EngineLogging.EngineLogger = oldLogger;
      }
    }

    [TestMethod]
    public void BasketResponseWithUnknownErrors()
    {
      var tripletResponseWithErrors = BasketAddResponseData.FromResponseXml(Resources.UnknownResponse);
      var basketResponse = ProviderBasketResponse.FromBasketResponseStatus(tripletResponseWithErrors.Status);
      Assert.AreNotEqual(0, basketResponse.ErrorCount);
      Assert.AreEqual(basketResponse.ErrorCount, basketResponse.Errors.Count());

      IBasketError error;
      var foundError = basketResponse.TryGetError("definitelyNotThere", out error);
      Assert.IsFalse(foundError);
      Assert.IsNull(error);

      foundError = basketResponse.TryGetError("0", out error);
      Assert.IsTrue(foundError);
      Assert.IsNotNull(error);

      foundError = basketResponse.TryGetError(null, out error);
      Assert.IsFalse(foundError);
      Assert.IsNull(error);
    }

    [TestMethod]
    public void BasketResponseWithSuccess()
    {
      var tripletResponseWithErrors = BasketAddResponseData.FromResponseXml(Resources.SuccessResponse);
      var basketResponse = ProviderBasketResponse.FromBasketResponseStatus(tripletResponseWithErrors.Status);
      Assert.AreEqual(0, basketResponse.ErrorCount);
      Assert.AreEqual(BasketResponseStatusType.Success.ToString(), basketResponse.Message);
    }

    [TestMethod]
    public void BasketAddRequestBuilderExtraElement()
    {
      var container = SetContext<TestBasketProvider>(MockShopperDataProvider.MockCreatedShopperId);
      var basket = container.Resolve<IBasketProvider>();
      var basketAddRequest = basket.NewAddRequest();
      basketAddRequest.AddElement(new XElement("AdditionalElement"));
      
      var request = BasketAddRequestBuilder.FromBasketAddRequest(MockShopperDataProvider.MockCreatedShopperId, basketAddRequest);
      var testElement = XElement.Parse(request.ToXML());

      var additionalElement = testElement.Descendants("AdditionalElement").FirstOrDefault();
      Assert.IsNotNull(additionalElement);
    }

    [TestMethod]
    public void BasketAddRequestBuilderNumericItemAttributes()
    {
      var container = SetContext<TestBasketProvider>(MockShopperDataProvider.MockCreatedShopperId);
      var basket = container.Resolve<IBasketProvider>();
      var basketAddRequest = basket.NewAddRequest();
      var basketAddItem = basket.NewBasketAddItem(101, null);
      basketAddItem.Duration = 1.5f;
      basketAddItem.StackId = 4;
      basketAddRequest.AddItem(basketAddItem);

      var request = BasketAddRequestBuilder.FromBasketAddRequest(MockShopperDataProvider.MockCreatedShopperId, basketAddRequest);
      var testElement = XElement.Parse(request.ToXML());

      var item = testElement.Descendants("item").FirstOrDefault();
      var stackId = item.Attribute(BasketItemAttributes.StackId);
      Assert.AreEqual("4", stackId.Value);
      var duration = item.Attribute(BasketItemAttributes.Duration);
      Assert.AreEqual("1.5", duration.Value);
    }

    [TestMethod]
    public void BasketAddRequestBuilderCustomXmlOnItem()
    {
      var container = SetContext<TestBasketProvider>(MockShopperDataProvider.MockCreatedShopperId);
      var basket = container.Resolve<IBasketProvider>();
      var basketAddRequest = basket.NewAddRequest();
      var basketAddItem = basket.NewBasketAddItem(101, null);
      basketAddItem.CustomXml = new XElement("DOMAIN");
      basketAddRequest.AddItem(basketAddItem);

      var request = BasketAddRequestBuilder.FromBasketAddRequest(MockShopperDataProvider.MockCreatedShopperId, basketAddRequest);
      var testElement = XElement.Parse(request.ToXML());

      var item = testElement.Descendants("item").FirstOrDefault();
      var domain = item.Element("DOMAIN");
      Assert.IsNotNull(domain);
    }

    [TestMethod]
    public void BasketAddRequestBuilderPriceOverride()
    {
      var container = SetContext<TestBasketProvider>(MockShopperDataProvider.MockCreatedShopperId);
      var basket = container.Resolve<IBasketProvider>();
      var basketAddRequest = basket.NewAddRequest();
      var basketAddItem = basket.NewBasketAddItem(101, null);
      basketAddItem.SetOverridePrice(999, 199);
      basketAddRequest.AddItem(basketAddItem);

      var request = BasketAddRequestBuilder.FromBasketAddRequest(MockShopperDataProvider.MockCreatedShopperId, basketAddRequest);
      var testElement = XElement.Parse(request.ToXML());

      var item = testElement.Descendants("item").FirstOrDefault();

      var ovList = item.Attribute(BasketItemAttributes.OverrideListPrice);
      Assert.IsNotNull(ovList);
      var ovCurrent = item.Attribute(BasketItemAttributes.OverrideCurrentPrice);
      Assert.IsNotNull(ovCurrent);
      var ovHash = item.Attribute(BasketItemAttributes.OverrideHash);
      Assert.IsNotNull(ovHash);
    }

    [TestMethod]
    public void BasketAddRequestBuilderMultipleItems()
    {
      var container = SetContext<TestBasketProvider>(MockShopperDataProvider.MockCreatedShopperId);
      var basket = container.Resolve<IBasketProvider>();
      var basketAddRequest = basket.NewAddRequest();
      var basketAddItem = basket.NewBasketAddItem(101, null);
      basketAddRequest.AddItem(basketAddItem);
      var secondItem = basket.NewBasketAddItem(58, null);
      basketAddRequest.AddItem(secondItem);

      var request = BasketAddRequestBuilder.FromBasketAddRequest(MockShopperDataProvider.MockCreatedShopperId, basketAddRequest);
      var testElement = XElement.Parse(request.ToXML());

      Assert.AreEqual(2, testElement.Descendants("item").Count());
    }

    [TestMethod]
    public void BasketAddRequestBuilderChildItems()
    {
      var container = SetContext<TestBasketProvider>(MockShopperDataProvider.MockCreatedShopperId);
      var basket = container.Resolve<IBasketProvider>();
      var basketAddRequest = basket.NewAddRequest();
      var basketAddItem = basket.NewBasketAddItem(101, null);
      basketAddRequest.AddItem(basketAddItem);
      var secondItem = basket.NewBasketAddItem(58, null);
      basketAddItem.AddChildItem(secondItem);

      var request = BasketAddRequestBuilder.FromBasketAddRequest(MockShopperDataProvider.MockCreatedShopperId, basketAddRequest);
      var testElement = XElement.Parse(request.ToXML());

      Assert.AreEqual(2, testElement.Descendants("item").Count());

      var firstItem = testElement.Descendants("item").FirstOrDefault();
      var lastItem = testElement.Descendants("item").LastOrDefault();

      var groupId = firstItem.Attribute(BasketItemAttributes.GroupId);
      Assert.IsNotNull(groupId);
      Assert.IsFalse(string.IsNullOrEmpty(groupId.Value));

      var parentgroupId = firstItem.Attribute(BasketItemAttributes.ParentGroupId);
      var childgroupId = lastItem.Attribute(BasketItemAttributes.GroupId);

      Assert.AreEqual(groupId.Value, parentgroupId.Value);
      Assert.AreEqual(groupId.Value, childgroupId.Value);

      var childparentgroupid = lastItem.Attribute(BasketItemAttributes.ParentGroupId);
      Assert.IsNull(childparentgroupid);
    }

    [TestMethod]
    public void BasketItemCount()
    {
      ConfigElement itemCountElement;
      Engine.Engine.TryGetConfigElement(BasketEngineRequests.ItemCount, out itemCountElement);
      itemCountElement.ResetStats();

      var container = SetContext<TestBasketProvider>(MockShopperDataProvider.MockCreatedShopperId);
      var basket = container.Resolve<IBasketProvider>();
      int total = basket.TotalItems;

      Assert.IsTrue(total >= 0);
      Assert.AreEqual(1, itemCountElement.Stats.Succeeded);

      int total2 = basket.TotalItems;
      Assert.AreEqual(total, total2);
      Assert.AreEqual(1, itemCountElement.Stats.Succeeded);
    }

    [TestMethod]
    public void BasketItemCountEmptyShopper()
    {
      ConfigElement itemCountElement;
      Engine.Engine.TryGetConfigElement(BasketEngineRequests.ItemCount, out itemCountElement);
      itemCountElement.ResetStats();

      var container = SetContext<TestBasketProvider>(string.Empty);
      var basket = container.Resolve<IBasketProvider>();
      int total = basket.TotalItems;

      Assert.AreEqual(0, total);
      Assert.AreEqual(0, itemCountElement.Stats.Succeeded);
      Assert.AreEqual(0, itemCountElement.Stats.Failed);
    }

    [TestMethod]
    public void BasketItemCountError()
    {
      ConfigElement itemCountElement;
      Engine.Engine.TryGetConfigElement(BasketEngineRequests.ItemCount, out itemCountElement);
      itemCountElement.ResetStats();

      var container = SetContext<TestBasketProvider>("ERR");
      var basket = container.Resolve<IBasketProvider>();
      int total = basket.TotalItems;

      Assert.AreEqual(0, total);
      Assert.AreEqual(1, itemCountElement.Stats.Failed);
    }

  }
}
