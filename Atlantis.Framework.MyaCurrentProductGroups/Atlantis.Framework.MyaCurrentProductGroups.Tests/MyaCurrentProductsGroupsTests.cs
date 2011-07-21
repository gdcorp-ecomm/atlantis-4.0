using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.MyaCurrentProductGroups.Interface;

namespace Atlantis.Framework.MyaCurrentProductGroups.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class MyaCurrentProductsGroupsTests
  {
    public MyaCurrentProductsGroupsTests()
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
    public void MyaCurrentProductGroupsBasic()
    {
      MyaCurrentProductGroupsRequestData request =
        new MyaCurrentProductGroupsRequestData("832652", string.Empty, string.Empty, string.Empty, 0, 1);
      MyaCurrentProductGroupsResponseData response = (MyaCurrentProductGroupsResponseData)Engine.Engine.ProcessRequest(request, 163);
      Assert.IsNull(response.RequestException);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void MyaCurrentProductGroupsReadOnly()
    {
      MyaCurrentProductGroupsRequestData request =
        new MyaCurrentProductGroupsRequestData("849631", string.Empty, string.Empty, string.Empty, 0, 1);
      MyaCurrentProductGroupsResponseData response = (MyaCurrentProductGroupsResponseData)Engine.Engine.ProcessRequest(request, 9163);
      Assert.IsNull(response.RequestException);
    }

  }
}
