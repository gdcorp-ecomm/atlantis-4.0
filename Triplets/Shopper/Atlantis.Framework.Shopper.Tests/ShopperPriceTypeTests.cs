using Atlantis.Framework.Interface;
using Atlantis.Framework.Shopper.Interface;
using Atlantis.Framework.Testing.MockEngine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;

namespace Atlantis.Framework.Shopper.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Shopper.Impl.dll")]
  public class ShopperPriceTypeTests
  {
    static IErrorLogger _defaultEngineLogger;
    static MockErrorLogger _mockLogger;

    [ClassInitialize]
    public static void MockLogger(TestContext context)
    {
      _defaultEngineLogger = Engine.EngineLogging.EngineLogger;
      _mockLogger = new MockErrorLogger();
      Engine.EngineLogging.EngineLogger = _mockLogger;
    }

    [ClassCleanup]
    public static void SetLoggerBack()
    {
      Engine.EngineLogging.EngineLogger = _defaultEngineLogger;
    }

    [TestMethod]
    public void ShopperPriceTypeRequestDataProperties()
    {
      var request = new ShopperPriceTypeRequestData("832652", 42);
      Assert.AreEqual("832652", request.ShopperID);
      Assert.AreEqual(42, request.PrivateLabelId);
    }

    [TestMethod]
    public void ShopperPriceTypeResponseDataPropertiesStandard()
    {
      Assert.AreEqual(0, ShopperPriceTypeResponseData.Standard.ActivePriceType);
      Assert.AreEqual(0, ShopperPriceTypeResponseData.Standard.MaskedPriceType);
    }

    [TestMethod]
    public void ShopperPriceTypeResponseDataPropertiesBasic()
    {
      var response = ShopperPriceTypeResponseData.FromRawPriceType(57, 1);
      Assert.IsNull(response.GetException());
      var xml = response.ToXML();
      XElement.Parse(xml);
      Assert.IsTrue(xml.Contains("\"57\""));
    }

    [TestMethod]
    public void ShopperPriceTypeResponseDataPropertiesFromMaskGoDaddy()
    {
      var response = ShopperPriceTypeResponseData.FromRawPriceType(57, 1);
      Assert.AreEqual(32, response.ActivePriceType);

      response = ShopperPriceTypeResponseData.FromRawPriceType(29, 1);
      Assert.AreEqual(8, response.ActivePriceType);

      response = ShopperPriceTypeResponseData.FromRawPriceType(17, 1);
      Assert.AreEqual(16, response.ActivePriceType);

      response = ShopperPriceTypeResponseData.FromRawPriceType(7, 1);
      Assert.AreEqual(0, response.ActivePriceType);
    }

    [TestMethod]
    public void ShopperPriceTypeResponseDataPropertiesFromMaskBlueRazor()
    {
      var response = ShopperPriceTypeResponseData.FromRawPriceType(57, 2);
      Assert.AreEqual(1, response.ActivePriceType);

      response = ShopperPriceTypeResponseData.FromRawPriceType(16, 2);
      Assert.AreEqual(0, response.ActivePriceType);
    }

    [TestMethod]
    public void ShopperPriceTypeResponseDataPropertiesFromMaskReseller()
    {
      var response = ShopperPriceTypeResponseData.FromRawPriceType(7, 1724);
      Assert.AreEqual(2, response.ActivePriceType);

      response = ShopperPriceTypeResponseData.FromRawPriceType(5, 1724);
      Assert.AreEqual(0, response.ActivePriceType);
    }

    const int _REQUESTTYPE = 736;

    [TestMethod]
    public void ShopperPriceTypeRequestValid()
    {
      _mockLogger.Exceptions.Clear();
      var request = new ShopperPriceTypeRequestData("832652", 1);
      var response = (ShopperPriceTypeResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.AreEqual(0, _mockLogger.Exceptions.Count);
    }

    [TestMethod]
    public void ShopperPriceTypeRequestNull()
    {
      var request = new ShopperPriceTypeRequestData(null, 1);
      var response = (ShopperPriceTypeResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.IsTrue(ReferenceEquals(response, ShopperPriceTypeResponseData.Standard));
    }

    [TestMethod]
    public void ShopperPriceTypeRequestEmpty()
    {
      var request = new ShopperPriceTypeRequestData(string.Empty, 1);
      var response = (ShopperPriceTypeResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.IsTrue(ReferenceEquals(response, ShopperPriceTypeResponseData.Standard));
    }

    [TestMethod]
    public void ShopperPriceTypeRequestInvalidPrivateLabel()
    {
      var request = new ShopperPriceTypeRequestData("832652", -32);
      var response = (ShopperPriceTypeResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.IsTrue(ReferenceEquals(response, ShopperPriceTypeResponseData.Standard));
    }

    [TestMethod]
    public void ShopperPriceTypeRequestException()
    {
      _mockLogger.Exceptions.Clear();
      var request = new WrongTypeRequestData();
      var response = (ShopperPriceTypeResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.IsTrue(ReferenceEquals(response, ShopperPriceTypeResponseData.Standard));
      Assert.AreNotEqual(0, _mockLogger.Exceptions.Count);
    }


  }
}
