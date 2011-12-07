using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.RegCheckCaContacts.Interface;

namespace Atlantis.Framework.RegCheckCaContacts.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class RegCheckCaContactsTests
  {
    public RegCheckCaContactsTests()
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
    public void RegCheckCaContactsTest()
    {
      CaContactProperties properties = new CaContactProperties();
      properties.CaDomainName = "testCAdomain.ca";
      properties.TechAddress1
        = properties.AdminAddress1 = "colfax";
      properties.TechCity 
        = properties.AdminCity = "denver";
      properties.AdminCountry 
        = properties.TechCountry = "United States";
      properties.AdminEmail 
        = properties.TechEmail = "test@godaddy.com";
      properties.TechFirstName 
        = properties.AdminFirstName = "Go";
      properties.AdminLastName 
        = properties.TechLastName = "Daddy";
      properties.TechState 
        = properties.AdminState = "co";
      properties.TechPhone 
        = properties.AdminPhone = "3031234567";
      properties.AdminZip 
        = properties.TechZip = "80111";
      properties.CaRegistrantName = "GoDaddy";
      properties.CaLegalType = "CCO";

      RegCheckCaContactsRequestData request = new RegCheckCaContactsRequestData(
        "832652", string.Empty, string.Empty, string.Empty, 0, properties);
      request.RequestTimeout = new TimeSpan(0, 0, 10);
      string requestXml = request.ToXML();
      RegCheckCaContactsResponseData response
        = (RegCheckCaContactsResponseData)Engine.Engine.ProcessRequest(request, 150);
      int errorsCount = response.Errors.Count;
      Assert.IsTrue(response.IsValid);
    }
  }
}
