using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.CDSRepository.Impl;
using Atlantis.Framework.CDSRepository.Interface;
using Atlantis.Framework.Interface;



namespace Atlantis.Framework.CDSRepository.Tests
{
  /// <summary>
  /// Summary description for CDSRepositoryTests
  /// </summary>
  [TestClass]
  public class CDSRepositoryTests
  {
    public CDSRepositoryTests()
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
    public void CDSRepository_FlatFile_Test1()
    {
      //Arrange
      string shopperId = "860316";
      int requestType = 452;
      string query = "sales/1/lp/email";
      CDSRepositoryRequestData requestData = new CDSRepositoryRequestData(shopperId, string.Empty, string.Empty, string.Empty, 1, query);
      //requestData.RequestTimeout = TimeSpan.FromSeconds(20);

      //Act
      CDSRepositoryResponseData responseData = (CDSRepositoryResponseData)DataCache.DataCache.GetProcessRequest(requestData, requestType);
      // this second call pulls from cache but not sure how to capture in the unit test to prove it without debugging into datacache.getprocessrequest
      var responseData2 = (CDSRepositoryResponseData)DataCache.DataCache.GetProcessRequest(requestData, requestType);

      //Assert
      Assert.IsTrue(responseData.IsSuccess);
      Assert.IsNotNull(responseData.ResponseData);
      Assert.AreEqual(responseData.ResponseData, responseData2.ResponseData);
    }


    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void CDSRepository_FlatFile_Test2()
    {
      //Arrange
      string shopperId = "860316";
      int requestType = 452;
      string query = "sales/1/lp/email";
      CDSRepositoryRequestData requestData = new CDSRepositoryRequestData(shopperId, string.Empty, string.Empty, string.Empty, 1, query);
     
      //Act
      CDSRepositoryResponseData responseData = (CDSRepositoryResponseData)DataCache.DataCache.GetProcessRequest(requestData, requestType);
     

      //Assert
      Assert.IsTrue(responseData.IsSuccess);
      Assert.IsNotNull(responseData.ResponseData);
      
    }





    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void CDSRepository_GetCacheMD5()
    {
      //Arrange
      string shopperId = "860316";     
      string query = "sales/1/lp/email";
      string expectedHash = "866D4C948747FC7816B111B8AB6739A6";
      CDSRepositoryRequestData requestData = new CDSRepositoryRequestData(shopperId, string.Empty, string.Empty, string.Empty, 1, query);
            
      //Act
      string result = requestData.GetCacheMD5();
      
      //Assert
      Assert.AreEqual(expectedHash, result);
    }


    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void CDSRepository_GetCacheMD5_On_Empty_Query()
    {
      //Arrange
      string expectedHash = "515DFED62F7510FA572FE0D9E7776CE9";
      CDSRepositoryRequestData requestData = new CDSRepositoryRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 1, string.Empty);

      //Act
      string result = requestData.GetCacheMD5();

      //Assert
      Assert.AreEqual(expectedHash, result);
    }
    

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void CDSRepository_Check_RequestTimesout_Default()
    {
      //Arrange
      string shopperId = "860316";     
      string query = "sales/1/lp/email";

      //Act
      CDSRepositoryRequestData requestData = new CDSRepositoryRequestData(shopperId, string.Empty, string.Empty, string.Empty, 1, query);
      
      //Assert
      Assert.AreEqual(20, requestData.RequestTimeout.Seconds);     
    }



    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [ExpectedException(typeof(AtlantisException))]
    public void CDSRepository_FlatFile_DoesntExist()
    {
      //Arrange
      string shopperId = "860316";
      int requestType = 452;
      string query = "sales/1/lp/nonexistent";
      CDSRepositoryRequestData requestData = new CDSRepositoryRequestData(shopperId, string.Empty, string.Empty, string.Empty, 1, query);
     

      //Act
      try
      {
        CDSRepositoryResponseData responseData = (CDSRepositoryResponseData)Engine.Engine.ProcessRequest(requestData, requestType);
      }
      catch (AtlantisException ex)
      {
        Assert.IsTrue(ex.Message.StartsWith(@"Could not find file"));
        throw;
      }
    }


    /// <summary>
    ///A test for RequestHandler
    ///</summary>
    [TestMethod()]
    [DeploymentItem("atlantis.config")]
    public void RequestHandlerTestRequestData()
    {
      //Arrange
      CDSRepositoryRequest target = new CDSRepositoryRequest();
      string shopperId = "860316";
      string query = "sales/1/lp/email";
      CDSRepositoryRequestData requestData = new CDSRepositoryRequestData(shopperId, "test.com/test","12345", string.Empty, 1, query);
      ConfigElement config = new ConfigElement("12345", "test.dll", true);
      IResponseData expected = null; // TODO: Initialize to an appropriate value
      IResponseData actual;


      actual = target.RequestHandler(requestData, config);
      Assert.AreEqual(expected, actual);

    }


    /// <summary>
    ///A test for RequestHandler
    ///</summary>
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [ExpectedException(typeof(NullReferenceException))]
    public void RequestHandlerTestRequestDataIsNull()
    {
      //Arrange
      CDSRepositoryRequest target = new CDSRepositoryRequest();      
      ConfigElement config = new ConfigElement("12345", "test.dll", true);      
      IResponseData actual;

      //Act
      actual = target.RequestHandler(null, config);
    }







  }
}
