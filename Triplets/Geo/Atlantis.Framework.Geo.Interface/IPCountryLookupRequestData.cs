namespace Atlantis.Framework.Geo.Interface
{
  public class IPCountryLookupRequestData : IPLookupRequestData
  {
    public IPCountryLookupRequestData(string ipAddress)
      : base(ipAddress)
    {
    }
  }
}
