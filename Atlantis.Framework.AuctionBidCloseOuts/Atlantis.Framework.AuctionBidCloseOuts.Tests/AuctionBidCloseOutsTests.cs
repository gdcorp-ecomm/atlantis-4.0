using System;
using Atlantis.Framework.AuctionBidCloseOuts.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.AuctionBidCloseOuts.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  [DeploymentItem("Atlantis.Framework.AuctionBidCloseOuts.Impl.dll")]
  public class AuctionBidCloseOutsTests
  {
    public AuctionBidCloseOutsTests()
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
    public void AuctionBidCloseOutsTest()
    {
      AuctionBidCloseOutsRequestData request = new AuctionBidCloseOutsRequestData("77311", string.Empty, 
        string.Empty, string.Empty, 0, "snackshack.com", false);
      request.RequestTimeout = TimeSpan.FromSeconds(5d);
      AuctionBidCloseOutsResponseData response = (AuctionBidCloseOutsResponseData)Engine.Engine.ProcessRequest(request, 214);
      bool result = response.IsValid;
      Assert.IsFalse(result);
    }
  }
}
