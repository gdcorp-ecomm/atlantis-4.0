using Atlantis.Framework.Interface;
using Atlantis.Framework.SplitTesting.Interface;

namespace Atlantis.Framework.Providers.SplitTesting.Tests.Mocks
{
  class MockActiveSplitTestDetailsRequest_AB_100_0: IRequest
  {

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {

      var side = 
        @"<data count=""2"">" +
        @"<item SplitTestSideID=""1"" SideName=""A"" InitialPercentAllocation=""00.00""/><item SplitTestSideID=""2"" SideName=""B"" InitialPercentAllocation=""100.00"" />" +
        @"</data>";
      var resp = ActiveSplitTestDetailsResponseData.FromCacheXml(side);
    
      
      return resp;
    }
  }
}
