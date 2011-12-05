using System.Diagnostics;
using Atlantis.Framework.PhotoGetGalleryListEx.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.PhotoGetGalleryListEx.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class PhotoGetGalleryListExTests
  {
    private const string _shopperId = "842749";
    private const string _domain = "gallery.ourdesignexamples.com";
    private const int _engineRequest = 227;

    public PhotoGetGalleryListExTests()
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
    public void WebServiceResponseTest()
    {
      PhotoGetGalleryListExRequestData _request = new PhotoGetGalleryListExRequestData(_shopperId, string.Empty, string.Empty, string.Empty, 0, _domain);
      PhotoGetGalleryListExResponseData _response = (PhotoGetGalleryListExResponseData)Engine.Engine.ProcessRequest(_request, _engineRequest);
      int count = _response.GalleryCount;

      _response = (PhotoGetGalleryListExResponseData)DataCache.DataCache.GetProcessRequest(_request, _engineRequest);
      count = _response.GalleryCount;

      foreach (GalleryItem data in _response.GalleryList)
      {
        Debug.WriteLine("**********************");
        Debug.WriteLine(string.Format("GalleryId :: {0}", data.GalleryId));
        Debug.WriteLine(string.Format("Title :: {0}", data.Title));
        Debug.WriteLine(string.Format("Notes :: {0}", data.Notes));
        Debug.WriteLine(string.Format("TotalPhotos :: {0}", data.TotalPhotos));
      }

      Assert.IsTrue(_response.IsSuccess);
    }
  }

}