using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Atlantis.Framework.QSCSetupAccount.Impl;
using Atlantis.Framework.QSCSetupAccount.Interface;


namespace Atlantis.Framework.QSCSetupAccount.Test
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetQSCSetupAccountTests
  {
  
    private const string _shopperId = "";
	
	
    public GetQSCSetupAccountTests()
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
    public void QSCSetupAccountTest()
    {
     QSCSetupAccountRequestData request = new QSCSetupAccountRequestData(_shopperId
        , string.Empty
        , string.Empty
        , string.Empty
        , 0,string.Empty,string.Empty,string.Empty,string.Empty,string.Empty );

      QSCSetupAccountResponseData response = (QSCSetupAccountResponseData)Engine.Engine.ProcessRequest(request, 333);
      
	  // Cache call
	  //QSCSetupAccountResponseData response = (QSCSetupAccountResponseData)DataCache.DataCache.GetProcessRequest(request, _requestType);

      //
      // TODO: Add test logic here
      //
	  
      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
