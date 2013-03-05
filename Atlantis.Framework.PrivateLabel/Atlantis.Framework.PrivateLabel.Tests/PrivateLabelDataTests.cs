using Atlantis.Framework.Interface;
using Atlantis.Framework.PrivateLabel.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;

namespace Atlantis.Framework.PrivateLabel.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  [DeploymentItem("Atlantis.Framework.PrivateLabel.Impl.dll")]
  public class PrivateLabelDataTests
  {
    [TestMethod]
    public void PrivateLabelDataRequestProperties()
    {
      var request = new PrivateLabelDataRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 99, 0);
      Assert.AreEqual(99, request.PrivateLabelId);
      Assert.AreEqual(0, request.DataCategoryId);
      Assert.AreEqual("99:0", request.GetCacheMD5());
    }

    [TestMethod]
    public void PrivateLabelDataCacheKeySame()
    {
      var request = new PrivateLabelDataRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1, 44);
      var requestSame = new PrivateLabelDataRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1, 44);
      Assert.AreEqual(request.GetCacheMD5(), requestSame.GetCacheMD5());
    }

    [TestMethod]
    public void PrivateLabelDataCacheKeyDifferentPrivateLabelId()
    {
      var request = new PrivateLabelDataRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1, 43);
      var requestDifferent = new PrivateLabelDataRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 2, 43);
      Assert.AreNotEqual(request.GetCacheMD5(), requestDifferent.GetCacheMD5());
    }

    [TestMethod]
    public void PrivateLabelDataCacheKeyDifferentCategoryId()
    {
      var request = new PrivateLabelDataRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 2, 44);
      var requestDifferent = new PrivateLabelDataRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 2, 43);
      Assert.AreNotEqual(request.GetCacheMD5(), requestDifferent.GetCacheMD5());
    }

    [TestMethod]
    public void PrivateLabelDataRequestXml()
    {
      var request = new PrivateLabelDataRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 99, 7);
      string xml = request.ToXML();
      Assert.IsTrue(xml.Contains("privatelabelid=\"99\""));
      Assert.IsTrue(xml.Contains("categoryid=\"7\""));
      XElement.Parse(xml);
    }

    [TestMethod]
    public void PrivateLabelDataResponseValid()
    {
      var response = PrivateLabelDataResponseData.FromDataValue("Hello World");
      Assert.IsNull(response.GetException());
      Assert.AreEqual("Hello World", response.DataValue);

      string xml = response.ToXML();
      Assert.IsTrue(xml.Contains("Hello World"));
      XElement.Parse(xml);
    }

    [TestMethod]
    public void PrivateLabelDataResponseEmpty()
    {
      var response = PrivateLabelDataResponseData.FromDataValue(string.Empty);
      Assert.IsNull(response.GetException());
      Assert.AreEqual(string.Empty, response.DataValue);

      string xml = response.ToXML();
      Assert.IsTrue(xml.Contains("\"\""));
      XElement.Parse(xml);
    }

    [TestMethod]
    public void PrivateLabelDataResponseNull()
    {
      var response = PrivateLabelDataResponseData.FromDataValue(null);
      Assert.IsNull(response.GetException());
      Assert.AreEqual(string.Empty, response.DataValue);

      string xml = response.ToXML();
      Assert.IsTrue(xml.Contains("\"\""));
      XElement.Parse(xml);
    }

    [TestMethod]
    public void PrivateLabelDataResponseException()
    {
      AtlantisException exception = new AtlantisException("PrivateLabelDataResponseException", "0", "TestMessage", "TestData", null, null);
      var response = PrivateLabelDataResponseData.FromException(exception);
      Assert.AreEqual(string.Empty, response.DataValue);
      AtlantisException exceptionFromResponse = response.GetException();
      Assert.AreEqual("TestMessage", exceptionFromResponse.Message);
    }

    const int _REQUESTTYPE = 659;

    [TestMethod]
    public void PrivateLabelDataExecuteValid()
    {
      var request = new PrivateLabelDataRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1724, 0);
      var response = (PrivateLabelDataResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.AreEqual("Hunter's, New Show", response.DataValue);
    }

    [TestMethod]
    public void PrivateLabelDataExecuteNotFound()
    {
      var request = new PrivateLabelDataRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, -44, 0);
      var response = (PrivateLabelDataResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.AreEqual(string.Empty, response.DataValue);
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void PrivateLabelDataExecuteException()
    {
      var request = new InvalidRequestData();
      var response = (PrivateLabelDataResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
    }

  }
}
