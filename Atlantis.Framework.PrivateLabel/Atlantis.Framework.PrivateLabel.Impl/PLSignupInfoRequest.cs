using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.Interface;
using Atlantis.Framework.PrivateLabel.Interface;

namespace Atlantis.Framework.PrivateLabel.Impl
{
  public class PLSignupInfoRequest : IRequest
  {
    const string _REQUESTXMLFORMAT = "<GetSignupInfoByEntityID><param name=\"n_EntityID\" value=\"{0}\" /></GetSignupInfoByEntityID>";
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      PLSignupInfoRequestData request = (PLSignupInfoRequestData)requestData;
      string requestXml = string.Format(_REQUESTXMLFORMAT, request.PrivateLabelId.ToString());
      string signupInfoXml;

      using (var comCache = GdDataCacheOutOfProcess.CreateDisposable())
      {
        signupInfoXml = comCache.GetCacheData(requestXml);  
      }

      return PLSignupInfoResponseData.FromCacheXml(signupInfoXml);
    }
  }
}
