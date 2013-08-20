using System.Web;

namespace Atlantis.Framework.Providers.Localization.Tests.Mocks.Http
{
  public class MockHttpCachePolicy : HttpCachePolicyBase
  {
    public override void SetCacheability(HttpCacheability cacheability)
    {
      // Do nothing
    }
  }
}
