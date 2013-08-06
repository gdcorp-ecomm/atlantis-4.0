using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Localization.Interface
{
  public class CountrySiteMarketMappingsRequestData : RequestData
  {
    public CountrySiteMarketMappingsRequestData(string countrySiteId)
    {
      CountrySite = countrySiteId;
    }

    public string CountrySite { get; private set; }

    public override string GetCacheMD5()
    {
      return BuildHashFromStrings(CountrySite);
    }
  }
}
