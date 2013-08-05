using Atlantis.Framework.Interface;
using Atlantis.Framework.LinkInfo.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.LinkInfo.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.LinkInfo.Impl.dll")]
  public class LinkInfoTests
  {
    [TestMethod]
    public void LinkInfoBasic()
    {
      var request = new LinkInfoRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1);
      var response = (LinkInfoResponseData)Engine.Engine.ProcessRequest(request, 12);
      Assert.AreNotEqual(0, response.Links.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void LinkInfoMissingCacheEmpty()
    {
      var request = new LinkInfoRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, -1);
      var response = (LinkInfoResponseData)Engine.Engine.ProcessRequest(request, 12);
      Assert.AreNotEqual(0, response.Links.Count);
    }

    [TestMethod]
    public void LinkInfoMissingCacheEmptyAllowed()
    {
      var request = new LinkInfoRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, -1);
      request.AllowEmptyLinkSet = true;
      var response = (LinkInfoResponseData)Engine.Engine.ProcessRequest(request, 12);
      Assert.AreEqual(0, response.Links.Count);
    }

    [TestMethod]
    public void LinkInfoSoilTheCache()
    {
      var request = new LinkInfoRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1);
      var response = (LinkInfoResponseData)Engine.Engine.ProcessRequest(request, 12);

      response.Links["SITEURL"] = "www.micco.name";

      var request2 = new LinkInfoRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1);
      var response2 = (LinkInfoResponseData)Engine.Engine.ProcessRequest(request, 12);

      Assert.AreNotEqual("www.micco.name", response2.Links["SITEURL"]);
    }

  }
}
