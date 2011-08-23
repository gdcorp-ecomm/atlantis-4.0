using Atlantis.Framework.SearchShoppers.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.SearchShoppers.Tests
{
  /// <summary>
  /// Summary description for SearchShoppersTest
  /// </summary>
  [TestClass]
  public class SearchShoppersTest
  {
    public SearchShoppersTest()
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
    public void SearchByEmail()
    {
      SearchShoppersRequestData requestData = new SearchShoppersRequestData(
        "850774", string.Empty, string.Empty, string.Empty, 0, "172.19.72.107");
      requestData.IPAddress = "172.19.72.107";
      requestData.AddSearchField("email", "sthota@godaddy.com");
      requestData.AddSearchField("privateLabelID", "1");
      requestData.AddReturnField("mktg_email");
      requestData.AddReturnField("mktg_mail");
      requestData.AddReturnField("mktg_nonpromotional_notices");

      SearchShoppersResponseData response = (SearchShoppersResponseData)Engine.Engine.ProcessRequest(requestData, 2);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void SearchByLogin()
    {
      SearchShoppersRequestData requestData = new SearchShoppersRequestData(
        string.Empty, string.Empty, string.Empty, string.Empty, 0, "SearchShopperTest");
      requestData.AddSearchField("loginName", "hunter");
      requestData.AddReturnField("shopper_id");

      SearchShoppersResponseData response = (SearchShoppersResponseData)Engine.Engine.ProcessRequest(requestData, 2);
      Assert.IsNotNull(response.ToXML());
    }
  }
}
