using System;
using Atlantis.Framework.MyaOrderHistory.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.MyaOrderHistory.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetHistory
  {
    public GetHistory()
    {
      //
      // TODO: Add constructor logic here
      //
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
    public void TestMethod1()
    {
      
      MyaOrderHistoryRequestData request = new MyaOrderHistoryRequestData("856907", string.Empty, string.Empty, string.Empty, 0);

      request.StartDate = DateTime.Now.AddYears(-1);
      request.EndDate = DateTime.Now;

      //request.StoredProcedureName = StoredProcedure.ReceiptByDate;

      request.StoredProcedureName = StoredProcedure.ReceiptByPaymentProfileId;
      request.PaymentProfileId = 58628;


      //request.StoredProcedureName = StoredProcedure.ReceiptByProductGroupId;
      //request.ProductGroupId = 1


      //request.StoredProcedureName = StoredProcedure.ReceiptByDomain;
      //request.DomainName = "needaboatloadofdomains10.biz"

      MyaOrderHistoryResponseData response = (MyaOrderHistoryResponseData)Engine.Engine.ProcessRequest(request, 571);
      
      Assert.IsTrue(response.IsSuccess);

    }
  }
}
