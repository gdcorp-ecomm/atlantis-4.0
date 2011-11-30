using Atlantis.Framework.CDS.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Atlantis.Framework.Interface;
using System.Net;

namespace Atlantis.Framework.CDS.Tests
{   
    /// <summary>
    ///This is a test class for CDSResponseDataTest and is intended
    ///to contain all CDSResponseDataTest Unit Tests
    ///</summary>
  [TestClass()]
  public class CDSResponseDataTest
  {


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
    //You can use the following additional attributes as you write your tests:
    //
    //Use ClassInitialize to run code before running the first test in the class
    //[ClassInitialize()]
    //public static void MyClassInitialize(TestContext testContext)
    //{
    //}
    //
    //Use ClassCleanup to run code after all tests in a class have run
    //[ClassCleanup()]
    //public static void MyClassCleanup()
    //{
    //}
    //
    //Use TestInitialize to run code before running each test
    //[TestInitialize()]
    //public void MyTestInitialize()
    //{
    //}
    //
    //Use TestCleanup to run code after each test has run
    //[TestCleanup()]
    //public void MyTestCleanup()
    //{
    //}
    //
    #endregion


    /// <summary>
    ///A test for CDSResponseData Constructor
    ///</summary>
    [TestMethod()]
    public void CDSResponseDataConstructorTest1()
    {
      //Arrange
      string shopperId = "12345";
      string query = "test.com";
      string pathway = Guid.NewGuid().ToString();
      string errorDescription = "this is a test error descrption!";
      CDSRequestData requestData = new CDSRequestData(shopperId, string.Empty, string.Empty, pathway, 1, query);      
      AtlantisException atlantisException = new AtlantisException(requestData, "test", errorDescription, "test");

      //Act
      CDSResponseData responseData = new CDSResponseData(atlantisException);
   
      //Assert     
      Assert.AreEqual(errorDescription, responseData.GetException().ErrorDescription);
    }

    /// <summary>
    ///A test for CDSResponseData Constructor
    ///</summary>
    [TestMethod()]
    public void CDSResponseDataConstructorTest2()
    {
      //Arrange
      string shopperId = "12345";
      string query = "test.com";
      string pathway = Guid.NewGuid().ToString();           
      HttpStatusCode statusCode = new HttpStatusCode();
      Exception exception = new Exception("This is a test exception!");
      CDSRequestData requestData = new CDSRequestData(shopperId, string.Empty, string.Empty, pathway, 1, query);
      
      //Act
      CDSResponseData responseData = new CDSResponseData(requestData, statusCode, exception);           

      //Assert
      Assert.AreEqual(exception.Message, responseData.GetException().ErrorDescription);
    }

    /// <summary>
    ///A test for CDSResponseData Constructor
    ///</summary>
    [TestMethod()]
    public void CDSResponseDataConstructorTest3()
    {
      //Arrange
      string responseData = "this is a test response data message!";   
      HttpStatusCode statusCode = new HttpStatusCode();           

      //Act
      CDSResponseData target = new CDSResponseData(responseData, statusCode);     

      //Assert
      Assert.AreEqual(responseData, target.ResponseData);     
    }

    /// <summary>
    ///A test for ToXML
    ///</summary>
    [TestMethod()]
    public void ToXMLTest()
    {
      //Arrange
      string shopperId = "12345";
      string query = "test.com";
      string pathway = Guid.NewGuid().ToString();
      string errorDescription = "this is a test error descrption!";
      CDSRequestData requestData = new CDSRequestData(shopperId, string.Empty, string.Empty, pathway, 1, query);
      AtlantisException atlantisException = new AtlantisException(requestData, "test", errorDescription, "test");      
      CDSResponseData responseData = new CDSResponseData(atlantisException);
      var actual = responseData.ToXML();
     
      //Assert
      Assert.IsNotNull(actual);
    }
  }

}
