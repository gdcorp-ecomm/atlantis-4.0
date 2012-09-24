using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Atlantis.Framework.GetBasketObjects.Impl;
using Atlantis.Framework.GetBasketObjects.Interface;


namespace Atlantis.Framework.GetBasketObjects.Test
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetGetBasketObjectsTests
  {
  
    private const string _shopperId = "";
	
	
    public GetGetBasketObjectsTests()
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
    [DeploymentItem("Atlantis.Framework.GetBasketObjects.Impl")]
    public void GetBasketObjectsTest()
    {
      string _shopperId = "853392";
      GetBasketObjectsRequest oRequest;
     GetBasketObjectsRequestData request = new GetBasketObjectsRequestData(_shopperId
        , string.Empty
        , string.Empty
        , string.Empty
        , 0 );

     GetBasketObjectsResponseData response = (GetBasketObjectsResponseData)Engine.Engine.ProcessRequest(request, 603);
	  
      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
