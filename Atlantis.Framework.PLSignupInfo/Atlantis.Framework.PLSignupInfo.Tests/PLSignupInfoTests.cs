using Atlantis.Framework.PLSignupInfo.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.PLSignupInfo.Tests
{
  [TestClass]
  public class PLSignupInfoTests
  {
    [TestMethod]
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.PLSignupInfo.Impl.dll")]
    public void BasicSignupInfoOff()
    {
      PLSignupInfoRequestData request = new PLSignupInfoRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1724);
      PLSignupInfoResponseData response = (PLSignupInfoResponseData)DataCache.DataCache.GetProcessRequest(request, 522);
      Assert.IsNotNull(response.DefaultTransactionCurrencyType);
    }

    [TestMethod]
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.PLSignupInfo.Impl.dll")]
    public void BasicSignupInfoOn()
    {
      PLSignupInfoRequestData request = new PLSignupInfoRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 281896);
      PLSignupInfoResponseData response = (PLSignupInfoResponseData)DataCache.DataCache.GetProcessRequest(request, 522);
      Assert.IsNotNull(response.DefaultTransactionCurrencyType);
    }

  }
}
