using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.PromoOffering.Interface;
using System.Linq;

namespace Atlantis.Framework.PromoOffering.Tests
{
  /// <summary>
  ///This is a test class for PromoOfferingRequestTest and is intended
  ///to contain all PromoOfferingRequestTest Unit Tests
  ///</summary>
  [TestClass()]
  public class PromoOfferingRequestTest
  {
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

    #region Additional test attributes
    // 
    //You can use the following additional attributes as you write your tests:
    //
    //Use ClassInitialize to run code before running the first test in the class
    //[ClassInitialize()]
    //public static void MyClassInitialize(TestContext testContext)
    //{
    //}
    //
    //Use ClassCleanup to run code after all tests in a class have run
    //[ClassCleanup()]
    //public static void MyClassCleanup()
    //{
    //}
    //
    //Use TestInitialize to run code before running each test
    //[TestInitialize()]
    //public void MyTestInitialize()
    //{
    //}
    //
    //Use TestCleanup to run code after each test has run
    //[TestCleanup()]
    //public void MyTestCleanup()
    //{
    //}
    //
    #endregion

    [TestMethod()]
    [DeploymentItem("Atlantis.config")]
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    public void DataCacheRequestTest()
    {
      PromoOfferingRequestData request = new PromoOfferingRequestData("858937", string.Empty, string.Empty, string.Empty, 0, 440859);
      PromoOfferingResponseData response = (PromoOfferingResponseData)DataCache.DataCache.GetProcessRequest(request, 573);
      Assert.IsTrue(response.IsSuccess);
      Assert.IsNotNull(response.Promotions);
      Assert.IsTrue(0 < response.Promotions.Count());

      response = (PromoOfferingResponseData)DataCache.DataCache.GetProcessRequest(request, 573);
      Assert.IsTrue(response.IsSuccess);
      Assert.IsNotNull(response.Promotions);
      Assert.IsTrue(0 < response.Promotions.Count());
    }

    /// <summary>
    ///A test for RequestHandler
    ///</summary>
    [TestMethod()]
    [DeploymentItem("Atlantis.config")]
    public void RequestHandlerTest()
    {
      PromoOfferingRequestData request = new PromoOfferingRequestData("858937", string.Empty, string.Empty, string.Empty, 0, 440859);
      PromoOfferingResponseData actual = (PromoOfferingResponseData)Engine.Engine.ProcessRequest(request, 573);
      Assert.IsTrue(actual.IsSuccess);
      Assert.IsNotNull(actual.Promotions);
      Assert.IsTrue(0 < actual.Promotions.Count());
      ResellerPromoItem testobject = null;
      Assert.IsTrue(actual.TryGetPromoItemByPromoGroupId(56, out testobject) && true == testobject.IsActive);
    }
  }
}
