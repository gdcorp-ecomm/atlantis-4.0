using System;
using System.Diagnostics;
using Atlantis.Framework.EEMCreateNewAccount.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Atlantis.Framework.EEMCreateNewAccount.Tests
{
  [TestClass]
  public class GetEEMCreateNewAccountTests
  {
    private const string _shopperId = "856907";
    private const int _requestType =455;
	
	
    public GetEEMCreateNewAccountTests()
    { }

    private TestContext testContextInstance;

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
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    public void EEMCreateNewAccountTest()
    {
      int billingResourceId = 4593;
      int parentBundleId = 440148;
      int parentBundleTypeId = 56;
      int pfid = 4701;
      string recurring = "annual";
      DateTime startDate = DateTime.Parse("2011-11-22 12:36:29.300");

      EEMCreateNewAccountRequestData request = new EEMCreateNewAccountRequestData(_shopperId
        , string.Empty
        , string.Empty
        , string.Empty
        , 0
        , billingResourceId
        , parentBundleId
        , parentBundleTypeId
        , pfid
        , 1
        , recurring
        , startDate);

      EEMCreateNewAccountResponseData response = (EEMCreateNewAccountResponseData)Engine.Engine.ProcessRequest(request, _requestType);      
	  
      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
      Assert.IsTrue(response.CustomerId > 0);
    }
  }
}
