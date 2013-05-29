using Atlantis.Framework.EcommInvoices.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Atlantis.Framework.EcommInvoices.Test
{
  [TestClass]
  public class EcommGetInvoiceTest
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.EcommInvoices.Impl.dll")]
    public void GetAllInvoicesForShopperTest()
    {
      RetrievalAttributes retAttr = new RetrievalAttributes();
      EcommInvoicesRequestData request = new EcommInvoicesRequestData("856907", string.Empty, string.Empty, string.Empty, 0, retAttr);
      EcommInvoicesResponseData response = (EcommInvoicesResponseData)Engine.Engine.ProcessRequest(request, 439);

      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.EcommInvoices.Impl.dll")]
    public void GetCancelledInvoicesForShopperTest()
    {
      RetrievalAttributes retAttr = new RetrievalAttributes(InvoiceStatus.Cancelled);
      EcommInvoicesRequestData request = new EcommInvoicesRequestData("830398", string.Empty, string.Empty, string.Empty, 0, retAttr);
      EcommInvoicesResponseData response = (EcommInvoicesResponseData)Engine.Engine.ProcessRequest(request, 439);

      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.EcommInvoices.Impl.dll")]
    public void GetOrderedByAmountInvoicesForShopperTest()
    {
      RetrievalAttributes retAttr = new RetrievalAttributes();
      retAttr.SortColumn = "amount";
      EcommInvoicesRequestData request = new EcommInvoicesRequestData("857623", string.Empty, string.Empty, string.Empty, 0, retAttr);
      EcommInvoicesResponseData response = (EcommInvoicesResponseData)Engine.Engine.ProcessRequest(request, 439);

      Assert.IsTrue(response.IsSuccess);
    }  
  }
}
