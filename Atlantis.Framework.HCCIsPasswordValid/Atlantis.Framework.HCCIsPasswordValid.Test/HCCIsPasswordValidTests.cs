using System.Diagnostics;
using Atlantis.Framework.HCCIsPasswordValid.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Atlantis.Framework.HCCIsPasswordValid.Test
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetHCCIsPasswordValidTests
  {
  
    private const string _shopperId = "12530";
	
	
    public GetHCCIsPasswordValidTests()
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
    public void HCCIsPasswordValidTest()
    {
      HCCIsPasswordValidRequestData request = new HCCIsPasswordValidRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , "Testuser23r4");

      HCCIsPasswordValidResponseData response = (HCCIsPasswordValidResponseData)Engine.Engine.ProcessRequest(request, 267);


      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
