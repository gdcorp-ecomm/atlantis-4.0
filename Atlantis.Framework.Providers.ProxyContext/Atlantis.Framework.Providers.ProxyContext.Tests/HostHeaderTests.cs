using System.Collections.Generic;
using System.Web;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.ProxyContext.Tests.Mocks;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.ProxyContext.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
  [DeploymentItem("UseHostHeader.config")]
  public class HostHeaderTests
  {
    private const string _URL = "http://mysite.com/default.aspx";

    private IProviderContainer SetContext(string url, string hostHeader, string serverName = null)
    {
      IProviderContainer container = new MockProviderContainer();
      container.RegisterProvider<IProxyContext, WebProxyContext>();

      MockHttpRequestWithHostHeader request = new MockHttpRequestWithHostHeader(url);
      Dictionary<string, string> headers = new Dictionary<string, string>();
      headers["Host"] = hostHeader;
      request.MockHeaderValues(headers);
      MockHttpContext.SetFromWorkerRequest(request);
      if (serverName != null)
      {
        HttpContext.Current.Items["serverName"] = serverName;
      }

      return container;
    }

    [TestMethod]
    public void ValidHostHeader_ValidServerName_ReturnsHostHeaderAsOriginHost()
    {
      IProviderContainer container = SetContext(_URL, "godaddy.com", "test-server-1");
      IProxyContext context = container.Resolve<IProxyContext>();

      Assert.AreEqual("godaddy.com", context.OriginHost);
    }

    [TestMethod]
    public void EmptyHostHeader_ValidServerName_ReturnsServerNameAsOriginHost()
    {
      IProviderContainer container = SetContext(_URL, string.Empty, "test-server-1");
      IProxyContext context = container.Resolve<IProxyContext>();

      Assert.AreEqual("test-server-1", context.OriginHost);
    }

    [TestMethod]
    public void InvalidHostHeader_ValidServerName_ReturnsServerNameAsOriginHost()
    {
      IProviderContainer container = SetContext(_URL, "$$$$$", "test-server-1");
      IProxyContext context = container.Resolve<IProxyContext>();

      Assert.AreEqual("test-server-1", context.OriginHost);
    }

    [TestMethod]
    public void DashHostHeader_InvalidServerName_ReturnsHostHeaderAsOriginHost()
    {
      IProviderContainer container = SetContext(_URL, "-", "#####");
      IProxyContext context = container.Resolve<IProxyContext>();

      Assert.AreEqual("-", context.OriginHost);
    }

    [TestMethod]
    public void InvalidHostHeader_InvalidServerName_ReturnsHostHeaderAsOriginHost()
    {
      IProviderContainer container = SetContext(_URL, "$$$$$", "#####");
      IProxyContext context = container.Resolve<IProxyContext>();

      Assert.AreEqual("$$$$$", context.OriginHost);
    }

    [TestMethod]
    public void P3VipHostHeader_ValidServerName_ReturnsHostHeaderAsOriginHost()
    {
      IProviderContainer container = SetContext(_URL, "p3pwcorpweb101-06.prod.phx3.gdg", "test-server-1");
      IProxyContext context = container.Resolve<IProxyContext>();

      Assert.AreEqual("p3pwcorpweb101-06.prod.phx3.gdg", context.OriginHost);
    }

    [TestMethod]
    public void P3VipHostHeader_InvalidServerName_ReturnsHostHeaderAsOriginHost()
    {
      IProviderContainer container = SetContext(_URL, "p3pwcorpweb101-06.prod.phx3.gdg", "#####");
      IProxyContext context = container.Resolve<IProxyContext>();

      Assert.AreEqual("p3pwcorpweb101-06.prod.phx3.gdg", context.OriginHost);
    }

    [TestMethod]
    public void M1VipHostHeader_ValidServerName_ReturnsHostHeaderAsOriginHost()
    {
      IProviderContainer container = SetContext(_URL, "m1pwcorpweb101-06.prod.mesa1.gdg", "test-server-1");
      IProxyContext context = container.Resolve<IProxyContext>();

      Assert.AreEqual("m1pwcorpweb101-06.prod.mesa1.gdg", context.OriginHost);
    }

    [TestMethod]
    public void M1VipHostHeader_InvalidServerName_ReturnsHostHeaderAsOriginHost()
    {
      IProviderContainer container = SetContext(_URL, "m1pwcorpweb101-06.prod.mesa1.gdg", "#####");
      IProxyContext context = container.Resolve<IProxyContext>();

      Assert.AreEqual("m1pwcorpweb101-06.prod.mesa1.gdg", context.OriginHost);
    }
  }
}
