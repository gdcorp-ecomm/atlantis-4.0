using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Nimitz.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class NimitzTests
  {
    public NimitzTests()
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
    public void GetConnectionStringBasic()
    {
      string connectionString = NetConnect.LookupConnectInfo("corp.web.mya.Godaddy","corp.web.mya.dev.client.godaddy.com","MYA","NimitzTests.GetConnectionStringBasic", ConnectLookupType.NetConnectionString);
      Assert.IsFalse(string.IsNullOrEmpty(connectionString));
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void GetConnectionStringMissingCertName()
    {
      string connectionString = NetConnect.LookupConnectInfo("corp.web.mya.Godaddy", string.Empty, "MYA", "NimitzTests.GetConnectionStringBasic", ConnectLookupType.NetConnectionString);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetConnectionStringConfig()
    {
      MockTripletRequestData request = new MockTripletRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      MockTripletResponseData response = (MockTripletResponseData)Engine.Engine.ProcessRequest(request, 999);
      Assert.IsTrue(response.Success);
    }

    [TestMethod]
    public void GetWebServiceConnectionBasic()
    {
      string connectInfo = NetConnect.LookupConnectInfo("corp.web.mya.orion", "corp.web.mya.dev.client.godaddy.com", "MYA", "NimitzTests.Webservice", ConnectLookupType.WebService);
      Assert.IsFalse(string.IsNullOrEmpty(connectInfo));
    }

    [TestMethod]
    public void GetXmlConnectionBasic()
    {
      string connectInfo = NetConnect.LookupConnectInfo("corp.web.mya.orion", "corp.web.mya.dev.client.godaddy.com", "MYA", "NimitzTests.Webservice", ConnectLookupType.Xml);
      Assert.IsFalse(string.IsNullOrEmpty(connectInfo));
    }

    [TestMethod]
    public void GetDelimitedConnectionBasic()
    {
      string connectInfo = NetConnect.LookupConnectInfo("corp.web.mya.orion", "corp.web.mya.dev.client.godaddy.com", "MYA", "NimitzTests.Webservice", ConnectLookupType.Delimited);
      Assert.IsFalse(string.IsNullOrEmpty(connectInfo));
    }

  }
}
