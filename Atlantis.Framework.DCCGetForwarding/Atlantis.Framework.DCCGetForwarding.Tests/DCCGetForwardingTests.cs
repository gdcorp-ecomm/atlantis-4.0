using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.DCCGetForwarding.Interface;

namespace Atlantis.Framework.DCCGetForwarding.Tests
{
  [TestClass]
  public class DCCGetForwardingTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("App.config")]
    public void GetForwardingTest()
    {
      DCCGetForwardingRequestData request = new DCCGetForwardingRequestData("857020", string.Empty, string.Empty, string.Empty, 0, "AAAABBBCCCFLKJ.COM");

      DCCGetForwardingResponseData response = (DCCGetForwardingResponseData)Engine.Engine.ProcessRequest(request, 110);
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
