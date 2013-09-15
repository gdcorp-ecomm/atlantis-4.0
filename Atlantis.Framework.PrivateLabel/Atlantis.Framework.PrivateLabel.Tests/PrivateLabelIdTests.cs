using Atlantis.Framework.Interface;
using Atlantis.Framework.PrivateLabel.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;

namespace Atlantis.Framework.PrivateLabel.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.PrivateLabel.Impl.dll")]
  public class PrivateLabelIdTests
  {
    [TestMethod]
    public void PrivateLabelIdRequestProperties()
    {
      var request = new PrivateLabelIdRequestData("Hunter");
      Assert.AreEqual("Hunter", request.ProgId);
      Assert.AreEqual("hunter", request.GetCacheMD5());
    }

    [TestMethod]
    public void PrivateLabelIdRequestPropertiesNullProgId()
    {
      var request = new PrivateLabelIdRequestData(null);
      Assert.AreEqual(string.Empty, request.ProgId);
    }

    [TestMethod]
    public void PrivateLabelIdCacheKeySame()
    {
      var request = new PrivateLabelIdRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "hunter");
      var requestSame = new PrivateLabelIdRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "Hunter");
      Assert.AreEqual(request.GetCacheMD5(), requestSame.GetCacheMD5());
    }

    [TestMethod]
    public void PrivateLabelIdCacheKeyDifferent()
    {
      var request = new PrivateLabelIdRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "hunter");
      var requestDifferent = new PrivateLabelIdRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "godaddy");
      Assert.AreNotEqual(request.GetCacheMD5(), requestDifferent.GetCacheMD5());
    }

    [TestMethod]
    public void PrivateLabelIdRequestXml()
    {
      var request = new PrivateLabelIdRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "clue");
      string xml = request.ToXML();
      Assert.IsTrue(xml.Contains("progid=\"clue\""));
      XElement.Parse(xml);
    }

    [TestMethod]
    public void PrivateLabelIdResponseValid()
    {
      var response = PrivateLabelIdResponseData.FromPrivateLabelId(400);
      Assert.IsNull(response.GetException());
      Assert.AreEqual(400, response.PrivateLabelId);

      string xml = response.ToXML();
      Assert.IsTrue(xml.Contains("400"));
      XElement.Parse(xml);
    }

    [TestMethod]
    public void PrivateLabelIdResponseZero()
    {
      var response = PrivateLabelIdResponseData.FromPrivateLabelId(0);
      Assert.IsNull(response.GetException());
      Assert.AreEqual(0, response.PrivateLabelId);

      string xml = response.ToXML();
      Assert.IsTrue(xml.Contains("0"));
      XElement.Parse(xml);
    }

    [TestMethod]
    public void PrivateLabelIdResponseException()
    {
      AtlantisException exception = new AtlantisException("PrivateLabelIdResponseException", "0", "TestMessage", "TestData", null, null);
      var response = PrivateLabelIdResponseData.FromException(exception);
      Assert.AreEqual(0, response.PrivateLabelId);
      AtlantisException exceptionFromResponse = response.GetException();
      Assert.AreEqual("TestMessage", exceptionFromResponse.Message);
    }

    const int _REQUESTTYPE = 660;

    [TestMethod]
    public void PrivateLabelIdExecuteValid()
    {
      var request = new PrivateLabelIdRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "hunter");
      var response = (PrivateLabelIdResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.AreEqual(1724, response.PrivateLabelId);
    }

    [TestMethod]
    public void PrivateLabelIdExecuteNotFound()
    {
      var request = new PrivateLabelIdRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "notsurewhatever#");
      var response = (PrivateLabelIdResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.AreEqual(0, response.PrivateLabelId);
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void PrivateLabelIdExecuteException()
    {
      var request = new InvalidRequestData();
      var response = (PrivateLabelIdResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
    }

  }
}
