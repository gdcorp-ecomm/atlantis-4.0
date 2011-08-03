using Microsoft.VisualStudio.TestTools.UnitTesting;

using Atlantis.Framework.GetMiniCart.Interface;

namespace Atlantis.Framework.GetMiniCart.Tests
{
  [TestClass]
  public class GetMiniCartTests
  {
    public GetMiniCartTests()
    {}

    public TestContext TestContext { get; set; }

    #region Additional test attributes
    #endregion

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void BasicMiniCart()
    {
      GetMiniCartRequestData request = new GetMiniCartRequestData("832652", string.Empty, string.Empty, string.Empty, 0);
      GetMiniCartResponseData response = (GetMiniCartResponseData)Engine.Engine.ProcessRequest(request, 3);
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
