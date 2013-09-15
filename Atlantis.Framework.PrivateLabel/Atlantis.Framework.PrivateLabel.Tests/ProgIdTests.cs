using Atlantis.Framework.Interface;
using Atlantis.Framework.PrivateLabel.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Xml.Linq;

namespace Atlantis.Framework.PrivateLabel.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.PrivateLabel.Impl.dll")]
  public class ProgIdTests
  {
    [TestMethod]
    public void ProgIdRequestProperties()
    {
      var request = new ProgIdRequestData(99);
      Assert.AreEqual(99, request.PrivateLabelId);
      Assert.AreEqual(Convert.ToString(99), request.GetCacheMD5());
    }

    [TestMethod]
    public void ProgIdCacheKeySame()
    {
      var request = new ProgIdRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1);
      var requestSame = new ProgIdRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1);
      Assert.AreEqual(request.GetCacheMD5(), requestSame.GetCacheMD5());
    }

    [TestMethod]
    public void ProgIdCacheKeyDifferent()
    {
      var request = new ProgIdRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1);
      var requestDifferent = new ProgIdRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 2);
      Assert.AreNotEqual(request.GetCacheMD5(), requestDifferent.GetCacheMD5());
    }

    [TestMethod]
    public void ProgIdRequestXml()
    {
      var request = new ProgIdRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 99);
      string xml = request.ToXML();
      Assert.IsTrue(xml.Contains("privatelabelid=\"99\""));
      XElement.Parse(xml);
    }

    [TestMethod]
    public void ProgIdResponseValid()
    {
      var response = ProgIdResponseData.FromProgId("hunter");
      Assert.IsNull(response.GetException());
      Assert.AreEqual("hunter", response.ProgId);

      string xml = response.ToXML();
      Assert.IsTrue(xml.Contains("hunter"));
      XElement.Parse(xml);
    }

    [TestMethod]
    public void ProgIdResponseEmpty()
    {
      var response = ProgIdResponseData.FromProgId(string.Empty);
      Assert.IsNull(response.GetException());
      Assert.AreEqual(string.Empty, response.ProgId);

      string xml = response.ToXML();
      Assert.IsTrue(xml.Contains("\"\""));
      XElement.Parse(xml);
    }

    [TestMethod]
    public void ProgIdResponseNull()
    {
      var response = ProgIdResponseData.FromProgId(null);
      Assert.IsNull(response.GetException());
      Assert.AreEqual(string.Empty, response.ProgId);

      string xml = response.ToXML();
      Assert.IsTrue(xml.Contains("\"\""));
      XElement.Parse(xml);
    }

    [TestMethod]
    public void ProgIdResponseException()
    {
      AtlantisException exception = new AtlantisException("ProgIdResponseException", "0", "TestMessage", "TestData", null, null);
      var response = ProgIdResponseData.FromException(exception);
      Assert.AreEqual(string.Empty, response.ProgId);
      AtlantisException exceptionFromResponse = response.GetException();
      Assert.AreEqual("TestMessage", exceptionFromResponse.Message);
    }

    const int _REQUESTTYPE = 661;

    [TestMethod]
    public void ProgIdExecuteValid()
    {
      var request = new ProgIdRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1724);
      var response = (ProgIdResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.AreEqual("hunter", response.ProgId);
    }

    [TestMethod]
    public void ProgIdExecuteNotFound()
    {
      var request = new ProgIdRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, -44);
      var response = (ProgIdResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.AreEqual(string.Empty, response.ProgId);
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void ProgIdExecuteException()
    {
      var request = new InvalidRequestData();
      var response = (ProgIdResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
    }

  }
}
