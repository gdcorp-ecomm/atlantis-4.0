using Atlantis.Framework.FaxEmailAddonPacks.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.FaxEmailAddonPacks.Tests
{
  [TestClass]
  public class FaxEmailAddonPacksTests
  {
    public FaxEmailAddonPacksTests()
    {
    }

    public TestContext TestContext { get; set; }

    #region Additional test attributes
    
    #endregion

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestGetFaxEmailAddonPacksForTollFreeResource()
    {
      const int fteResourceId = 431621;
      var request = new FaxEmailAddonPacksRequestData("857527", "sourceUrl", "orderId", "pathway", 1, fteResourceId);
      var response = Engine.Engine.ProcessRequest(request, 459) as FaxEmailAddonPacksResponseData;

      Assert.IsNotNull(response);
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestGetFaxEmailAddonPacksForStandardResource()
    {
      const int fteResourceId = 431774;
      var request = new FaxEmailAddonPacksRequestData("857527", "sourceUrl", "orderId", "pathway", 1, fteResourceId);
      var response = Engine.Engine.ProcessRequest(request, 459) as FaxEmailAddonPacksResponseData;

      Assert.IsNotNull(response);
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
