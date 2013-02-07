using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.PixelsGetXML.Interface;
using Atlantis.Framework.PixelsGetXML.Impl;
using Atlantis.Framework.Engine;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.PixelsGetXML.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.PixelsGetXML.Impl.dll")]
  [DeploymentItem("netConnect.dll")]
  public class PixelsGetXMLTests
  {
    private const int _REQUESTID = 648;
    private const string _shopperId = "";

    [TestMethod]
    public void PixelsGetXMLBasic()
    {
      PixelsGetXMLRequestData request = new PixelsGetXMLRequestData(_shopperId, string.Empty, string.Empty, string.Empty, 0, "CART", "Order_Confirmation");
      PixelsGetXMLResponseData response = (PixelsGetXMLResponseData)Engine.Engine.ProcessRequest(request, _REQUESTID);
    }

    [TestMethod]
    public void PixelsGetXMLBasicSuccess()
    {
      PixelsGetXMLRequestData request = new PixelsGetXMLRequestData(_shopperId, string.Empty, string.Empty, string.Empty, 0, "CART", "Order_Confirmation");
      PixelsGetXMLResponseData response = (PixelsGetXMLResponseData)Engine.Engine.ProcessRequest(request, _REQUESTID);
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    public void PixelsGetXMLBasicFail()
    {
      PixelsGetXMLRequestData request = new PixelsGetXMLRequestData(_shopperId, string.Empty, string.Empty, string.Empty, 0, string.Empty, string.Empty);
      PixelsGetXMLResponseData response = (PixelsGetXMLResponseData)Engine.Engine.ProcessRequest(request, _REQUESTID);
      Assert.IsFalse(response.IsSuccess);
    }

    [TestMethod]
    public void PixelsGetXMLDataCache()
    {
      PixelsGetXMLRequestData request = new PixelsGetXMLRequestData(_shopperId, string.Empty, string.Empty, string.Empty, 0, "CART", "Order_Confirmation");
      PixelsGetXMLResponseData response = (PixelsGetXMLResponseData)DataCache.DataCache.GetProcessRequest(request, _REQUESTID);
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
