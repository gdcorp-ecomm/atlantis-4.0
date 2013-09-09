using Atlantis.Framework.Interface;
using System;

namespace Atlantis.Framework.Providers.Shopper.Tests
{
  public class ExceptionRequest : IRequest
  {
    public const int RequestType = 736000;

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      throw new Exception("Boom!");
    }
  }
}
