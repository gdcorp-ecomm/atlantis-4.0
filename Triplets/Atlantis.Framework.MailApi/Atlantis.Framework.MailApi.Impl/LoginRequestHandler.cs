using Atlantis.Framework.Interface;
using Atlantis.Framework.MailApi.Interface;

namespace Atlantis.Framework.MailApi.Impl
{
  // Possible atlantis.config entry - remove this before peer review
  // <ConfigElement progid="Atlantis.Framework.MailApi.Impl.LoginRequestHandler" assembly="Atlantis.Framework.MailApi.Impl.dll" request_type="###" />

  public class LoginRequestHandler : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      var request = (LoginRequestData) requestData;

      IResponseData result = null;

      // Handle the request and return the IResponseData object for the request
      // Returning null will cause an exception

      return result;
    }
  }
}
