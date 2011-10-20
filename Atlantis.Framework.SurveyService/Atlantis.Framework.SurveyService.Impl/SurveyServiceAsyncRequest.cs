using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SurveyService.Interface;

namespace Atlantis.Framework.SurveyService.Impl
{
  class SurveyServiceAsyncRequest : IAsyncRequest
  {
    #region IAsyncRequest Members

    public IAsyncResult BeginHandleRequest(RequestData oRequestData, ConfigElement oConfig, AsyncCallback oCallback, object oState)
    {
      SurveyServiceRequestData oSurveyRequestData = (SurveyServiceRequestData)oRequestData;
      WSCgdSurvey.WSCgdSurveyService oSurveyWS = new WSCgdSurvey.WSCgdSurveyService();
      oSurveyWS.Url = ((WsConfigElement)oConfig).WSURL;
      AsyncState oAsyncState = new AsyncState(oRequestData, oConfig, oSurveyWS, oState);

      return oSurveyWS.BeginSaveSBAnswers(oSurveyRequestData.IPAddress,
                                          oSurveyRequestData.AdVersion,
                                          oSurveyRequestData.AgeGroupID,
                                          oSurveyRequestData.PoliticalID,
                                          oSurveyRequestData.Answers,
                                          oCallback, oAsyncState);
    }

    public IResponseData EndHandleRequest(IAsyncResult oAsyncResult)
    {
      IResponseData oResponseData = null;
      string sResponseXML = "";

      AsyncState oAsyncState = (AsyncState)oAsyncResult.AsyncState;

      try
      {
        WSCgdSurvey.WSCgdSurveyService oSurveyWS = (WSCgdSurvey.WSCgdSurveyService)oAsyncState.Request;
        oResponseData = new SurveyServiceResponseData(oSurveyWS.EndSaveSBAnswers(oAsyncResult));
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new SurveyServiceResponseData(sResponseXML, exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new SurveyServiceResponseData(sResponseXML, oAsyncState.RequestData, ex);
      }

      return oResponseData;
    }

    #endregion
  }
}
