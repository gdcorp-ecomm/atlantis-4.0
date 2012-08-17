using System;
using System.Net;
using Atlantis.Framework.Interface;
using Atlantis.Framework.PromoOrderLevelCreate.Interface;

namespace Atlantis.Framework.PromoOrderLevelUpdate.Interface
{
  public class PromoOrderLevelUpdateResponseData : PromoOrderLevelCreateResponseData
  {
    public PromoOrderLevelUpdateResponseData(RequestData requestData, string responseXml)
      : base(requestData, responseXml)
    {
    }

    public PromoOrderLevelUpdateResponseData(RequestData requestData, OrderLevelPromoException exception)
      : base(requestData, exception)
    {
    }

    public PromoOrderLevelUpdateResponseData(RequestData requestData, string responseXml, OrderLevelPromoException exception)
      : base(requestData, responseXml, exception)
    {
    }

    public PromoOrderLevelUpdateResponseData(RequestData requestData, WebExceptionStatus exception)
      :base(requestData,exception)
    {
    }

    public PromoOrderLevelUpdateResponseData(RequestData requestData, string responseXml, Exception exception)
      : base(responseXml, requestData, exception)
    {
    }

    public PromoOrderLevelUpdateResponseData(RequestData requestData, string responseXml, AtlantisException exception)
      : base(requestData, responseXml, exception)
    {
    }
  }
}
