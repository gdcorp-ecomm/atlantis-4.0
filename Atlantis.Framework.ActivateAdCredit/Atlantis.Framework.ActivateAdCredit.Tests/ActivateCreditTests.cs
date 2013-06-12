using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.ActivateAdCredit.Interface;

namespace Atlantis.Framework.ActivateAdCredit.Tests
{
  [TestClass]
  public class ActivateCreditTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.ActivateAdCredit.Impl.dll")]
    public void ActivateCredit()
    {
      int couponKey = 5317;

      ActivateAdCreditRequestData request = new ActivateAdCreditRequestData("856907", string.Empty, string.Empty, string.Empty, 0, couponKey);

      ActivateAdCreditResponseData response = (ActivateAdCreditResponseData) Engine.Engine.ProcessRequest(request, 711);
      string xml = response.ToXML();
    }
  }
}
