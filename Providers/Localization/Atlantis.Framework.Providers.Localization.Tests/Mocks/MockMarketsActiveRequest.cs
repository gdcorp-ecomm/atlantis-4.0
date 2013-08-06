using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Localization.Interface;

namespace Atlantis.Framework.Providers.Localization.Tests.Mocks
{
  public class MockMarketsActiveRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      return MarketsActiveResponseData.FromCacheDataXml(config.GetConfigValue("test-xml"));
    }

    #endregion
  }
}
