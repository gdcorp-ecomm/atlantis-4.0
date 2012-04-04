using System.Diagnostics;
using Atlantis.Framework.MYARecentOrders.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.MYARecentOrders.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetMYARecentOrdersTests
  {

    private const string _shopperId = "832652";
    private const int _recentOrdersRequestType = 142;

    public GetMYARecentOrdersTests()
    { }

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

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.MYARecentOrders.Impl.dll")]
    public void MYARecentOrdersTest()
    {
      int orderCount = RecentOrderCount.All;
      int days = 1200;
      MYARecentOrdersRequestData request = new MYARecentOrdersRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , orderCount
         , days);

      MYARecentOrdersResponseData response = (MYARecentOrdersResponseData)Engine.Engine.ProcessRequest(request, _recentOrdersRequestType);

      foreach (RecentOrder ro in response.RecentOrders)
      {
        Debug.WriteLine(string.Format("DateEntered: {0}", ro.DateEntered));
        Debug.WriteLine(string.Format("IsRefund: {0}", ro.IsRefund));
        Debug.WriteLine(string.Format("OrderId: {0}", ro.OrderId));
        Debug.WriteLine(string.Format("OrderSource: {0}", ro.OrderSource));
        Debug.WriteLine(string.Format("ShopperId: {0}", ro.ShopperId));
        Debug.WriteLine(string.Format("TotalAmount: {0}", ro.TransactionTotal.ToString("C")));
        Debug.WriteLine("************************************");
      }

      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.MYARecentOrders.Impl.dll")]
    public void MYARecentOrdersSerializeTest()
    {
      MockHttpContext.SetMockHttpContext("default.aspx", "http://www.mytest.com/default.aspx", string.Empty);

      int orderCount = 10;
      int days = 600;
      MYARecentOrdersRequestData request = new MYARecentOrdersRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , orderCount
         , days);

      MYARecentOrdersResponseData response = SessionCache.SessionCache.GetProcessRequest<MYARecentOrdersResponseData>(request, _recentOrdersRequestType);
      MYARecentOrdersResponseData response2 = SessionCache.SessionCache.GetProcessRequest<MYARecentOrdersResponseData>(request, _recentOrdersRequestType);
      Assert.AreEqual(response.RecentOrders.Count, response2.RecentOrders.Count);

      foreach (RecentOrder ro in response.RecentOrders)
      {
        foreach (RecentOrder sro in response2.RecentOrders)
        {
          if (ro.OrderId == sro.OrderId)
          {
            Assert.AreEqual(ro.DateEntered.ToString(), sro.DateEntered.ToString());
            Assert.AreEqual(ro.TransactionTotal.ToString(), sro.TransactionTotal.ToString());
          }
        }
      }

    }
  }
}
