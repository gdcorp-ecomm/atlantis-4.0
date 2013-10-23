using System.Collections.Specialized;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.Links;
using Atlantis.Framework.Providers.Links;
using Atlantis.Framework.Providers.Sso.Interface;
using Atlantis.Framework.Providers.Sso.Tests.Mocks;
using Atlantis.Framework.Providers.Sso.Tests.Mocks.Http;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Web;
using MockHttpContext = Atlantis.Framework.Testing.MockHttpContext;
using MockHttpRequest = Atlantis.Framework.Testing.MockHttpContext.MockHttpRequest;

namespace Atlantis.Framework.Providers.Sso.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Providers.Sso.Interface")]
  [DeploymentItem("Atlantis.Framework.Providers.Sso")]
  [DeploymentItem("Atlantis.Framework.Links.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.AuthRetrieve.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DataCacheGeneric.Impl.dll")]
  public class ClusterSsoProviderTests
  {
    #region Setup
    private IProviderContainer GetProviderContainer(string url, int privateLabelId = 1, SsoProviderType ssoProviderType = SsoProviderType.GoDaddy, string httpMethod = "GET", string virtualDirectoryName = "")
    {
      MockHttpRequest request = new MockCustomHttpRequest(url, httpMethod, virtualDirectoryName);
      MockHttpContext.MockHttpContext.SetFromWorkerRequest(request);

      string filename;
      string queryString;
      ParseUrl(url, out filename, out queryString);
      var mockRequest = new Mocks.Http.MockHttpRequest(new HttpRequest(filename, url, queryString), httpMethod, virtualDirectoryName);
      var context = new Mocks.Http.MockHttpContext(mockRequest, new Mocks.Http.MockHttpResponse());
      HttpContextFactory.SetHttpContext(context);


      IProviderContainer result = new MockProviderContainer();
      result.RegisterProvider<IShopperContext, MockShopperContext>();
      result.RegisterProvider<ISiteContext, MockSiteContext>();
      result.RegisterProvider<IManagerContext, MockNoManagerContext>();
      result.RegisterProvider<ILinkProvider, LinkProvider>();

      if (ssoProviderType == SsoProviderType.GoDaddy)
      {
        result.RegisterProvider<ISsoProvider, ClusterProvider>();
      }
      else if (ssoProviderType == SsoProviderType.DBP)
      {
        result.RegisterProvider<ISsoProvider, ClusterProviderDbp>();
      }
      else
      {
        result.RegisterProvider<ISsoProvider, ClusterProviderPrivateLabel>();
      }

      result.SetData(MockSiteContextSettings.PrivateLabelId, privateLabelId);

      return result;
    }

    private void ParseUrl(string url, out string filename, out string queryString)
    {
      string[] parts = url.Split(new char[] { '?' }, 2, StringSplitOptions.RemoveEmptyEntries);
      if (parts.Length == 2)
        queryString = parts[1];
      else
      {
        queryString = string.Empty;
      }

      filename = parts[0].Substring(parts[0].LastIndexOf('/') + 1);
      if (!filename.Contains("."))
        filename = string.Empty;
    }

    private string GetArtifact(string spKey)
    {
      //REMOVE THIS IF YOU HAVE THE SSOPUBLISH LIB INSTALLED
      return string.Empty;

      //string publishXml = "<Request><ShopperID>867900</ShopperID><FirstName></FirstName><MiddleName></MiddleName><LastName></LastName><ParentShopperId></ParentShopperId>";
      //publishXml += "<FederatedIdentities><FederatedIdentity ShopperID=\"867900\" PrivateLabelID=\"1\"/></FederatedIdentities></Request>";

      //var publisher = new Interop.SSOPublishLib.PublishAuth();
      //return publisher.Publish(publishXml, spKey);
    }
    #endregion

    [TestMethod]
    public void TestSimpleDataExists()
    {
      var providerContainer = GetProviderContainer("http://www.godaddy.com");
      var clusterProvider = providerContainer.Resolve<ISsoProvider>();

      Assert.IsNotNull(clusterProvider.ServerOrClusterName);
      Assert.IsNotNull(clusterProvider.ServiceProviderGroupName);
      Assert.IsNotNull(clusterProvider.SpKey);

      Assert.IsTrue(clusterProvider.ServerOrClusterName.Length > 0);
      Assert.IsTrue(clusterProvider.ServiceProviderGroupName.Length > 0);
      Assert.IsTrue(clusterProvider.SpKey.Length > 0);
    }

    [TestMethod]
    [Ignore]
    public void TestParseArtifactSuccessWithCount()
    {
      var providerContainer = GetProviderContainer("http://www.godaddy.com");
      var cp = providerContainer.Resolve<ISsoProvider>();
      string artifact = GetArtifact(cp.SpKey);

      string shopperId;
      int failureCount;

      cp.ParseArtifact(artifact, out shopperId, out failureCount);

      Assert.IsTrue(shopperId.Equals("867900"));
      Assert.IsTrue(failureCount == 0);
    }

    [TestMethod]
    [Ignore]
    public void TestParseArtifactSuccessNoCount()
    {
      var providerContainer = GetProviderContainer("http://www.godaddy.com");
      var cp = providerContainer.Resolve<ISsoProvider>();
      string artifact = GetArtifact(cp.SpKey);

      string shopperId;

      cp.ParseArtifact(artifact, out shopperId);
      Assert.IsTrue(shopperId.Equals("867900"));
    }

    [TestMethod]
    public void TestParseArtifactFailNoCount()
    {
      var providerContainer = GetProviderContainer("http://www.godaddy.com");
      var cp = providerContainer.Resolve<ISsoProvider>();
      string shopperId;

      Assert.IsFalse(cp.ParseArtifact("does not exist", out shopperId));
      Assert.IsNotNull(shopperId);
    }
    
    [TestMethod]
    public void TestParseArtifactFailWithCount()
    {
      var providerContainer = GetProviderContainer("http://www.godaddy.com");
      var cp = providerContainer.Resolve<ISsoProvider>();
      string shopperId;
      int failureCount;

      Assert.IsFalse(cp.ParseArtifact("does not exist", out shopperId, out failureCount));
      Assert.IsNotNull(shopperId);
      Assert.IsTrue(failureCount == 1);
    }

    [TestMethod]
    public void TestParseArtifactHasDoubleFailureCount()
    {
      var providerContainer = GetProviderContainer("http://www.godaddy.com");
      var cp = providerContainer.Resolve<ISsoProvider>();
      string shopperId;
      int failureCount;

      Assert.IsFalse(cp.ParseArtifact("does not exist", out shopperId, out failureCount));
      Assert.IsFalse(cp.ParseArtifact("does not exist", out shopperId, out failureCount));

      Assert.IsTrue(failureCount == 2);
    }

    [TestMethod]
    public void TestGetLoginUrl()
    {
      var providerContainer = GetProviderContainer("http://www.godaddy.com");
      var cp = providerContainer.Resolve<ISsoProvider>();
      var spkey = cp.SpKey.ToLower();
      var loginUrl = cp.GetUrl(SsoUrlType.Login).ToLower();

      Assert.IsTrue(loginUrl.Contains(spkey));
      Assert.IsTrue(loginUrl.Contains("login.aspx"));
    }

    [TestMethod]
    public void TestGetLoginUrlWithSpkeyOverride()
    {
      var providerContainer = GetProviderContainer("http://www.godaddy.com");
      var cp = providerContainer.Resolve<ISsoProvider>();
      var spkey = cp.SpKey.ToLower();

      var overrideSpkey = "thedude";
      var nvc = new NameValueCollection();
      nvc["SpKeY"] = overrideSpkey;
      var loginUrl = cp.GetUrl(SsoUrlType.Login, nvc).ToLower();
      
      Assert.IsTrue(loginUrl.Contains(overrideSpkey));
      Assert.IsTrue(loginUrl.Contains("login.aspx"));
    }

    [TestMethod]
    public void TestGetLogoutUrl()
    {
      var providerContainer = GetProviderContainer("http://www.godaddy.com");
      var cp = providerContainer.Resolve<ISsoProvider>();

      var spkey = cp.SpKey.ToLower();
      var logoutUrl = cp.GetUrl(SsoUrlType.Logout).ToLower();

      Assert.IsTrue(logoutUrl.Contains(spkey));
      Assert.IsTrue(logoutUrl.Contains("logout.aspx"));
    }

    [TestMethod]
    public void TestGetKeepAliveUrl()
    {
      var providerContainer = GetProviderContainer("http://www.godaddy.com");
      var cp = providerContainer.Resolve<ISsoProvider>();

      var spkey = cp.SpKey.ToLower();
      var keepaliveUrl = cp.GetUrl(SsoUrlType.KeepAlive).ToLower();

      Assert.IsTrue(keepaliveUrl.Contains("keepalive.aspx"));
    }

    [TestMethod]
    public void TestGetLoginUrlPrivateLabel()
    {
      var providerContainer = GetProviderContainer("http://www.securepaynet.net?prog_id=maddogdomains", 1941, SsoProviderType.PrivateLabel);
      var cp = providerContainer.Resolve<ISsoProvider>();
      var spkey = cp.SpKey.ToLower();
      var loginUrl = cp.GetUrl(SsoUrlType.Login).ToLower();

      Assert.IsTrue(loginUrl.Contains(spkey));
      Assert.IsTrue(loginUrl.Contains("maddogdomains"));
      Assert.IsTrue(loginUrl.Contains("login.aspx"));
    }

    [TestMethod]
    public void TestGetLogoutUrlPrivateLabel()
    {
      var providerContainer = GetProviderContainer("http://www.securepaynet.net?prog_id=maddogdomains", 1941, SsoProviderType.PrivateLabel);
      var cp = providerContainer.Resolve<ISsoProvider>();

      var spkey = cp.SpKey.ToLower();
      var logoutUrl = cp.GetUrl(SsoUrlType.Logout).ToLower();

      Assert.IsTrue(logoutUrl.Contains(spkey));
      Assert.IsTrue(logoutUrl.Contains("maddogdomains"));
      Assert.IsTrue(logoutUrl.Contains("logout.aspx"));
    }

    [TestMethod]
    public void TestGetKeepAliveUrlPrivateLabel()
    {
      var providerContainer = GetProviderContainer("http://www.securepaynet.net?prog_id=maddogdomains", 1941, SsoProviderType.PrivateLabel);
      var cp = providerContainer.Resolve<ISsoProvider>();

      var spkey = cp.SpKey.ToLower();
      var keepaliveUrl = cp.GetUrl(SsoUrlType.KeepAlive).ToLower();

      Assert.IsTrue(keepaliveUrl.Contains("maddogdomains"));
      Assert.IsTrue(keepaliveUrl.Contains("keepalive.aspx"));
    }

    [TestMethod]
    public void TestRestFailureCount()
    {
      var providerContainer = GetProviderContainer("http://www.securepaynet.net?pl_id=234", 234, SsoProviderType.PrivateLabel);
      var cp = providerContainer.Resolve<ISsoProvider>();
      int failureCount;
      string shopperId;
      cp.ParseArtifact("does not exist", out shopperId, out failureCount);
      cp.ParseArtifact("does not exist", out shopperId, out failureCount);

      Assert.IsTrue(failureCount == 2);

      ClusterSsoProvider target = new ClusterProvider(providerContainer);
      PrivateObject obj = new PrivateObject(cp);
      obj.Invoke("ResetFailedLoginTransaction");

      cp.ParseArtifact("does not exist", out shopperId, out failureCount);

      Assert.IsTrue(failureCount == 1);
    }

    [TestMethod]
    public void TestGetLinkException()
    {
      SsoProviderEngineRequests.Links = 23;
      var providerContainer = GetProviderContainer("http://www.godaddy.com");
      var cp = providerContainer.Resolve<ISsoProvider>();
      var spkey = cp.SpKey.ToLower();
      var loginUrl = cp.GetUrl(SsoUrlType.Login).ToLower();

      Assert.IsNotNull(loginUrl);
      Assert.IsTrue(loginUrl.Length > 0);
    }

    [TestMethod]
    public void TestDataCacheException()
    {
      var providerContainer = GetProviderContainer("http://www.godaddy.com");
      var cp = providerContainer.Resolve<ISsoProvider>();
      var spkey = cp.SpKey.ToLower();
      var loginUrl = cp.GetUrl(SsoUrlType.Login).ToLower();
    }

    [TestMethod]
    public void TestParamsAreReturnedWhenLinkTypeNotFound()
    {

      var providerContainer = GetProviderContainer("http://www.securepaynet.net?prog_id=domainsbyproxy", 1941, SsoProviderType.DBP);
      var cp = providerContainer.Resolve<ISsoProvider>();
      var addparams = new NameValueCollection();
      addparams["parm1"] = "seth";
      var loginUrl = cp.GetUrl(SsoUrlType.Login, addparams).ToLower();

      Assert.IsTrue(loginUrl.Contains("seth"));
      Assert.IsTrue(loginUrl.Contains("spkey"));
    }
  }

  public enum SsoProviderType
  {
    GoDaddy,
    PrivateLabel,
    DBP
  }

  public class ClusterProvider : ClusterSsoProvider
  {
    string spGroupName = "GDCARTNET";
    string clusterName = "G1DWCARTWEB";
    public ClusterProvider(IProviderContainer container) : base(container) { }
    public override string ServiceProviderGroupName
    {
      get
      {
        return spGroupName;
      }
    }
    public override string ServerOrClusterName
    {
      get { return clusterName; }
    }
  }

  public class ClusterProviderPrivateLabel : ClusterSsoProvider
  {
    string spGroupName = "SPCARTNET";
    string clusterName = "G1DWCARTWEB";
    public ClusterProviderPrivateLabel(IProviderContainer container) : base(container) { }
    public override string ServiceProviderGroupName
    {
      get
      {
        return spGroupName;
      }
    }
    public override string ServerOrClusterName
    {
      get { return clusterName; }
    }
  }

  public class ClusterProviderDbp : ClusterSsoProvider
  {
    string spGroupName = "DBPCARTNET2";
    string clusterName = "G1DWCARTWEB";

    public ClusterProviderDbp(IProviderContainer container) : base(container) { }
    public override string ServiceProviderGroupName
    {
      get
      {
        return spGroupName;
      }
    }
    public override string ServerOrClusterName
    {
      get { return clusterName; }
    }
  }
}
