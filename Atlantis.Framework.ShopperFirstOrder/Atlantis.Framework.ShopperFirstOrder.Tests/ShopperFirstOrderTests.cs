using Atlantis.Framework.ShopperFirstOrder.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Atlantis.Framework.ShopperFirstOrder.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.ShopperFirstOrder.Impl.dll")]
  [DeploymentItem("netConnect.dll")]
  public class ShopperFirstOrderTests
  {
    const int _REQUESTID = 641;

    [TestMethod]
    public void ShopperFirstOrderBasic()
    {
      ShopperFirstOrderRequestData request = new ShopperFirstOrderRequestData("832652", string.Empty, string.Empty, string.Empty, 0);
      ShopperFirstOrderResponseData response = (ShopperFirstOrderResponseData)Engine.Engine.ProcessRequest(request, _REQUESTID);
    }

    [TestMethod]
    public void EmptyShopper()
    {
      ShopperFirstOrderRequestData request = new ShopperFirstOrderRequestData("", string.Empty, string.Empty, string.Empty, 0);
      ShopperFirstOrderResponseData response = (ShopperFirstOrderResponseData)Engine.Engine.ProcessRequest(request, _REQUESTID);
      Assert.IsFalse(response.HasOrder);
    }

    [TestMethod]
    public void NullShopper()
    {
      ShopperFirstOrderRequestData request = new ShopperFirstOrderRequestData(null, string.Empty, string.Empty, string.Empty, 0);
      ShopperFirstOrderResponseData response = (ShopperFirstOrderResponseData)Engine.Engine.ProcessRequest(request, _REQUESTID);
      Assert.IsFalse(response.HasOrder);
    }

    [TestMethod]
    public void ShopperWithNoOrders()
    {
      ShopperFirstOrderRequestData request = new ShopperFirstOrderRequestData("noZhxp2er", string.Empty, string.Empty, string.Empty, 0);
      ShopperFirstOrderResponseData response = (ShopperFirstOrderResponseData)Engine.Engine.ProcessRequest(request, _REQUESTID);
      Assert.IsFalse(response.HasOrder);
    }

    [TestMethod]
    public void ShopperWithWithOrders()
    {
      ShopperFirstOrderRequestData request = new ShopperFirstOrderRequestData("832652", string.Empty, string.Empty, string.Empty, 0);
      ShopperFirstOrderResponseData response = (ShopperFirstOrderResponseData)Engine.Engine.ProcessRequest(request, _REQUESTID);
      Assert.IsTrue(response.HasOrder);
    }

    [TestMethod]
    public void ShopperWithWithFirstOrderId()
    {
      ShopperFirstOrderRequestData request = new ShopperFirstOrderRequestData("832652", string.Empty, string.Empty, string.Empty, 0);
      ShopperFirstOrderResponseData response = (ShopperFirstOrderResponseData)Engine.Engine.ProcessRequest(request, _REQUESTID);
      Assert.IsFalse(string.IsNullOrEmpty(response.FirstOrderId));
    }

    [TestMethod]
    public void DefaultTimeout()
    {
      ShopperFirstOrderRequestData request = new ShopperFirstOrderRequestData("noZhxp2er", string.Empty, string.Empty, string.Empty, 0);
      Assert.AreEqual(TimeSpan.FromSeconds(2), request.RequestTimeout);
    }

    [TestMethod]
    public void RequestToXml()
    {
      ShopperFirstOrderRequestData request = new ShopperFirstOrderRequestData("noZhxp2er", string.Empty, string.Empty, string.Empty, 0);
      Assert.IsTrue(request.ToXML().Contains("noZhxp2er"));
    }
  
  }
}
