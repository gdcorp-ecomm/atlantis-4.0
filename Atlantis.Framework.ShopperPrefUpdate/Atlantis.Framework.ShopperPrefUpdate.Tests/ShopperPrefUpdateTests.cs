using System.Diagnostics;
using Atlantis.Framework.ShopperPrefUpdate.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.ShopperPrefUpdate.Test
{
  /// <summary>
  /// Summary description for GetShopperPrefUpdateTests
  /// </summary>
  [TestClass]
  public class GetShopperPrefUpdateTests
  {

    private const string _shopperId = "12345";
    private const int _requestType = 271;


    public GetShopperPrefUpdateTests()
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
    public void ShopperPrefUpdateTest()
    {
      ShopperPrefUpdateRequestData request = new ShopperPrefUpdateRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0);

      request.AddPreference("gdshop_currencyType", "GBP");
      request.AddPreference("countryFlag", null);

      ShopperPrefUpdateResponseData response = (ShopperPrefUpdateResponseData)Engine.Engine.ProcessRequest(request, _requestType);

      Debug.WriteLine(response.ToXML());
      // this isn't working as expected, was hoping to get the records affected...
      // this proc has SET NOCOUNT ON
      //Assert.AreEqual(1, response.ResultCode);
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [ExpectedException(typeof(Atlantis.Framework.Interface.AtlantisException))]
    public void ShopperPrefUpdateInvalidPrefTest()
    {
      ShopperPrefUpdateRequestData request = new ShopperPrefUpdateRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0);

      request.AddPreference("invalid_pref", "bogus");
      try
      {
        ShopperPrefUpdateResponseData response = (ShopperPrefUpdateResponseData)Engine.Engine.ProcessRequest(request, _requestType);
        Assert.IsFalse(response.IsSuccess);
      }
      catch (Atlantis.Framework.Interface.AtlantisException ex)
      {
        Assert.AreEqual("@invalid_pref is not a parameter for procedure gdshop_shopperPreferenceUpdate_sp.", ex.Message);
        throw;
      }
    }
  }
}
