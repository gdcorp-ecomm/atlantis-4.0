using Atlantis.Framework.Interface;
using Atlantis.Framework.SplitTesting.Interface;

namespace Atlantis.Framework.Providers.SplitTesting.Tests.Mocks
{
  class MockActiveSplitTestDetailsRequest_AB_50_50: IRequest
  {

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {

      var side = 
        @"<data count=""2"">" +
        @"<item SplitTestSideID=""1"" SideName=""A"" InitialPercentAllocation=""50.00""/><item SplitTestSideID=""2"" SideName=""B"" InitialPercentAllocation=""50.00"" />" +
        @"</data>";
      var resp = ActiveSplitTestDetailsResponseData.FromCacheXml(side);
    
      
      return resp;
    }
  }
}
