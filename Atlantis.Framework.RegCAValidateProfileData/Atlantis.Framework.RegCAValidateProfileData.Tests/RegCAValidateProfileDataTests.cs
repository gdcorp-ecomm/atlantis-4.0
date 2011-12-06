using Atlantis.Framework.RegCAValidateProfileData.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.RegCAValidateProfileData.Tests
{
  /// <summary>
  /// Summary description for RegCAValidateProfileDataTests
  /// </summary>
  [TestClass]
  public class RegCAValidateProfileDataTests
  {
    public RegCAValidateProfileDataTests()
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
    public void RegGetCAProfileData()
    {
      RegCAValidateProfileDataRequestData requestData = new RegCAValidateProfileDataRequestData(
        "832652", string.Empty, string.Empty, string.Empty, 0, "ciraRegName", "ciraProfileId");
      RegCAValidateProfileDataResponseData response = (RegCAValidateProfileDataResponseData)Engine.Engine.ProcessRequest(requestData, 133);
      Assert.IsTrue(response.Errors.Count == 0);
    }
  }
}
