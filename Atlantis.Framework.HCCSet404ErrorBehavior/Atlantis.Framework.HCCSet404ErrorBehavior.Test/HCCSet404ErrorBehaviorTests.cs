using System;
using Atlantis.Framework.HCC.Interface.Constants;
using Atlantis.Framework.HCCSet404ErrorBehavior.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.HCCSet404ErrorBehavior.Test
{
  [TestClass]
  public class HccSet404ErrorBehaviorTests
  {
    private const string ShopperId = "859775";
    private readonly string _sourceUrl = string.Empty;
    private readonly string _orderIo = string.Empty;
    private readonly string _pathway = string.Empty;
    private const int PageCount = 0;

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void HccSet404ErrorBehaviorForValidHostingAccountToDefault()

    {
      MockHttpContext.SetMockHttpContext("default.aspx", "http://localhost/default.aspx", string.Empty);

      string accountUid = "c14a5576-3209-11df-9b7b-005056952fd6";

      var request = new HCCSet404ErrorBehaviorRequestData(ShopperId,
      _sourceUrl,
      _orderIo,
      _pathway,
      PageCount,
      accountUid,
      HCCErrorBehaviorPageType.DEFAULT,
      string.Empty,
      string.Empty
     );

      var response = SessionCache.SessionCache.GetProcessRequest<HCCSet404ErrorBehaviorResponseData>(request, 336, true);
      Console.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void HccSet404ErrorBehaviorForValidHostingAccountToHome()
    {
      MockHttpContext.SetMockHttpContext("default.aspx", "http://localhost/default.aspx", string.Empty);

      const string accountUid = "c14a5576-3209-11df-9b7b-005056952fd6";

      var request = new HCCSet404ErrorBehaviorRequestData(ShopperId,
      _sourceUrl,
      _orderIo,
      _pathway,
      PageCount,
      accountUid,
      HCCErrorBehaviorPageType.HOME,
      string.Empty,
      string.Empty
     );

      var response = SessionCache.SessionCache.GetProcessRequest<HCCSet404ErrorBehaviorResponseData>(request, 336);

      Console.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void HccSet404ErrorBehaviorForValidHostingAccountToCustom()
    {
      MockHttpContext.SetMockHttpContext("default.aspx", "http://localhost/default.aspx", string.Empty);

      const string accountUid = "c14a5576-3209-11df-9b7b-005056952fd6";

      var request = new HCCSet404ErrorBehaviorRequestData(ShopperId,
      _sourceUrl,
      _orderIo,
      _pathway,
      PageCount,
      accountUid,
      HCCErrorBehaviorPageType.CUSTOM,
      "errors/",
      "apierrorpage.html"
     );

      var response = SessionCache.SessionCache.GetProcessRequest<HCCSet404ErrorBehaviorResponseData>(request, 336);

      Console.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void HccSet404ErrorBehaviorForInValidHostingAccountToDefault()
    {
      MockHttpContext.SetMockHttpContext("default.aspx", "http://localhost/default.aspx", string.Empty);

      const string accountUid = "00000000-0000-0000-0000-000000000000";

      var request = new HCCSet404ErrorBehaviorRequestData(ShopperId,
      _sourceUrl,
      _orderIo,
      _pathway,
      PageCount,
      accountUid,
      HCCErrorBehaviorPageType.DEFAULT,
      string.Empty,
      string.Empty
     );

      var response = SessionCache.SessionCache.GetProcessRequest<HCCSet404ErrorBehaviorResponseData>(request, 336);

      Console.WriteLine(response.ToXML());
      Assert.IsTrue(response.Response.GetResponseStatusCode() == 102);
      Assert.IsFalse(response.IsSuccess);
    }
  }
}
