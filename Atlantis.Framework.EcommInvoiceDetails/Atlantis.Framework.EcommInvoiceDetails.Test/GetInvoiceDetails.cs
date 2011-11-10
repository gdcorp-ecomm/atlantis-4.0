using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.EcommInvoiceDetails.Interface;

namespace Atlantis.Framework.EcommInvoiceDetails.Test
{
  [TestClass]
  public class GetInvoiceDetails
  {
    string invoiceId = "E02DFC75-4CC9-4C2D-A8F1-97B6ABFD5B29";

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetInvoiceDetailTest()
    {
      EcommInvoiceDetailsRequestData request = new EcommInvoiceDetailsRequestData("830398", string.Empty, string.Empty, string.Empty, 0, invoiceId);
      EcommInvoiceDetailsResponseData response = (EcommInvoiceDetailsResponseData)Engine.Engine.ProcessRequest(request, 440);

      Assert.IsTrue(response.IsSuccess);
    }
  }
}
