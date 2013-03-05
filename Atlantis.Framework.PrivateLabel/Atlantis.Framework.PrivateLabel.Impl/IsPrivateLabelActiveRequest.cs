using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.Interface;
using Atlantis.Framework.PrivateLabel.Interface;
using System;

namespace Atlantis.Framework.PrivateLabel.Impl
{
  public class IsPrivateLabelActiveRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;

      try
      {
        var isActiveInput = (IsPrivateLabelActiveRequestData)requestData;
        using (GdDataCacheOutOfProcess comCache = GdDataCacheOutOfProcess.CreateDisposable())
        {
          bool isActive = comCache.IsPrivateLabelActive(isActiveInput.PrivateLabelId);
          result = IsPrivateLabelActiveResponseData.FromIsActive(isActive);
        }
      }
      catch (Exception ex)
      {
        AtlantisException exception = new AtlantisException(requestData, "IsPrivateLabelActiveRequest.RequestHandler", ex.Message + ex.StackTrace, requestData.ToXML());
        result = IsPrivateLabelActiveResponseData.FromException(exception);
      }

      return result;
    }
  }
}
