using Atlantis.Framework.Interface;
using System;

namespace Atlantis.Framework.Providers.SplitTesting.Tests.Mocks
{
  class MockActiveSplitTestDetailsRequest_ThrowsException: IRequest
  {

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      throw new Exception("MockActiveSplitTestDetailsRequest_ThrowsException throwing this exception");
    }
  }
}
