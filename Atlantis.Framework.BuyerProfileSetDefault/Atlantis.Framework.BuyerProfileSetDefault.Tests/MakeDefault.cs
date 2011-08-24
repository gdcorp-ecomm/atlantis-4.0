using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.BuyerProfileSetDefault.Interface;

namespace Atlantis.Framework.BuyerProfileSetDefault.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class MakeDefault
  {
    public MakeDefault()
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
    [DeploymentItem("App.config")]    
    public void TestMethod1()
    {
      BuyerProfileSetDefaultRequestData request = new BuyerProfileSetDefaultRequestData("850774", string.Empty, string.Empty, string.Empty, 0, "150304", true);

      BuyerProfileSetDefaultResponseData response = (BuyerProfileSetDefaultResponseData)Engine.Engine.ProcessRequest(request, 415);

      Assert.IsTrue(response.IsSuccess);
    }
  }
}
