namespace Atlantis.Framework.GeoByIP.Interface
{
  public static class DataFiles
  {
    static string _cityFile = "GeoIPv6.dat";
    static string _countryFile = "GeoLiteCityv6.dat";
    static DataFilePathTypeEnum _pathType = DataFilePathTypeEnum.AssemblyLocation;

    public static string CityFile
    {
      get { return _cityFile; }
      set { _cityFile = value; }
    }

    public static string CountryFile
    {
      get { return _countryFile; }
      set { _countryFile = value; }
    }

    public static DataFilePathTypeEnum PathType
    {
      get { return _pathType; }
      set { _pathType = value; }
    }
  }
}
