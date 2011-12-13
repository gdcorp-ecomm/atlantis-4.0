using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.EcommEmailAccountUID.Interface;

namespace Atlantis.Framework.EcommEmailAccountUID.Tests
{
  [TestClass]
  public class GetAccountUID
  {
    [TestMethod]
    public void TestMethod1()
    {
      EcommEmailAccountUIDRequestData request = new EcommEmailAccountUIDRequestData("75866", string.Empty, "249571", string.Empty, 0);
      //request.ApplicationName = "Cart";
      //request.CertificateName = "corp.web.cart.dev.client.godaddy.com";
      //request.DataSourceName = "corp.web.cart.Godaddy";
      EcommEmailAccountUIDRsponseData response = (EcommEmailAccountUIDRsponseData)Engine.Engine.ProcessRequest(request, 277);
      //Assert.AreEqual(54867, response.InstantPurchaseShopperProfileID);
      Assert.AreEqual(response.IsSuccess, true);

    }
  }
}
