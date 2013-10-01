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
  [DeploymentItem("Atlantis.Framework.ECCGetShopperByEmailAddress.Impl.dll")]
  public class UnitTest1
  {
    string _token = "9f2aa70b!Zd3V";
    string _authId = "IDPDEV";

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetShopperByEmailTest()
    {
      string emailAddress = "blah@intrepidkjs.com";
      var request = new ECCGetShopperByEmailAddressRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, emailAddress, _token, _authId);
      var response = (ECCGetShopperByEmailAddressResponseData)Engine.Engine.ProcessRequest(request, 495);

      Assert.IsTrue(response.IsSuccess);
      Assert.IsTrue(response.ShopperID.Length > 1);
      Assert.IsTrue(response.Message.Length <= 0);
      Assert.IsTrue(response.ResultCode.Length > 0);
      Debug.WriteLine(response.ShopperID);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetShopperByInvalidEmailTest()
    {
      string emailAddress = "thisemailshouldnotexist@godaddy.com";
      var request = new ECCGetShopperByEmailAddressRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, emailAddress, _token, _authId);
      var response = (ECCGetShopperByEmailAddressResponseData)Engine.Engine.ProcessRequest(request, 495);

      Assert.IsTrue(response.IsSuccess);
      Assert.IsTrue(response.ShopperID.Length <= 1);
      Assert.IsTrue(response.Message.Length > 0);
      Assert.IsTrue(response.ResultCode.Length > 0);
      Debug.WriteLine(response.ShopperID);
      Debug.WriteLine(response.Message);
      Debug.WriteLine(response.ResultCode);
    }
  }
}
