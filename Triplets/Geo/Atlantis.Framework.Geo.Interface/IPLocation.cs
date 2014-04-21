namespace Atlantis.Framework.Geo.Interface
{
  public class IPLocation
  {
    const string _notFoundCode = "--";
    public static IPLocation Unknown { get; private set; }

    static IPLocation()
    {
      Unknown = new IPLocation();
      Unknown.CountryCode = _notFoundCode;
      Unknown.City = string.Empty;
      Unknown.Latitude = 0;
      Unknown.Longitude = 0;
      Unknown.MetroCode = 0;
      Unknown.PostalCode = string.Empty;
      Unknown.RegionName = string.Empty;
    }

    private string _countryCode = string.Empty;
    public string CountryCode
    {
      get { return _countryCode; }
      set 
      {
        if (value != null)
        {
          value = value.ToLowerInvariant();
        }
        _countryCode = value;
      }
    }

    private string _region = string.Empty;
    public string Region
    {
      get { return _region; }
      set { _region = value; }
    }

    private string _city = string.Empty;
    public string City
    {
      get { return _city; }
      set { _city = value; }
    }

    private string _postalCode = string.Empty;
    public string PostalCode
    {
      get { return _postalCode; }
      set { _postalCode = value; }
    }

    private double _latitude = 0d;
    public double Latitude
    {
      get { return _latitude; }
      set { _latitude = value; }
    }

    private double _longitude = 0d;
    public double Longitude
    {
      get { return _longitude; }
      set { _longitude = value; }
    }

    private int _metroCode = 0;
    public int MetroCode
    {
      get { return _metroCode; }
      set { _metroCode = value; }
    }

    private string _regionName = string.Empty;

    public string RegionName
    {
      get { return _regionName; }
      set { _regionName = value; }
    }
  }
}
