using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.EcommDomainFreeProductCredit.Interface;

namespace Atlantis.Framework.EcommDomainFreeProductCreditTest
{
  [TestClass]
  public class EcommdomainFreeProductCredit
  {
    [TestMethod]
    public void TestMethod1()
    {
      EcommDomainFreeProductCreditRequestData request =
 new EcommDomainFreeProductCreditRequestData("75866", string.Empty, "249382", string.Empty, 0,16);
      //request.ApplicationName = "Cart";
      //request.CertificateName = "corp.web.cart.dev.client.godaddy.com";
      //request.DataSourceName = "corp.web.cart.Godaddy";
      EcommDomainFreeProductCreditResponseData response = (EcommDomainFreeProductCreditResponseData)Engine.Engine.ProcessRequest(request, 294);
      //Assert.AreEqual(54867, response.InstantPurchaseShopperProfileID);
      Assert.AreEqual(response.IsSuccess, true);
    }
  }
}
