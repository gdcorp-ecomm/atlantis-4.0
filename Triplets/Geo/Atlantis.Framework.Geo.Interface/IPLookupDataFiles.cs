namespace Atlantis.Framework.Geo.Interface
{
  public static class IPLookupDataFiles
  {
    static string _locationFile = "GeoIPCity.dat";
    static string _countryFile = "GeoIP.dat";
    static IPLookupPathTypes _pathType = IPLookupPathTypes.AssemblyLocation;

    public static string CountryFile
    {
      get { return _countryFile; }
      set { _countryFile = value; }
    }

    public static string LocationFile
    {
      get { return _locationFile; }
      set { _locationFile = value; }
    }

    public static IPLookupPathTypes PathType
    {
      get { return _pathType; }
      set { _pathType = value; }
    }
  }
}
