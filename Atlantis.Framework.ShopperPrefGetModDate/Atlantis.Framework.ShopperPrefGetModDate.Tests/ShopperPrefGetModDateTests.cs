using System;
using System.Diagnostics;
using Atlantis.Framework.ShopperPrefGetModDate.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.ShopperPrefGetModDate.Test
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetShopperPrefGetModDateTests
  {

    private const string _shopperId = "860316";
    private const int _requestType = 263;


    public GetShopperPrefGetModDateTests()
    {
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
    public void ShopperPrefGetModDateTest()
    {
      ShopperPrefGetModDateRequestData request = new ShopperPrefGetModDateRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0);

      ShopperPrefGetModDateResponseData response = (ShopperPrefGetModDateResponseData)Engine.Engine.ProcessRequest(request, _requestType);

      Assert.IsTrue(response.IsSuccess);
      Assert.IsInstanceOfType(response.ModifyDate,  typeof(DateTime));
      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void ShopperPrefGetModDateNullTest()
    {
      ShopperPrefGetModDateRequestData request = new ShopperPrefGetModDateRequestData("55555"
         , string.Empty
         , string.Empty
         , string.Empty
         , 0);

      ShopperPrefGetModDateResponseData response = (ShopperPrefGetModDateResponseData)Engine.Engine.ProcessRequest(request, _requestType);

      Assert.IsTrue(response.IsSuccess);
      Assert.AreEqual(DateTime.MinValue, response.ModifyDate);
      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
