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
      LinkInfoRequestData request = new LinkInfoRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1);
      LinkInfoResponseData response = (LinkInfoResponseData)DataCache.DataCache.GetProcessRequest(request, 12);
      Assert.AreNotEqual(0, response.Links.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void LinkInfoMissingCacheEmpty()
    {
      DataCache.DataCache.ClearInProcessCachedData("GetProcessRequest" + 12.ToString());
      LinkInfoRequestData request = new LinkInfoRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, -1);
      LinkInfoResponseData response = (LinkInfoResponseData)DataCache.DataCache.GetProcessRequest(request, 12);
      Assert.AreNotEqual(0, response.Links.Count);
    }

    [TestMethod]
    public void LinkInfoMissingCacheEmptyAllowed()
    {
      DataCache.DataCache.ClearInProcessCachedData("GetProcessRequest" + 12.ToString());
      LinkInfoRequestData request = new LinkInfoRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, -1);
      request.AllowEmptyLinkSet = true;
      LinkInfoResponseData response = (LinkInfoResponseData)DataCache.DataCache.GetProcessRequest(request, 12);
      Assert.AreEqual(0, response.Links.Count);
    }

    [TestMethod]
    public void LinkInfoSoilTheCache()
    {
      DataCache.DataCache.ClearInProcessCachedData("GetProcessRequest" + 12.ToString());
      LinkInfoRequestData request = new LinkInfoRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1);
      LinkInfoResponseData response = (LinkInfoResponseData)DataCache.DataCache.GetProcessRequest(request, 12);

      response.Links["SITEURL"] = "www.micco.name";

      LinkInfoRequestData request2 = new LinkInfoRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1);
      LinkInfoResponseData response2 = (LinkInfoResponseData)DataCache.DataCache.GetProcessRequest(request, 12);

      Assert.AreNotEqual("www.micco.name", response2.Links["SITEURL"]);
    }

    

  }
}
