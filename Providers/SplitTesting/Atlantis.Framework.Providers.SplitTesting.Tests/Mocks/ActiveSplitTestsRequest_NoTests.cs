using Atlantis.Framework.Interface;
using Atlantis.Framework.SplitTesting.Interface;

namespace Atlantis.Framework.Providers.SplitTesting.Tests.Mocks
{
  class ActiveSplitTestsRequest_NoTests : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config) { return ActiveSplitTestsResponseData.Empty; }
  }
}
