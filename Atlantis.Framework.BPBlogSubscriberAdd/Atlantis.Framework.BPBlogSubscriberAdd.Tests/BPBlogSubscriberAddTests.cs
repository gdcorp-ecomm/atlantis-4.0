using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.BPBlogSubscriberAdd.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BPBlogSubscriberAdd.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class BPBlogSubscriberAddTests
  {
    public BPBlogSubscriberAddTests()
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
    public void AddSubscriberBasic()
    {
      BPBlogSubscriberAddRequestData request = new BPBlogSubscriberAddRequestData(
        string.Empty, string.Empty, string.Empty, string.Empty, 0,
        "mmicco@godaddy.com", "Michael", "Micco", false);
      BPBlogSubscriberAddResponseData response = (BPBlogSubscriberAddResponseData)Engine.Engine.ProcessRequest(request, 139);
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [ExpectedException(typeof(AtlantisException))]
    public void AddSubscriberError()
    {
      BPBlogSubscriberAddRequestData request = new BPBlogSubscriberAddRequestData(
        string.Empty, string.Empty, string.Empty, string.Empty, 0,
        "blue", "Michael", "Micco", false);
      BPBlogSubscriberAddResponseData response = (BPBlogSubscriberAddResponseData)Engine.Engine.ProcessRequest(request, 139);
      Assert.IsTrue(response.IsSuccess);
    }

  }
}
