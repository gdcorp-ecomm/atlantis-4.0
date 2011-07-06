
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BasePages.Providers
{
  public class GoDaddyOnlySiteContextProvider : SiteContextProviderBase
  {
    public GoDaddyOnlySiteContextProvider(IProviderContainer providerContainer) : base(providerContainer)
    {
    }

    public override int ContextId
    {
      get { return ContextIds.GoDaddy; }
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
      get { return string.Empty; }
    }
  }
}
