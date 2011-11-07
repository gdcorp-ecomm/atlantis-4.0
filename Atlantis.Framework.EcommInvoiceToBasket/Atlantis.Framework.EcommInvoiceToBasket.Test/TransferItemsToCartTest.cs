using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.EcommInvoiceToBasket.Interface;

namespace Atlantis.Framework.EcommInvoiceToBasket.Test
{
  [TestClass]
  public class TransferItemsToCartTest
  {

    #warning Test will never work because the shopper id 830398 does not have any invoices with order item details.

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
