using Atlantis.Framework.RedeemCreditEx.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.RedeemCreditEx.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class UnitTest1
  {
    public UnitTest1()
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
    [DeploymentItem("atlantis.config")]
    public void TestMethod1()
    {
      string redeemXML = "<RedeemCredit shopperID=\"ak5\" parentResourceID=\"369037\" parentResourceType=\"75\" parentIDType=\"BILLING\" childUnifiedProductID=\"3604\" returnIDType=\"ORION\" customXML=\"\" />";
      RedeemCreditExRequestData requestData = new RedeemCreditExRequestData("", "", "", "", 0, redeemXML);
      RedeemCreditExResponseData responseData = (RedeemCreditExResponseData)Engine.Engine.ProcessRequest(requestData, 79);

      Assert.IsTrue(responseData.Success);

    }
  }
}
