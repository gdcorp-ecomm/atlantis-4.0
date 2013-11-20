using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Localization.Interface
{
  public class MarketMappingsRequestData : RequestData
  {
      public MarketMappingsRequestData(string marketId)
      {
        MarketId = marketId;
      }

      public string MarketId { get; private set; }

      public override string GetCacheMD5()
      {
        return MarketId.ToLowerInvariant();
      }
  }
}
