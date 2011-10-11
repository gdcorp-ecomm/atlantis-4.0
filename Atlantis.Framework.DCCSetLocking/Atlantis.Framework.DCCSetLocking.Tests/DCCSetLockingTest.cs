using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.DCCSetLocking.Interface;

namespace Atlantis.Framework.DCCSetLocking.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class DCCSetLockingTest
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void DCCSetLockingValid()
    {
      DCCSetLockingRequestData request = new DCCSetLockingRequestData("857020", string.Empty, string.Empty, string.Empty, 0, 1, 1666955, 1, "MOBILE_CSA_DCC");
      DCCSetLockingResponseData response = (DCCSetLockingResponseData)Engine.Engine.ProcessRequest(request, 102);
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void DCCSetLockingForDomainThatShopperDoesNotOwn()
    {
      DCCSetLockingRequestData request = new DCCSetLockingRequestData("847235", string.Empty, string.Empty, string.Empty, 0, 1, 1666955, 1, "MOBILE_CSA_DCC");
      DCCSetLockingResponseData response = (DCCSetLockingResponseData)Engine.Engine.ProcessRequest(request, 102);

      // Pending updates by the DCC team.  They are returning success in this case.
      Assert.IsFalse(response.IsSuccess);
    }
  }
}
