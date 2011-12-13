using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.EcommDomainEmailCredit.Interface;

namespace Atlantis.Framework.EcommDomainEmailCredit.Tests
{
  [TestClass]
  public class EcommDomainEmailCreditResponseTest
  {
    [TestMethod]
    public void TestMethod1()
    {
      EcommDomainEmailCreditRequestData request =
   new EcommDomainEmailCreditRequestData("75866", string.Empty,"249382", string.Empty, 0);
      //request.ApplicationName = "Cart";
      //request.CertificateName = "corp.web.cart.dev.client.godaddy.com";
      //request.DataSourceName = "corp.web.cart.Godaddy";
      EcommDomainEmailCreditResponseData response = (EcommDomainEmailCreditResponseData)Engine.Engine.ProcessRequest(request, 270);
      //Assert.AreEqual(54867, response.InstantPurchaseShopperProfileID);
      Assert.AreEqual(response.IsSuccess, true);

    }

  }
}
