using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.Interface;
using Atlantis.Framework.PrivateLabel.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.PrivateLabel.Impl
{
  public class PrivateLabelIdRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;

      try
      {
        var plIdInput = (PrivateLabelIdRequestData)requestData;
        using (GdDataCacheOutOfProcess comCache = GdDataCacheOutOfProcess.CreateDisposable())
        {
          int plid = comCache.GetPrivateLabelId(plIdInput.ProgId);
          result = PrivateLabelIdResponseData.FromPrivateLabelId(plid);
        }
      }
      catch (Exception ex)
      {
        AtlantisException exception = new AtlantisException(requestData, "PrivateLabelIdRequest.RequestHandler", ex.Message + ex.StackTrace, requestData.ToXML());
        result = PrivateLabelIdResponseData.FromException(exception);
      }

      return result;
    }
  }
}
