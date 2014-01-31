using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Atlantis.Framework.EcommFreeProduct.Impl;
using Atlantis.Framework.EcommFreeProduct.Interface;
using Atlantis.Framework.Testing.MockHttpContext;


namespace Atlantis.Framework.RegFreeProduct.Test
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetRegFreeProductTests
  {

    private const string _shopperId = "840420";


    public GetRegFreeProductTests()
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

    //[TestMethod]
    //[DeploymentItem("atlantis.config")]
    //[DeploymentItem("Atlantis.Framework.EcommFreeProduct.Impl.dll")]
    //public void RegFreeProductTest()
    //{
    //  MockHttpContext.SetMockHttpContext(string.Empty, "http://localhost", string.Empty);

    //  var request = new RegisterFreeProductRequestData(_shopperId, string.Empty);
    //  request.AddItem("2701", "1");

    //  var _requestType = 679;
    //  var response = (RegisterFreeProductResponseData)Engine.Engine.ProcessRequest(request, _requestType);

    //  Debug.WriteLine(response.ToXML());
    //  Assert.IsTrue(response.IsSuccess);
    //}

    //[TestMethod]
    //[DeploymentItem("atlantis.config")]
    //[DeploymentItem("Atlantis.Framework.EcommFreeProduct.Impl.dll")]
    //public void RegFreeTrialProductTest()
    //{
    //  MockHttpContext.SetMockHttpContext(string.Empty, "http://localhost", string.Empty);

    //  var request = new RegisterFreeProductRequestData(_shopperId, string.Empty);
    //  request.AddItem("2701", "1", "58071");

    //  var _requestType = 679;
    //  var response = (RegisterFreeProductResponseData)Engine.Engine.ProcessRequest(request, _requestType);

    //  Debug.WriteLine(response.ToXML());
    //  Assert.IsTrue(response.IsSuccess);
    //}


    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.EcommFreeProduct.Impl.dll")]
    public void RegFreeTrialProductXMLTest()
    {
      MockHttpContext.SetMockHttpContext(string.Empty, "http://localhost", string.Empty);

      var request = new RegisterFreeProductRequestData(_shopperId, string.Empty);
      request.AddItem("2701", "1", "58071");



      Debug.WriteLine(request.ToXML());
      Assert.IsTrue(true);
    }

  }
}
