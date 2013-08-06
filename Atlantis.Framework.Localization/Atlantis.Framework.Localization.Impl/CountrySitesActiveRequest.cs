using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Localization.Interface;

namespace Atlantis.Framework.Localization.Impl
{
  public class CountrySitesActiveRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      const string SERVICE_REQUEST = "<CountrySiteGetActive />";

      string serviceResponse;

      using (var comCache = GdDataCacheOutOfProcess.CreateDisposable())
      {
        serviceResponse = comCache.GetCacheData(SERVICE_REQUEST);
      }

      IResponseData result = CountrySitesActiveResponseData.FromCacheDataXml(serviceResponse);

      return result;
    }

    #endregion
  }
}
