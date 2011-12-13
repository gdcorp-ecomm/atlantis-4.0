using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Atlantis.Framework.EcommPaymentProfileUnique.Impl;
using Atlantis.Framework.EcommPaymentProfileUnique.Interface;


namespace Atlantis.Framework.EcommPaymentProfileUnique.Test
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetEcommPaymentProfileUniqueTests
  {
  
    
	
	
    public GetEcommPaymentProfileUniqueTests()
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
    public void EcommPaymentProfileUniqueTest()
    {
        string _shopperId = "862794";
        string _orderId = "1455533";
        EcommPaymentProfileUniqueRequestData request = new EcommPaymentProfileUniqueRequestData(_shopperId
            , "https://cart.dev.godaddy-com.ide/basket.aspx"
            , _orderId
            , string.Empty
            , 1 );
        request.RequestTimeout = System.TimeSpan.FromSeconds(20);

     EcommPaymentProfileUniqueResponseData response = (EcommPaymentProfileUniqueResponseData)Engine.Engine.ProcessRequest(request, 345);
      
      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
