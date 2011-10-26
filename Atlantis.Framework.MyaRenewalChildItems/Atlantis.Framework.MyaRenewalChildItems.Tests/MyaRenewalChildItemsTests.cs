using Atlantis.Framework.Engine;
using Atlantis.Framework.MyaRenewalChildItems.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyaRenewalChildItems.Tests
{
  [TestClass]
  public class MyaRenewalChildItemsTests
  {  
    private TestContext testContextInstance;

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext
    {
      get
      {
        return testContextInstance;
      }
      set
      {
        testContextInstance = value;
      }
    }

    #region Additional test attributes

    #endregion

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void MYAGetChildItems()
    {
      MyaRenewalChildItemsRequestData requestData = new MyaRenewalChildItemsRequestData("822497", string.Empty, string.Empty, string.Empty, 0, 7, "parking");
      MyaRenewalChildItemsResponseData response = (MyaRenewalChildItemsResponseData)Engine.ProcessRequest(requestData, 195);
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
