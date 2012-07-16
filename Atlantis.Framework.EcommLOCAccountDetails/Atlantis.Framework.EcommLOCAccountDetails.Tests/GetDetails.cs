using System;
using Atlantis.Framework.EcommLOCAccountDetails.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.EcommLOCAccountDetails.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetDetails
  {
    public GetDetails()
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
      DateTime startDate = new DateTime(2012, 7, 1);
      DateTime endDate = new DateTime(2012, 7, 2);
      int accountId = 1375; //1376

      EcommLOCAccountDetailsRequestData request = new EcommLOCAccountDetailsRequestData("856907", string.Empty, string.Empty, string.Empty, 0, startDate, endDate, accountId);
      EcommLOCAccountDetailsResponseData response = (EcommLOCAccountDetailsResponseData)Engine.Engine.ProcessRequest(request, 565);
      LineOfCreditAccountDetail detail = new LineOfCreditAccountDetail(response.ResponseXML);
      detail = response.LOCDetail();

      Assert.IsTrue(response.IsSuccess);

    }
  }
}
