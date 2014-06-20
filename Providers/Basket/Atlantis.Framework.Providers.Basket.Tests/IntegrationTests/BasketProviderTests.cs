using System;
using System.Xml.Linq;
using Atlantis.Framework.Basket.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Basket.Interface;
using Atlantis.Framework.Providers.Shopper.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Atlantis.Framework.Providers.Basket.Tests.IntegrationTests
{
  [TestClass]
  public class BasketProviderTests
  {
    [TestMethod]
    public void WillCreateBasketResponseFromBasketResponseStatus()
    {
      var basketProvider = new TestBasketProvider(new Mock<ISiteContext>().Object, new Mock<IShopperContext>().Object,
        new Mock<IShopperDataProvider>().Object);
      var response =
        basketProvider.CreateBasketResponse(
          BasketDeleteResponseData.FromResponseXml(
            new XElement("Response", new XElement("MESSAGE", "Success")).ToString(SaveOptions.DisableFormatting)).Status);

      Assert.IsNotNull(response);
      Assert.IsInstanceOfType(response, typeof(IBasketResponse));
    }

    [TestMethod]
    public void WillCreateBasketResponseFromException()
    {
      var basketProvider = new TestBasketProvider(new Mock<ISiteContext>().Object, new Mock<IShopperContext>().Object,
        new Mock<IShopperDataProvider>().Object);
      var response = basketProvider.CreateBasketResponse(new Exception("Some message"));

      Assert.IsNotNull(response);
      Assert.IsInstanceOfType(response, typeof(IBasketResponse));
    }
  }
}
