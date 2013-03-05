using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.Interface;
using Atlantis.Framework.PrivateLabel.Interface;
using System;

namespace Atlantis.Framework.PrivateLabel.Impl
{
  public class PrivateLabelDataRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;

      try
      {
        var plDataInput = (PrivateLabelDataRequestData)requestData;
        using (GdDataCacheOutOfProcess comCache = GdDataCacheOutOfProcess.CreateDisposable())
        {
          string dataValue = comCache.GetPLData(plDataInput.PrivateLabelId, plDataInput.DataCategoryId);
          result = PrivateLabelDataResponseData.FromDataValue(dataValue);
        }
      }
      catch (Exception ex)
      {
        AtlantisException exception = new AtlantisException(requestData, "PrivateLabelDataRequest.RequestHandler", ex.Message + ex.StackTrace, requestData.ToXML());
        result = PrivateLabelDataResponseData.FromException(exception);
      }

      return result;
    }
  }
}
