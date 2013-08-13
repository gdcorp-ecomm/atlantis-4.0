using Atlantis.Framework.Interface;
using Atlantis.Framework.SplitTesting.Interface;

namespace Atlantis.Framework.Providers.SplitTesting.Tests.Mocks
{
  class MockActiveSplitTestsRequest_3Tests : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      var tests = 
        @"<data count=""3"">"+ 
        @"<item SplitTestID=""1"" VersionNumber=""1"" EligibilityRules=""dataCenter(AP)"" SplitTestRunID=""1"" TestStartDate=""04/22/2013""/>" +
        @"<item SplitTestID=""2"" VersionNumber=""2"" EligibilityRules=""dataCenter(AP)"" SplitTestRunID=""2"" TestStartDate=""04/23/2013""/>" +
        @"<item SplitTestID=""3"" VersionNumber=""3"" EligibilityRules=""dataCenter(AP)"" SplitTestRunID=""3"" TestStartDate=""04/24/2013""/>" +
        @"</data>";
      var resp = ActiveSplitTestsResponseData.FromCacheXml(tests);
      return resp;
    }
  }
}
