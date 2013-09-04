using Atlantis.Framework.Geo.Interface;

namespace Atlantis.Framework.Providers.Geo
{
  public static class GeoProviderEngineRequests
  {
    public static int Countries { get; set; }
    public static int Regions { get; set; }
    public static int IPCountryLookup { get; set; }
    public static int IPLocationLookup { get; set; }
    public static int States { get; set; }
    public static int CountryNames { get; set; }
    public static int StateNames { get; set; }

    static GeoProviderEngineRequests()
    {
      Countries = 664;
      Regions = 666;
      IPCountryLookup = 697;
      IPLocationLookup = 719;
      States = 665;
      CountryNames = 722;
      StateNames = 723;
    }
  }
}
