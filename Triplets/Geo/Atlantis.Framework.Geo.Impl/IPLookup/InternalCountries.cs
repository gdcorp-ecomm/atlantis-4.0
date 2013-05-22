namespace Atlantis.Framework.Geo.Impl.IPLookup
{
  internal static class InternalCountries
  {
    internal static string LookupCountryCode(string ipAddress)
    {
      string result = string.Empty;

      if (ipAddress != null)
      {
        if (ipAddress.StartsWith("172."))
        {
          result = LookupInternalIPCountryCode(ipAddress);
        }
        else
        {
          result = LookupKnownProxyCountryCode(ipAddress);
        }
      }

      return result;
    }

    private static string LookupInternalIPCountryCode(string userHostAddress)
    {
      string result = "us";
      if (userHostAddress.StartsWith("172.29."))
      {
        int third = GetOctet(userHostAddress, 3);
        if ((third >= 32) && (third <= 36))
        {
          result = "in";
        }
      }
      return result;
    }

    private static string LookupKnownProxyCountryCode(string userHostAddress)
    {
      string result = string.Empty;

      // India CSR proxied through singapore datacenter
      if (userHostAddress.StartsWith("182.50.145."))
      {
        int lastOctect = GetOctet(userHostAddress, 4);
        if ((lastOctect >= 32) && (lastOctect <= 34))
        {
          result = "in";
        }
      }
      else if (userHostAddress.StartsWith("182.94.14."))
      {
        int lastOctect = GetOctet(userHostAddress, 4);
        if ((lastOctect >= 0) && (lastOctect <= 159))
        {
          result = "in";
        }
      }

      return result;
    }

    private static int GetOctet(string userHostAddress, int octetNumber)
    {
      int result = -1;

      if ((userHostAddress != null) && (octetNumber > 0) && (octetNumber <= 4))
      {
        string[] parts = userHostAddress.Split('.');
        if (parts.Length == 4)
        {
          int octet;
          if (int.TryParse(parts[octetNumber - 1], out octet))
          {
            result = octet;
          }
        }
      }

      return result;
    }


  }
}
