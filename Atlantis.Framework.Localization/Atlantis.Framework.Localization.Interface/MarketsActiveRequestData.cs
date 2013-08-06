using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Localization.Interface
{
  public class MarketsActiveRequestData : RequestData
  {
    public override string GetCacheMD5()
    {
      return "MARKETS_ACTIVE";
    }
  }
}
