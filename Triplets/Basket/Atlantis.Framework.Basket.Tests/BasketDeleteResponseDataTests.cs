using System.Xml.Linq;
using Atlantis.Framework.Basket.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Basket.Tests
{
  [TestClass]
  public class BasketDeleteResponseDataTests
  {
    [TestMethod]
    public void WillFromDataSetStatusToNotNull()
    {
      var responseXml =
        new XElement("Response", new XElement("MESSAGE", "Success")).ToString(SaveOptions.DisableFormatting);
      var response = BasketDeleteResponseData.FromData(responseXml);

      Assert.IsNotNull(response.Status);
    }


    [TestMethod]
    public void WillFromDataSetStatusToBeOfTypeBasketResponseStatus()
    {
      var responseXml =
        new XElement("Response", new XElement("MESSAGE", "Success")).ToString(SaveOptions.DisableFormatting);
      var response = BasketDeleteResponseData.FromData(responseXml);

      Assert.IsInstanceOfType(response.Status, typeof(BasketResponseStatus));
    }
  }
}
