using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Providers.Split
{
  internal class StandardSplitValue : SplitValueBase
  {
    internal StandardSplitValue(ISiteContext siteContext)
      : base(siteContext)
    { }

    protected override int MinValue
    {
      get { return 1; }
    }

    protected override int MaxValue
    {
      get { return 100; }
    }

    protected override string CookieNameFormat
    {
      get { return "SplitValue{0}"; }
    }
  }
}
