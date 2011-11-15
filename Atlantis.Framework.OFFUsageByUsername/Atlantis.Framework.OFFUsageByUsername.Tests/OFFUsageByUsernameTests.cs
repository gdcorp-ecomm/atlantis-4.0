using Atlantis.Framework.OFFUsageByUsername.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.OFFUsageByUsername.Tests
{
  [TestClass]
  public class OFFUsageByUsernameTests
  {
    private const int OFFUsageByUsernameRequestType = 446;

    public OFFUsageByUsernameTests()
    {
    }

    public TestContext TestContext { get; set; }

    #region Additional test attributes
    
    #endregion

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestGetSpaceUsedPercentage()
    {
      const string username = "cshipley@godaddy.com";
      var request = new OFFUsageByUsernameRequestData("842904", "sourceUrl", "orderId", "pathway", 1, username);
      var response = Engine.Engine.ProcessRequest(request, OFFUsageByUsernameRequestType) as OFFUsageByUsernameResponseData;

      Assert.IsNotNull(response);
      Assert.IsNull(response.GetException());
      Assert.IsTrue(response.SpaceAvailable >= 0);
      Assert.IsTrue(response.SpaceUsed >= 0);
    }
  }
}
