using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Providers.Split
{
  internal class PCSplitValue : SplitValueBase
  {
    internal PCSplitValue(ISiteContext siteContext)
      : base(siteContext)
    { }

    protected override int MinValue
    {
      get { return 1; }
    }

    protected override int MaxValue
    {
      get { return 4; }
    }

    protected override string CookieNameFormat
    {
      get { return "PCSplitValue{0}"; }
    }
  }
}
