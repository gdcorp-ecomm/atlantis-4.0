using System;
using Atlantis.Framework.DataCache;
using Atlantis.Framework.MYAGetMobileCarriers.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Engine;

namespace MYAGetMobileCarriers.Tests
{
  [TestClass]
  public class MYAGetMobileCarriersTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("App.config")]
    public void MYAGetMobileCarriers()
    {
      MYAGetMobileCarriersRequestData requestData = new MYAGetMobileCarriersRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);

      MYAGetMobileCarriersResponseData response =
        (MYAGetMobileCarriersResponseData)Engine.ProcessRequest(requestData, 311);

      var myCarrierList = response.CarrierList;
      Console.WriteLine(response.ToXML());
      Assert.IsTrue(myCarrierList.Count == 9);
      Assert.IsTrue(response.IsSuccess);
    }
  

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("App.config")]
    public void MYAGetMobileCarriersFromDataCache()
    {
      MYAGetMobileCarriersRequestData requestData = new MYAGetMobileCarriersRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);

      MYAGetMobileCarriersResponseData response =
        (MYAGetMobileCarriersResponseData)DataCache.GetProcessRequest(requestData, 311);

      var myCarrierList = response.CarrierList;
      Console.WriteLine(response.ToXML());
      Assert.IsTrue(myCarrierList.Count == 9);
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
