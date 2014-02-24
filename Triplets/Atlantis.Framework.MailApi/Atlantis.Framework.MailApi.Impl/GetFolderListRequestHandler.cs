using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MailApi.Impl
{

  public class GetFolderListRequestHandler : IRequest
  {

    private string BodyString = "method=getFolderList&params={\"extended_info\":\"true\"}";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;

      // Handle the request and return the IResponseData object for the request
      // Returning null will cause an exception

      return result;
    }
  }
}
