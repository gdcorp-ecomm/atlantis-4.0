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
    public TestContext TestContext { get; set; }

    private IShopperSegmentationProvider InitProvider(int privateLabelId, string shopperId)
    {
      HttpProviderContainer.Instance.RegisterProvider<ISiteContext, MockSiteContext>();
      HttpProviderContainer.Instance.RegisterProvider<IShopperContext, MockShopperContext>();
      HttpProviderContainer.Instance.RegisterProvider<IManagerContext, MockNoManagerContext>();
      HttpProviderContainer.Instance.RegisterProvider<IShopperSegmentationProvider, ShopperSegmentationProvider>();

      HttpContext.Current.Items[MockSiteContextSettings.PrivateLabelId] = privateLabelId;
      var shopperContext = HttpProviderContainer.Instance.Resolve<IShopperContext>();
      shopperContext.SetNewShopper(shopperId);

      return HttpProviderContainer.Instance.Resolve<IShopperSegmentationProvider>();
    }

    [TestMethod]
    public void GetShopperSegmentIdTest()
    {

      string[] shopperIds = new[] { "","1001206", "1001232", "1000534", "100075", "100019" };
      
      foreach (var item in shopperIds)
      {
        // Get a new request
        HttpWorkerRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
        MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
        IShopperSegmentationProvider provider = InitProvider(1, item);
        Assert.IsNotNull(provider);
        var segment = provider.GetShopperSegmentId();
        Assert.IsTrue("nacent|activebusiness|ecomm|webpro|domainer".Contains(segment));
        Assert.IsFalse(string.IsNullOrEmpty(segment));
        this.TestContext.WriteLine("shopperId: {0}, segmentId: {1}", item, provider.GetShopperSegmentId());
      }

      shopperIds = new[] { "1001708", "100147", "100090", "1001909" };
      foreach (var item in shopperIds)
      {
        // Get a new request
        HttpWorkerRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
        MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
        IShopperSegmentationProvider provider = InitProvider(1, item);
        this.TestContext.WriteLine("shopperId: {0}, segmentId: {1}", item, provider.GetShopperSegmentId());
      }

      shopperIds = new[] { "1001010", "100101", "100102", "100103", "100104", "100105", "100106", "100107", "100108", "100109", "100110", "100111", "100112", "100113", "100114", "100115", "100116", "100117", "100118", "100119", "100120", "100121", "100122", "100123", "100124", "100125", "100126", "100127", "100128", "100129", "100130", "100131", "100132", "100133", "100134", "100135", "100136", "100137", "100138", "100139", "100140", "100141", "100142", "100144", "100145", "100146", "100147", "100148", "100149", "100150", "100151", "100152", "100153", "100154", "100155", "100156", "100157", "100158", "100159", "100160", "100161", "100162", "100163", "100164", "100165"};
      foreach (var item in shopperIds)
      {
        // Get a new request
        HttpWorkerRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
        MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
        IShopperSegmentationProvider provider = InitProvider(1, item);
        this.TestContext.WriteLine("shopperId: {0}, segmentId: {1}", item, provider.GetShopperSegmentId());
      }

    }


  }
}
