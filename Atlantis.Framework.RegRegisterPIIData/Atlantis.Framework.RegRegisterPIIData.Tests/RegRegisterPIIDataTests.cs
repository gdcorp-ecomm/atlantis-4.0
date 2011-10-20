using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.RegRegisterPIIData.Interface;

namespace Atlantis.Framework.RegRegisterPIIData.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class RegRegisterPIIDataTests
  {
    public RegRegisterPIIDataTests()
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
    public void RegRegisterPIIDataBasic()
    {
      RegRegisterPIIDataRequestData request = new RegRegisterPIIDataRequestData(
        "832652", string.Empty, string.Empty, string.Empty, 0, "unit_test", 121);
      request.AddRequestValue("itPINNumber", "123456789");
      request.AddRequestValue("itPINType", "PinTest");
      RegRegisterPIIDataResponseData response = (RegRegisterPIIDataResponseData)Engine.Engine.ProcessRequest(request, 115);
      Assert.IsTrue(response.IsSuccess);
      Assert.IsFalse(string.IsNullOrEmpty(response.Token));
    }
  }
}
