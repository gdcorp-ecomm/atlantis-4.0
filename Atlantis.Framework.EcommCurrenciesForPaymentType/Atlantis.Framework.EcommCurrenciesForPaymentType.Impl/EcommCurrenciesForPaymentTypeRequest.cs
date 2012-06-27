using System;
using Atlantis.Framework.EcommCurrenciesForPaymentType.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommCurrenciesForPaymentType.Impl
{
  public class EcommCurrenciesForPaymentTypeRequest : IRequest
  {

    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      EcommCurrenciesForPaymentTypeResponseData oResponseData = null;
      string sResponseXML = "";
      int result = -1;
      try
      {
        EcommCurrenciesForPaymentTypeRequestData request = (EcommCurrenciesForPaymentTypeRequestData)oRequestData;
        using (WSCgdPaymentTypes.WSCgdPaymentTypesService oSvc = new Atlantis.Framework.EcommCurrenciesForPaymentType.Impl.WSCgdPaymentTypes.WSCgdPaymentTypesService())
        {
          oSvc.Url = ((WsConfigElement)oConfig).WSURL;
          sResponseXML = string.Empty;
          result = oSvc.GetAvailableCurrenciesForPaymentType(request.BasketType, request.PaymentType, request.PaymentSubType, out sResponseXML);
          if (result != 0)
          {
            AtlantisException exAtlantis = new AtlantisException(oRequestData,
                                                                 "EcommCurrenciesForPaymentTypeRequest.RequestHandler",
                                                                 sResponseXML,
                                                                 oRequestData.ToXML());

            oResponseData = new EcommCurrenciesForPaymentTypeResponseData(sResponseXML, exAtlantis);
          }
          else
          {
            oResponseData = new EcommCurrenciesForPaymentTypeResponseData(sResponseXML);
          }
        }
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new EcommCurrenciesForPaymentTypeResponseData(sResponseXML, exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new EcommCurrenciesForPaymentTypeResponseData(sResponseXML, oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion

  }
}
