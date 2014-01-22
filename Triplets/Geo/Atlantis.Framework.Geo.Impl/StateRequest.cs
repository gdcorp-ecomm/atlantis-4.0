using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.Geo.Interface;
using Atlantis.Framework.Interface;
using System;

namespace Atlantis.Framework.Geo.Impl
{
  public class StateRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;

      try
      {
        var stateRequest = (StateRequestData)requestData;
        string statesXml;
        using (var comCache = GdDataCacheOutOfProcess.CreateDisposable())
        {
          statesXml = comCache.GetStatesXml(stateRequest.CountryId);
        }

        result = StateResponseData.FromDataCacheXml(statesXml);
      }
      catch (Exception ex)
      {
        AtlantisException exception = new AtlantisException("StateRequest.RequestHandler", 0, ex.Message + ex.StackTrace, requestData.ToXML());
        result = StateResponseData.FromException(exception);
      }

      return result;
    }
  }
}
