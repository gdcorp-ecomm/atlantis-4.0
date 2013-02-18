using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Atlantis.Framework.PromoReferISCAvailCheck.Impl;
using Atlantis.Framework.PromoReferISCAvailCheck.Interface;


namespace Atlantis.Framework.PromoReferISCAvailCheck.Test
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetPromoReferISCAvailCheckTests
  {



    public GetPromoReferISCAvailCheckTests()
    {

      //reference this class so it is included in the build
      PromoReferISCAvailCheckRequest request;
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
    public void PromoReferISC_is_Available()
    {
      string _shopperId = "862794";
      string orderId = "1455533";
      string _couponCode = "testcoupon123";

      PromoReferISCAvailCheckRequestData request = new PromoReferISCAvailCheckRequestData(_shopperId
         , string.Empty
         , orderId
         , string.Empty
         , 0, _couponCode);

      int _requestType = 348;

      PromoReferISCAvailCheckResponseData response = (PromoReferISCAvailCheckResponseData)Engine.Engine.ProcessRequest(request, _requestType);


      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
      Assert.IsTrue(response.IsPromoAvailable);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void PromoReferISC_is_not_Available()
    {
      string _shopperId = "859012";
      string orderId = "1452305";
      string _couponCode = "CJN";

      PromoReferISCAvailCheckRequestData request = new PromoReferISCAvailCheckRequestData(_shopperId
         , string.Empty
         , orderId
         , string.Empty
         , 0, _couponCode);

      int _requestType = 348;

      PromoReferISCAvailCheckResponseData response = (PromoReferISCAvailCheckResponseData)Engine.Engine.ProcessRequest(request, _requestType);


      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
      Assert.IsFalse(response.IsPromoAvailable);
    }


    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void PromoReferISC_Blank_Coupon()
    {
      string _shopperId = "859012";
      string orderId = "1452305";
      string _couponCode = string.Empty;

      PromoReferISCAvailCheckRequestData request = new PromoReferISCAvailCheckRequestData(_shopperId
         , string.Empty
         , orderId
         , string.Empty
         , 0, _couponCode);

      int _requestType = 348;

      PromoReferISCAvailCheckResponseData response = (PromoReferISCAvailCheckResponseData)Engine.Engine.ProcessRequest(request, _requestType);


      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
      Assert.IsFalse(response.IsPromoAvailable);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void PromoReferISC_Null_Coupon()
    {
      string _shopperId = "859012";
      string orderId = "1452305";
      string _couponCode = null;

      PromoReferISCAvailCheckRequestData request = new PromoReferISCAvailCheckRequestData(_shopperId
         , string.Empty
         , orderId
         , string.Empty
         , 0, _couponCode);

      int _requestType = 348;

      PromoReferISCAvailCheckResponseData response = (PromoReferISCAvailCheckResponseData)Engine.Engine.ProcessRequest(request, _requestType);


      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
      Assert.IsFalse(response.IsPromoAvailable);
    }
  }
}
