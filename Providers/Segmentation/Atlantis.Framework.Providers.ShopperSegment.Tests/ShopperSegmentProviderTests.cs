using System.Web;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Containers;
using Atlantis.Framework.Providers.ShopperSegment.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.ShopperSegment.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.ShopperSegment.Impl.dll")]
  public class ShopperSegmentProviderTests
  {
    private IShopperSegmentProvider InitProvider(int privateLabelId, string shopperId)
    {
      HttpProviderContainer.Instance.RegisterProvider<ISiteContext, MockSiteContext>();
      HttpProviderContainer.Instance.RegisterProvider<IShopperContext, MockShopperContext>();
      HttpProviderContainer.Instance.RegisterProvider<IManagerContext, MockNoManagerContext>();
      HttpProviderContainer.Instance.RegisterProvider<IShopperSegmentProvider, ShopperSegmentProvider>();

      HttpContext.Current.Items[MockSiteContextSettings.PrivateLabelId] = privateLabelId;
      var shopperContext = HttpProviderContainer.Instance.Resolve<IShopperContext>();
      shopperContext.SetNewShopper(shopperId);

      return HttpProviderContainer.Instance.Resolve<IShopperSegmentProvider>();
    }

    [TestMethod]
    public void GetShopperSegmentIdTest()
    {
      // Get a new request
      HttpWorkerRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      string[] shopperIds = new[] { "1001206", "1001232", "1000534", "100075", "100019", "100147" };
      
      foreach (var item in shopperIds)
      {
        IShopperSegmentProvider provider = InitProvider(1, item);
        Assert.IsNotNull(provider);
        Assert.AreNotEqual(0, provider.GetShopperSegmentId());
      }

      shopperIds = new[] { "1001708", "1001909", "100090" };

      foreach (var item in shopperIds)
      {
        IShopperSegmentProvider provider = InitProvider(1, item);
        Assert.IsNotNull(provider);
        Assert.AreEqual(0, provider.GetShopperSegmentId());
      }
    }


  }
}
