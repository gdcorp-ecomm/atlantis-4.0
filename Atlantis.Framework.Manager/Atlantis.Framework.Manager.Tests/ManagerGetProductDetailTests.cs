﻿using System.Data;
using Atlantis.Framework.Manager.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Manager.Tests
{
  /// <summary>
  ///   Summary description for ManagerGetProductDetailTests
  /// </summary>
  [TestClass]
  public class ManagerGetProductDetailTests
  {
    /// <summary>
    ///   Gets or sets the test context which provides
    ///   information about and functionality for the current test run.
    /// </summary>
    public TestContext TestContext { get; set; }

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
    public void VerifyDataSet()
    {
      int shopperIdInt = 862009;
      string shopperId = shopperIdInt.ToString();
      decimal pfid = 111100M;
      int privateLabelId = 1;
      int isAdmin = 1;
      int managerUserId = 5032;
      var request = new ManagerGetProductDetailRequestData(shopperId, string.Empty, string.Empty, string.Empty, 0, pfid,
                                                           privateLabelId, isAdmin, managerUserId);
      var response = (ManagerGetProductDetailResponseData) Engine.Engine.ProcessRequest(request, 395);
      DataTable dt = response.ProductCatalogDetails;
      Assert.IsNotNull(dt);
    }

    [TestMethod]
    public void VerifyDataExists()
    {
      //
      // TODO: Add test logic here
      //
      int shopperIdInt = 862009;
      string shopperId = shopperIdInt.ToString();
      decimal pfid = 111100M;
      int privateLabelId = 1;
      int isAdmin = 1;
      int managerUserId = 5032;
      var request = new ManagerGetProductDetailRequestData(shopperId, string.Empty, string.Empty, string.Empty, 0, pfid,
                                                           privateLabelId, isAdmin, managerUserId);
      var response = (ManagerGetProductDetailResponseData) Engine.Engine.ProcessRequest(request, 395);
      DataTable dt = response.ProductCatalogDetails;
      Assert.IsTrue(dt != null && dt.Rows.Count > 0, "DataTable is null or does not contain any rows.");
    }

    [TestMethod]
    public void VerifyExpectedRowCount()
    {
      //
      // TODO: Add test logic here
      //
      int shopperIdInt = 862009;
      string shopperId = shopperIdInt.ToString();
      decimal pfid = 111100M;
      int privateLabelId = 1;
      int isAdmin = 1;
      int managerUserId = 5032;
      var request = new ManagerGetProductDetailRequestData(shopperId, string.Empty, string.Empty, string.Empty, 0, pfid,
                                                           privateLabelId, isAdmin, managerUserId);
      var response = (ManagerGetProductDetailResponseData) Engine.Engine.ProcessRequest(request, 395);
      DataTable dt = response.ProductCatalogDetails;
      Assert.IsTrue(dt != null && dt.Rows.Count == 1, "DataTable is null or does not contain the expected rowcount.");
    }
  }
}