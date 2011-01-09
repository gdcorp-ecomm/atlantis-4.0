
namespace Atlantis.Framework.Interface
{
  public interface IRequest
  {
    IResponseData RequestHandler(RequestData requestData, ConfigElement config);
  }
}