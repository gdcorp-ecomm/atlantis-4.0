
namespace Atlantis.Framework.Localization.Interface
{
  internal class CountrySiteMarketMapping
  {
    internal CountrySiteMarketMapping(string countrySiteId, string marketId, string languageUrlSegment, bool internalOnly)
    {
      CountrySiteId = countrySiteId;
      MarketId = marketId;
      LanguageUrlSegment = languageUrlSegment;
      IsInternalOnly = internalOnly;
    }

    public string CountrySiteId { get; private set; }

    public string MarketId { get; private set; }

    public string LanguageUrlSegment { get; private set; }

    public bool IsInternalOnly { get; private set; }

    internal bool IsFallbackMapping { get; set; }
  }
}
