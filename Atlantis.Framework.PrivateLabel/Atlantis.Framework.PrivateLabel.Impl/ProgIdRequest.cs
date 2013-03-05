using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.Interface;
using Atlantis.Framework.PrivateLabel.Interface;
using System;

namespace Atlantis.Framework.PrivateLabel.Impl
{
  public class ProgIdRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;

      try
      {
        var progIdInput = (ProgIdRequestData)requestData;
        using (GdDataCacheOutOfProcess comCache = GdDataCacheOutOfProcess.CreateDisposable())
        {
          string progId = comCache.GetProgId(progIdInput.PrivateLabelId);
          result = ProgIdResponseData.FromProgId(progId);
        }
      }
      catch (Exception ex)
      {
        AtlantisException exception = new AtlantisException(requestData, "ProgIdRequest.RequestHandler", ex.Message + ex.StackTrace, requestData.ToXML());
        result = ProgIdResponseData.FromException(exception);
      }

      return result;
    }
  }
}
