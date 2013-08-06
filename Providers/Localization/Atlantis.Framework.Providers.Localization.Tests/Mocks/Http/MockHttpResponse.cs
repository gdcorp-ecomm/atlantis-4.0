using System.Web;

namespace Atlantis.Framework.Providers.Localization.Tests.Mocks.Http
{
  public class MockHttpResponse : HttpResponseBase
  {
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
  }
}
