using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Localization.Interface
{
  public class CountryCodeMarketIdsRequestData : RequestData
  {
    public string CountryCode { get; private set; }
    public CountryCodeMarketIdsRequestData(string countryCode)
    {
      CountryCode = countryCode;
    }
  }
}
