using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SplitTesting.Interface;

namespace Atlantis.Framework.Providers.SplitTesting.Tests.Mocks
{
  class MockActiveSplitTestDetailsRequest_A: IRequest
  {

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {

      var side = 
        @"<data count=""1"">" +
        @"<item SplitTestSideID=""1"" SideName=""A"" InitialPercentAllocation=""100.00""/>" +
        @"</data>";
      var resp = ActiveSplitTestDetailsResponseData.FromCacheXml(side);
    
      
      return resp;
    }
  }
}
