using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Providers.Brand
{
  // Possible atlantis.config entry - remove this before peer review
  // <ConfigElement progid="Atlantis.Framework.Providers.Brand.CompanyName" assembly="Atlantis.Framework.Providers.Brand.dll" request_type="###" />

  public class CompanyName : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result;

      // Handle the request and return the IResponseData object for the request
      // Returning null will cause an exception

      return result;
    }
  }
}
