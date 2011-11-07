using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.EcommInvoiceCancel.Interface;

namespace Atlantis.Framework.EcommInvoiceCancel.Test
{
  [TestClass]
  public class CancelInvoice
  {
    string uid = "F89C9E87-10C0-4E01-A7D9-20500E4EA3D2";
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void CancelInvoiceTest()
    {
      EcommInvoiceCancelRequestData request = new EcommInvoiceCancelRequestData("830398", string.Empty, string.Empty, string.Empty, 0, uid);
      EcommInvoiceCancelResponseData response = (EcommInvoiceCancelResponseData)Engine.Engine.ProcessRequest(request, 442);

      Assert.IsTrue(response.IsSuccess);
    }
  }
}
