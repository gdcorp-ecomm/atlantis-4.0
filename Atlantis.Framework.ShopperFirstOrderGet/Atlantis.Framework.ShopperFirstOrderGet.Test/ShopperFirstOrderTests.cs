using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Atlantis.Framework.ShopperFirstOrderGet.Impl;
using Atlantis.Framework.ShopperFirstOrderGet.Interface;


namespace Atlantis.Framework.ShopperFirstOrderGet.Test
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetShopperFirstOrderGetTests
  {
  
    
	
    public GetShopperFirstOrderGetTests()
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
    public void ShopperFirstOrderGetTest()
    {
        //string _shopperId = "859012";
        //string orderId = "1452305";
        string _shopperId = "862794";
        string orderId = "1455533";
	
     ShopperFirstOrderGetRequestData request = new ShopperFirstOrderGetRequestData(_shopperId
        , "http://cart.dev.godaddy-com.ide/basket.aspx"
        , orderId
        , string.Empty
        , 0 );

        int _requestType = 348;

      ShopperFirstOrderGetResponseData response = (ShopperFirstOrderGetResponseData)Engine.Engine.ProcessRequest(request, _requestType);
      
	 
      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void ShopperFirstOrderGetTest2()
    {
        string _shopperId = "859012";
        string orderId = "1452305";

        ShopperFirstOrderGetRequestData request = new ShopperFirstOrderGetRequestData(_shopperId
           , "http://cart.dev.godaddy-com.ide/basket.aspx"
           , orderId
           , string.Empty
           , 0);

        int _requestType = 348;

        ShopperFirstOrderGetResponseData response = (ShopperFirstOrderGetResponseData)Engine.Engine.ProcessRequest(request, _requestType);


        Debug.WriteLine(response.ToXML());
        Assert.IsTrue(response.IsSuccess);
    }
  }
}
