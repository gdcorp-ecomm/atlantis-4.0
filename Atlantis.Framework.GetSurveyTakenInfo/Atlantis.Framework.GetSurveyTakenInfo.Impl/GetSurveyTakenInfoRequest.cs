using System;
using Atlantis.Framework.GetSurveyTakenInfo.Impl.SurveyWS;
using Atlantis.Framework.GetSurveyTakenInfo.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GetSurveyTakenInfo.Impl
{
  public class GetSurveyTakenInfoRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      GetSurveyTakenInfoResponseData responseData = null;

      try
      {
        GetSurveyTakenInfoRequestData requestData = (GetSurveyTakenInfoRequestData)oRequestData;

        using (PQCWS service = new PQCWS())
        {
          service.Url = ((WsConfigElement)oConfig).WSURL;
          service.Timeout = (int)requestData.RequestTimeout.TotalMilliseconds;

          string surveyTakenInfo = service.ReturnSurveyTakenInfobySurveyIDAndShopperID(requestData.SurveyId, requestData.ShopperID);
          responseData = new GetSurveyTakenInfoResponseData(surveyTakenInfo);
        }
      }
      catch (Exception ex)
      {
        responseData = new GetSurveyTakenInfoResponseData(oRequestData, ex);
      }

      return responseData;
    }

    #endregion
  }
}
