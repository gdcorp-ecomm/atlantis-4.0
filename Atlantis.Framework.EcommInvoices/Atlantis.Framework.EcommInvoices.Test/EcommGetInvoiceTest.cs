using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.EcommInvoices.Interface;
using Atlantis.Framework.EcommInvoices.Impl;
namespace Atlantis.Framework.EcommInvoices.Test
{
  [TestClass]
  public class EcommGetInvoiceTest
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetAllInvoicesForShopperTest()
    {
      RetrievalAttributes retAttr = new RetrievalAttributes();
      EcommInvoicesRequestData request = new EcommInvoicesRequestData("830398", string.Empty, string.Empty, string.Empty, 0, retAttr);
      EcommInvoicesResponseData response = (EcommInvoicesResponseData)Engine.Engine.ProcessRequest(request, 439);

      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetCancelledInvoicesForShopperTest()
    {
      RetrievalAttributes retAttr = new RetrievalAttributes(InvoiceStatus.Cancelled);
      EcommInvoicesRequestData request = new EcommInvoicesRequestData("830398", string.Empty, string.Empty, string.Empty, 0, retAttr);
      EcommInvoicesResponseData response = (EcommInvoicesResponseData)Engine.Engine.ProcessRequest(request, 439);

      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetOrderedByAmountInvoicesForShopperTest()
    {
      RetrievalAttributes retAttr = new RetrievalAttributes();
      retAttr.SortColumn = "amount";
      EcommInvoicesRequestData request = new EcommInvoicesRequestData("830398", string.Empty, string.Empty, string.Empty, 0, retAttr);
      EcommInvoicesResponseData response = (EcommInvoicesResponseData)Engine.Engine.ProcessRequest(request, 439);

      Assert.IsTrue(response.IsSuccess);
    }  
  }
}
