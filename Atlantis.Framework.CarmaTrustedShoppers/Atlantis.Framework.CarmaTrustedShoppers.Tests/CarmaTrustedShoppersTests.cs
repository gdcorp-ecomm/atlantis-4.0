using System.Diagnostics;
using Atlantis.Framework.CarmaTrustedShoppers.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.CarmaTrustedShoppers.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetCarmaTrustedShoppersTests
  {

    private const string _shopperId = "856907";  // Shopper with Account Exec: 856907 | Shopper without: 857623
    private const int _requestType = 409;


    public GetCarmaTrustedShoppersTests()
    { }

    private TestContext testContextInstance;

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext
    {
      get { return testContextInstance; }
      set { testContextInstance = value; }
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
    public void CarmaTrustedShoppersTest()
    {
      CarmaTrustedShoppersRequestData request = new CarmaTrustedShoppersRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0);

      CarmaTrustedShoppersResponseData response = (CarmaTrustedShoppersResponseData)Engine.Engine.ProcessRequest(request, _requestType);

      Assert.IsTrue(response.IsSuccess);

      Debug.WriteLine(response.ToXML());
      Debug.WriteLine(string.Format("Has Trusted Shoppers: {0}", response.HasTrustedShoppers));
      if (response.HasTrustedShoppers)
      {
        foreach (TrustedShopper ts in response.TrustedShoppers)
        {
          Debug.WriteLine(string.Format("ShopperId: {0} | Name: {1} {2}", ts.ShopperId, ts.FirstName, ts.LastName));
        }
      }
    }
  }
}
