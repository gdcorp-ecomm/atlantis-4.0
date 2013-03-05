using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;
using Atlantis.Framework.PrivateLabel.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PrivateLabel.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  [DeploymentItem("Atlantis.Framework.PrivateLabel.Impl.dll")]
  public class PrivateLabelTypeTests
  {
    [TestMethod]
    public void PrivateLabelTypeRequestProperties()
    {
      var request = new PrivateLabelTypeRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 99);
      Assert.AreEqual(99, request.PrivateLabelId);
      Assert.AreEqual(Convert.ToString(99), request.GetCacheMD5());
    }

    [TestMethod]
    public void PrivateLabelTypeCacheKeySame()
    {
      var request = new PrivateLabelTypeRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1);
      var requestSame = new PrivateLabelTypeRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1);
      Assert.AreEqual(request.GetCacheMD5(), requestSame.GetCacheMD5());
    }

    [TestMethod]
    public void PrivateLabelTypeCacheKeyDifferent()
    {
      var request = new PrivateLabelTypeRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1);
      var requestDifferent = new PrivateLabelTypeRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 2);
      Assert.AreNotEqual(request.GetCacheMD5(), requestDifferent.GetCacheMD5());
    }

    [TestMethod]
    public void PrivateLabelTypeRequestXml()
    {
      var request = new PrivateLabelTypeRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 99);
      string xml = request.ToXML();
      Assert.IsTrue(xml.Contains("privatelabelid=\"99\""));
      XElement.Parse(xml);
    }

    [TestMethod]
    public void PrivateLabelTypeResponseValid()
    {
      var response = PrivateLabelTypeResponseData.FromPrivateLabelType(2);
      Assert.IsNull(response.GetException());
      Assert.AreEqual(2, response.PrivateLabelType);

      string xml = response.ToXML();
      Assert.IsTrue(xml.Contains("2"));
      XElement.Parse(xml);
    }

    [TestMethod]
    public void PrivateLabelTypeResponseEmpty()
    {
      var response = PrivateLabelTypeResponseData.FromPrivateLabelType(0);
      Assert.IsNull(response.GetException());
      Assert.AreEqual(0, response.PrivateLabelType);

      string xml = response.ToXML();
      Assert.IsTrue(xml.Contains("0"));
      XElement.Parse(xml);
    }

    [TestMethod]
    public void PrivateLabelTypeResponseException()
    {
      AtlantisException exception = new AtlantisException("PrivateLabelTypeResponseException", "0", "TestMessage", "TestData", null, null);
      var response = PrivateLabelTypeResponseData.FromException(exception);
      Assert.AreEqual(0, response.PrivateLabelType);
      AtlantisException exceptionFromResponse = response.GetException();
      Assert.AreEqual("TestMessage", exceptionFromResponse.Message);
    }

    const int _REQUESTTYPE = 662;

    [TestMethod]
    public void PrivateLabelTypeExecuteValid()
    {
      var request = new PrivateLabelTypeRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1724);
      var response = (PrivateLabelTypeResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.AreEqual(2, response.PrivateLabelType);
    }

    [TestMethod]
    public void PrivateLabelTypeExecuteNotFound()
    {
      var request = new PrivateLabelTypeRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, -44);
      var response = (PrivateLabelTypeResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.AreEqual(0, response.PrivateLabelType);
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void PrivateLabelTypeExecuteException()
    {
      var request = new InvalidRequestData();
      var response = (PrivateLabelTypeResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
    }

  }
}
