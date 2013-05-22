namespace Atlantis.Framework.Providers.Geo
{
  public static class GeoProviderEngineRequests
  {
    public static int Countries { get; set; }
    public static int Regions { get; set; }
    public static int IPCountryLookup { get; set; }

    static GeoProviderEngineRequests()
    {
      Countries = 664;
      Regions = 666;
      IPCountryLookup = 697;
    }
  }
}
