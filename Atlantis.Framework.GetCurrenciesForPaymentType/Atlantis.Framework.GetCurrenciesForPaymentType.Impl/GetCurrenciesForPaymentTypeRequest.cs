using System;
using Atlantis.Framework.GetCurrenciesForPaymentType.Interface;
using Atlantis.Framework.Interface;


namespace Atlantis.Framework.GetCurrenciesForPaymentType.Impl
{
  public class GetCurrenciesForPaymentTypeRequest : IRequest
  {

    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      GetCurrenciesForPaymentTypeResponseData oResponseData = null;
      string sResponseXML = "";
      int result = -1;
      try
      {
        GetCurrenciesForPaymentTypeRequestData request = (GetCurrenciesForPaymentTypeRequestData)oRequestData;
        using (WSCgdPaymentTypes.WSCgdPaymentTypesService oSvc = new Atlantis.Framework.GetCurrenciesForPaymentType.Impl.WSCgdPaymentTypes.WSCgdPaymentTypesService())
        {
          oSvc.Url = ((WsConfigElement)oConfig).WSURL;
          sResponseXML = string.Empty;
          result = oSvc.GetAvailableCurrenciesForPaymentType(request.BasketType, request.PaymentType, request.PaymentSubType, out sResponseXML);
          if (result != 0)
          {
            AtlantisException exAtlantis = new AtlantisException(oRequestData,
                                                                 "GetCurrenciesForPaymentTypeRequest.RequestHandler",
                                                                 sResponseXML,
                                                                 oRequestData.ToXML());

            oResponseData = new GetCurrenciesForPaymentTypeResponseData(sResponseXML, exAtlantis);
          }
          else
          {
            oResponseData = new GetCurrenciesForPaymentTypeResponseData(sResponseXML);
          }
        }
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new GetCurrenciesForPaymentTypeResponseData(sResponseXML, exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new GetCurrenciesForPaymentTypeResponseData(sResponseXML, oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion

  }
}
