using Atlantis.Framework.DotTypeCache;
using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Atlantis.Framework.Tokens.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Atlantis.Framework.TH.Domains.Tests
{
  [TestClass]
  [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("dottypecache.config")]
  [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.CH.DotTypeCache.dll")]
  [DeploymentItem("Atlantis.Framework.DCCDomainsDataCache.Interface.dll")]
  [DeploymentItem("Atlantis.Framework.DCCDomainsDataCache.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DomainContactFields.Interface.dll")]
  [DeploymentItem("Atlantis.Framework.DomainContactFields.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DotTypeAvailability.Interface.dll")]
  [DeploymentItem("Atlantis.Framework.DotTypeAvailability.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DotTypeCache.Interface.dll")]
  [DeploymentItem("Atlantis.Framework.DotTypeCache.dll")]
  [DeploymentItem("Atlantis.Framework.DotTypeCache.Static.dll")]
  [DeploymentItem("Atlantis.Framework.DotTypeCache.StaticTypes.dll")]
  [DeploymentItem("Atlantis.Framework.RegDotTypeProductIds.Interface.dll")]
  [DeploymentItem("Atlantis.Framework.RegDotTypeProductIds.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.RegDotTypeRegistry.Interface.dll")]
  [DeploymentItem("Atlantis.Framework.RegDotTypeRegistry.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.TLDDataCache.Interface.dll")]
  [DeploymentItem("Atlantis.Framework.TLDDataCache.Impl.dll")]
  public class DomainsTokenRenderContextTests
  {

    /// <summary>
    /// Gets or sets the test context which provides
    /// information about and functionality for the current test run.
    /// </summary>
    public TestContext TestContext
    {
      get;
      set;
    }

    private IProviderContainer InitializeContainer(int privateLabelId)
    {
      IProviderContainer result = new MockProviderContainer();
      result.SetData(MockSiteContextSettings.PrivateLabelId, privateLabelId);
      result.RegisterProvider<ISiteContext, MockSiteContext>();
      result.RegisterProvider<IDebugContext, MockDebugProvider>();
      result.RegisterProvider<IDotTypeProvider, DotTypeProvider>();

      return result;
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void DomainsTokenRenderContext_ConstructorMissingProviderTest()
    {
      var target = new DomainsTokenRenderContext(null);
      Assert.IsNotNull(target);
    }

    [TestMethod]
    public void DomainsTokenRenderContext_ConstructorTest()
    {
      MockHttpRequest request = new MockHttpRequest("http://www.godaddy.com/default.aspx?ci=1");
      MockHttpContext.SetFromWorkerRequest(request);

      IProviderContainer providerContainer = InitializeContainer(1);
      var target = new DomainsTokenRenderContext(providerContainer);
      Assert.IsNotNull(target);
      Assert.IsNotNull(target.ProviderContainer);
      Assert.AreEqual(providerContainer, target.ProviderContainer);
    }

    [TestInitialize]
    public void DomainsTokenRenderContext_InitializeTests()
    {
      TestContext = new TimerTestContext(TestContext);
    }

    [TestMethod]
    public void DomainsTokenRenderContext_RenderICANNTldsTokenSuccessTest()
    {
      MockHttpRequest request = new MockHttpRequest("http://www.godaddy.com/default.aspx?ci=1");
      MockHttpContext.SetFromWorkerRequest(request);

      IProviderContainer providerContainer = InitializeContainer(1);
      var target = new DomainsTokenRenderContext(providerContainer);
      Assert.IsNotNull(target);

      string tokenData = "<icanntlds />";
      string tokenString = String.Format("[@T[domains:{0}]@T]", tokenData);
      IToken token = new GrammaticalDelimiterToken("domains", tokenData, tokenString);
      var privates = new PrivateType(typeof(DomainsTokenRenderContext));
      var actual = privates.InvokeStatic("RenderICANNTldsToken", token, providerContainer.Resolve<IDotTypeProvider>());
      TestContext.WriteLine("Result:{0}", actual);
      Assert.IsNotNull(actual);
      Assert.IsFalse(string.IsNullOrEmpty(actual.ToString()));
    }

    [TestMethod]
    public void DomainsTokenRenderContext_RenderTokenForICANNTldsSuccessTest()
    {
      MockHttpRequest request = new MockHttpRequest("http://www.godaddy.com/default.aspx?ci=1");
      MockHttpContext.SetFromWorkerRequest(request);

      IProviderContainer provider = InitializeContainer(1);
      var target = new DomainsTokenRenderContext(provider);
      Assert.IsNotNull(target);

      string tokenData = "<icanntlds />";
      string tokenString = String.Format("[@T[domains:{0}]@T]", tokenData);
      IToken token = new GrammaticalDelimiterToken("domains", tokenData, tokenString);
      var actual = target.RenderToken(token);
      Assert.IsTrue(actual);
      Assert.IsFalse(string.IsNullOrEmpty(token.TokenResult));
      Assert.IsTrue(string.IsNullOrEmpty(token.TokenError));
    }
  }
}
