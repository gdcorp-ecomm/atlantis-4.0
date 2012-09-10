using System.Net;
using System.Net.Sockets;

namespace Atlantis.Framework.GeoByIP.Impl
{
  internal static class IPAddressExtensions
  {
    public static IPAddress TryConvertToIPv6(this IPAddress address)
    {
      IPAddress result = address;

      if (address.AddressFamily == AddressFamily.InterNetwork)
      {
        string ipv6 = "::ffff:" + address.ToString();
        if (!IPAddress.TryParse(ipv6, out result))
        {
          result = address;
        }
      }

      return result;
    }
  }
}
