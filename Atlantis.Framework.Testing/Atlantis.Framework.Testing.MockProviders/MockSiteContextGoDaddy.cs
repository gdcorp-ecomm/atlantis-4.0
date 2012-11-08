using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Testing.MockProviders
{
  public class MockSiteContextGoDaddy : MockSiteContextBase
  {
    public MockSiteContextGoDaddy(IProviderContainer container)
      : base(container)
    {
    }

    public override int ContextId
    {
      get { return 1; }
    }

    public override string StyleId
    {
      get { return "1"; }
    }

    public override int PrivateLabelId
    {
      get { return 1; }
    }

    public override string ProgId
    {
      get { return DataCache.DataCache.GetProgID(PrivateLabelId); }
    }
  }
}
