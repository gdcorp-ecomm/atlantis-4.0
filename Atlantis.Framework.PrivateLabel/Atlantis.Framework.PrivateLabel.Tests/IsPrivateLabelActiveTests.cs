using Atlantis.Framework.Interface;
using Atlantis.Framework.PrivateLabel.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Xml.Linq;

namespace Atlantis.Framework.PrivateLabel.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  [DeploymentItem("Atlantis.Framework.PrivateLabel.Impl.dll")]
  public class IsPrivateLabelActiveTests
  {
    [TestMethod]
    public void IsPrivateLabelActiveRequestProperties()
    {
      var request = new IsPrivateLabelActiveRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 99);
      Assert.AreEqual(99, request.PrivateLabelId);
      Assert.AreEqual(Convert.ToString(99), request.GetCacheMD5());
    }

    [TestMethod]
    public void IsPrivateLabelActiveCacheKeySame()
    {
      var request = new IsPrivateLabelActiveRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1);
      var requestSame = new IsPrivateLabelActiveRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1);
      Assert.AreEqual(request.GetCacheMD5(), requestSame.GetCacheMD5());
    }

    [TestMethod]
    public void IsPrivateLabelActiveCacheKeyDifferent()
    {
      var request = new IsPrivateLabelActiveRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1);
      var requestDifferent = new IsPrivateLabelActiveRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 2);
      Assert.AreNotEqual(request.GetCacheMD5(), requestDifferent.GetCacheMD5());
    }

    [TestMethod]
    public void IsPrivateLabelActiveRequestXml()
    {
      var request = new IsPrivateLabelActiveRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 99);
      string xml = request.ToXML();
      Assert.IsTrue(xml.Contains("privatelabelid=\"99\""));
      XElement.Parse(xml);
    }

    [TestMethod]
    public void IsPrivateLabelActiveResponseTrue()
    {
      var response = IsPrivateLabelActiveResponseData.FromIsActive(true);
      Assert.IsNull(response.GetException());
      Assert.IsTrue(response.IsActive);

      string xml = response.ToXML();
      Assert.IsTrue(xml.Contains("true"));
      XElement.Parse(xml);
    }

    [TestMethod]
    public void IsPrivateLabelActiveResponseFalse()
    {
      var response = IsPrivateLabelActiveResponseData.FromIsActive(false);
      Assert.IsNull(response.GetException());
      Assert.IsFalse(response.IsActive);

      string xml = response.ToXML();
      Assert.IsTrue(xml.Contains("false"));
      XElement.Parse(xml);
    }

    [TestMethod]
    public void IsPrivateLabelActiveResponseException()
    {
      AtlantisException exception = new AtlantisException("IsPrivateLabelActiveResponseException", "0", "TestMessage", "TestData", null, null);
      var response = IsPrivateLabelActiveResponseData.FromException(exception);
      Assert.IsFalse(response.IsActive);
      AtlantisException exceptionFromResponse = response.GetException();
      Assert.AreEqual("TestMessage", exceptionFromResponse.Message);
    }

    const int _REQUESTTYPE = 663;

    [TestMethod]
    public void IsPrivateLabelActiveExecuteTrue()
    {
      var request = new IsPrivateLabelActiveRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1);
      var response = (IsPrivateLabelActiveResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.IsTrue(response.IsActive);
    }

    [TestMethod]
    public void IsPrivateLabelActiveExecuteFalse()
    {
      var request = new IsPrivateLabelActiveRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, -44);
      var response = (IsPrivateLabelActiveResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.IsFalse(response.IsActive);

    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void IsPrivateLabelActiveExecuteException()
    {
      var request = new InvalidRequestData();
      var response = (IsPrivateLabelActiveResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
    }

    
  }
}
