using System;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.EEMResellerOptIn.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.EEMResellerOptIn.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class UnitTest1
  {
    public UnitTest1()
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
    [DeploymentItem("App.config")]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("DataProvider.xml")]
    public void TestMethod1()
    {
      String emailAddress = "kklink@godaddy.com";
      String shopperId = "858421";
      string firstName = "Kris";
      string lastName = "Klink";
      int emailTypeId = 1;

      EEMResellerOptInRequestData request = new EEMResellerOptInRequestData(shopperId, string.Empty, string.Empty, string.Empty, 0, 1, emailTypeId ,emailAddress, firstName, lastName);
      EEMResellerOptInResponseData response = (EEMResellerOptInResponseData)Engine.Engine.ProcessRequest(request, 312);

      Assert.IsTrue(response.IsSuccess);
      Debug.WriteLine(response.ToXML());

    }
  }
}
