using System;
using Atlantis.Framework.EcommInstoreStatement.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.EcommInstoreStatement.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetStatement
  {
    private TestContext testContextInstance;

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
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

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("app.config")]
    [DeploymentItem("Atlantis.Framework.EcommInstoreStatement.Impl.dll")]
    public void InstoreCreditStatementTest()
    {
      var startDate = new DateTime(2012,05,01);
      var endDate = new DateTime(2013, 05, 06);
      var request = new EcommInstoreStatementRequestData("856907", string.Empty, string.Empty, string.Empty, 0, startDate, endDate);
      var response = (EcommInstoreStatementResponseData)Engine.Engine.ProcessRequest(request, 496);

      var xml = response.ProcessedToXmlString;
      var list = response.StatementByCurrencyList;
      var emptyList = response.EmptyStatement;

      Assert.IsTrue(response.IsSuccess);
    }
  }
}
