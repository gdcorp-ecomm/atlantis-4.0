using System.Collections.Generic;
using System.Diagnostics;
using Atlantis.Framework.Personalization.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Personalization.Test
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetTargetedMessageTests
  {
  
    private const string _shopperId = "";


    public GetTargetedMessageTests()
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
    [DeploymentItem("Atlantis.Framework.Personalization.Impl.dll")]
    public void GetTargetedMessagesTest()
    {
      string appId = "2";
      string interactionPoint = "ProductUpsell";

      Dictionary<string, string> contextData = new Dictionary<string, string>();
      contextData.Add("ClientIP", "127.0.0.1");
      contextData.Add("TransactionalCurrency", "USD");
      contextData.Add("DataCenter", "US");

      Dictionary<string, string> shopperData = new Dictionary<string, string>();
      shopperData.Add("PrivateLabelID", "1");
      shopperData.Add("ShopperID", "12345");
      shopperData.Add("IsShopperAuthenticated", "1");

      Dictionary<string, string> spoofData = new Dictionary<string, string>();
      Dictionary<string, string> interactionData = new Dictionary<string, string>();


      TargetedMessagesRequestData request = new TargetedMessagesRequestData(_shopperId, string.Empty, string.Empty, string.Empty, 0, appId, interactionPoint, contextData, shopperData);

      TargetedMessagesResponseData response = (TargetedMessagesResponseData)Engine.Engine.ProcessRequest(request, 669);

      var md5Cache = request.GetCacheMD5();
      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
