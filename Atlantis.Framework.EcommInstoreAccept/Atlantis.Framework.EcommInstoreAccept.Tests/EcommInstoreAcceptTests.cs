using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.EcommInstoreAccept.Interface;

namespace Atlantis.Framework.EcommInstoreAccept.Tests
{
  [TestClass]
  public class EcommInstoreAcceptTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.EcommInstoreAccept.Impl.dll")]
    public void NoPendingCreditToAccept()
    {
      EcommInstoreAcceptRequestData request = new EcommInstoreAcceptRequestData("855503", string.Empty, string.Empty, string.Empty, 0);
      EcommInstoreAcceptResponseData response = (EcommInstoreAcceptResponseData)Engine.Engine.ProcessRequest(request, 597);
      Assert.AreEqual(InstoreAcceptResult.UnknownResult, response.Result);
    }
  }
}
