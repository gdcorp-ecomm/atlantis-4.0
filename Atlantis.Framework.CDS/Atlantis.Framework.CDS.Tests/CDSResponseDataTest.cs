using System;
using Atlantis.Framework.CDS.Interface;
using Atlantis.Framework.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.CDS.Tests
{   
  [TestClass()]
  public class CDSResponseDataTest
  {

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
      CDSResponseData responseData = new CDSResponseData(requestData, atlantisException);
   
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
      Exception exception = new Exception("This is a test exception!");
      CDSRequestData requestData = new CDSRequestData(shopperId, string.Empty, string.Empty, pathway, 1, query);
      
      //Act
      CDSResponseData responseData = new CDSResponseData(requestData, exception);           

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

      //Act
      CDSResponseData target = new CDSResponseData(responseData);     

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
      CDSResponseData responseData = new CDSResponseData(requestData, atlantisException);
      var actual = responseData.ToXML();
     
      //Assert
      Assert.IsNotNull(actual);
    }
        
  }

}
