using System;
using System.Diagnostics;
using Atlantis.Framework.CommerceOrderXml.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Atlantis.Framework.CommerceOrderXml.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetCommerceOrderXmlTests
  {
    private const string _shopperId = "856907";
    private const int _orderDetailRequestType = 146;

    public GetCommerceOrderXmlTests()
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
    [DeploymentItem("Atlantis.Framework.CommerceOrderXml.Impl.dll")]
    public void CommerceOrderXmlTest()
    {
      string recentOrderId = "1467535";
      CommerceOrderXmlRequestData request = new CommerceOrderXmlRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , recentOrderId);

      request.RequestTimeout = TimeSpan.FromSeconds(10);

      CommerceOrderXmlResponseData response = (CommerceOrderXmlResponseData)Engine.Engine.ProcessRequest(request, _orderDetailRequestType);

      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
