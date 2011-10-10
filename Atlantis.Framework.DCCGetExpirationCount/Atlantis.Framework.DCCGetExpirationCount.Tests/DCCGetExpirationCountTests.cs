using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.DCCGetExpirationCount.Interface;

namespace Atlantis.Framework.DomainGetExpirationCount.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class DCCGetExpirationCountTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void DCCGetExpirationCountBasic()
    {
      DCCGetExpirationCountRequestData request = new DCCGetExpirationCountRequestData("832652", string.Empty, string.Empty, string.Empty, 0, "FOS", 91);
      DCCGetExpirationCountResponseData response = (DCCGetExpirationCountResponseData)Engine.Engine.ProcessRequest(request, 120);
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
