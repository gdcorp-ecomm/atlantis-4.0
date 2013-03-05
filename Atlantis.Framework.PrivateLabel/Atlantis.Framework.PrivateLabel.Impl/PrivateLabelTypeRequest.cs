using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.Interface;
using Atlantis.Framework.PrivateLabel.Interface;
using System;

namespace Atlantis.Framework.PrivateLabel.Impl
{
  public class PrivateLabelTypeRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;

      try
      {
        var privateLabelTypeInput = (PrivateLabelTypeRequestData)requestData;
        using (GdDataCacheOutOfProcess comCache = GdDataCacheOutOfProcess.CreateDisposable())
        {
          int privateLabelType = comCache.GetPrivateLabelType(privateLabelTypeInput.PrivateLabelId);
          result = PrivateLabelTypeResponseData.FromPrivateLabelType(privateLabelType);
        }
      }
      catch (Exception ex)
      {
        AtlantisException exception = new AtlantisException(requestData, "PrivateLabelTypeRequest.RequestHandler", ex.Message + ex.StackTrace, requestData.ToXML());
        result = PrivateLabelTypeResponseData.FromException(exception);
      }

      return result;
    }
  }
}
