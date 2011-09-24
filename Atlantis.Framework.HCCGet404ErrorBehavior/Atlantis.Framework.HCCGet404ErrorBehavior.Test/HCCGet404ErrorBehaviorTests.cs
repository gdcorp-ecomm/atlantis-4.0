using System;
using Atlantis.Framework.HCCGet404ErrorBehavior.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.HCCGet404ErrorBehavior.Test
{
  [TestClass]
  public class HCCGet404ErrorBehaviorTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void HCCGet404ErrorBehaviorForValidHostingAccount()
    {
      MockHttpContext.SetMockHttpContext("default.aspx", "http://localhost/default.aspx", string.Empty);

      string shopperId = "859775";
      string sourceUrl = string.Empty; // TODO: Initialize to an appropriate value
      string orderIo = string.Empty; // TODO: Initialize to an appropriate value
      string pathway = string.Empty; // TODO: Initialize to an appropriate value
      int pageCount = 0; // TODO: Initialize to an appropriate value
      string accountUid = "c14a5576-3209-11df-9b7b-005056952fd6";

       HCCGet404ErrorBehaviorRequestData request = new HCCGet404ErrorBehaviorRequestData(shopperId,
       sourceUrl,
       orderIo,
       pathway,
       pageCount,
       accountUid
      );

      var response = SessionCache.SessionCache.GetProcessRequest<HCCGet404ErrorBehaviorResponseData>(request, 335);

      Console.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void HCCGet404ErrorBehaviorForInValidHostingAccount()
    {
      MockHttpContext.SetMockHttpContext("default.aspx", "http://localhost/default.aspx", string.Empty);

      string shopperId = "859775";
      string sourceUrl = string.Empty; // TODO: Initialize to an appropriate value
      string orderIo = string.Empty; // TODO: Initialize to an appropriate value
      string pathway = string.Empty; // TODO: Initialize to an appropriate value
      int pageCount = 0; // TODO: Initialize to an appropriate value
      string accountUid = "c14a5576-3209-11df-9b7b-005056952fd";

      HCCGet404ErrorBehaviorRequestData request = new HCCGet404ErrorBehaviorRequestData(shopperId,
      sourceUrl,
      orderIo,
      pathway,
      pageCount,
      accountUid
     );

      var response = SessionCache.SessionCache.GetProcessRequest<HCCGet404ErrorBehaviorResponseData>(request, 335);

      Console.WriteLine(response.ToXML());
      Assert.IsTrue(response.Response.GetResponseStatusCode() == 100);
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
