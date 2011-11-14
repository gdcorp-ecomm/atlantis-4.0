using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.EcommInvoiceDetails.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.EcommInvoiceDetails.Test
{
  [TestClass]
  public class GetInvoiceDetails
  {
    string invoiceId = "EE3CF0BF-7EE4-47D7-AEBA-B0D6E4CC9231";

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetInvoiceDetailTest()
    {
      EcommInvoiceDetailsRequestData request = new EcommInvoiceDetailsRequestData("830398", string.Empty, string.Empty, string.Empty, 0, invoiceId);
      EcommInvoiceDetailsResponseData response = (EcommInvoiceDetailsResponseData)Engine.Engine.ProcessRequest(request, 440);

      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void ParseXmlTest()
    {
      EcommInvoiceDetailsRequestData request = new EcommInvoiceDetailsRequestData("867900", string.Empty, string.Empty, string.Empty, 0, invoiceId);
      EcommInvoiceDetailsResponseData response = (EcommInvoiceDetailsResponseData)Engine.Engine.ProcessRequest(request, 440);

      XDocument xd = new XDocument();
      xd = XDocument.Parse(response.ToXML());
      string s = "hi";
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
