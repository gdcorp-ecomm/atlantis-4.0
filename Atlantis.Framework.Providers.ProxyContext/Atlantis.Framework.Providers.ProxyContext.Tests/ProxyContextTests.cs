using System.Collections.Specialized;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Atlantis.Framework.Testing.MockHttpContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;

namespace Atlantis.Framework.Providers.ProxyContext.Tests
{
  [TestClass]
  public class ProxyContextTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    public void RequestNoProxies()
    {
      MockHttpContext.SetMockHttpContext("default.aspx","http://mysite.com/default.aspx", string.Empty);
      HttpProviderContainer.Instance.RegisterProvider<IProxyContext, WebProxyContext>();

      IProxyContext context = HttpProviderContainer.Instance.Resolve<IProxyContext>();
      Assert.AreEqual(ProxyStatusType.None, context.Status);
      Assert.IsFalse(context.IsLocalARR);
      Assert.IsFalse(context.IsResellerDomain);
      Assert.IsFalse(context.IsTransalationDomain);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    public void RequestARR()
    {
      MockHttpContext.SetMockHttpContext("default.aspx", "http://mysite.com/default.aspx", string.Empty);
      HttpProviderContainer.Instance.RegisterProvider<IProxyContext, WebProxyContext>();
      HttpContext.Current.Request.Headers["X-ARR-OriginalIP"] = "68.68.68.68";
      HttpContext.Current.Request.Headers["X-ARR-OriginalHost"] = "www.originalhost.com";
      HttpContext.Current.Request.Headers["X-ARR-OriginalUrl"] = "http://www.originalhost.com/default.aspx";

      IProxyContext context = HttpProviderContainer.Instance.Resolve<IProxyContext>();
      Assert.IsTrue(context.IsLocalARR);
      Assert.IsFalse(context.IsResellerDomain);
      Assert.IsFalse(context.IsTransalationDomain);
      Assert.AreEqual("68.68.68.68", context.OriginIP);
    }


  }
}
