using System;

namespace Atlantis.Framework.GeoByIP.Interface
{
  public class GeoLocation
  {
    public GeoLocation(GeoCountry country)
    {
      Country = country;
    }

    public GeoCountry Country { get; private set; }
    public String Region { get; set; }
    public String City { get; set; }
    public String PostalCode { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int DMACode { get; set; }
    public int AreaCode { get; set; }
    public String RegionName { get; set; }
    public int MetroCode { get; set; }

    private static double EARTH_DIAMETER = 2 * 6378.2;
    private static double PI = 3.14159265;
    private static double RAD_CONVERT = PI / 180;

    public double GreatCircleMiles(GeoLocation otherLocation)
    {
      if (otherLocation == null)
      {
        throw new ArgumentException("Other location cannot be null.");
      }

      double delta_lat, delta_lon;
      double temp;

      double lat1 = Latitude;
      double lon1 = Longitude;
      double lat2 = otherLocation.Latitude;
      double lon2 = otherLocation.Longitude;

      // convert degrees to radians
      lat1 *= RAD_CONVERT;
      lat2 *= RAD_CONVERT;

      // find the deltas
      delta_lat = lat2 - lat1;
      delta_lon = (lon2 - lon1) * RAD_CONVERT;

      // Find the great circle distance
      temp = Math.Pow(Math.Sin(delta_lat / 2), 2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Pow(Math.Sin(delta_lon / 2), 2);
      return EARTH_DIAMETER * Math.Atan2(Math.Sqrt(temp), Math.Sqrt(1 - temp));
    }

  }
}
