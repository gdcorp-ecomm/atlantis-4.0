using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Atlantis.Framework.EcommOrderItemCommonName.Impl;
using Atlantis.Framework.EcommOrderItemCommonName.Interface;


namespace Atlantis.Framework.EcommOrderItemCommonName.Tests
{
  [TestClass]
  public class GetEcommOrderItemCommonNameTests
  {
    private const string _shopperId = "856907";
    private const int _requestType = 613;
  
  
    public GetEcommOrderItemCommonNameTests()
    { }

    private TestContext testContextInstance;

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
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    public void EcommOrderItemCommonNameItemFoundTest()
    {
      EcommOrderItemCommonNameRequestData request = new EcommOrderItemCommonNameRequestData(_shopperId
        , string.Empty
        , "1464714"
        , string.Empty
        , 0
        , 14
        , 2);

      EcommOrderItemCommonNameResponseData response = (EcommOrderItemCommonNameResponseData)Engine.Engine.ProcessRequest(request, _requestType);
      
      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    public void EcommOrderItemCommonNameItemNotFoundTest()
    {
      EcommOrderItemCommonNameRequestData request = new EcommOrderItemCommonNameRequestData(_shopperId
        , string.Empty
        , "1464714"
        , string.Empty
        , 0
        , 14
        , 0);

      EcommOrderItemCommonNameResponseData response = (EcommOrderItemCommonNameResponseData)Engine.Engine.ProcessRequest(request, _requestType);

      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    public void EcommOrderItemCommonNameProcNotFoundTest()
    {
      EcommOrderItemCommonNameRequestData request = new EcommOrderItemCommonNameRequestData(_shopperId
        , string.Empty
        , "1464714"
        , string.Empty
        , 0
        , 999
        , 2);

      EcommOrderItemCommonNameResponseData response = (EcommOrderItemCommonNameResponseData)Engine.Engine.ProcessRequest(request, _requestType);

      Debug.WriteLine(response.ToXML());
      Assert.IsFalse(response.IsSuccess);
    }
  }
}
