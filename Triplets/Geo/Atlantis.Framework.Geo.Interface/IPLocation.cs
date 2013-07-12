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

    private string _countryCode;
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

    public string Region { get; set; }
    public string City { get; set; }
    public string PostalCode { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int MetroCode { get; set; }

    public string RegionName { get; set; }
  }
}
