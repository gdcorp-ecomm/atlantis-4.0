using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Atlantis.Framework.Providers.Split.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;
using Atlantis.Framework.Testing.MockProviders;

namespace Atlantis.Framework.Providers.Split.Tests
{
  [TestClass]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  public class SplitProviderTests
  {
    public SplitProviderTests()
    {

    }

    private TestContext testContextInstance;
    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext
    {
      get
      {
        return testContextInstance;
      }
      set
      {
        testContextInstance = value;
      }
    }

    [TestInitialize]
    public void InitializeTests()
    {
    }

    private ISplitProvider InitializeProvidersAndReturnSplitProvider(int privateLabelId, string shopperId)
    {
      HttpProviderContainer.Instance.RegisterProvider<ISiteContext, MockSiteContext>();
      HttpProviderContainer.Instance.RegisterProvider<IShopperContext, MockShopperContext>();
      HttpProviderContainer.Instance.RegisterProvider<ISplitProvider, SplitProvider>();

      HttpContext.Current.Items[MockSiteContextSettings.PrivateLabelId] = privateLabelId;
      IShopperContext shopperContext = HttpProviderContainer.Instance.Resolve<IShopperContext>();
      shopperContext.SetNewShopper(shopperId);

      ISplitProvider splitProvider = HttpProviderContainer.Instance.Resolve<ISplitProvider>();
      return splitProvider;
    }

    [TestMethod]
    public void IsInRange()
    {
      MockHttpContext.SetMockHttpContext("default.aspx", "http://www.godaddy.com/default.aspx", String.Empty);
      ISplitProvider split = InitializeProvidersAndReturnSplitProvider(1, "858884");

      Assert.IsNotNull(split);
      Assert.IsTrue(split.SplitValue > 0);
      Assert.IsTrue(split.SplitValue <= 100);
    }

    [TestMethod]
    public void StandardAndPCSplits()
    {
      MockHttpContext.SetMockHttpContext("default.aspx", "http://www.godaddy.com/default.aspx", String.Empty);
      ISplitProvider split = InitializeProvidersAndReturnSplitProvider(1, "858884");

      Assert.IsNotNull(split);
      Assert.IsTrue(split.SplitValue > 0);
      Assert.IsTrue(split.SplitValue <= 100);

      Assert.IsTrue(split.PCSplitValue > 0);
      Assert.IsTrue(split.PCSplitValue <= 4);
    }

    [TestMethod]
    public void StandardAndPCSplitsToConsole()
    {
      for (int x = 0; x < 100; x++)
      {
        MockHttpContext.SetMockHttpContext("default.aspx", "http://www.godaddy.com/default.aspx", String.Empty);
        ISplitProvider split = InitializeProvidersAndReturnSplitProvider(1, "858884");
        Console.WriteLine(split.SplitValue.ToString() + " : " + split.PCSplitValue.ToString());
      }
    }

    [TestMethod]
    public void SplitValueCookieExists()
    {
      MockHttpRequest request = new MockHttpRequest("http://www.godaddy.com/default.aspx");
      MockHttpContext.SetFromWorkerRequest(request);
      HttpCookie splitCookie = new HttpCookie("SplitValue1", "72");
      splitCookie.Domain = ".godaddy.com";
      HttpContext.Current.Request.Cookies.Add(splitCookie);

      ISplitProvider split = InitializeProvidersAndReturnSplitProvider(1, string.Empty);
      Assert.AreEqual(72, split.SplitValue);
      Assert.AreEqual(0, HttpContext.Current.Response.Cookies.Count);
    }


  }
}
