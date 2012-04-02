using System;

namespace Atlantis.Framework.GeoByIP.Interface
{
  public class GeoCountry : IComparable<GeoCountry>, IComparable<string>
  {
    public static GeoCountry UnknownCountry { get; private set; }

    static GeoCountry()
    {
      UnknownCountry = new GeoCountry("--", "N/A");
    }

    public string Code { get; private set; }
    public string Name { get; private set; }

    public GeoCountry(string code, string name)
    {
      Code = code;
      Name = name;
    }

    public int CompareTo(string other)
    {
      return Code.CompareTo(other);
    }

    public int CompareTo(GeoCountry other)
    {
      if (other == null)
      {
        return Code.CompareTo(null);
      }
      else
      {
        return Code.CompareTo(other.Code);
      }
    }
  }
}
