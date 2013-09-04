using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Providers.Geo.Tests
{
  public class FailRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      throw new Exception("Fail!");
    }
  }
}
