using System.Diagnostics;
using Atlantis.Framework.PhotoGetPhotoPageEx.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.PhotoGetPhotoPageEx.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class PhotoGetPhotoPageExTests
  {
    private const string _shopperId = "842749";
    private int _galleryId = 129292;
    private int _photosPerPage = 50;
    private int _pageNumber = 1;
    private const string _domain = "gallery.ourdesignexamples.com";
    private const int _engineRequest = 228;

    public PhotoGetPhotoPageExTests()
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
      PhotoGetPhotoPageExRequestData _request = new PhotoGetPhotoPageExRequestData(_shopperId, string.Empty, string.Empty, string.Empty, 0,
        _galleryId, _photosPerPage, _pageNumber, _domain);

      PhotoGetPhotoPageExResponseData _response = (PhotoGetPhotoPageExResponseData)Engine.Engine.ProcessRequest(_request, _engineRequest);

      int count = _response.TotalPhotos;

      _response = (PhotoGetPhotoPageExResponseData)DataCache.DataCache.GetProcessRequest(_request, _engineRequest);
      count = _response.TotalPages;

      foreach (PhotoItem photo in _response.PhotoList)
      {
        Debug.WriteLine("**********************");
        Debug.WriteLine(string.Format("PhotoId :: {0}", photo.PhotoId));
        Debug.WriteLine(string.Format("Title :: {0}", photo.Title));
      }

      Assert.IsTrue(_response.IsSuccess);
    }
  }

}