using Atlantis.Framework.EcommInvoiceToBasket.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.EcommInvoiceToBasket.Test
{
  [TestClass]
  public class TransferItemsToCartTest
  {
    string uid = "3484CABE-A2B1-4947-8C3B-6E487C74D79F";
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TransferItemsToCartFromInvoiceTest()
    {
      EcommInvoiceToBasketRequestData request = new EcommInvoiceToBasketRequestData("830398", string.Empty, string.Empty, string.Empty, 0, uid);
      EcommInvoiceToBasketResponseData response = (EcommInvoiceToBasketResponseData)Engine.Engine.ProcessRequest(request, 441);

      Assert.IsTrue(response.IsSuccess);

    }
  }
}
