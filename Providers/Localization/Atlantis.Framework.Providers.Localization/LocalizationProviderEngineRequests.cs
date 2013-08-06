namespace Atlantis.Framework.Providers.Localization
{
  public static class LocalizationProviderEngineRequests
  {
    static LocalizationProviderEngineRequests()
    {
      CountrySitesActiveRequest = 725;
      MarketsActiveRequest = 729;
      CountrySiteMarketMappingsRequest = 730;
    }

    public static int CountrySitesActiveRequest { get; set; }

    public static int MarketsActiveRequest { get; set; }

    public static int CountrySiteMarketMappingsRequest { get; set; }
  }
}
