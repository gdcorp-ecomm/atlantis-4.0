using System.Web;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Containers;
using Atlantis.Framework.Providers.Segmentation;
using Atlantis.Framework.Providers.Segmentation.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.ShopperSegment.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Segmentation.Impl.dll")]
  public class ShopperSegmentProviderTests
  {
    private ISegmentationProvider InitProvider(int privateLabelId, string shopperId)
    {
      HttpProviderContainer.Instance.RegisterProvider<ISiteContext, MockSiteContext>();
      HttpProviderContainer.Instance.RegisterProvider<IShopperContext, MockShopperContext>();
      HttpProviderContainer.Instance.RegisterProvider<IManagerContext, MockNoManagerContext>();
      HttpProviderContainer.Instance.RegisterProvider<ISegmentationProvider, SegmentationProvider>();

      HttpContext.Current.Items[MockSiteContextSettings.PrivateLabelId] = privateLabelId;
      var shopperContext = HttpProviderContainer.Instance.Resolve<IShopperContext>();
      shopperContext.SetNewShopper(shopperId);

      return HttpProviderContainer.Instance.Resolve<ISegmentationProvider>();
    }

    [TestMethod]
    public void GetShopperSegmentIdTest()
    {
      // Get a new request
      HttpWorkerRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      string[] shopperIds = new[] { "1001206", "1001232", "1000534", "100075", "100019", "1001708", "1001909", "100147", "100090" };
      
      foreach (var item in shopperIds)
      {
        ISegmentationProvider provider = InitProvider(1, item);
        Assert.IsNotNull(provider);
        Assert.AreNotEqual(0, provider.GetShopperSegmentId());
      }

      //shopperIds = new[] { "100090" };

      //foreach (var item in shopperIds)
      //{
      //  ISegmentationProvider provider = InitProvider(1, item);
      //  Assert.IsNotNull(provider);
      //  Assert.AreEqual(0, provider.GetShopperSegmentId());
      //}
    }


  }
}
