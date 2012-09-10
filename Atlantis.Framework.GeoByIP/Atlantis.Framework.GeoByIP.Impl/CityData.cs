using System.Net;
using Atlantis.Framework.GeoByIP.Interface;

namespace Atlantis.Framework.GeoByIP.Impl
{
  internal class CityData : GeoDataFileBase
  {
    // File MUST be the IPv6 dat format
    public CityData(string filePath)
      : base(filePath, GEOIP_STANDARD)
    { }

    public GeoLocation GetLocation(string ipAddress)
    {
      // TODO: Handle Internal IP mappings and Known Proxy mappings

      GeoLocation result = null;

      IPAddress address;
      if (IPAddress.TryParse(ipAddress, out address))
      {
        result = GetLocationV6(address.TryConvertToIPv6());
      }

      return result;
    }
  }
}
