using System;
using Atlantis.Framework.DomainGetResourceDomainId.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.DomainGetResourceDomainId.Test
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.DomainGetResourceDomainId.Impl.dll")]
  public class UnitTest1
  {
    private string domain = "oauthtester.com";
    private string shopper_id = "867900";
    private string order_id = "1484025";

    [TestMethod]
    public void GetResourceIdAndDomainId()
    {
      var request = new DomainGetResourceDomainIdRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0,domain, order_id, shopper_id);
      var response = Engine.Engine.ProcessRequest(request, 732) as DomainGetResourceDomainIdResponseData;

      Assert.IsTrue(response.BillingResourceId.Length > 1);
      Assert.IsTrue(response.DomainId.Length > 1);
    }

    [TestMethod]
    public void CheckForEmpty()
    {
      var request = new DomainGetResourceDomainIdRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "n", "9", "9");
      var response = Engine.Engine.ProcessRequest(request, 732) as DomainGetResourceDomainIdResponseData;

      Assert.IsNotNull(response.BillingResourceId);
      Assert.IsNotNull(response.DomainId);
    }
  }
}
