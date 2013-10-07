using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Tokens.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Testing.MockProviders;
using Atlantis.Framework.Testing.MockHttpContext;
using System.Collections.Generic;
using Atlantis.Framework.Providers.Links;
using Atlantis.Framework.Providers.Interface.Links;
using Atlantis.Framework.Providers.Localization;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.Providers.Sso.Interface;
using Atlantis.Framework.Providers.Sso;

namespace Atlantis.Framework.TH.SSO.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Providers.Sso.Interface")]
  [DeploymentItem("Atlantis.Framework.Providers.Sso")]
  [DeploymentItem("Atlantis.Framework.Links.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.Providers.Localization.dll")]
  [DeploymentItem("Atlantis.Framework.Localization.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DataCacheGeneric.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.TH.SSO.dll")]
  public class SSOTokenRenderContextTest
  {
    private IProviderContainer InitializeContainer(int privateLabelId)
    {

      IProviderContainer result = new MockProviderContainer();
      result.SetData(MockSiteContextSettings.PrivateLabelId, privateLabelId);
      result.RegisterProvider<ISiteContext, MockSiteContext>();
      result.RegisterProvider<ILinkProvider, LinkProvider>();
      result.RegisterProvider<ILocalizationProvider, CountrySubdomainLocalizationProvider>();
      //result.RegisterProvider<IDebugContext, DebugProvider>();
      result.RegisterProvider<ISsoProvider, ClusterProvider>();

      return result;
    }
    
    [TestMethod]
    public void SSOTokenRenderContext_ConstructorTest()
    {
      MockHttpRequest request = new MockHttpRequest("http://www.godaddy.com/default.aspx?ci=1");
      MockHttpContext.SetFromWorkerRequest(request);

      IProviderContainer providerContainer = InitializeContainer(1);
      SSOTokenRenderContext target = new SSOTokenRenderContext(providerContainer);
      Assert.IsNotNull(target);
      Assert.IsNotNull(target.ProviderContainer);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void SSOTokenRenderContext_ConstructorMissingProviderTest()
    {
      SSOTokenRenderContext target = new SSOTokenRenderContext(null);
      Assert.IsNotNull(target);
    }

    [TestMethod]
    public void SSTokenRenderContext_GetProviderInfoTest()
    {
      MockHttpRequest request = new MockHttpRequest("http://www.godaddy.com/default.aspx?ci=1");
      MockHttpContext.SetFromWorkerRequest(request);

      IProviderContainer providerContainer = InitializeContainer(1);
      SSOTokenRenderContext target = new SSOTokenRenderContext(providerContainer);
      Assert.IsNotNull(target);

      PrivateObject privates = new PrivateObject(target);
      var actual = privates.Invoke("GetProviderInfo");
      Assert.IsNotNull(actual);

      PrivateObject temp = new PrivateObject(actual);
      Assert.IsFalse(string.IsNullOrEmpty(temp.GetProperty("LogOutUrl").ToString()));
      Assert.IsTrue(temp.GetProperty("LogOutUrl").ToString().Contains("logout.aspx"));
      Assert.IsFalse(string.IsNullOrEmpty(temp.GetProperty("LogInUrl").ToString()));
      Assert.IsTrue(temp.GetProperty("LogInUrl").ToString().Contains("login.aspx"));
      Assert.IsFalse(string.IsNullOrEmpty(temp.GetProperty("SpGroupName").ToString()));
      Assert.AreEqual("GDSWNET", temp.GetProperty("SpGroupName"));
      Assert.IsFalse(string.IsNullOrEmpty(temp.GetProperty("SpKey").ToString()));
      Assert.AreEqual("GDSWNET-130122134218001", temp.GetProperty("SpKey"));
    }

    [TestMethod]
    public void SSOTokenRenderContext_RenderBlueRazorTokenSuccessTest()
    {
      MockHttpRequest request = new MockHttpRequest("http://www.bluerazor.com/default.aspx?ci=1");
      MockHttpContext.SetFromWorkerRequest(request);

      IProviderContainer providerContainer = InitializeContainer(2);
      SSOTokenRenderContext target = new SSOTokenRenderContext(providerContainer);
      Assert.IsNotNull(target);
      
      string tokendata = "spgroupname";
      string tokenString = string.Format("[@T[sso:{0}]@T]", tokendata);
      IToken token = new SimpleToken("sso", tokendata, tokenString);
      bool actual = target.RenderToken(token);
      Assert.IsTrue(actual);
      Assert.IsFalse(string.IsNullOrEmpty(token.TokenResult));
      Assert.AreEqual("BRSWNET", token.TokenResult);

      tokendata = "spkey";
      tokenString = string.Format("[@T[sso:{0}]@T]", tokendata);
      token = new SimpleToken("sso", tokendata, tokenString);
      actual = target.RenderToken(token);
      Assert.IsTrue(actual);
      Assert.IsFalse(string.IsNullOrEmpty(token.TokenResult));
      Assert.AreEqual("BRSWNET-130122134218000", token.TokenResult);

      tokendata = "logouturl";
      tokenString = string.Format("[@T[sso:{0}]@T]", tokendata);
      token = new SimpleToken("sso", tokendata, tokenString);
      actual = target.RenderToken(token);
      Assert.IsTrue(actual);
      Assert.IsFalse(string.IsNullOrEmpty(token.TokenResult));
      Assert.IsTrue(token.TokenResult.Contains("logout.aspx"));
      Assert.IsTrue(token.TokenResult.Contains("bluerazor-com"));

      tokendata = "loginurl";
      tokenString = string.Format("[@T[sso:{0}]@T]", tokendata);
      token = new SimpleToken("sso", tokendata, tokenString);
      actual = target.RenderToken(token);
      Assert.IsTrue(actual);
      Assert.IsFalse(string.IsNullOrEmpty(token.TokenResult));
      Assert.IsTrue(token.TokenResult.Contains("login.aspx"));
      Assert.IsTrue(token.TokenResult.Contains("bluerazor-com"));
    }

    [TestMethod]
    public void SSOTokenRenderContext_RenderWWDTokenSuccessTest()
    {
      MockHttpRequest request = new MockHttpRequest("http://www.wildwestdomains.com/default.aspx?ci=1");
      MockHttpContext.SetFromWorkerRequest(request);

      IProviderContainer providerContainer = InitializeContainer(1387);
      SSOTokenRenderContext target = new SSOTokenRenderContext(providerContainer);
      Assert.IsNotNull(target);

      string tokendata = "spgroupname";
      string tokenString = string.Format("[@T[sso:{0}]@T]", tokendata);
      IToken token = new SimpleToken("sso", tokendata, tokenString);
      bool actual = target.RenderToken(token);
      Assert.IsTrue(actual);
      Assert.IsFalse(string.IsNullOrEmpty(token.TokenResult));
      Assert.AreEqual("WWDSWNET", token.TokenResult);

      tokendata = "spkey";
      tokenString = string.Format("[@T[sso:{0}]@T]", tokendata);
      token = new SimpleToken("sso", tokendata, tokenString);
      actual = target.RenderToken(token);
      Assert.IsTrue(actual);
      Assert.IsFalse(string.IsNullOrEmpty(token.TokenResult));
      Assert.AreEqual("WWDSWNET-130122134218003", token.TokenResult);

      tokendata = "logouturl";
      tokenString = string.Format("[@T[sso:{0}]@T]", tokendata);
      token = new SimpleToken("sso", tokendata, tokenString);
      actual = target.RenderToken(token);
      Assert.IsTrue(actual);
      Assert.IsFalse(string.IsNullOrEmpty(token.TokenResult));
      Assert.IsTrue(token.TokenResult.Contains("logout.aspx"));
      Assert.IsTrue(token.TokenResult.Contains("wildwestdomains-com"));

      tokendata = "loginurl";
      tokenString = string.Format("[@T[sso:{0}]@T]", tokendata);
      token = new SimpleToken("sso", tokendata, tokenString);
      actual = target.RenderToken(token);
      Assert.IsTrue(actual);
      Assert.IsFalse(string.IsNullOrEmpty(token.TokenResult));
      Assert.IsTrue(token.TokenResult.Contains("login.aspx"));
      Assert.IsTrue(token.TokenResult.Contains("wildwestdomains-com"));
    }

    [TestMethod]
    public void SSOTokenRenderContext_RenderSPNTokenSuccessTest()
    {
      MockHttpRequest request = new MockHttpRequest("http://www.securepaynet.com/default.aspx?ci=1");
      MockHttpContext.SetFromWorkerRequest(request);

      IProviderContainer providerContainer = InitializeContainer(1592);

      SSOTokenRenderContext target = new SSOTokenRenderContext(providerContainer);
      Assert.IsNotNull(target);

      string tokendata = "spgroupname";
      string tokenString = string.Format("[@T[sso:{0}]@T]", tokendata);
      IToken token = new SimpleToken("sso", tokendata, tokenString);
      bool actual = target.RenderToken(token);
      Assert.IsTrue(actual);
      Assert.IsFalse(string.IsNullOrEmpty(token.TokenResult));
      Assert.AreEqual("SPSWNET", token.TokenResult);

      tokendata = "spkey";
      tokenString = string.Format("[@T[sso:{0}]@T]", tokendata);
      token = new SimpleToken("sso", tokendata, tokenString);
      actual = target.RenderToken(token);
      Assert.IsTrue(actual);
      Assert.IsFalse(string.IsNullOrEmpty(token.TokenResult));
      Assert.AreEqual("SPSWNET-130122134218002", token.TokenResult);

      tokendata = "logouturl";
      tokenString = string.Format("[@T[sso:{0}]@T]", tokendata);
      token = new SimpleToken("sso", tokendata, tokenString);
      actual = target.RenderToken(token);
      Assert.IsTrue(actual);
      Assert.IsFalse(string.IsNullOrEmpty(token.TokenResult));
      Assert.IsTrue(token.TokenResult.Contains("logout.aspx"));
      Assert.IsTrue(token.TokenResult.Contains("securepaynet-net"));

      tokendata = "loginurl";
      tokenString = string.Format("[@T[sso:{0}]@T]", tokendata);
      token = new SimpleToken("sso", tokendata, tokenString);
      actual = target.RenderToken(token);
      Assert.IsTrue(actual);
      Assert.IsFalse(string.IsNullOrEmpty(token.TokenResult));
      Assert.IsTrue(token.TokenResult.Contains("login.aspx"));
      Assert.IsTrue(token.TokenResult.Contains("securepaynet-net"));
    }

    [TestMethod]
    public void SSOTokenRenderContext_RenderTokenSuccessTest()
    {
      MockHttpRequest request = new MockHttpRequest("http://www.godaddy.com/default.aspx?ci=1");
      MockHttpContext.SetFromWorkerRequest(request);

      IProviderContainer providerContainer = InitializeContainer(1);

      SSOTokenRenderContext target = new SSOTokenRenderContext(providerContainer);
      Assert.IsNotNull(target);

      string tokendata = "spkey";
      string tokenString = string.Format("[@T[sso:{0}]@T]", tokendata);
      IToken token = new SimpleToken("sso", tokendata, tokenString);
      bool actual = target.RenderToken(token);
      Assert.IsTrue(actual);
      Assert.IsFalse(string.IsNullOrEmpty(token.TokenResult));
      Assert.AreEqual("GDSWNET-130122134218001", token.TokenResult);

      tokendata = "spgroupname";
      tokenString = string.Format("[@T[sso:{0}]@T]", tokendata);
      token = new SimpleToken("sso", tokendata, tokenString);
      actual = target.RenderToken(token);
      Assert.IsTrue(actual);
      Assert.IsFalse(string.IsNullOrEmpty(token.TokenResult));
      Assert.AreEqual("GDSWNET", token.TokenResult);

      tokendata = "logouturl";
      tokenString = string.Format("[@T[sso:{0}]@T]", tokendata);
      token = new SimpleToken("sso", tokendata, tokenString);
      actual = target.RenderToken(token);
      Assert.IsTrue(actual);
      Assert.IsFalse(string.IsNullOrEmpty(token.TokenResult));
      Assert.IsTrue(token.TokenResult.Contains("logout.aspx"));
      Assert.IsTrue(token.TokenResult.Contains("godaddy-com"));

      tokendata = "loginurl";
      tokenString = string.Format("[@T[sso:{0}]@T]", tokendata);
      token = new SimpleToken("sso", tokendata, tokenString);
      actual = target.RenderToken(token);
      Assert.IsTrue(actual);
      Assert.IsFalse(string.IsNullOrEmpty(token.TokenResult));
      Assert.IsTrue(token.TokenResult.Contains("login.aspx"));
      Assert.IsTrue(token.TokenResult.Contains("godaddy-com"));
    }

    [TestMethod]
    public void SSOTokenRenderContext_RenderTokenFailTest()
    {
      MockHttpRequest request = new MockHttpRequest("http://www.godaddy.com/default.aspx?ci=1");
      MockHttpContext.SetFromWorkerRequest(request);

      IProviderContainer providerContainer = InitializeContainer(1);

      SSOTokenRenderContext target = new SSOTokenRenderContext(providerContainer);
      Assert.IsNotNull(target);

      string tokendata = "x";
      string tokenString = string.Format("[@T[sso:{0}]@T]", tokendata);
      IToken token = new SimpleToken("sso", tokendata, tokenString);
      bool actual = target.RenderToken(token);
      Assert.IsFalse(actual);
      Assert.IsTrue(string.IsNullOrEmpty(token.TokenResult));
    }
  }

  public class ClusterProvider : ClusterSsoProvider
  {
    private const string BR_PREFIX = "BRSWNET";
    private const string GD_PREFIX = "GDSWNET";
    private const string SPN_PREFIX = "SPSWNET";
    private const string WWD_PREFIX = "WWDSWNET";

    private string _clusterName;
    private string _serviceProviderGroupSuffix;
    private string _spGroupName;
    private ILocalizationProvider _localization;

    public ClusterProvider(IProviderContainer container)
      : base(container)
    {
    }

    public override string ServiceProviderGroupName
    {
      get
      {
        if (string.IsNullOrEmpty(_spGroupName))
        {
          _spGroupName = GetSpGroupName(); 
        }
        return _spGroupName;
      }
    }

    public override string ServerOrClusterName
    {
      get
      {
        if (string.IsNullOrEmpty(_clusterName))
        {
          _clusterName = string.Format("{0}CORPWEB", System.Configuration.ConfigurationManager.AppSettings["DataCenter"]);
        }
        return _clusterName;
      }
    }

    private ILocalizationProvider Localization
    {
      get
      {
        return _localization ?? (_localization = Container.Resolve<ILocalizationProvider>());
      }
    }

    private string ServiceProviderGroupSuffix
    {
      get
      {
        if (string.IsNullOrEmpty(_serviceProviderGroupSuffix))
        {
          _serviceProviderGroupSuffix = Localization.IsGlobalSite() ? string.Empty : string.Concat(".", Localization.CountrySite.ToUpperInvariant());
        }
        return _serviceProviderGroupSuffix;
      }
    }

    private string GetSpGroupName()
    {
      string returnValue = string.Empty;

      switch (_siteContext.Value.ContextId)
      {
        case 1:
          returnValue = GD_PREFIX;
          break;
        case 2:
          returnValue = WWD_PREFIX;
          break;
        case 5:
          returnValue = BR_PREFIX;
          break;
        default:
          returnValue = SPN_PREFIX;
          break;
      }
      string isInternal = _links.Value.IsDebugInternal() ? "-DEBUG64" : string.Empty;
      returnValue = string.Format("{0}{1}{2}", returnValue, ServiceProviderGroupSuffix, isInternal).Trim();

      return returnValue;
    }
  }
}
