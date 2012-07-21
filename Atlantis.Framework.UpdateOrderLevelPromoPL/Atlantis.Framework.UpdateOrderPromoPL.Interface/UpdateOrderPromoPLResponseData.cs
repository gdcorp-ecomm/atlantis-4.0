using System;
using System.Net;
using Atlantis.Framework.Interface;
using Atlantis.Framework.AddOrderLevelPromoPL.Interface;

namespace Atlantis.Framework.UpdateOrderPromoPL.Interface
{
  public class UpdateOrderPromoPLResponseData : PLOrderLevelPromoResponseData
  {
    public UpdateOrderPromoPLResponseData(RequestData requestData, string responseXml)
      : base(requestData, responseXml)
    {
    }

    public UpdateOrderPromoPLResponseData(RequestData requestData, PLOrderLevelPromoException exception)
      : base(requestData, exception)
    {
    }

    public UpdateOrderPromoPLResponseData(RequestData requestData, string responseXml, PLOrderLevelPromoException exception)
      : base(requestData, responseXml, exception)
    {
    }

    public UpdateOrderPromoPLResponseData(RequestData requestData, WebExceptionStatus exception)
      :base(requestData,exception)
    {
    }

    public UpdateOrderPromoPLResponseData(RequestData requestData, string responseXml, Exception exception)
      : base(responseXml, requestData, exception)
    {
    }

    public UpdateOrderPromoPLResponseData(RequestData requestData, string responseXml, AtlantisException exception)
      : base(requestData, responseXml, exception)
    {
    }
  }
}
