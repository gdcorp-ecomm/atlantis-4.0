using Atlantis.Framework.GetCreditGroupSummary.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.GetCreditGroupSummary.Tests
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
    [DeploymentItem("Atlantis.Framework.GetCreditGroupSummary.Impl.dll")]
    public void TestMethod1()
    {
      GetCreditGroupSummaryRequestData requestData = new GetCreditGroupSummaryRequestData("842904",
        "http://localhost", "", "", 0, 193);
      GetCreditGroupSummaryResponseData responseData = (GetCreditGroupSummaryResponseData)Engine.Engine.ProcessRequest(requestData, 80);
      Assert.AreEqual(responseData.XML.Substring(0, 22), "<CREDITS totalCredits=");
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.GetCreditGroupSummary.Impl.dll")]
    public void TestMethod2()
    {
      GetCreditGroupSummaryRequestData requestData = new GetCreditGroupSummaryRequestData("842904",
        "http://localhost", "", "", 0, 187);
      GetCreditGroupSummaryResponseData responseData = (GetCreditGroupSummaryResponseData)Engine.Engine.ProcessRequest(requestData, 80);
      Assert.AreEqual(responseData.XML.Substring(0, 22), "<CREDITS totalCredits=");
    }
  }
}
