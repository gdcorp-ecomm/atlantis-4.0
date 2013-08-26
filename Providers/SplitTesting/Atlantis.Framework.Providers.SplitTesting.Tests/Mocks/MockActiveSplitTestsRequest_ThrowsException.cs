using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Providers.SplitTesting.Tests.Mocks
{
  class MockActiveSplitTestsRequest_ThrowsException : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config) { throw new Exception("MockActiveSplitTestsRequest_ThrowsException throwing this exception."); }
  }
}
