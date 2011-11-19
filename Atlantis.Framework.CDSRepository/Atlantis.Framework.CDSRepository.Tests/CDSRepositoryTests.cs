using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.CDSRepository.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CDSRepository.Tests
{
  /// <summary>
  /// Summary description for CDSRepositoryTests
  /// </summary>
  [TestClass]
  public class CDSRepositoryTests
  {
    public CDSRepositoryTests()
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
    public void CDSRepository_FlatFile_Test1()
    {
      //Arrange
      string shopperId = "860316";
      int requestType = 452;
      string query = "sales/1/lp/email";
      CDSRepositoryRequestData requestData = new CDSRepositoryRequestData(shopperId, string.Empty, string.Empty, string.Empty, 1, query);
      //requestData.RequestTimeout = TimeSpan.FromSeconds(20);

      //Act
      CDSRepositoryResponseData responseData = (CDSRepositoryResponseData)DataCache.DataCache.GetProcessRequest(requestData, requestType);

      //Assert
      Assert.IsTrue(responseData.IsSuccess);
      Assert.IsNotNull(responseData.ResponseData);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [ExpectedException(typeof(AtlantisException))]
    public void CDSRepository_FlatFile_DoesntExist()
    {
      //Arrange
      string shopperId = "860316";
      int requestType = 452;
      string query = "sales/1/lp/nonexistent";
      CDSRepositoryRequestData requestData = new CDSRepositoryRequestData(shopperId, string.Empty, string.Empty, string.Empty, 1, query);
      //requestData.RequestTimeout = TimeSpan.FromSeconds(20);

      //Act
      try
      {
        CDSRepositoryResponseData responseData = (CDSRepositoryResponseData)Engine.Engine.ProcessRequest(requestData, requestType);
      }
      catch (AtlantisException ex)
      {
        Assert.IsTrue(ex.Message.StartsWith(@"Could not find file"));
        throw;
      }
    }
  }
}
