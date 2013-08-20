using System.Web;
using System.Web.Caching;

namespace Atlantis.Framework.Providers.Localization.Tests.Mocks.Http
{
  public class MockHttpResponse : HttpResponseBase
  {
    private HttpCachePolicyBase _cache = new MockHttpCachePolicy();

    public MockHttpResponse(HttpResponse response = null) : base()
    {
      BaseResponse = response;
    }

    private HttpResponse BaseResponse { get; set; }

    public override void RedirectPermanent(string url)
    {
      RedirectPermanent(url, false);
    }

    public override void RedirectPermanent(string url, bool endResponse)
    {
      RedirectedToUrl = url;
    }

    public string RedirectedToUrl { get; private set; }

    public override HttpCachePolicyBase Cache
    {
      get
      {
        {
          return _cache;
        }
      }
    }
  }
}
