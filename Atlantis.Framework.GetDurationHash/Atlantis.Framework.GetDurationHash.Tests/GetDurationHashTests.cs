using Microsoft.VisualStudio.TestTools.UnitTesting;

using Atlantis.Framework.GetDurationHash.Interface;

namespace Atlantis.Framework.GetDurationHash.Tests
{
  [TestClass]
  public class GetDurationHashTests
  {
    public GetDurationHashTests()
    {}

    public TestContext TestContext { get; set; }

    #region Additional test attributes
    
    #endregion

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestGetDurationHash()
    {
      var request = new GetDurationHashRequestData("shopperId", string.Empty,
        string.Empty, string.Empty, 0, 42101, 1, 2d);
      
      var response = (GetDurationHashResponseData)Engine.Engine.ProcessRequest(request, 27);
      
      Assert.IsNotNull(response);
      Assert.IsNull(response.GetException());
      Assert.IsFalse(string.IsNullOrWhiteSpace(response.Hash));
    }
  }
}
