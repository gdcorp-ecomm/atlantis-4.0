using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MktgSubscribeAdd.Interface;

namespace Atlantis.Framework.MktgSubscribeAdd.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class SubscriberAdd
  {
    public SubscriberAdd()
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
    public void AddSubscriber()
    {
      MktgSubscribeAddRequestData request = new MktgSubscribeAddRequestData("850774", string.Empty, string.Empty, string.Empty,
                                             0, "sthota@godaddy.com", 25, 1, 0, "Sridhar", "Thota", false, "172.19.72.107");
      request.IPAddress = "172.19.72.107";

      MktgSubscribeAddResponseData response = (MktgSubscribeAddResponseData)Engine.Engine.ProcessRequest(request, 169);
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
