using Atlantis.Framework.EcommInstorePending.Interface;
using Atlantis.Framework.Interface;
using System;

namespace Atlantis.Framework.EcommInstorePending.Impl
{
  public class EcommInstorePendingRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;
      try
      {
        EcommInstorePendingRequestData acceptRequest = (EcommInstorePendingRequestData)requestData;

        result = new EcommInstorePendingResponseData(0, string.Empty, 0, "USD");
      }
      catch (Exception ex)
      {
        result = new EcommInstorePendingResponseData(requestData, ex);
      }

      return result;
    }
  }
}
