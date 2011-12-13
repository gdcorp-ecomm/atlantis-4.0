using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Atlantis.Framework.CMSIsDomainUsed.Impl;
using Atlantis.Framework.CMSIsDomainUsed.Interface;


namespace Atlantis.Framework.CMSIsDomainUsed.Test
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetCMSIsDomainUsedTests
  {
  
    private const string _shopperId = "";
	
	
    public GetCMSIsDomainUsedTests()
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
    public void CMSIsDomainUsedTest()
    {
     //CMSIsDomainUsedRequestData request = new CMSIsDomainUsedRequestData(_shopperId
     //   , string.Empty
     //   , string.Empty
     //   , string.Empty
     //   , 0 );

     // CMSIsDomainUsedResponseData response = (CMSIsDomainUsedResponseData)Engine.Engine.ProcessRequest(request, _requestType);
      
	  // Cache call
	  //CMSIsDomainUsedResponseData response = (CMSIsDomainUsedResponseData)DataCache.DataCache.GetProcessRequest(request, _requestType);

      //
      // TODO: Add test logic here
      //
	  
      //Debug.WriteLine(response.ToXML());
      //Assert.IsTrue(response.IsSuccess);
    }
  }
}
