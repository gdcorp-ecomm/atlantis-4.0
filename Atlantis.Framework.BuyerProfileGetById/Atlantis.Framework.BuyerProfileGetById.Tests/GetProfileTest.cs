using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.BuyerProfileGetById.Interface;
using Atlantis.Framework.BuyerProfileGetById.Interface.BuyerProfileDetails;

namespace Atlantis.Framework.BuyerProfileGetById.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetProfileTest
  {

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("App.config")]    
    public void GetProfileTest1()
    {
      BuyerProfileGetByIdRequestData request = new BuyerProfileGetByIdRequestData("850774", string.Empty, string.Empty, string.Empty, 0, "150304");

      BuyerProfileGetByIdResponseData response = (BuyerProfileGetByIdResponseData)Engine.Engine.ProcessRequest(request, 408);

      if (response.IsSuccess)
      {
        BuyerProfileDetails detail = response.BuyerProfileDetail;
      }
      Assert.IsTrue(response.IsSuccess);

    }

  }
}
