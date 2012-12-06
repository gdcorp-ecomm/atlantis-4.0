using Microsoft.VisualStudio.TestTools.UnitTesting;

using Atlantis.Framework.GetOverrideHash.Interface;

namespace Atlantis.Framework.GetOverrideHash.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.GetOverrideHash.Impl.dll")]
  [DeploymentItem("Interop.gdOverrideLib.dll")]
  public class GetOverrideHashTest
  {
    public GetOverrideHashTest()
    { }

    public TestContext TestContext { get; set; }

    #region Additional test attributes

    #endregion

    [TestMethod]
    public void BasicOverride()
    {
      var requestData = new GetOverrideHashRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1952, 2, 1);
      var response = (GetOverrideHashResponseData)Engine.Engine.ProcessRequest(requestData, 26);
      Assert.IsFalse(string.IsNullOrWhiteSpace(response.Hash));
    }
  }
}
