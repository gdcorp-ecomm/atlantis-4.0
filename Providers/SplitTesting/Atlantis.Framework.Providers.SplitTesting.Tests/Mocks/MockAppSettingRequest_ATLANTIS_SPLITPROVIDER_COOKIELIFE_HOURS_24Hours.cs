using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.AppSettings.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Providers.SplitTesting.Tests.Mocks
{
  class MockAppSettingRequest_ATLANTIS_SPLITPROVIDER_COOKIELIFE_HOURS_24Hours : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      string settingValue = "24";
      return AppSettingResponseData.FromSettingValue(settingValue);
    }
  }
}
