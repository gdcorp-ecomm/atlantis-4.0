using Atlantis.Framework.Currency.Interface;
using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.Currency.Impl
{
  public class MultiCurrencyContextsRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      string delimitedContextsFromSetting;

      using (var outCache = GdDataCacheOutOfProcess.CreateDisposable())
      {
        delimitedContextsFromSetting = outCache.GetAppSetting("ATLANTIS_MCP_ACTIVE_CONTEXTS");
      }

      return MultiCurrencyContextsResponseData.FromDelimitedSetting(delimitedContextsFromSetting);
    }
  }
}
