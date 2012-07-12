using System.Collections.Generic;
using Atlantis.Framework.EcommLOCAccounts.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.EcommLOCAccounts.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetAccounts
  {
    public GetAccounts()
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
    public void LOCAccounts()
    {
      EcommLOCAccountsRequestData request = new EcommLOCAccountsRequestData("856907", string.Empty, string.Empty, string.Empty, 0);
      EcommLOCAccountsResponseData response = (EcommLOCAccountsResponseData)Engine.Engine.ProcessRequest(request, 563);
      if (response.IsSuccess)
      {
        Dictionary<int, string> _accountsDict = new Dictionary<int, string>(3);
        _accountsDict = response.LOCAccounts;
      }

      Assert.IsTrue(response.IsSuccess);

    }
  }
}
