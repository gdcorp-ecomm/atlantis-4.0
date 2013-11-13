using System;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Xml.Linq;
using Atlantis.Framework.BasePages.Providers;
using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MiniEncrypt;
using Atlantis.Framework.Testing.MockEngine;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.BasePages.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Manager.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.Shopper.Impl.dll")]
  public class GdgManagerContextTests
  {
    readonly MockErrorLogger _mockLogger = new MockErrorLogger();

    [TestInitialize]
    public void TestInitialize()
    {
      Engine.EngineLogging.EngineLogger = _mockLogger;
    }

    private IProviderContainer SetValidManagerContext(string shopperId)
    {
      string url = "http://gdgmanagertests.host/mypage.aspx?mgrshopper=" + BuildMgrShopperId(shopperId);
      var request = new MockHttpRequest(url);
      MockHttpContext.SetFromWorkerRequest(request);
      HttpContext.Current.User = Thread.CurrentPrincipal;

      var result = new MockProviderContainer();
      result.RegisterProvider<ISiteContext, MockSiteContext>();
      result.RegisterProvider<IShopperContext, MockShopperContext>();
      result.RegisterProvider<IManagerContext, GdgManagerContextProvider>();

      return result;
    }

    private string BuildMgrShopperId(string shopperId)
    {
      string expires = DateTime.Now.AddHours(1).ToUniversalTime().ToString();
      string mgrshopper = string.Concat(shopperId, "|", GetManagerUserId(), "|", expires);
      string result;

      using (var cookieEncrypt = CookieEncryption.CreateDisposable())
      {
        result = cookieEncrypt.EncryptCookieValue(mgrshopper);
      }

      return result;
    }

    const string _MANAGERUSERREQUESTFORMAT =
      "<ManagerUserGetByNTLogin><param name=\"NTLogin\" value=\"{0}\"/></ManagerUserGetByNTLogin>";

    private string GetManagerUserId()
    {
      var principal = Thread.CurrentPrincipal as WindowsPrincipal;
      string[] domainParts = principal.Identity.Name.Split('\\');
      string userName = domainParts[1];

      string request = string.Format(_MANAGERUSERREQUESTFORMAT, userName);
      string cacheXml;
      using (var comCache = GdDataCacheOutOfProcess.CreateDisposable())
      {
        cacheXml = comCache.GetCacheData(request);
      }

      var dataElement = XElement.Parse(cacheXml);
      var result = dataElement.Descendants("item").First().Attribute("ManagerUserID").Value;

      return result;
    }

    [TestMethod]
    public void GdgManagerContextValid()
    {
      _mockLogger.Exceptions.Clear();

      var container = SetValidManagerContext("832652");
      var manager = container.Resolve<IManagerContext>();

      Assert.IsTrue(manager.IsManager);
      Assert.AreEqual(0, _mockLogger.Exceptions.Count);
    }
  }
}
