using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Atlantis.Framework.EcommLastOrderLang.Impl;
using Atlantis.Framework.EcommLastOrderLang.Interface;


namespace Atlantis.Framework.EcommLastOrderLang.Test
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class EcommLastOrderLangTests
  {
  
    private const string _shopperId = "";
	
	
    public EcommLastOrderLangTests()
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
    public void EcommLastOrderLangTest()
    {
     EcommLastOrderLangRequestData request = new EcommLastOrderLangRequestData(_shopperId
        , string.Empty
        , string.Empty
        , string.Empty
        , 0 );

      EcommLastOrderLangResponseData response = (EcommLastOrderLangResponseData)Engine.Engine.ProcessRequest(request, _requestType);
      
	  // Cache call
	  //EcommLastOrderLangResponseData response = (EcommLastOrderLangResponseData)DataCache.DataCache.GetProcessRequest(request, _requestType);

      //
      // TODO: Add test logic here
      //
	  
      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
