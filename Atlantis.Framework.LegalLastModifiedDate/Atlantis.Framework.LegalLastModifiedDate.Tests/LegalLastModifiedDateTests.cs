using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.LegalLastModifiedDate.Interface;

namespace Atlantis.Framework.LegalLastModifiedDate.Tests
{
  [TestClass]
  public class GetHelpArticleTests
  {
    private const string SHOPPER_ID = "832652";

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.LegalLastModifiedDate.Impl.dll")]
    public void LegalLastModifiedDatetestBasic()
    {
      LegalLastModifiedDateRequestData requestData = new LegalLastModifiedDateRequestData(SHOPPER_ID, string.Empty, string.Empty, string.Empty, 0, "UTOS", 1);
      LegalLastModifiedDateResponseData responseData = (LegalLastModifiedDateResponseData)Engine.Engine.ProcessRequest(requestData, 191);
      Assert.IsTrue(responseData.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.LegalLastModifiedDate.Impl.dll")]
    public void LegalLastModifiedDatetestCached()
    {
      LegalLastModifiedDateRequestData requestData = new LegalLastModifiedDateRequestData(SHOPPER_ID, string.Empty, string.Empty, string.Empty, 0, "UTOS", 1);
      LegalLastModifiedDateResponseData responseData = (LegalLastModifiedDateResponseData)DataCache.DataCache.GetProcessRequest(requestData, 191);
      Assert.IsTrue(responseData.IsSuccess);
    }
  }
}
