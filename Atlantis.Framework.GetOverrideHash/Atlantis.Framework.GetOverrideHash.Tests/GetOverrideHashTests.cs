using Microsoft.VisualStudio.TestTools.UnitTesting;

using Atlantis.Framework.GetOverrideHash.Interface;

namespace Atlantis.Framework.GetOverrideHash.Tests
{
  [TestClass]
  public class GetOverrideHashTest
  {
    public GetOverrideHashTest()
    { }

    public TestContext TestContext { get; set; }

    #region Additional test attributes

    #endregion

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestMethod1()
    {

      var requestData = new GetOverrideHashRequestData("850774", string.Empty, string.Empty, string.Empty, 0, 1952, 2, 1);
      //GetOverrideHashRequestData requestData = new GetOverrideHashRequestData(
      // "850774", string.Empty, string.Empty, string.Empty, 0, 1234, 1, 3456, 2222);

      var response = (GetOverrideHashResponseData)Engine.Engine.ProcessRequest(requestData, 26);
      string hash = response.Hash;

      Assert.IsFalse(string.IsNullOrWhiteSpace(hash));
    }
  }
}
