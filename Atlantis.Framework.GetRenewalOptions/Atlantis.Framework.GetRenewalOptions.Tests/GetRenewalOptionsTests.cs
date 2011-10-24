using Atlantis.Framework.GetRenewalOptions.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.GetRenewalOptions.Tests
{
  [TestClass]
  public class GetRenewalOptionsTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetRenewalOptionsBasic()
    {
      GetRenewalOptionsRequestData requestData = new GetRenewalOptionsRequestData("847235", "http://localhost",
        "", "", 0, "383392", "hosting", "billing", 1);
      GetRenewalOptionsResponseData responseData = (GetRenewalOptionsResponseData)Engine.Engine.ProcessRequest(requestData, 75);
      Assert.AreEqual(2873, responseData.XML.Length);
    }
  }
}
