using System.Globalization;
using Atlantis.Framework.Geo.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Geo.Impl
{
  public class StateNamesRequest : LanguageNamesRequest
  {
    public override IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      var stateNamesRequest = (StateNamesRequestData)requestData;

      if (string.IsNullOrEmpty(stateNamesRequest.FullLanguage))
      {
        return StateNamesResponseData.Empty;
      }

      var url = ((WsConfigElement)config).WSURL + stateNamesRequest.CountryId.ToString(CultureInfo.InvariantCulture) + "/" + stateNamesRequest.FullLanguage;
      var xml = GetServiceDataXml(url, requestData.RequestTimeout);
      return StateNamesResponseData.FromServiceData(xml);
    }
  }
}
