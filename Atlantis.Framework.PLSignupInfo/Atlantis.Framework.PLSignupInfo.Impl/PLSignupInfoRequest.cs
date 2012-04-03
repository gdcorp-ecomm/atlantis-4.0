using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.PLSignupInfo.Interface;

namespace Atlantis.Framework.PLSignupInfo.Impl
{
  public class PLSignupInfoRequest : IRequest
  {
    const string _REQUESTXMLFORMAT = "<GetSignupInfoByEntityID><param name=\"n_EntityID\" value=\"{0}\" /></GetSignupInfoByEntityID>";
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;
      PLSignupInfoRequestData request = (PLSignupInfoRequestData)requestData;

      try
      {
        string requestXml = string.Format(_REQUESTXMLFORMAT, request.PrivateLabelId.ToString());
        string signupInfoXml = DataCache.DataCache.GetCacheData(requestXml);
        result = new PLSignupInfoResponseData(request, signupInfoXml);
      }
      catch (Exception ex)
      {
        result = new PLSignupInfoResponseData(request, ex);
      }

      return result;
    }
  }
}
