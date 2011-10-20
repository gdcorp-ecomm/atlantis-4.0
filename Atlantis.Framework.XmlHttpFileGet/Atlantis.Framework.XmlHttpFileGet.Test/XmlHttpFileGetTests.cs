using System.Diagnostics;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.XmlHttpFileGet.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Atlantis.Framework.XmlHttpFileGet.Test
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetXmlHttpFileGetTests
  {
    private const string ShopperId = "12530";


    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext { get; set; }

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
    public void XmlHttpFileGetTest()
    {
      MockHttpContext.SetMockHttpContext("default.aspx", "http://localhost/default.aspx", string.Empty);

      var request = new XmlHttpFileGetRequestData(ShopperId
                                                  , string.Empty
                                                  , string.Empty
                                                  , string.Empty
                                                  , 0
                                                  , "http://localhost/TestLab/media.xml");

      var response = (XmlHttpFileGetResponseData) DataCache.DataCache.GetProcessRequest(request, 310);

      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void XmlHttpFileGetCachingTest()
    {
      MockHttpContext.SetMockHttpContext("default.aspx", "http://localhost/default.aspx", string.Empty);

      var request = new XmlHttpFileGetRequestData(ShopperId
                                                  , string.Empty
                                                  , string.Empty
                                                  , string.Empty
                                                  , 0
                                                  , "http://localhost/TestLab/media.xml");

      var response = (XmlHttpFileGetResponseData)DataCache.DataCache.GetProcessRequest(request, 310);

      var request2 = new XmlHttpFileGetRequestData(ShopperId
                                                  , string.Empty
                                                  , string.Empty
                                                  , string.Empty
                                                  , 0
                                                  , "http://localhost/TestLab/media.xml");

      var response2 = (XmlHttpFileGetResponseData)DataCache.DataCache.GetProcessRequest(request2, 310);

      Assert.IsTrue(response.RetrieveDate == response2.RetrieveDate);
      Assert.IsTrue(response.IsSuccess && response2.IsSuccess);
    }
  }
}