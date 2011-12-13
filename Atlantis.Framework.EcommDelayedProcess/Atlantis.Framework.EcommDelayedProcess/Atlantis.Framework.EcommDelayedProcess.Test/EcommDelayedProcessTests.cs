using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Atlantis.Framework.EcommDelayedProcess.Impl;
using Atlantis.Framework.EcommDelayedProcess.Interface;


namespace Atlantis.Framework.EcommDelayedProcess.Test
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetEcommDelayedProcessTests
  {
  
    private const string _shopperId = "";
	
	
    public GetEcommDelayedProcessTests()
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
    public void EcommDelayedProcessTest()
    {
      string encryptedResults = string.Empty;
     EcommDelayedProcessRequestData request = new EcommDelayedProcessRequestData(_shopperId
        , string.Empty
        , string.Empty
        , string.Empty
        , 0 ,encryptedResults,"6BCEC931-AE9F-44F0-9485-1BA8E1EBDF82");
      
      EcommDelayedProcessResponseData response = (EcommDelayedProcessResponseData)Engine.Engine.ProcessRequest(request, 433);
      
	  // Cache call
	  //EcommDelayedProcessResponseData response = (EcommDelayedProcessResponseData)DataCache.DataCache.GetProcessRequest(request, _requestType);

      //
      // TODO: Add test logic here
      //
	  
      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
