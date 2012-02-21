using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Engine;
using Atlantis.Framework.ECCGetShopperByEmailAddress.Interface;
using Atlantis.Framework.Interface;
using System.Diagnostics;

namespace Atlantis.Framework.ECCGetShopperByEmailAddress.Test
{
  [TestClass]
  public class UnitTest1
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetShopperByEmailTest()
    {
      string emailAddress = "blah@intrepidkjs.com";
      var request = new ECCGetShopperByEmailAddressRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, emailAddress);
      var response = (ECCGetShopperByEmailAddressResponseData)Engine.Engine.ProcessRequest(request, 495);

      Assert.IsTrue(response.IsSuccess);
      Assert.IsTrue(response.ShopperID.Length > 1);
      Debug.WriteLine(response.ShopperID);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetShopperByInvalidEmailTest()
    {
      string emailAddress = "thisemailshouldnotexist@godaddy.com";
      var request = new ECCGetShopperByEmailAddressRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, emailAddress);
      var response = (ECCGetShopperByEmailAddressResponseData)Engine.Engine.ProcessRequest(request, 495);

      Assert.IsTrue(response.IsSuccess);
      Assert.IsTrue(response.ShopperID.Length <= 1);
      Debug.WriteLine(response.ShopperID);
    }
  }
}
