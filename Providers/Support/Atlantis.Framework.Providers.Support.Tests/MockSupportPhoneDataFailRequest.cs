using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Providers.Support.Tests
{
  public class MockSupportPhoneDataFailRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      throw new Exception("Fail!");
    }
  }
}
