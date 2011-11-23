using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Atlantis.Framework.Providers.Split.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.Split.Tests
{
  [TestClass]
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
      HttpProviderContainer.Instance.RegisterProvider<ISiteContext, TestContexts>();
      HttpProviderContainer.Instance.RegisterProvider<IShopperContext, TestContexts>();
      HttpProviderContainer.Instance.RegisterProvider<ISplitProvider, SplitProvider>();
    }

    private ISplitProvider NewSplitProvider(int privateLabelId, string shopperId)
    {
      ISiteContext siteContext = HttpProviderContainer.Instance.Resolve<ISiteContext>();
      ((TestContexts)siteContext).SetContextInfo(privateLabelId, shopperId);
      IShopperContext shopperContext = HttpProviderContainer.Instance.Resolve<IShopperContext>();
      ((TestContexts)shopperContext).SetContextInfo(privateLabelId, shopperId);

      ISplitProvider splitProvider = HttpProviderContainer.Instance.Resolve<ISplitProvider>();
      return splitProvider;
    }

    [TestMethod]
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    public void IsInRange()
    {
      MockHttpContext.SetMockHttpContext("default.aspx", "http://www.godaddy.com/default.aspx", String.Empty);
      ISplitProvider split = NewSplitProvider(1, "858884");

      Assert.IsNotNull(split);
      Assert.IsTrue(split.SplitValue > 0);
      Assert.IsTrue(split.SplitValue <= 100);
    }

    [TestMethod]
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    public void StandardAndPCSplits()
    {
      MockHttpContext.SetMockHttpContext("default.aspx", "http://www.godaddy.com/default.aspx", String.Empty);
      ISplitProvider split = NewSplitProvider(1, "858884");

      Assert.IsNotNull(split);
      Assert.IsTrue(split.SplitValue > 0);
      Assert.IsTrue(split.SplitValue <= 100);

      Assert.IsTrue(split.PCSplitValue > 0);
      Assert.IsTrue(split.PCSplitValue <= 4);
    }

  }
}
