using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Atlantis.Framework.RenewalPeriods.Impl;
using Atlantis.Framework.RenewalPeriods.Interface;


namespace Atlantis.Framework.RenewalPeriods.Test
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetRenewalPeriodsTests
  {

    private const string _shopperId = "";


    public GetRenewalPeriodsTests()
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
    public void RenewalPeriodsTest()
    {
      int _requestType = 398;
      string _shopperId = "859012";
      RenewalPeriodsRequestData request = new RenewalPeriodsRequestData(_shopperId
        , string.Empty
        , string.Empty
        , string.Empty
        , 0, new System.Collections.Generic.List<int>() { 
          //domain ids
          //2119527, 		2122298, 		2119063, 		2119067, 		2119069, 		2119083, 		2122299, 		2122295, 		2122296 ,		2122297 ,			
          //2122892, 			1729423 ,			1729487 ,			2117484 ,			2119285 ,			1668072 ,			1668073 ,			1668074 ,			1668075 ,	
          //1729427 ,			1729424 ,			1729432 ,			1729431 ,			1729422 ,			1729429 ,			1729428 ,			1729425 ,			1729426 ,		
          //2119059 ,			2119057 ,			2119065 ,			2119064 ,			2119066 ,			2119060 ,			2119061 ,			2119062 ,			2118300 ,		
          //2118488 ,			2119058 ,			1729430 ,			2117288 ,			2118489 ,			1721929    });

          //resourceids
          422413, 421871, 421870, 421877, 421876,420945,420947,421857,421859,421854,421858,421855,
          421874,421872,421853,413233,421875,421873,421856,418421,421358 });

      RenewalPeriodsResponseData response = (RenewalPeriodsResponseData)Engine.Engine.ProcessRequest(request, _requestType);

      Assert.IsTrue(response.IsSuccess);
      Debug.WriteLine(response.ToXML());
    }
  }
}
