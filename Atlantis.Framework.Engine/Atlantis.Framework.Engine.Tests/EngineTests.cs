using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Engine.Tests.MockTriplet;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Engine.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class EngineTests
  {
    public EngineTests()
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
    public void ConfigElementWithCustomValues()
    {
      ConfigTestRequestData request = new ConfigTestRequestData("832652", string.Empty, string.Empty, string.Empty, 0);
      ConfigTestResponseData response = (ConfigTestResponseData)Engine.ProcessRequest(request, 9999);
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void ConfigElementWithoutCustomValues()
    {
      ConfigTestRequestData request = new ConfigTestRequestData("832652", string.Empty, string.Empty, string.Empty, 0);
      ConfigTestResponseData response = (ConfigTestResponseData)Engine.ProcessRequest(request, 9998);
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void LogException()
    {
      AtlantisException ex = new AtlantisException("EngineTests.LogException", "911", "Test log message only.", string.Empty, null, null);
      Engine.LogAtlantisException(ex);
    }
  }
}
