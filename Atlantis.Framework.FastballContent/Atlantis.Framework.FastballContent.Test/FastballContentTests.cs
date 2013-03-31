using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Atlantis.Framework.FastballContent.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.Currency;
using Atlantis.Framework.Providers.Interface.Links;
using Atlantis.Framework.Providers.Links;

namespace Atlantis.Framework.FastballContent.Test
{
  [TestClass]
  [DeploymentItem("Interop.gdMiniEncryptLib.dll")]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.FastballContent.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.LinkInfo.Impl.dll")]
  public class GetFastballContentTests
  {

    private const string _shopperId = "";


    public GetFastballContentTests()
    {
      /// BB46XQSC1
      /// V2NCSB2
      /// sslxas100
      /// gd4915d
      MockHttpContext.SetMockHttpContext("default.aspx", "http://localhost/default.aspx&isc=gd4915d", "isc=gd4915d");
      HttpProviderContainer.Instance.RegisterProvider<ISiteContext, TestContexts>();
      HttpProviderContainer.Instance.RegisterProvider<IShopperContext, TestContexts>();
      HttpProviderContainer.Instance.RegisterProvider<IManagerContext, TestContexts>();
      HttpProviderContainer.Instance.RegisterProvider<ICurrencyProvider, MockCurrency>();
      HttpProviderContainer.Instance.RegisterProvider<ILinkProvider, LinkProvider>();
    }

    [TestMethod]
    public void FastballContentTest()
    {
      string placement = "banner-iscBannerNoFormat";

      FastballContentRequestData request = new FastballContentRequestData(
        "832652", string.Empty, string.Empty, string.Empty, 0,
        2, placement, HttpProviderContainer.Instance);

      FastballContentResponseData response = (FastballContentResponseData)Engine.Engine.ProcessRequest(request, 359);
      Debug.WriteLine(response.ResponseData);
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
