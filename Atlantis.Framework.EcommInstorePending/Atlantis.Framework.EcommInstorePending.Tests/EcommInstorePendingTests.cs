using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.EcommInstorePending.Interface;

namespace Atlantis.Framework.EcommInstorePending.Tests
{
  [TestClass]
  public class EcommInstorePendingTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.EcommInstorePending.Impl.dll")]
    public void NoPendingCredit()
    {
      EcommInstorePendingRequestData request = new EcommInstorePendingRequestData("855503", string.Empty, string.Empty, string.Empty, 0, "USD");
      EcommInstorePendingResponseData response = (EcommInstorePendingResponseData)Engine.Engine.ProcessRequest(request, 596);
      Assert.AreEqual(0, response.Amount);
      Assert.AreEqual(InstorePendingResult.NoCreditsToConsume, response.Result);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.EcommInstorePending.Impl.dll")]
    public void HasPendingCredit()
    {
      EcommInstorePendingRequestData request = new EcommInstorePendingRequestData("832652", string.Empty, string.Empty, string.Empty, 0, "USD");
      EcommInstorePendingResponseData response = (EcommInstorePendingResponseData)Engine.Engine.ProcessRequest(request, 596);
      Assert.IsTrue(response.Amount > 0);
      Assert.IsFalse(string.IsNullOrEmpty(response.TransactionalCurrencyType));
    }

  }
}
