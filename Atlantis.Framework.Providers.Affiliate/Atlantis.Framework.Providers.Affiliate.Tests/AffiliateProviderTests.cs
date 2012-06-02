using System;
using System.Diagnostics;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Affiliate.Interface;
using Atlantis.Framework.Providers.Affiliates.Tests;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Atlantis.Framework.Testing.MockHttpContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.Affiliate.Tests
{
  [TestClass]
  public class AffiliateProviderTests
  {
    private const string _shopperId = "856907";
	
    public AffiliateProviderTests()
    { }

    private TestContext testContextInstance;

    public TestContext TestContext
    {
      get { return testContextInstance; }
      set { testContextInstance = value; }
    }

    #region Additional test attributes
    //
    // You can use the following additional attributes as you write your tests:
    //
    // Use ClassInitialize to run code before running the first test in the class
    // [ClassInitialize()]
    // public static void MyClassInitialize(TestContext testContext) { }
    //
    // Use ClassCleanup to run code after all tests in a class have run
    // [ClassCleanup()]
    // public static void MyClassCleanup() { }
    //
    // Use TestInitialize to run code before running each test 
    // [TestInitialize()]
    // public void MyTestInitialize() { }
    //
    // Use TestCleanup to run code after each test has run
    // [TestCleanup()]
    // public void MyTestCleanup() { }
    //
    #endregion

    [TestInitialize]
    public void InitializeTests()
    {
      HttpProviderContainer.Instance.RegisterProvider<ISiteContext, TestContexts>();
      HttpProviderContainer.Instance.RegisterProvider<IShopperContext, TestContexts>();
      HttpProviderContainer.Instance.RegisterProvider<IAffiliateProvider, AffiliateProvider>();
    }

    private IAffiliateProvider NewAffiliateProvider(int privateLabelId)
    {
       ISiteContext siteContext = HttpProviderContainer.Instance.Resolve<ISiteContext>();
      ((TestContexts)siteContext).SetContextInfo(privateLabelId, _shopperId);
      IShopperContext shopperContext = HttpProviderContainer.Instance.Resolve<IShopperContext>();
      ((TestContexts)shopperContext).SetContextInfo(privateLabelId, _shopperId);

      IAffiliateProvider affiliateProvider = HttpProviderContainer.Instance.Resolve<IAffiliateProvider>();

      return affiliateProvider;
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.AffiliateMetaData.Impl.dll")]
    public void IsInvalidAffiliateTest()
    {
      MockHttpContext.SetMockHttpContext("default.aspx", "http://mya.bluerazor.com/default.aspx", String.Empty);
      IAffiliateProvider affiliate = NewAffiliateProvider(2);
      Assert.IsFalse(affiliate.IsValidAffiliate("cju"));
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.AffiliateMetaData.Impl.dll")]
    public void IsValidAffiliateTest()
    {
      MockHttpContext.SetMockHttpContext("default.aspx", "http://mya.godaddy.com/default.aspx", String.Empty);
      IAffiliateProvider affiliate = NewAffiliateProvider(1);
      Assert.IsTrue(affiliate.IsValidAffiliate("cju"));
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.AffiliateMetaData.Impl.dll")]
    public void GetValidAffiliateInfoTest()
    {
      MockHttpContext.SetMockHttpContext("default.aspx", "http://mya.godaddy.com/default.aspx?isc=cju667", String.Empty);
      IAffiliateProvider affiliate = NewAffiliateProvider(1);
 //     ISiteContext siteContext = HttpProviderContainer.Instance.Resolve<ISiteContext>();
 //     ((TestContexts)siteContext).SetContextInfo(1, _shopperId);
      
      string affiliateType = string.Empty;
      DateTime affiliateStartDate = DateTime.MinValue;

      Assert.IsTrue(affiliate.ProcessAffiliateSourceCode("cju667", out affiliateType, out affiliateStartDate));

      Debug.WriteLine(string.Format("Affilitate: {0} | StartDate: {1}", affiliateType, affiliateStartDate));

    }
  }
}
