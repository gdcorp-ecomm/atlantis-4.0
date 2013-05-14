using System;
using System.Web;
using Atlantis.Framework.Segmentation.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.ShopperSegment.Test
{
  [TestClass]
  public class RequestShopperSegmentTests
  {
    private const int REQUEST_TYPE_ID = 686;
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

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.Segmentation.Impl.dll")]
    public void GetShopperSegmentTest()
    {
      ShopperSegmentRequestData request = new ShopperSegmentRequestData("1001206", string.Empty, string.Empty, string.Empty, 0);
      ShopperSegmentResponseData actual = (ShopperSegmentResponseData)Engine.Engine.ProcessRequest(request, REQUEST_TYPE_ID);
      Assert.IsTrue(actual.IsSuccess);
      Assert.AreNotEqual(0, actual.SegmentId);

      request = new ShopperSegmentRequestData("1001232", string.Empty, string.Empty, string.Empty, 0);
      actual = (ShopperSegmentResponseData)Engine.Engine.ProcessRequest(request, REQUEST_TYPE_ID);
      Assert.IsTrue(actual.IsSuccess);
      Assert.AreNotEqual(0, actual.SegmentId);

      request = new ShopperSegmentRequestData("1000534", string.Empty, string.Empty, string.Empty, 0);
      actual = (ShopperSegmentResponseData)Engine.Engine.ProcessRequest(request, REQUEST_TYPE_ID);
      Assert.IsTrue(actual.IsSuccess);
      Assert.AreNotEqual(0, actual.SegmentId);

      request = new ShopperSegmentRequestData("100075", string.Empty, string.Empty, string.Empty, 0);
      actual = (ShopperSegmentResponseData)Engine.Engine.ProcessRequest(request, REQUEST_TYPE_ID);
      Assert.IsTrue(actual.IsSuccess);
      Assert.AreNotEqual(0, actual.SegmentId);

      request = new ShopperSegmentRequestData("100019", string.Empty, string.Empty, string.Empty, 0);
      actual = (ShopperSegmentResponseData)Engine.Engine.ProcessRequest(request, REQUEST_TYPE_ID);
      Assert.IsTrue(actual.IsSuccess);
      Assert.AreNotEqual(0, actual.SegmentId);

      request = new ShopperSegmentRequestData("1001708", string.Empty, string.Empty, string.Empty, 0);
      actual = (ShopperSegmentResponseData)Engine.Engine.ProcessRequest(request, REQUEST_TYPE_ID);
      Assert.IsTrue(actual.IsSuccess);
      Assert.AreEqual(0, actual.SegmentId);

      request = new ShopperSegmentRequestData("100147", string.Empty, string.Empty, string.Empty, 0);
      actual = (ShopperSegmentResponseData)Engine.Engine.ProcessRequest(request, REQUEST_TYPE_ID);
      Assert.IsTrue(actual.IsSuccess);
      Assert.AreNotEqual(0, actual.SegmentId);

      request = new ShopperSegmentRequestData("1001909", string.Empty, string.Empty, string.Empty, 0);
      actual = (ShopperSegmentResponseData)Engine.Engine.ProcessRequest(request, REQUEST_TYPE_ID);
      Assert.IsTrue(actual.IsSuccess);
      Assert.AreEqual(0, actual.SegmentId);

      request = new ShopperSegmentRequestData("100090", string.Empty, string.Empty, string.Empty, 0);
      actual = (ShopperSegmentResponseData)Engine.Engine.ProcessRequest(request, REQUEST_TYPE_ID);
      Assert.IsTrue(actual.IsSuccess);
      Assert.AreEqual(0, actual.SegmentId);

    }

    [TestMethod]
    public void GetShopperSegmentFromSessionTest()
    {
      HttpWorkerRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      ShopperSegmentRequestData request = new ShopperSegmentRequestData("1001206", string.Empty, string.Empty, string.Empty, 0);
      ShopperSegmentResponseData actual = SessionCache.SessionCache.GetProcessRequest<ShopperSegmentResponseData>(request, REQUEST_TYPE_ID, TimeSpan.FromMinutes(5));
      Assert.AreEqual(2, actual.SegmentId);

      request = new ShopperSegmentRequestData("1001206", string.Empty, string.Empty, string.Empty, 0);
      actual = SessionCache.SessionCache.GetProcessRequest<ShopperSegmentResponseData>(request, REQUEST_TYPE_ID, TimeSpan.FromMinutes(5));
      Assert.AreEqual(2, actual.SegmentId);


    }
  }
}
