using System;
using Atlantis.Framework.Basket.Impl;
using Atlantis.Framework.Basket.Interface;
using Atlantis.Framework.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Atlantis.Framework.Basket.Tests
{
  [TestClass]
  public class BasketDeleteRequestTests
  {
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void WillRequestHandlerThrowArgumentExceptionIfRequestDataIsNotOfTypeBasketDeleteRequestData()
    {
      var request = new BasketDeleteRequest();
      request.RequestHandler(It.IsAny<RequestData>(), It.IsAny<ConfigElement>());
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void WillRequestHandlerThrowArgumentExceptionIfItemsToDeleteIsEmpty()
    {
      var request = new BasketDeleteRequest();
      request.RequestHandler(new BasketDeleteRequestData(string.Empty, null), It.IsAny<WsConfigElement>());
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void WillRequestHandlerThrowArgumentExceptionIfConfigIsNotOfTypeWsConfigElement()
    {
      var request = new BasketDeleteRequest();
      request.RequestHandler(new BasketDeleteRequestData(string.Empty, new[] { new BasketDeleteItemKey(1, 2) }), It.IsAny<ConfigElement>());
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void WillRequestHandlerThrowExceptionIfResponseXmlContainsError()
    {
      var basketService = new Mock<IBasketService>();
      basketService.Setup(
        s =>
          s.DeleteItemsByPairWithType(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(),
            It.IsAny<string>(), It.IsAny<int>()))
        .Returns("<error>");
      var request = new BasketDeleteRequest(basketService.Object);
      request.RequestHandler(new BasketDeleteRequestData(string.Empty, new[] {new BasketDeleteItemKey(1, 2)}),
        new WsConfigElement(0, string.Empty, string.Empty,
          "http://someUrl"));
    }

    [TestMethod]
    public void RequestHandlerShouldNotThrowIfIsManagerAndResponseXmlDoesNotContainError()
    {
      var basketService = new Mock<IBasketService>();
      basketService.Setup(
        s =>
          s.DeleteItemsByPairWithType(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(),
            It.IsAny<string>(), It.IsAny<int>()))
        .Returns("<something />");
      var request = new BasketDeleteRequest(basketService.Object);
      var requestData = new BasketDeleteRequestData(string.Empty, new[] {new BasketDeleteItemKey(1, 2)})
      {
        IsManager = true
      };
      var responseData = request.RequestHandler(requestData,
        new WsConfigElement(0, string.Empty, string.Empty, "http://someUrl"));

      Assert.IsNotNull(responseData);
    }

    [TestMethod]
    public void RequestHandlerShouldNotReturnNull()
    {
      var basketService = new Mock<IBasketService>();
      basketService.Setup(
        s =>
          s.DeleteItemsByPairWithType(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(),
            It.IsAny<string>(), It.IsAny<int>()))
        .Returns("<something />");
      var request = new BasketDeleteRequest(basketService.Object);
      var responseData =
        request.RequestHandler(new BasketDeleteRequestData(string.Empty, new[] { new BasketDeleteItemKey(1, 2) }),
          new WsConfigElement(0, string.Empty, string.Empty,
            "http://someUrl"));

      Assert.IsNotNull(responseData);
    }

    [TestMethod]
    public void WillRequestHandlerReturnBasketDeleteResponseData()
    {
      var basketService = new Mock<IBasketService>();
      basketService.Setup(
        s =>
          s.DeleteItemsByPairWithType(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(),
            It.IsAny<string>(), It.IsAny<int>()))
        .Returns("<something />");
      var request = new BasketDeleteRequest(basketService.Object);
      var responseData =
        request.RequestHandler(new BasketDeleteRequestData(string.Empty, new[] {new BasketDeleteItemKey(1, 2)}),
          new WsConfigElement(0, string.Empty, string.Empty,
            "http://someUrl"));

      Assert.IsInstanceOfType(responseData, typeof(BasketDeleteResponseData));
    }
  }
}
