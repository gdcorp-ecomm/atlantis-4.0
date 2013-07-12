namespace Atlantis.Framework.Geo.Interface
{
  public class IPLocationLookupRequestData : IPLookupRequestData
  {
    public IPLocationLookupRequestData(string ipAddress)
      : base(ipAddress)
    {
    }
  }
}
