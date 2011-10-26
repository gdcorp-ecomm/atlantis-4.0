using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Atlantis.Framework.MYAResellerUpgrades.Impl;
using Atlantis.Framework.MYAResellerUpgrades.Interface;


namespace Atlantis.Framework.MYAResellerUpgrades.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetMYAResellerUpgradesTests
  {
    private const string _shopperId = "856907";
    private const int _resellerUpgradeRequestType = 144;

    public GetMYAResellerUpgradesTests()
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
    public void MYAResellerUpgradesTest()
    {
      const int billingResourceId = 272225;
      MYAResellerUpgradesRequestData request = new MYAResellerUpgradesRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , billingResourceId);

      MYAResellerUpgradesResponseData response = (MYAResellerUpgradesResponseData)Engine.Engine.ProcessRequest(request, _resellerUpgradeRequestType);

      Debug.WriteLine("*********************************");
      foreach (ResellerUpgrade ru in response.ResellerUpgrades)
      {
        Debug.WriteLine(string.Format("ProductId: {0}", ru.ProductId));
        Debug.WriteLine(string.Format("Description: {0}", ru.Description));
        Debug.WriteLine("*********************************");
      }

      Debug.WriteLine(string.Empty);
      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
