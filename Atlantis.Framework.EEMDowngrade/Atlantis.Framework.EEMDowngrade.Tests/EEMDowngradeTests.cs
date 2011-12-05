using Atlantis.Framework.EEMDowngrade.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.EEMDowngrade.Tests
{
  [TestClass]
  public class EEMDowngradeTests
  {
    private const string EnteredBy = "Atlantis.Framework.EEMDowngrade.Tests.EEMDowngradeTests";

    public EEMDowngradeTests()
    {
    }

    public TestContext TestContext { get; set; }

    #region Additional test attributes
    
    #endregion

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestDowngrade()
    {
      string shopperId = "850659";
      int eemResourceId = 5082;      //must be a valid eem (old/non-orion-based) billing resourceid for the given shopper
      int downgradeProductId = 4756;  //must be a valid downgrade productid for the given acct

      var request = new EEMDowngradeRequestData(shopperId, "sourceUrl", "orderId", "pathway", 1, eemResourceId, downgradeProductId, 1, EnteredBy);
      var response = Engine.Engine.ProcessRequest(request, 463) as EEMDowngradeResponseData;

      Assert.IsNotNull(response);
      Assert.IsTrue(response.IsSuccessful);
    }
  }
}
