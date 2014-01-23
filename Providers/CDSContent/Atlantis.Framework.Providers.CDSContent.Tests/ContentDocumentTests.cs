using System;
using Atlantis.Framework.CDS.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Containers;
using Atlantis.Framework.Providers.Interface.Links;
using Atlantis.Framework.Providers.RenderPipeline.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Atlantis.Framework.Providers.CDSContent.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.CDS.Impl.dll")]
  public class ContentDocumentTests
  {
    private IProviderContainer _objectProviderContainer;
    private IProviderContainer ObjectProviderContainer
    {
      get { return _objectProviderContainer ?? (_objectProviderContainer = new ObjectProviderContainer()); }
    }

    private void RegisterProviders()
    {
      ObjectProviderContainer.RegisterProvider<ISiteContext, MockSiteContext>();
      ObjectProviderContainer.RegisterProvider<IShopperContext, MockShopperContext>();
      ObjectProviderContainer.RegisterProvider<IManagerContext, MockManagerContext>();
      ObjectProviderContainer.RegisterProvider<IDebugContext, MockDebugContext>();
    }

    private void SetupHttpContext()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
    }

    private void ApplicationStart()
    {
      SetupHttpContext();
      RegisterProviders();
    }

    [TestInitialize]
    public void Initialize()
    {
      ApplicationStart();
    }

    [TestMethod]
    public void CDT_PublishedDocNotFound()
    {
      string rawPath = "content/blah blah/hosting/email-hosting";
      ContentDocument contentDoc = new ContentDocument(ObjectProviderContainer, rawPath);
      Assert.IsTrue(contentDoc.GetContent() == ContentDocument.NullRenderContent);
    }

    [TestMethod]
    public void CDT_PublishedDocFound()
    {
      string rawPath = "content/sales/unittest/defaultcontentpath_getcontenttests";
      ContentDocument contentDoc = new ContentDocument(ObjectProviderContainer, rawPath);
      IRenderContent renderContent = contentDoc.GetContent();
      Assert.IsTrue(renderContent.Content.Contains("Current DataCenter:"));
    }

    [TestMethod]
    public void AddDebugInfoToDubgContext_ValidData()
    {
      // Arrange
      var fakeUrl = "http://www.fakeurl.com/";
      var docId = new ContentId { oid = "12345" };
      var verId = new ContentId { oid = "testVersionId" };
      var relativeUrl = string.Format(CDSDocument.CDSM_CONTENT_RELATIVE_PATH, docId.oid, verId.oid);
      var resultHtml = string.Format("<a href='{0}' target='_blank'>{0}</a>", fakeUrl + relativeUrl);
      var debugInfoKey = "DebugKey";

      IDebugContext mockedDebugProider;
      Mock<IProviderContainer> mockedProviderContainer;
      Mock<ICDSDebugInfo> mockedCdsDebugInfo;

      SetupDebugInfoMocks(out mockedDebugProider, out mockedProviderContainer, out mockedCdsDebugInfo, docId, verId, fakeUrl, debugInfoKey);
      
      // Act
      var mockedWhiteListDoc  = new WhitelistDocument(mockedProviderContainer.Object, string.Empty);
      var mockedRulesDoc      = new RulesDocument(mockedProviderContainer.Object, string.Empty, string.Empty);
      var mockedContentDoc    = new ContentDocument(mockedProviderContainer.Object, string.Empty);

      mockedWhiteListDoc.LogCDSDebugInfo(mockedCdsDebugInfo.Object);
      mockedRulesDoc.LogCDSDebugInfo(mockedCdsDebugInfo.Object);
      mockedContentDoc.LogCDSDebugInfo(mockedCdsDebugInfo.Object);

      // Assert
      var trackingData = mockedDebugProider.GetDebugTrackingData();
      Assert.IsNotNull(trackingData);
      Assert.AreEqual(3, trackingData.Count);
      Assert.AreEqual(resultHtml, trackingData[0].Value);
      Assert.AreEqual(resultHtml, trackingData[1].Value);
      Assert.AreEqual(resultHtml, trackingData[2].Value);
      Assert.IsTrue(trackingData[0].Key.Contains(debugInfoKey));
      Assert.IsTrue(trackingData[1].Key.Contains(debugInfoKey));
      Assert.IsTrue(trackingData[2].Key.Contains(debugInfoKey));
    }

    [TestMethod]
    public void AddDebugInfoToDubgContext_InvalidData()
    {
      // Arrange
      IDebugContext mockedDebugProider;
      Mock<IProviderContainer> mockedProviderContainer;
      Mock<ICDSDebugInfo> mockedCdsDebugInfo;

      SetupDebugInfoMocks(out mockedDebugProider, out mockedProviderContainer, out mockedCdsDebugInfo, null, null, string.Empty, string.Empty);

      // Act
      var mockedWhiteListDoc = new WhitelistDocument(mockedProviderContainer.Object, string.Empty);
      mockedWhiteListDoc.LogCDSDebugInfo(mockedCdsDebugInfo.Object);

      // Assert
      var trackingData = mockedDebugProider.GetDebugTrackingData();
      Assert.IsNotNull(trackingData);
      Assert.AreEqual(0, trackingData.Count);
    }

    private void SetupDebugInfoMocks(out IDebugContext mockedDebugProider, out Mock<IProviderContainer> mockedProviderContainer,
                                      out Mock<ICDSDebugInfo> mockedCdsDebugInfo, ContentId docId, ContentId verId, string siteAdminUrl, string debugInfoKey)
    {
      var mockedLinkProvider = new Mock<ILinkProvider>();
      mockedDebugProider = ObjectProviderContainer.Resolve<IDebugContext>();
      mockedCdsDebugInfo = new Mock<ICDSDebugInfo>();
      var mockedSiteContext = new Mock<ISiteContext>();
      mockedProviderContainer = new Mock<IProviderContainer>();

      var debugProvider = mockedDebugProider;
      var linkProvider = mockedLinkProvider.Object;

      mockedSiteContext.Setup(m => m.IsRequestInternal).Returns(true);
      mockedLinkProvider.Setup(m => m.GetUrl(CDSDocument.SITE_ADMIN_URL_KEY, null)).Returns(siteAdminUrl);

      mockedProviderContainer.Setup(m => m.Resolve<ISiteContext>()).Returns(mockedSiteContext.Object);
      mockedProviderContainer.Setup(m => m.TryResolve<IDebugContext>(out debugProvider)).Returns(true);
      mockedProviderContainer.Setup(m => m.TryResolve<ILinkProvider>(out linkProvider)).Returns(true);
      mockedCdsDebugInfo.Setup(m => m.DocumentId).Returns(docId);
      mockedCdsDebugInfo.Setup(m => m.VersionId).Returns(verId);
      mockedCdsDebugInfo.Setup(m => m.DebugKey).Returns(debugInfoKey);
    }

  }
}
