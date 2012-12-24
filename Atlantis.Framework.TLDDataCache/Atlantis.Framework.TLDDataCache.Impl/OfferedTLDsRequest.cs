using Atlantis.Framework.Interface;
using Atlantis.Framework.TLDDataCache.Interface;
using System;
using System.Data;

namespace Atlantis.Framework.TLDDataCache.Impl
{
  public class OfferedTLDsRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;

      try
      {
        var offeredTldRequest = ((OfferedTLDsRequestData)requestData);
        DataTable offeredTlds = DataCache.DataCache.GetTLDList(offeredTldRequest.PrivateLabelId, (int)offeredTldRequest.TLDProductType);
        result = OfferedTLDsResponseData.FromDataTable(offeredTlds);
      }
      catch (Exception ex)
      {
        result = OfferedTLDsResponseData.FromException(requestData, ex);
      }

      return result;
    }
  }
}
