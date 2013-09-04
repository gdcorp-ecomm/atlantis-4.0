using Atlantis.Framework.Geo.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Geo.Impl
{
  public class CountryNamesRequest : LanguageNamesRequest
  {
    public override IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      var countryNamesRequest = (CountryNamesRequestData)requestData;

      if (string.IsNullOrEmpty(countryNamesRequest.FullLanguage))
      {
        return CountryNamesResponseData.Empty;
      }

      var url = ((WsConfigElement) config).WSURL + countryNamesRequest.FullLanguage;
      var xml = GetServiceDataXml(url, requestData.RequestTimeout);
      return CountryNamesResponseData.FromServiceData(xml);
    }
  }
}
